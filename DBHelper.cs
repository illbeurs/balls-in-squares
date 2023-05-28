using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace my_balls
{
	public class DBHelper
	{
		public MySqlConnection connection;
		public DBHelper()
		{
			connection = new MySqlConnection("Server=localhost;User ID=root;Password=;Database=my_balls");
			connection.Open();
			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandText = "CREATE TABLE IF NOT EXISTS `Score` (" +
				"Color varchar(15)," +
				"Score int default 0)";
			//cmd = "CREATE TABLE IF EXISTS Score";
			cmd.Connection = connection;
			cmd.ExecuteNonQuery();
		}
		public void add_elem(String color)
		{
			string cmdstr = "INSERT INTO Score (Color) " +
							  $"VALUES ('{color}')";
			MySqlCommand cmd = connection.CreateCommand();
			cmd.CommandText = cmdstr;
			cmd.ExecuteNonQuery();
		}
		public void change_score(String color, int score)
		{
			string cmdstr = "UPDATE Score " +
							  $"SET Score = {score} " +
							  $"WHERE Color = '{color}';";
			MySqlCommand cmd = connection.CreateCommand();
			cmd.CommandText = cmdstr;
			cmd.ExecuteNonQuery();
		}
		public int select_color_score(String color, object locker)
        {
			string cmdstr = "SELECT score from Score " +
							$"WHERE color = {color}";
			MySqlCommand command = new MySqlCommand($"SELECT score from Score WHERE color =@zip", connection);
			command.Parameters.AddWithValue("@zip", color);
			// int result = command.ExecuteNonQuery();
			lock (locker)
			{
				using (MySqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						return Convert.ToInt32(reader["score"]);
					}
				}
			}
			return -1;

		}

	}
}
