import { Injectable } from '@angular/core';
import { TreatmentModel } from '../models/TreatmentModel';
import { TreatmentStatus } from '../models/TreatmentStatus';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class TreatmentService {

    readonly BASEURI: string = "http://localhost:81/api/treatment/";

    constructor(private http: HttpClient) {
    }

    getAll(): Observable<any> {
        return this.http.get<any>(this.BASEURI);
    }

    getById(id: string): Observable<any> {
        return this.http.get<any>(this.BASEURI + id);
    }
    
    create(treatment: TreatmentModel): Observable<TreatmentModel> {
        const body = {
            vtbi: treatment.values.vtbi,
            dose: treatment.values.dose,
            rate: treatment.values.rate
        };
        const headers = {
            'X-Session-Token': 'saved-session-token'
        };
        return this.http.post<TreatmentModel>(this.BASEURI, body, { headers });
    }
}