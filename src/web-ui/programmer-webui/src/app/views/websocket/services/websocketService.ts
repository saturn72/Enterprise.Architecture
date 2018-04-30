import { Injectable } from '@angular/core'
import { Subject } from 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import { WebSocketMessageModel } from '../models/WebSocketMessageModel';
import * as socketIo from 'socket.io-client';

const SERVER_URL = 'http://localhost:8080';
Injectable()
export class WebSocketService {
    private socket: Subject<WebSocketMessageModel>;
    public connect(url): Subject<WebSocketMessageModel> {
        if (!this.socket) {
            this.socket = this.create(url);
        }
        return this.socket;
    }
    private create(url): Subject<WebSocketMessageModel> {
        let ws = new WebSocket(url);
        let observable = Observable.create(
            (obs: Observer<WebSocketMessageModel>) => {
                ws.onmessage = obs.next.bind(obs);
                ws.onerror = obs.error.bind(obs);
                ws.onclose = obs.complete.bind(obs);
                return ws.close.bind(ws);
            }
        );
        let observer = {
            next: (data: Object) => {
                if (ws.readyState === WebSocket.OPEN) {
                    ws.send(JSON.stringify(data));
                }
            },
        };
        return Subject.create(observer, observable);
    }
}

