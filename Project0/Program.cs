namespace Wordle
{
    class Program
    {
        static void Main(string[] args)
        {
            IRepository repo = new SqlRepository("Server=tcp:dwxserver.database.windows.net,1433;Initial Catalog=MyDatabase;Persist Security Info=False;User ID=dwx;Password=Der1ck123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            Model model = new Model(repo);
            model.RunGame();
        }
    }
}