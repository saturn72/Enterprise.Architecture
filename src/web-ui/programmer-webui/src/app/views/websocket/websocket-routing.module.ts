import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WebSocketComponent } from './websocket.component';
const routes: Routes = [
  {
    path: '',
    component: WebSocketComponent,
    data: {
      title: 'WebSocket example'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebSocketRoutingModule { }
