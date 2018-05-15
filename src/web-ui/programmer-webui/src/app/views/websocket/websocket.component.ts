import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { WebSocketSubject } from 'rxjs/observable/dom/WebSocketSubject';
import { WebSocketMessageModel } from './models/WebSocketMessageModel';

const WebSocketUri = 'ws://localhost:81/ws';

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
        message => this.binToString(message),
        err => console.log("There was an error: " + err));
  }
  binToString(str: string) {
    var message = "";
    for (var i = 0; i < str.length; i++) {
      message += String.fromCharCode(parseInt(str[i], 2));
    }

    console.log("Returned as websocket stream: " + message)
  }
}
