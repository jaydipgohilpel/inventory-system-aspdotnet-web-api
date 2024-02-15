using inventory_system_aspdotnet_web_api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace inventory_system_aspdotnet_web_api.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;
        private readonly string _EncryptionKey;
        IConfiguration _config;

        public UserRepository(IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = configuration.GetConnectionString("dbcs");
            _EncryptionKey = configuration.GetSection("EncryptionKey").Value;
        }


        public int AddOrUpdateUser(int? userId, Users user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    var SPName = "add_user_registration";
                    if (userId != null) SPName = "update_customer";
                    SqlCommand cmd = new SqlCommand(SPName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (userId != null) cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(userId));
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    string encryptedPassword = Encrypt(user.Password, _EncryptionKey);
                    cmd.Parameters.AddWithValue("@password", encryptedPassword);

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

        public string LoginUser(LoginUsers user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {


                    SqlCommand cmd = new SqlCommand("get_user_login", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    //method 1 to check password
                    string encryptedPassword = Encrypt(user.Password, _EncryptionKey);
                    cmd.Parameters.AddWithValue("@password", encryptedPassword);
                    con.Open();

                    // Execute the command and retrieve a single result
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        ReturnLoginUsers u = new ReturnLoginUsers();
                        u.UserId = Convert.ToInt32(rdr["userId"]);
                        u.Name = rdr["name"].ToString();
                        u.Email = rdr["email"].ToString();

                        //method 2 to check password
                        var DecryptPassword = Decrypt(rdr["password"].ToString(), _EncryptionKey);
                        if (u != null && DecryptPassword == user.Password && user.Email == u.Email)
                        {
                            var token = GenerateToken(u);
                            return token;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        return null;
                    }

                    con.Close();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static string Encrypt(string plainText, string key)
        {
            byte[] iv = new byte[16]; // Initialization Vector
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = GetValidKey(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }
                    array = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(array);
        }

        public static byte[] GetValidKey(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                // AES supports key sizes of 128, 192, or 256 bits (16, 24, or 32 bytes respectively)
                byte[] validKey = new byte[32]; // 256 bits
                Array.Copy(hash, validKey, Math.Min(hash.Length, validKey.Length));
                return validKey;
            }
        }

        public static string Decrypt(string cipherText, string key)
        {
            byte[] iv = new byte[16]; // Initialization Vector
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GetValidKey(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        // Define your custom claim types
        const string CustomNameClaimType = "name";
        const string CustomEmailClaimType = "email";
        const string CustomUserIdClaimType = "userId";
        private string GenerateToken(ReturnLoginUsers user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
         {
                new Claim(CustomNameClaimType, user.Name.Trim()),
                new Claim(CustomEmailClaimType, user.Email.Trim()),
                new Claim(CustomUserIdClaimType, user.UserId.ToString())
        };


            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(24), // Set expixpiration time here
            signingCredentials: credentials
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }



}