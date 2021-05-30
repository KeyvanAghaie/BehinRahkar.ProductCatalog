using Newtonsoft.Json;
using System;

namespace Services.DTO
{
    public interface IDTO<TKey>
    {
        TKey Id { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ModifyDate { get; set; }

        [JsonIgnore]
        bool Deleted { get; set; }
    }
}
