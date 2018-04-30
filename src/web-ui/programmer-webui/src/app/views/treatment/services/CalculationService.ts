import { Injectable } from '@angular/core';
import { TreatmentModel, TreatmentValuesModel } from '../models/TreatmentModel';
import { TreatmentStatus } from '../models/TreatmentStatus';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CalculationService {

    readonly BASEURI = "http://localhost:3004/calculator";

    constructor(private http: HttpClient) {
    }

    getValues(key: string, treatmentValues: TreatmentValuesModel): Observable<TreatmentValuesModel[]> {
        let query: string = "?"
        if (treatmentValues.vtbi) {
            query += "&vtbi=" + treatmentValues.vtbi;
        }
        if (treatmentValues.dose) {
            query += "&dose=" + treatmentValues.dose;
        }
        if (treatmentValues.rate) {
            query += "&rate=" + treatmentValues.rate;
        }
        query = query.replace("?&", "?");
        return this.http.get<TreatmentValuesModel[]>(this.BASEURI + query);
    }
}