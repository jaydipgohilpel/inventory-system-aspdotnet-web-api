using inventory_system_aspdotnet_web_api.Models;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_aspdotnet_web_api.Repository
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcs");
        }
        public IEnumerable<GetOrder> GetAllOrders()
        {
            List<GetOrder> orders = new List<GetOrder>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("get_all_order", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        GetOrder order = new GetOrder();
                        order.OrderId = Convert.ToInt32(rdr["order_id"]);
                        order.ProductId = Convert.ToInt32(rdr["product_id"]);
                        order.OrderDate = Convert.ToDateTime(rdr["order_date"]);
                        order.CustomerId = Convert.ToInt32(rdr["customer_id"]);
                        order.ProductName = rdr["product_name"].ToString();
                        order.ProductDescription = rdr["product_description"].ToString();
                        order.OrderDetailQuantity = Convert.ToInt32(rdr["order_detail_quantity"]);
                        order.OrderDetailUnitPrice= Convert.ToInt32(rdr["order_detail_unit_price"]);
                        order.OrderTotalAmount = Convert.ToDecimal(rdr["order_total_amount"]);
                        order.CustomerName = rdr["customer_name"].ToString();
                        order.CustomerEmail = rdr["customer_email"].ToString();
                        order.CustomerAddress = rdr["customer_address"].ToString();
                        order.CustomerPhone = rdr["customer_phone"].ToString();
                       

                        orders.Add(order);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return orders;
        }

        public int AddOrUpdateOrder(int? orderId, AddOrder order)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    
                   
                   
                    var SPName = "add_order";
                    if (orderId != null) SPName = "update_order";
                    SqlCommand cmd = new SqlCommand(SPName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (orderId != null) cmd.Parameters.AddWithValue("@OrderId", Convert.ToInt32(orderId));
                    cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    cmd.Parameters.AddWithValue("@ProductId", Convert.ToInt32(order.ProductId));
                    cmd.Parameters.AddWithValue("@OrderDetailQuantity", Convert.ToInt32(order.OrderDetailQuantity));
                    cmd.Parameters.AddWithValue("@OrderDetailUnitPrice", Convert.ToInt32(order.OrderDetailUnitPrice));
                    cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(order.OrderDate));
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery(); // Assuming the first parameter returned is the OrderId
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateOrder(int orderId, Order order)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("update_order", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    cmd.Parameters.AddWithValue("@customer_id", order.CustomerId);
                    cmd.Parameters.AddWithValue("@order_date", order.OrderDate);
                    cmd.Parameters.AddWithValue("@order_status", order.OrderStatus);
                    cmd.Parameters.AddWithValue("@order_total_amount", order.OrderTotalAmount);
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

        public int DeleteOrder(int orderId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("delete_order", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
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
