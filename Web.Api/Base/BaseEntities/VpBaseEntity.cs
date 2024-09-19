namespace Web.Api.Base.BaseEntities
{
    public abstract class VpBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
