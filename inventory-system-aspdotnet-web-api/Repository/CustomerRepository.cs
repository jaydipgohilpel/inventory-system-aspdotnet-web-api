using inventory_system_aspdotnet_web_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;

namespace inventory_system_aspdotnet_web_api.Repository
{

    public class CustomerRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private  string _UserId;

        public CustomerRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("dbcs");
            _httpContextAccessor = httpContextAccessor;
           
        }


        public string getUserId()
        {
            return _httpContextAccessor.HttpContext.Items["userId"].ToString();
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
                        lastCustomer.Add(ReadData(rdr));
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

        public GetCustomer AddOrUpdateCustomer(int? customerId, Customer customer)
        {
            try
            {
                _UserId = getUserId();
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
                    cmd.Parameters.AddWithValue("@userId", _UserId);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    GetCustomer newCustomer = new GetCustomer();
                    if (rdr.Read())
                    {
                        newCustomer = ReadData(rdr);
                        con.Close();
                    }
                    return newCustomer;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public GetCustomer ReadData(SqlDataReader rdr)
        {
            GetCustomer newCustomer = new GetCustomer();
            newCustomer.CustomerId = Convert.ToInt32(rdr["customer_id"]);
            newCustomer.CustomerName = rdr["customer_name"].ToString();
            newCustomer.CustomerEmail = rdr["customer_email"].ToString();
            newCustomer.CustomerAddress = rdr["customer_address"].ToString();
            newCustomer.CustomerPhone = rdr["customer_phone"].ToString();
            newCustomer.CreatedAt = Convert.ToDateTime(rdr["created_at"]);
            if (!rdr.IsDBNull(rdr.GetOrdinal("updated_at")))
               newCustomer.UpdatedAt = Convert.ToDateTime(rdr["updated_at"]);
            if (!rdr.IsDBNull(rdr.GetOrdinal("userId")))
                newCustomer.UserId = Convert.ToInt32(rdr["userId"]);
            return newCustomer;
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
