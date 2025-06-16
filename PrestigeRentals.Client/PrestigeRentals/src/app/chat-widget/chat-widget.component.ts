import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-chat-widget',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-widget.component.html',
  styleUrl: './chat-widget.component.scss',
})
export class ChatWidgetComponent {
  chatOpen = false;
  userInput = '';
  messages: { sender: 'user' | 'bot'; text?: SafeHtml; image?: string }[] = [];

  @ViewChild('chatBody') chatBody!: ElementRef;

  constructor(
    private http: HttpClient,
    private router: Router,
    private renderer: Renderer2,
    private sanitizer: DomSanitizer
  ) {}

  toggleChat() {
    this.chatOpen = !this.chatOpen;

    if (this.chatOpen && this.messages.length === 0) {
      this.messages.push({
        sender: 'bot',
        text: this.sanitizer.bypassSecurityTrustHtml(
          "Hi, I'm <strong>Luna</strong>, your personal virtual assistant. How can I help you today?"
        ),
      });
    }
  }

  sendMessage() {
    const text = this.userInput.trim();
    if (!text) return;

    // User message
    this.messages.push({
      sender: 'user',
      text: this.sanitizer.bypassSecurityTrustHtml(text),
    });

    this.http
      .post<any[]>('http://localhost:5005/webhooks/rest/webhook', {
        sender: 'user123',
        message: text,
      })
      .subscribe((response) => {
        response.forEach((res) => {
          this.messages.push({
            sender: 'bot',
            text: res.text
              ? this.sanitizer.bypassSecurityTrustHtml(res.text)
              : '',
            image: res.image,
          });
        });
      });

    this.userInput = '';
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
    this.attachLinkHandlers();
  }

  scrollToBottom(): void {
    try {
      this.chatBody.nativeElement.scrollTop =
        this.chatBody.nativeElement.scrollHeight;
    } catch (err) {}
  }

  attachLinkHandlers() {
    const links =
      this.chatBody?.nativeElement.querySelectorAll('.vehicle-link');
    links?.forEach((el: HTMLElement) => {
      this.renderer.listen(el, 'click', () => {
        const link = el.getAttribute('data-link');
        if (link) {
          this.router.navigate([link]);
          location.reload();
          this.chatOpen = false; // închide chat-ul dacă vrei
        }
      });
    });
  }
}
