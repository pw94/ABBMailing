using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABBMailing.Models
{
    public class Topic
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<AddressTopic> AddressTopics { get; set; }
    }

}