import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { WebSocketSubject } from 'rxjs/observable/dom/WebSocketSubject';
import { WebSocketMessageModel } from './models/WebSocketMessageModel';

const WebSocketUri = 'ws://localhost:57603/ws';

@Component({
  templateUrl: 'websocket.component.html'
})
export class WebSocketComponent implements OnInit {

  private socket$: WebSocketSubject<any>;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.socket$ = WebSocketSubject.create(WebSocketUri);

    this.socket$
      .subscribe(
        (message) => this.doSomething(message),
        (err) => console.error(err),
        () => console.warn('Completed!')
      );
  }

  doSomething(message: any): void {
    console.log(JSON.stringify(message));
  }
}
