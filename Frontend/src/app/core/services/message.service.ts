import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageCreatePayload, MessageItem } from '../models/message.model';

@Injectable({ providedIn: 'root' })
export class MessageService {
  private readonly url = 'http://localhost:5255/api/message';

  constructor(private readonly http: HttpClient) {}

  send(payload: MessageCreatePayload) {
    return this.http.post<MessageItem>(this.url, payload);
  }

  myMessages() {
    return this.http.get<MessageItem[]>(`${this.url}/my`);
  }
}
