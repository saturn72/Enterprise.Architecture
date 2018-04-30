// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";

// Theme Routing
import { WebSocketRoutingModule } from './websocket-routing.module';
import { WebSocketComponent } from './websocket.component';

@NgModule({
  imports: [
    CommonModule,
    WebSocketRoutingModule,
    FormsModule
  ],
  declarations: [
    WebSocketComponent
  ]
})
export class WebSocketModule { }
