using Newtonsoft.Json;
using System;

namespace Services.DTO
{
    public class DTO : IDTO<long>
    {
        [JsonIgnore]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
