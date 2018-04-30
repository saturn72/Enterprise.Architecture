// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";

import { TreatmentComponent } from './treatment.component';
import { TreatmentCreateOrEditComponent } from './treatment-create-or-edit.component'

// Theme Routing
import { TreatmentRoutingModule } from './treatment-routing.module';
import { TreatmentService } from './services/TreatmentService';
import { CalculationService } from './services/CalculationService';

@NgModule({
  imports: [
    CommonModule,
    TreatmentRoutingModule,
    FormsModule
  ],
  declarations: [
    TreatmentComponent,
    TreatmentCreateOrEditComponent
  ],
  providers: [
    TreatmentService,
    CalculationService
  ]
})
export class TreatmentModule { }
