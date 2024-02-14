using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inventory_system_aspdotnet_web_api.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public decimal OrderTotalAmount { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual GetCustomer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

public partial class AddOrder
{
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int ProductId { get; set; }

    [Required]
    [PositiveNonZero(ErrorMessage = "The Order Detail Quantity must be a positive non-zero value.")]
    public int OrderDetailQuantity { get; set; }
    [Required]
    public decimal OrderDetailUnitPrice { get; set; }
    public DateTime? OrderDate { get; set; }

}



public partial class GetOrder
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public string? OrderStatus { get; set; }
    public decimal OrderTotalAmount { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ProductDescription { get; set; } = "";
    public int? OrderDetailQuantity { get; set; } = 0;
    public decimal OrderDetailUnitPrice { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string? CustomerAddress { get; set; } = null!;
    public string? CustomerPhone { get; set; } = null!;

}


public class PositiveNonZeroAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || Convert.ToInt32(value) <= 0)
        {
            return new ValidationResult("The field must be a positive non-zero value.");
        }

        return ValidationResult.Success;
    }
}