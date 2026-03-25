import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { MessageItem } from '../../core/models/message.model';
import { MessageService } from '../../core/services/message.service';
import { AuthService } from '../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

interface Conversation {
  id: string;
  propertyId: number;
  propertyTitle: string;
  otherUserId: number;
  otherUserName: string;
  messages: MessageItem[];
  lastMessageAt: Date;
}

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule, DatePipe, FormsModule],
  templateUrl: './messages.html'
})
export class MessagesPage implements OnInit {
  conversations: Conversation[] = [];
  activeConversation: Conversation | null = null;
  loading = true;
  error = '';
  
  replyContent = '';
  replyLoading = false;
  currentUserId: number | null = null;

  constructor(
    private readonly messageService: MessageService,
    private readonly authService: AuthService,
    private readonly cdr: ChangeDetectorRef,
    private readonly ngZone: NgZone
  ) {
    this.currentUserId = this.authService.getUserId();
  }

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.loading = true;
    this.error = '';
    this.messageService.myMessages().subscribe({
      next: (res) => {
        this.ngZone.run(() => {
          this.processMessages(res ?? []);
          this.loading = false;
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          console.error('[Messages] Error:', err);
          this.error = 'Unable to load messages.';
          this.loading = false;
          this.cdr.detectChanges();
        });
      }
    });
  }

  processMessages(messages: MessageItem[]) {
    if (!this.currentUserId) return;
    
    const map = new Map<string, Conversation>();
    for (const msg of messages) {
      const otherUserId = msg.senderId === this.currentUserId ? msg.receiverId : msg.senderId;
      const otherUserName = msg.senderId === this.currentUserId ? msg.receiverName : msg.senderName;
      const convId = `${msg.propertyId}_${otherUserId}`;
      
      if (!map.has(convId)) {
        map.set(convId, {
          id: convId,
          propertyId: msg.propertyId,
          propertyTitle: msg.propertyTitle,
          otherUserId,
          otherUserName,
          messages: [],
          lastMessageAt: new Date(msg.createdAt)
        });
      }
      map.get(convId)!.messages.push(msg);
    }

    const convs = Array.from(map.values());
    for (const c of convs) {
      c.messages.reverse(); // Chronological order
    }
    
    // Sort conversations by latest message
    this.conversations = convs.sort((a, b) => b.lastMessageAt.getTime() - a.lastMessageAt.getTime());

    if (this.activeConversation) {
      this.activeConversation = this.conversations.find(c => c.id === this.activeConversation!.id) || null;
    }
  }

  selectConversation(conv: Conversation) {
    this.activeConversation = conv;
    this.replyContent = '';
  }

  clearSelection() {
    this.activeConversation = null;
  }

  sendReply() {
    if (!this.activeConversation || !this.replyContent.trim() || !this.currentUserId) return;

    this.replyLoading = true;
    
    const payload = {
      propertyId: this.activeConversation.propertyId,
      receiverId: this.activeConversation.otherUserId,
      content: this.replyContent.trim()
    };

    this.messageService.send(payload).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.replyLoading = false;
          this.replyContent = '';
          this.fetch(); // Refresh all messages
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.replyLoading = false;
          console.error('[Messages] Reply Error:', err);
          alert('Failed to send reply. ' + (err?.error?.message || ''));
          this.cdr.detectChanges();
        });
      }
    });
  }
}
