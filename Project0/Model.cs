namespace Wordle
{
    class Model
    {
        private const uint WORD_SIZE = 5;
        private HashSet<string> wordList;
        private string word;
        private Controller controller;
        private View view;
        private int numGuess;
        private int maxGuess;
        private string guess;
        public enum guessStates
        {
            Correct,
            WrongPosition,
            Incorrect
        }
        private guessStates[] validity;
        private bool won;        
        public Model(int maxGuess=6) {
            wordList = new HashSet<string>(File
                .ReadLines(@"./wordlist.txt")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line)), StringComparer.OrdinalIgnoreCase);
            this.controller = new Controller();
            this.view = new View();
            this.numGuess = 0;
            this.maxGuess = maxGuess;
            this.validity = new guessStates[WORD_SIZE];
            this.won = false;
        }

        public void RunGame()
        {
            // view.StartPrompt();
            // System.Console.WriteLine("Welcome to Wordle!");
            SelectWord();
            
            while (numGuess < maxGuess || !won)
            {
                MainLoop();
            }

            // view.EndPrompt(won);
        }

        private void MainLoop()
        {
            // view.MainPrompt();
            /*
            System.Console.WriteLine($"{numGuess}/6");
            System.Console.WriteLine("Please guess a 5 letter word: ");
            */
            /*
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
                // view.DisplayResult(guess, validity);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // view.InvalidGuess(ex);
                // Console.WriteLine(ex.Message);
            }
            */
        }

        public void SelectWord()
        {
            Random random = new Random();
            word = wordList.ElementAt(random.Next(wordList.Count));
        }
        public void CheckGuess(string guess)
        {
            bool[] marked = {false, false, false, false, false};
            for (int i = 0; i < WORD_SIZE; i++)
            {
                if (guess[i] == word[i]) {
                    validity[i] = guessStates.Correct;
                }
                else
                {
                    string copy = word;
                    bool set = false;
                    int index = -1;
                    while (!set)
                    {
                        index = copy.IndexOf(guess[i], index + 1);
                        if (index != -1) {
                            if (guess[i] != word[i] && !marked[index])
                            {
                                validity[i] = guessStates.WrongPosition;
                                marked[index] = true;
                                set = true;
                                break;
                            }
                        } 
                        else {
                            validity[i] = guessStates.Incorrect;
                            set = true;
                            break;
                        }
                    }
                    if (!set)
                    {
                        validity[i] = guessStates.Incorrect;
                    }
                }
            }

            System.Console.WriteLine(word);
            for (int i = 0; i < WORD_SIZE; i++)
            {
                System.Console.WriteLine(validity[i]);
            }
        }

    }
}