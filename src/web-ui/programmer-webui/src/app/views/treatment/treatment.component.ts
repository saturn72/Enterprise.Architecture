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

  getTreatments(){
    this.treatmentService.getAll()
      .subscribe(data => {
        console.log(JSON.stringify(data));
        this.treatments = data;
      });
  }

  treatmentCreate() {
    this.router.navigate(["create/"], { relativeTo: this.activatedRoute })
  }

  treatmentRead(pumpInfoId) {
    this.router.navigate(["read/" + pumpInfoId], { relativeTo: this.activatedRoute })
  }
}
