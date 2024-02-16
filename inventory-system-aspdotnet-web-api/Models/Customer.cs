using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inventory_system_aspdotnet_web_api.Models
{
    public partial class Customer
    {
        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = null!;
        public string? CustomerAddress { get; set; } = null!;
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression("^((\\+91)|(0091))-{0,1}\\d{3}-{0,1}\\d{6}$|^\\d{10}$|^\\d{4}-\\d{6}$")]
      
        public string? CustomerPhone { get; set; } = null!;

    }

    public partial class GetCustomer : Customer
    {
        public int? CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null!;        
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}