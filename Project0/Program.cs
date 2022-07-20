namespace Wordle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare Objects.
            IRepository repo = new SqlRepository("Your Connection String Here");
            Model model = new Model(repo);
            // Game Loop?
            // model.RunGame();
            model.RunGame();
        }
    }
}