using System;
using System.Collections.Generic;

namespace inventory_system_aspdotnet_web_api.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int OrderDetailQuantity { get; set; }

    public decimal OrderDetailUnitPrice { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual GetProductWithOtherDetails OrderNavigation { get; set; } = null!;
}
