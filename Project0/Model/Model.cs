namespace Wordle
{
    class Model
    {
        private string word;
        private uint numGuess;
        private const uint maxGuess = 6;
        private Letter[] validity;
        private bool won;
        private IRepository repo;
        public HashSet<string> wordList;
        public User? currUser;
        public const uint WORD_SIZE = 5;
        
        public Model(IRepository repo, uint maxGuess=6) {
            wordList = new HashSet<string>(File
                .ReadLines(@"./wordlist.txt")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line)), StringComparer.OrdinalIgnoreCase);
            this.numGuess = 0;
            this.validity = new Letter[WORD_SIZE];
            this.won = false;
            this.repo = repo;
        }

        public void RunGame()
        {
            View.AskName();
            bool userError = true;
            while (userError)
            {
                try
                {
                    currUser = repo.GetUser(Controller.CheckUser());
                    userError = false;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    View.OutOfRangeMessage(ex);
                }
            }

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

                UpdateStats(won);
                repo.InsertUser(currUser);
                View.DisplayStats(currUser);

                View.EndPrompt(won);
                bool endError = true;
                while (endError) {
                    try 
                    {
                        play = Controller.PlayAgain();
                        endError = false;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        View.OutOfRangeMessage(ex);
                    }
                }
            } while (play);
            
        }

        private void MainLoop()
        {
            View.MainPrompt(numGuess);
            try
            {
                string guess = Controller.GetGuess(wordList);
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

        private void UpdateStats(bool won)
        {
            if (won)
            {
                currUser.wins++;
                currUser.streak++;
                currUser.guessNums[numGuess - 1]++;
            }
            else
            {
                currUser.losses++;
                currUser.streak = 0;
            }
        }

    }
}