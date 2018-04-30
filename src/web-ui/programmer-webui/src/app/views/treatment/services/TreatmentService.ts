import { Injectable } from '@angular/core';
import { TreatmentModel } from '../models/TreatmentModel';
import { TreatmentStatus } from '../models/TreatmentStatus';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class TreatmentService {

    readonly BASEURI : string = "http://localhost:3004/treatment/";

    constructor(private http: HttpClient) {
    }

    getAll(): Observable<TreatmentModel[]> {
        return this.http.get<TreatmentModel[]>(this.BASEURI);
    }

    getById(id: string): Observable<TreatmentModel> {
        return this.http.get<TreatmentModel>(this.BASEURI + id);
    }
    create(treatment: TreatmentModel): Observable<TreatmentModel> {
        return this.http.post<TreatmentModel>(this.BASEURI, treatment);
      }
}