namespace Programmer.Common.Domain
{
    public abstract class DomainModelBase<TId>
    {
        public TId Id { get; set; }
    }
}