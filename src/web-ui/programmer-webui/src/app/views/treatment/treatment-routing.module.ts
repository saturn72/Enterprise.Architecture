import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TreatmentComponent } from './treatment.component';
import { TreatmentCreateOrEditComponent } from './treatment-create-or-edit.component';
const routes: Routes = [
  {
    path: '',
    component: TreatmentComponent,
    data: {
      title: 'All Treatment Types'
    }
  }, {
    path: 'read/:id',
    component: TreatmentCreateOrEditComponent,
    data: {
      title: 'Treament details'
    }
  }, {
    path: 'create',
    component: TreatmentCreateOrEditComponent,
    data: {
      title: 'Create Treament'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TreatmentRoutingModule { }
