
using inventory_system_aspdotnet_web_api.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace inventory_system_aspdotnet_web_api.Repository
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcs");
        }

        public IEnumerable<GetProduct> GetAllProducts()
        {
            List<GetProduct> products = new List<GetProduct>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("get_all_product", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        GetProduct product = new GetProduct();
                        product.ProductId = Convert.ToInt32(rdr["product_id"]);
                        product.ProductName = rdr["product_name"].ToString();
                        product.ProductDescription = rdr["product_description"].ToString();
                        product.ProductQuantityInStock = Convert.ToInt32(rdr["product_quantity_in_stock"]);
                        product.ProductCostPrice = Convert.ToDecimal(rdr["product_cost_price"]);
                        product.ProductSellingPrice = Convert.ToDecimal(rdr["product_selling_price"]);
                        product.CreatedAt = Convert.ToDateTime(rdr["created_at"]);
                        product.ProductSupplierId = rdr["product_supplier_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(rdr["product_supplier_id"]);
                        product.ProductReorderPoint = rdr["product_reorder_point"] == DBNull.Value ? null : (int?)Convert.ToInt32(rdr["product_reorder_point"]);


                        products.Add(product);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return products;
        }


        public int AddOrUpdateProduct(int? productId, Product product)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var SPName = "add_product";
                    if (productId != null) SPName = "update_product";
                    SqlCommand cmd = new SqlCommand(SPName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (productId != null) cmd.Parameters.AddWithValue("@product_id", Convert.ToInt32(productId));
                    cmd.Parameters.AddWithValue("@product_name", product.ProductName);
                    cmd.Parameters.AddWithValue("@product_description", product.ProductDescription);
                    cmd.Parameters.AddWithValue("@product_quantity_in_stock", product.ProductQuantityInStock);
                    cmd.Parameters.AddWithValue("@product_cost_price", product.ProductCostPrice);
                    cmd.Parameters.AddWithValue("@product_selling_price", product.ProductSellingPrice);
                    cmd.Parameters.AddWithValue("@product_reorder_point", product.ProductReorderPoint);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int DeleteProduct(int? productId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("delete_product", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@product_id", productId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
