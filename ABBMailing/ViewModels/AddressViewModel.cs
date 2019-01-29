using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABBMailing.ViewModels
{
    public class AddressViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinimumListLength(1)]
        public IList<int> Topics { get; set; }
    }
}