using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [JsonIgnore]
        public DateTime? ModifiedDate { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public Status Status { get; set; } = Status.Active;
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
