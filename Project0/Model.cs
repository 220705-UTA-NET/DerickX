const unsigned_int WORD_SIZE = 5;

namespace Wordle
{
    class Model
    {
        private HashSet<string> wordList;
        private string word;
        private Controller controller;
        private View view;
        private int numGuess;
        private int maxGuess;
        private string guess;
        private bool[] validity {public set;}        
        public Model(Controller controller, View view, int maxGuess=6) {
            wordList = new HashSet<string>(File
                .ReadLines(@"./wordlist.txt")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line)), StringComparer.OrdinalIgnoreCase);
            this.controller = controller;
            this.view = view;
            this.numGuess = 0;
            this.maxGuess = maxGuess;
            this.won = false;
            bool[] validity = new bool[WORD_SIZE];
        }

        public void RunGame()
        {
            view.StartPrompt();
            //System.Console.WriteLine("Welcome to Wordle!");
            SelectWord();
        }

        private void MainLoop()
        {
            view.MainPrompt();
            /*
            System.Console.WriteLine($"{numGuess}/6");
            System.Console.WriteLine("Please guess a 5 letter word: ");
            */
            
            string guess = controller.GetGuess();

            if (!Regex.IsMatch(guess, @"^[a-zA-Z]+$"))
            {
                throw new ArgumentOutOfRangeException(nameof(guess), "Guess must only be alphabetic characters");
            }
            else if (guess.size() != 5)
            {
                throw new ArgumentOutOfRangeException(nameof(guess), "Guess must be a 5-letter word");
            }
            try
            {
                validity = CheckGuess(guess);
                numGuess++;
                view.DisplayResult(guess, validity);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                view.InvalidGuess(ex);
                // Console.WriteLine(ex.Message);
            }
        }

        private void SelectWord()
        {

        }
        private void CheckGuess(string guess)
        {
            
        }

    }
}