import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { getStyle, rgbToHex } from '@coreui/coreui/js/src/utilities/'
import { TreatmentModel, TreatmentValuesModel } from './models/TreatmentModel';
import { TreatmentService } from './services/TreatmentService';
import { TreatmentStatus } from './models/TreatmentStatus';
import { CalculationService } from './services/CalculationService';

@Component({
  templateUrl: 'treatment-create-or-edit.component.html'
})
export class TreatmentCreateOrEditComponent implements OnInit {

  id: string;
  treatment: TreatmentModel;
  ranges: {} = {
    doseRanges: '[0-100]',
    vtbiRanges: '[0-100]',
    rateRanges: '[0-100]'
  }
  isCalculating: boolean = false;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private treatmentService: TreatmentService, private calculationService: CalculationService) {
  }

  ngOnInit() {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');

    if (this.id) {
      this.treatmentService.getById(this.id)
        .subscribe(data => this.treatment = data);
    }
    else {
      this.resetForm();
    }
  }
  fillTreatmentValues(key) {
    if ((this.treatment.values.vtbi > 0 && this.treatment.values.rate > 0)
      || (this.treatment.values.vtbi > 0 && this.treatment.values.dose > 0)
      || (this.treatment.values.rate > 0 && this.treatment.values.dose > 0)) {

      this.isCalculating = true;
      this.calculationService.getValues(key, this.treatment.values).
        subscribe(values => {
          this.treatment.values = values[0];
          this.isCalculating = false;
        });
    }
  }
  submitTreatment() {
    this.treatment.status = TreatmentStatus.SentToPump;

    this.treatmentService.create(this.treatment)
      .subscribe(data => {
        this.router.navigate(["../"], { relativeTo: this.activatedRoute })
      });
  }
  isValidForm() {
    const values = this.treatment.values;
    return values.dose > 0 && values.rate > 0 && values.vtbi > 0;
  }

  resetForm() {
    this.treatment = new TreatmentModel();
    this.treatment.values = new TreatmentValuesModel();
  }
}