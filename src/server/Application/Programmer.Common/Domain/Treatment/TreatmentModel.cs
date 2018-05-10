namespace Programmer.Common.Domain.Treatment
{
    public class TreatmentModel:DomainModelBase<string>
    {
        public string SessionId;
        public decimal Vtbi { get; set; }
        public decimal Rate { get; set; }
        public decimal Dose { get; set; }
    }
}