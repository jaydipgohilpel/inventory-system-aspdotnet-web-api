using inventory_system_aspdotnet_web_api.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_aspdotnet_web_api.Repository
{

    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcs");
        }

        public IEnumerable<GetCustomer> GetAllCustomer()
        {
            List<GetCustomer> lastCustomer = new List<GetCustomer>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("get_all_customer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        GetCustomer customer = new GetCustomer();
                        customer.CustomerId = Convert.ToInt32(rdr["customer_id"]);
                        customer.CustomerName = rdr["customer_name"].ToString();
                        customer.CustomerEmail = rdr["customer_email"].ToString();
                        customer.CustomerAddress = rdr["customer_address"].ToString();
                        customer.CustomerPhone = rdr["customer_phone"].ToString();
                        customer.CreatedAt = Convert.ToDateTime(rdr["created_at"]);

                        lastCustomer.Add(customer);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return lastCustomer;
        }

        public int AddOrUpdateCustomer(int? customerId, Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var SPName = "add_customer";
                    if (customerId != null) SPName = "update_customer";
                    SqlCommand cmd = new SqlCommand(SPName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (customerId != null) cmd.Parameters.AddWithValue("@customer_id", Convert.ToInt32(customerId));
                    cmd.Parameters.AddWithValue("@customer_name", customer.CustomerName);
                    cmd.Parameters.AddWithValue("@customer_email", customer.CustomerEmail);
                    cmd.Parameters.AddWithValue("@customer_address", customer.CustomerAddress);
                    cmd.Parameters.AddWithValue("@customer_phone", customer.CustomerPhone);
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


        public int DeleteCustomer(int? customerId)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {

                    SqlCommand cmd = new SqlCommand("delete_customer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customer_id", customerId);
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
