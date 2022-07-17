namespace Wordle
{
    class Model
    {
        private const uint WORD_SIZE = 5;
        private string word;
        private Controller controller;
        private View view;
        private int numGuess;
        private int maxGuess;
        private string guess;
        private Letter[] validity;
        private bool won;
        public HashSet<string> wordList;
        public Model(int maxGuess=6) {
            wordList = new HashSet<string>(File
                .ReadLines(@"./wordlist.txt")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line)), StringComparer.OrdinalIgnoreCase);
            this.controller = new Controller();
            this.view = new View();
            this.numGuess = 0;
            this.maxGuess = maxGuess;
            this.validity = new Letter[WORD_SIZE];
            this.won = false;
        }

        public void RunGame()
        {
            // view.StartPrompt();
            // System.Console.WriteLine("Welcome to Wordle!");
            SelectWord();
            
            while (numGuess < maxGuess && !won)
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
            try
            {
                string guess = controller.GetGuess(wordList);
                CheckGuess(guess);
                numGuess++;
                // view.DisplayResult(guess, validity);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // view.InvalidGuess(ex);
                Console.WriteLine(ex.Message);
            }
        }

        private void SelectWord()
        {
            Random random = new Random();
            word = wordList.ElementAt(random.Next(wordList.Count));
            System.Console.WriteLine(word);
        }
        private void CheckGuess(string guess)
        {
            bool[] marked = {false, false, false, false, false};
            for (int i = 0; i < WORD_SIZE; i++)
            {
                if (guess[i] == word[i]) {
                    validity[i] = new GreenLetter(guess[i]);
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
                            if (guess[index] != word[index] && !marked[index])
                            {
                                validity[i] = new YellowLetter(guess[i]);
                                marked[index] = true;
                                set = true;
                                break;
                            }
                        } 
                        else {
                            validity[i] = new RedLetter(guess[i]);
                            set = true;
                            break;
                        }
                    }
                    if (!set)
                    {
                        validity[i] = new RedLetter(guess[i]);
                    }
                }
            }

            for (int i = 0; i < WORD_SIZE; i++)
            {
                validity[i].DisplayLetter();
            }
            System.Console.WriteLine();

            for (int i = 0; i < WORD_SIZE; i++)
            {
                if (!(validity[i] is GreenLetter)) {
                    return;
                }
            }
            won = true;
        }

    }
}