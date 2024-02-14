using System;
using System.Collections.Generic;

namespace inventory_system_aspdotnet_web_api.Models;

public partial class Product
{
  
    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; } = "";

    public int ProductQuantityInStock { get; set; }

    public decimal ProductCostPrice { get; set; }

    public decimal ProductSellingPrice { get; set; } = 0;

    public int? ProductReorderPoint { get; set; } = null!;

}

public partial class GetProduct: Product
{
    public int ProductId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ProductSupplierId { get; set; } = null!;

}

public partial class GetProductWithOtherDetails : GetProduct
{
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}


