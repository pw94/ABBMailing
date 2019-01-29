using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABBMailing.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public ICollection<AddressTopic> AddressTopics { get; set; }
        [Required]
        public string UnsubscribeToken { get; set; }
        public bool? Subscribed { get; set; }
    }
}