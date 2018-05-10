import { Injectable } from '@angular/core';
import { TreatmentModel, TreatmentValuesModel } from '../models/TreatmentModel';
import { TreatmentStatus } from '../models/TreatmentStatus';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CalculationService {

    readonly BASEURI = "http://localhost:83/api/rates";

    constructor(private http: HttpClient) {
    }

    getValues(key: string, treatmentValues: TreatmentValuesModel): Observable<TreatmentValuesModel> {
        let query: string = "?vtbi=" + treatmentValues.vtbi
            + "&dose=" + treatmentValues.dose
            + "&rate=" + treatmentValues.rate;
        const headers = new HttpHeaders();
        headers.append('Content-Type', 'application/json');
        headers.append('Accept', 'application/json'),
        headers.append('Access-Control-Allow-Headers', 'Content-Type');
        
        return this.http.get<TreatmentValuesModel>(this.BASEURI + query, { headers });
    }
}