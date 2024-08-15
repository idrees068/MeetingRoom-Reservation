using MeetingRoomReservation.Models;
using MeetingRoomReservation.Repository.Interface;
using System.Data.SqlClient;

namespace MeetingRoomReservation.Repository.Implementation
{
    public class Users : IUsers
    {
        private readonly IConfiguration _configuration;

        public Users(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool RegisterUser(UsersModel user)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                string query = @"INSERT INTO Users (Name, Designation, Email,PasswordHash )
                    VALUES(@Name, @Designation, @Email, @PasswordHash); ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Name", (object)user.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)user.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PasswordHash", (object)user.Password ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Designation", (object)user.Designation ?? DBNull.Value);


                    try
                    {

                        int rowAffected = command.ExecuteNonQuery();

                        if (rowAffected > 0)
                        {
                            connection.Close();
                            return true;
                        }

                    }
                    catch
                    {
                        connection.Close();
                        return false;
                    }
                }
            }

            return false;
        }

        public UsersModel GetUserByEmailAndPassword(string email, string password)
        {

            UsersModel user = null;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                string query = @"SELECT ID, Name, Designation, Email FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PasswordHash", password);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new UsersModel
                        {
                            ID = (int)reader["ID"],
                            Name = (string)reader["Name"],
                            Designation = (string)reader["Designation"],
                            Email = (string)reader["Email"]
                        };
                    }
                }
            }

            return user;
        }

        public UsersModel GetUserById(int userId)
        {
            UsersModel user = null;
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"SELECT * FROM Users WHERE ID = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UsersModel
                            {
                                ID = (int)reader["ID"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["PasswordHash"].ToString(),
                                Designation = reader["Designation"].ToString(),
                            };
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
            }

            return user;
        }
        public List<UsersModel> GetAllUsers()
        {
            List<UsersModel> users = new List<UsersModel>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"SELECT ID, Name, Designation FROM Users";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                          UsersModel  user = new UsersModel
                            {
                                ID = (int)reader["ID"],
                                Name = reader["Name"].ToString(),
                                Designation = reader["Designation"].ToString(),
                            };
                            users.Add(user);
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
            }

            return users;
        }
    }
}
