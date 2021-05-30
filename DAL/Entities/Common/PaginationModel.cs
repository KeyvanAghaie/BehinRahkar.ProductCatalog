using System.Collections.Generic;

namespace DAL.Entities
{
    public class PaginationModel<T>
    {
        public List<T> Items { get; set; }

        public long TotalItems { get; set; }

        public long PageItems { get; set; }
    }
}
