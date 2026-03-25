export interface MessageItem {
  id: number;
  propertyId: number;
  propertyTitle: string;
  senderId: number;
  senderName: string;
  receiverId: number;
  receiverName: string;
  content: string;
  isRead: boolean;
  createdAt: string;
}

export interface MessageCreatePayload {
  propertyId: number;
  receiverId: number;
  content: string;
}
