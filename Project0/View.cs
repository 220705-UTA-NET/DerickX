namespace Wordle
{
    static class View
    {
        public static void StartPrompt()
        {
            System.Console.WriteLine("Welcome to Wordle!");
        }

        public static void MainPrompt(uint numGuess)
        {
            System.Console.Write($"{numGuess}/6 ");
        }

        public static void EndPrompt(bool won)
        {
            if (won) {
                System.Console.WriteLine("Congratulations, Play Again?");
            }
            else
            {
                System.Console.WriteLine("Sorry, Try Again?");
            }
        }
        
        public static void DisplayResult(string guess, Letter[] validity)
        {
            for (int i = 0; i < Model.WORD_SIZE; i++)
            {
                validity[i].DisplayLetter();
            }
            System.Console.WriteLine();
        }

        public static void OutOfRangeMessage(ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}