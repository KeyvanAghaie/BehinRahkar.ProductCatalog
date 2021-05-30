using System;

namespace DAL.Entities
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Deleted { get; set; } = false;
    }

    public abstract class BaseEntity : BaseEntity<long>
    {
    }
}
