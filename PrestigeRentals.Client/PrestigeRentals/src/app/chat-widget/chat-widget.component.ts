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
      this.typeTextSlowly(
        "Hi, I'm <strong>Luna</strong>, your personal virtual assistant. How can I help you today?"
      );
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
          if (res.text) {
            this.typeTextSlowly(res.text);
          }

          if (res.image) {
            this.messages.push({
              sender: 'bot',
              image: res.image,
            });
          }
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
          this.chatOpen = false;
        }
      });
    });
  }

  typeTextSlowly(fullText: string, delay: number = 25) {
    let current = '';
    let index = 0;

    const interval = setInterval(() => {
      current += fullText[index++];
      const partial = this.sanitizer.bypassSecurityTrustHtml(current);

      if (
        this.messages.length &&
        this.messages[this.messages.length - 1].sender === 'bot' &&
        !this.messages[this.messages.length - 1].image
      ) {
        this.messages[this.messages.length - 1].text = partial;
      } else {
        this.messages.push({ sender: 'bot', text: partial });
      }

      if (index >= fullText.length) {
        clearInterval(interval);
      }
    }, delay);
  }
}
