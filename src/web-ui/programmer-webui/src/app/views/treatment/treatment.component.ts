import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { getStyle, rgbToHex } from '@coreui/coreui/js/src/utilities/'
import { TreatmentService } from './services/TreatmentService';
import { TreatmentModel } from './models/TreatmentModel';

@Component({
  templateUrl: 'treatment.component.html'
})
export class TreatmentComponent implements OnInit {

  treatments: TreatmentModel[];

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private treatmentService: TreatmentService) { }

  ngOnInit() {
    this.getTreatments();
  }

  getTreatments() {
    this.treatmentService.getAll()
      .subscribe(res => {
        this.treatments = new Array<TreatmentModel>();
        res.data.forEach(element => {
          const tm = new TreatmentModel();
          tm.id = element.id;
          tm.values = {
            vtbi: element.vtbi,
            rate: element.rate,
            dose: element.dose
          }
          this.treatments.push(tm);
        })
      });
  }

  treatmentCreate() {
    this.router.navigate(["create/"], { relativeTo: this.activatedRoute })
  }

  treatmentRead(pumpInfoId) {
    this.router.navigate(["read/" + pumpInfoId], { relativeTo: this.activatedRoute })
  }
}
