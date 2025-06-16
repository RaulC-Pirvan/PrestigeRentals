import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { response } from 'express';

@Component({
  selector: 'app-chat-widget',
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-widget.component.html',
  styleUrl: './chat-widget.component.scss',
})
export class ChatWidgetComponent {
  chatOpen = false;
  userInput = '';
  messages: { sender: 'user' | 'bot'; text?: string; image?: string }[] = [];

  @ViewChild('chatBody') chatBody!: ElementRef;

  constructor(private http: HttpClient) {}

  toggleChat() {
    this.chatOpen = !this.chatOpen;
  }

  sendMessage() {
    const text = this.userInput.trim();
    if (!text) return;

    this.messages.push({ sender: 'user', text });

    this.http
      .post<any[]>('http://localhost:5005/webhooks/rest/webhook', {
        sender: 'user123',
        message: text,
      })
      .subscribe((response) => {
        response.forEach((res) => {
          this.messages.push({
            sender: 'bot',
            text: res.text,
            image: res.image,
          });
        });
      });

    this.userInput = '';
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.chatBody.nativeElement.scrollTop =
        this.chatBody.nativeElement.scrollHeight;
    } catch (err) {}
  }
}
