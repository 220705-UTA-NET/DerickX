using System.Data.SqlClient;

namespace Wordle
{
    public class SqlRepository : IRepository //by extending the interface, this class agrees to fulfill all the methods of the interface.
    {
        // Fields

        // your connection string has all the details needed to connect to a database
        private readonly string connectionString;

        //the readonly modifier allows us to set a value in the constructor, then prevents modification.



        // Constuctor
        public SqlRepository( string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        //Methods

        public void InsertUser(User user)
        {
            // a SQLConnection object is created to connect to the database, and is provided the connection string
            using SqlConnection connection = new SqlConnection(this.connectionString);
    
            connection.Open();

            SqlCommand check_ID = new SqlCommand("SELECT * FROM Wordle.Users WHERE Name = @Name", connection);
            check_ID.Parameters.AddWithValue("@Name", user.name);
            SqlDataReader reader = check_ID.ExecuteReader();

            if(reader.HasRows)
            {
                reader.Close();

                string cmdText = "UPDATE Wordle.Users SET Streak = @Streak, Wins = @Wins, Losses = @Losses, Guess1 = @Guess1, Guess2 = @Guess2, Guess3 = @Guess3, Guess4 = @Guess4, Guess5 = @Guess5, Guess6 = @Guess6 WHERE Name=@Name";

                using SqlCommand cmd = new SqlCommand(cmdText, connection);

                cmd.Parameters.AddWithValue("@Streak", user.streak);
                cmd.Parameters.AddWithValue("@Wins", user.wins);
                cmd.Parameters.AddWithValue("@Losses", user.losses);
                cmd.Parameters.AddWithValue("@Guess1", user.guessNums[0]);
                cmd.Parameters.AddWithValue("@Guess2", user.guessNums[1]);
                cmd.Parameters.AddWithValue("@Guess3", user.guessNums[2]);
                cmd.Parameters.AddWithValue("@Guess4", user.guessNums[3]);
                cmd.Parameters.AddWithValue("@Guess5", user.guessNums[4]);
                cmd.Parameters.AddWithValue("@Guess6", user.guessNums[5]);
                cmd.Parameters.AddWithValue("@Name", user.name);
                cmd.ExecuteNonQuery();
            }
            else
            {
                reader.Close();

                string cmdText =
                @"INSERT INTO Wordle.Users (Name, Streak, Wins, Losses, Guess1, Guess2, Guess3, Guess4, Guess5, Guess6)
                VALUES
                (@Name, @Streak, @Wins, @Losses, @Guess1, @Guess2, @Guess3, @Guess4, @Guess5, @Guess6)";

                using SqlCommand cmd = new SqlCommand(cmdText, connection);

                cmd.Parameters.AddWithValue("@Name", user.name);
                cmd.Parameters.AddWithValue("@Streak", user.streak);
                cmd.Parameters.AddWithValue("@Wins", user.wins);
                cmd.Parameters.AddWithValue("@Losses", user.losses);
                cmd.Parameters.AddWithValue("@Guess1", user.guessNums[0]);
                cmd.Parameters.AddWithValue("@Guess2", user.guessNums[1]);
                cmd.Parameters.AddWithValue("@Guess3", user.guessNums[2]);
                cmd.Parameters.AddWithValue("@Guess4", user.guessNums[3]);
                cmd.Parameters.AddWithValue("@Guess5", user.guessNums[4]);
                cmd.Parameters.AddWithValue("@Guess6", user.guessNums[5]);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public User GetUser(string name)
        {
            User tmpUser = new User(name);

            using SqlConnection connection = new SqlConnection(this.connectionString);
            connection.Open();

            string cmdText = @"SELECT * FROM Wordle.Users WHERE Name = @Name;";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.Parameters.AddWithValue("@Name", name);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int streak = reader.GetInt32(1);
                int wins = reader.GetInt32(2);
                int losses = reader.GetInt32(3);
                int[] guessNums = {reader.GetInt32(4),
                                   reader.GetInt32(5),
                                   reader.GetInt32(6),
                                   reader.GetInt32(7),
                                   reader.GetInt32(8),
                                   reader.GetInt32(9)};

                tmpUser = new User(name, streak, wins, losses, guessNums);
            }

            return tmpUser;
        }
    }
}