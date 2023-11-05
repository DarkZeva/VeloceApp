using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace VeloceQuest.Pages.Users
{
    public class EditUserModel : PageModel
    {
		public UserInfo userInfo = new UserInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public DateTime currentTime = DateTime.Now;
		public string today = "";
		
		public void OnGet()
        {
            String id = Request.Query["id"];
			today = currentTime.ToString("yyyy-MM-dd");
            try
            {
				String connectionString = "Data Source = localhost; Initial Catalog = usersnote; Integrated Security = True;";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM users WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								userInfo.id = "" + reader.GetInt32(0);
								userInfo.name = reader.GetString(1);
								userInfo.surname = reader.GetString(2);
								userInfo.birthdate = reader.GetDateTime(3).ToString("dd/MM/yyyy");
								userInfo.sex = reader.GetString(4);
								userInfo.phone = reader.GetString(5);
								userInfo.position = reader.GetString(6);
								userInfo.shoesize = reader.GetString(7);
							}
						}
					}
				}
			}
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }
        }
        public void OnPost() 
        {
			userInfo.id = Request.Form["id"];
			userInfo.name = Request.Form["name"];
			userInfo.surname = Request.Form["surname"];
			userInfo.sex = Request.Form["sex"];
			userInfo.phone = Request.Form["phone"];
			userInfo.position = Request.Form["position"];
			userInfo.shoesize = Request.Form["shoesize"];
			userInfo.birthdate = Request.Form["birthdate"];

			if (userInfo.name.Length < 2)
			{
				errorMessage = "Given name is too short";
				return;
			}
			else if (userInfo.name.Length > 50)
			{
				errorMessage = "Name cannot exeed 50 characters";
				return;
			}

			if (userInfo.surname.Length < 2)
			{
				errorMessage = "Given surname is too short";
				return;
			}
			else if (userInfo.surname.Length > 150)
			{
				errorMessage = "Surname cannot exeed 150 characters";
				return;
			}
			if (userInfo.sex.Length == 0)
			{
				errorMessage = "Sex cannot be empty";
				return;
			}
			if (userInfo.phone.Length > 15)
			{
				errorMessage = "Phone number incorrect";
				return;
			}
			else if (userInfo.sex.ToLower() != "male" && userInfo.sex.ToLower() != "female")
			{
				errorMessage = "Incorrect sex. Only male/female is accepted";
				return;
			}
			if (int.Parse(userInfo.shoesize) > 75 || int.Parse(userInfo.shoesize) < 1)
			{
				errorMessage = "Shoe size incorrect";
				return;
			}
			if (userInfo.birthdate.Length == 0)
			{
				errorMessage = "Birthdate cannot be empty";
				return;
			}

			try
			{
				String connectionString = "Data Source=localhost;Initial Catalog=usersnote;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE users SET name=@name, surname=@surname, birthdate=@birthdate, sex=@sex, " +
								 "phone=@phone, position=@position, shoesize=@shoesize WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", userInfo.id);
						command.Parameters.AddWithValue("@name", userInfo.name);
						command.Parameters.AddWithValue("@surname", userInfo.surname);
						command.Parameters.AddWithValue("@birthdate", userInfo.birthdate);
						command.Parameters.AddWithValue("@sex", userInfo.sex.ToLower());
						command.Parameters.AddWithValue("@phone", userInfo.phone);
						command.Parameters.AddWithValue("@position", userInfo.position);
						command.Parameters.AddWithValue("@shoesize", userInfo.shoesize);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception exception) 
			{ 
				errorMessage = exception.Message;
				return;
			}

			Response.Redirect("/Users/Index");
		}
    }
}
