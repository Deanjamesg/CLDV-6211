using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class LoginModel {

        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=deanjamesgreeff;Password=Dean_cldv;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);

        public int SelectUser(string username, string password)
        {
            int userId = -1; // Default value if user is not found
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT User_ID FROM [User] WHERE User_Username = @Username AND User_Password COLLATE SQL_Latin1_General_CP1_CS_AS = @Password";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return userId;
        }

    }


}
