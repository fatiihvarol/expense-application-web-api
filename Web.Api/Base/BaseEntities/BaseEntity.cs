namespace Web.Api.Base.BaseEntities
{
    public abstract class BaseEntity
    {
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
