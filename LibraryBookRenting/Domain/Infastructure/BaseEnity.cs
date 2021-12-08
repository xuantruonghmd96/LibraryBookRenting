using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Domain.Infastructure
{
    public interface IEntity
    {
    }

    public abstract class BaseEnity<TKey> : IEntity
    {
        [Key]
        public TKey Id { get; set; }
    }

    public abstract class AuditableEntity<TKey> : BaseEnity<TKey>
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
