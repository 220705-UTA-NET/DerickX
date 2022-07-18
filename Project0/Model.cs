namespace Wordle
{
    class Model
    {
        public const uint WORD_SIZE = 5;
        private string word;
        private Controller controller;
        private uint numGuess;
        private uint maxGuess;
        private string guess;
        private Letter[] validity;
        private bool won;
        public HashSet<string> wordList;
        public Model(uint maxGuess=6) {
            wordList = new HashSet<string>(File
                .ReadLines(@"./wordlist.txt")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line)), StringComparer.OrdinalIgnoreCase);
            this.controller = new Controller();
            this.numGuess = 0;
            this.maxGuess = maxGuess;
            this.validity = new Letter[WORD_SIZE];
            this.won = false;
        }

        public void RunGame()
        {
            View.StartPrompt();
            
            bool play = true;
            do
            {
                SelectWord();
                won = false;
                numGuess = 0;
                Array.Clear(validity, 0, validity.Length);
            
                while (numGuess < maxGuess && !won)
                {
                    MainLoop();
                }

                View.EndPrompt(won);
                bool errored = true;
                while (errored) {
                    try 
                    {
                        play = controller.PlayAgain();
                        errored = false;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        View.OutOfRangeMessage(ex);
                        errored = true;
                    }
                }
            } while (play);
            
        }

        private void MainLoop()
        {
            View.MainPrompt(numGuess);
            try
            {
                string guess = controller.GetGuess(wordList);
                CheckGuess(guess);
                numGuess++;
                View.DisplayResult(guess, validity);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                View.OutOfRangeMessage(ex);
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
                if (!(validity[i] is GreenLetter)) {
                    return;
                }
            }
            won = true;
        }

    }
}