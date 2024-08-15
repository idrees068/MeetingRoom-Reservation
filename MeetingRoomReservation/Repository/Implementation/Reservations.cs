using MeetingRoomReservation.Models;
using MeetingRoomReservation.Repository.Interface;
using System.Data.SqlClient;

namespace MeetingRoomReservation.Repository.Implementation
{
    public class Reservations : IReservation
    {
        private readonly IConfiguration _configuration;

        public Reservations(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*  public List<ReservationModel> GetReservationList()
          {
              List<ReservationModel> reservationModel = new List<ReservationModel>();

              using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
              {
                  string query = @"SELECT r.ReservationId, r.StartTime, r.EndTime, r.IsActive, u.ID, u.Name, u.Designation, u.Email 
                                   FROM Reservation r 
                                   INNER JOIN Users u ON r.UserId = u.ID";
                  SqlCommand cmd = new SqlCommand(query, con);
                  con.Open();
                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                      while (reader.Read())
                      {
                          reservationModel.Add(new ReservationModel
                          {
                              ReservationId = (int)reader["ReservationId"],
                              StartTime = (DateTime)reader["StartTime"],
                              EndTime = (DateTime)reader["EndTime"],
                              IsActive = (bool)reader["IsActive"],
                              User = new UsersModel
                              {
                                  ID = (int)reader["ID"],
                                  Name = (string)reader["Name"],
                                  Designation = (string)reader["Designation"],
                                  Email = (string)reader["Email"]
                              }
                          });
                      }
                  }
              }

              return reservationModel;
          }
  */
        public bool AddReservation(ReservationModel reservation)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"INSERT INTO Reservation (UserId, StartTime, EndTime, IsActive) VALUES (@UserId, @StartTime, @EndTime, @IsActive)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", reservation.UserId);
                cmd.Parameters.AddWithValue("@StartTime", reservation.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", reservation.EndTime);
                cmd.Parameters.AddWithValue("@IsActive", reservation.IsActive);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool EndReservation(int reservationId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"UPDATE Reservation SET IsActive = 0 WHERE ReservationId = @ReservationId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ReservationId", reservationId);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /*        public List<ReservationModel> GetReservationList()
                {
                    List<ReservationModel> reservationModel = new List<ReservationModel>();

                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        string query = @"SELECT * FROM Reservation";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {

                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ReservationModel Reserv = new ReservationModel
                                    {
                                        ReservationId = (int)reader["ReservationId"],
                                        UserId = (int)reader["UserId"],
                                        StartTime = (DateTime)reader["StartTime"],
                                        EndTime = (DateTime)reader["EndTime"],
                                        IsActive = (bool)reader["IsActive"],
                                    };
                                    reservationModel.Add(Reserv);
                                }
                            }
                        }
                    }

                    return reservationModel;
                }*/
        public List<ReservationModel> GetReservationList()
        {
            List<ReservationModel> reservationList = new List<ReservationModel>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
            SELECT r.ReservationId, r.UserId, r.StartTime, r.EndTime, r.IsActive, u.Name, u.Designation
            FROM Reservation r
            JOIN Users u ON r.UserId = u.ID
            WHERE CAST(r.StartTime AS DATE) = CAST(GETDATE() AS DATE)";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReservationModel reservation = new ReservationModel
                        {
                            ReservationId = (int)reader["ReservationId"],
                            UserId = (int)reader["UserId"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                            IsActive = (bool)reader["IsActive"],
                            User = new UsersModel
                            {
                                Name = reader["Name"].ToString(),
                                Designation = reader["Designation"].ToString()
                            }
                        };
                        reservationList.Add(reservation);
                    }
                }
            }
            return reservationList;
        }
        /*        public ReservationModel GetReservationsByUserId(int userId)
                {
                    ReservationModel reservationList = null;
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        string query = "SELECT * FROM Reservations WHERE UserId = @userId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@userId", userId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReservationModel reservation = new ReservationModel
                                {
                                    ReservationId = (int)reader["ReservationId"],
                                    UserId = (int)reader["UserId"],
                                    StartTime = (DateTime)reader["StartTime"],
                                    EndTime = (DateTime)reader["EndTime"],
                                    IsActive = (bool)reader["IsActive"]
                                };
                            }
                        }
                    }
                    return reservationList;
                }
            }*/
        public List<ReservationModel> GetReservationsByUserId(int userId)
        {
            List<ReservationModel> reservationList = new List<ReservationModel>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = "SELECT * FROM Reservation WHERE UserId = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReservationModel reservation = new ReservationModel
                        {
                            ReservationId = (int)reader["ReservationId"],
                            UserId = (int)reader["UserId"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        reservationList.Add(reservation);
                    }
                }
            }
            return reservationList;
        }
    }
    }
