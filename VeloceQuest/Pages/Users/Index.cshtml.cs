using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace VeloceQuest.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<UserInfo> listUsers = new List<UserInfo>();
        public string successMessage = "";
		public string errorMessage = "";
		public void OnGet()
        {
            try
            {
                String connectionString = "Data Source = localhost; Initial Catalog = usersnote; Integrated Security = True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM users";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.id = "" + reader.GetInt32(0);
                                userInfo.name = reader.GetString(1);
                                userInfo.surname = reader.GetString(2);
                                userInfo.birthdate = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                                userInfo.sex = reader.GetString(4);
                                userInfo.phone = reader.GetString(5);
                                userInfo.position = reader.GetString(6);
                                userInfo.shoesize = reader.GetString(7);

                                listUsers.Add(userInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
		public void OnGetRaport()
		{
            OnGet();
			try
			{
				String connectionString = "Data Source = localhost; Initial Catalog = usersnote; Integrated Security = True;";
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("yyyy_MM_dd HH_mm_ss");
                string path = formattedDateTime + ".txt";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM users";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							using (StreamWriter writetext = new StreamWriter(path, true))
							{
								writetext.WriteLine("Birthdate\tAge\tSex\tTitle\tName and surname\n");
							}
							while (reader.Read())
							{
								int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
								int dob = int.Parse(reader.GetDateTime(3).ToString("yyyyMMdd"));
								int age = (now - dob) / 10000;
                                string raportLine = reader.GetDateTime(3).ToString("dd/MM/yyyy") + "\t" + age.ToString() + "\t" + reader.GetString(4) + "\t"; 
								if (reader.GetString(4).ToLower() == "male")
                                {
                                    raportLine += "Mr\t";
                                }
                                else
                                {
                                    raportLine += "Mrs\t";
                                }
                                raportLine += reader.GetString(1) + " " + reader.GetString(2);
                                
                                using(StreamWriter writetext = new StreamWriter(path, true))
                                {
                                    writetext.WriteLine(raportLine);
                                }
							}
						}
					}
				}
                successMessage = "Raport generated with name: " + path + " in project folder";
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
                return;
			}
		}
	}
    public class UserInfo
    {
        public String id;
        public String name;
        public String surname;
        public String birthdate;
        public String sex;
        public String phone;
        public String position;
        public String shoesize;
    }
}
