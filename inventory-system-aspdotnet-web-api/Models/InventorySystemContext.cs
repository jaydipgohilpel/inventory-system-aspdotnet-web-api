using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace inventory_system_aspdotnet_web_api.Models;

public partial class InventorySystemContext : DbContext
{
    public InventorySystemContext()
    {
    }

    public InventorySystemContext(DbContextOptions<InventorySystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=inventory_system;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GetCustomer>(entity =>
        {
            entity.ToTable("customers");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(100)
                .HasColumnName("customer_address");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(30)
                .HasColumnName("customer_email");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(40)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(13)
                .HasColumnName("customer_phone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(12)
                .HasColumnName("order_status");
            entity.Property(e => e.OrderTotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("order_total_amount");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_orders_customers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("order_details");

            entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderDetailQuantity).HasColumnName("order_detail_quantity");
            entity.Property(e => e.OrderDetailUnitPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("order_detail_unit_price");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_details_orders");

            entity.HasOne(d => d.OrderNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_order_details_products");
        });

        modelBuilder.Entity<GetProduct>(entity =>
        {
            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductCostPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("product_cost_price");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(100)
                .HasColumnName("product_description");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductQuantityInStock).HasColumnName("product_quantity_in_stock");
            entity.Property(e => e.ProductReorderPoint).HasColumnName("product_reorder_point");
            entity.Property(e => e.ProductSellingPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("product_selling_price");
            entity.Property(e => e.ProductSupplierId).HasColumnName("product_supplier_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
