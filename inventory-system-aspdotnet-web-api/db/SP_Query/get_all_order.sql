USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[get_all_order]    Script Date: 16-02-2024 07:13:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[get_all_order]
AS
BEGIN
    SELECT o.order_id,o.order_date,
	od.product_id,
	p.product_name,p.product_description,
	od.order_detail_quantity,od.order_detail_unit_price,
	o.order_total_amount,
	o.customer_id,
	cu.customer_name,cu.customer_email,customer_phone,cu.customer_address
    FROM orders as o
    left JOIN order_details as od
	ON o.order_id = od.order_id
	left join customers as cu
	on cu.customer_id=o.customer_id
	left join products as p
	on p.product_id=od.product_id

	order by o.created_at desc
END;
 