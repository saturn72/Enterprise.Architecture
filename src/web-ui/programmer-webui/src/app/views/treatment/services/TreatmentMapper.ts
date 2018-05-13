import { TreatmentModel } from "../models/TreatmentModel";

export class TreatmentMapper{
    public static ApiModelToTreatmentModel(apiModel: any): TreatmentModel{
        const tm = new TreatmentModel();
        tm.id = apiModel.id;
        tm.values = {
            vtbi: apiModel.vtbi,
            rate: apiModel.rate,
            dose: apiModel.dose
        }
        return tm;
    }
}