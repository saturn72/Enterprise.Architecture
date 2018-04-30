import { TreatmentStatus } from "./TreatmentStatus";

export class TreatmentModel {
    values: TreatmentValuesModel;
    id: string;
    status: TreatmentStatus;

    constructor() {
        this.values = null;
    }
}

export class TreatmentValuesModel {
    vtbi: number;
    rate: number;
    dose: number;
}



