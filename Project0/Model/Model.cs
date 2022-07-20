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
            // Setup current user.
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

            // Game Loop.
            View.StartPrompt();
            bool play = true;
            do
            {
                // Variable initialization that needs to be done before every new game.
                SelectWord();
                won = false;
                numGuess = 0;
                Array.Clear(validity, 0, validity.Length);
            
                while (numGuess < maxGuess && !won)
                {
                    MainLoop();
                }

                // Stats get pushed to the database after every game.
                UpdateStats(won);
                repo.InsertUser(currUser);
                View.DisplayStats(currUser);

                // Asking to play again.
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
            // Check validity of guess, then evaluate.
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
            // Marks if letter was already used to grant yellow status to a letter in the guess.
            bool[] marked = {false, false, false, false, false};
            
            for (int i = 0; i < WORD_SIZE; i++)
            {
                // Letter is in correct spot.
                if (guess[i] == word[i]) {
                    validity[i] = new GreenLetter(guess[i]);
                }
                else
                {
                    // See if letter has an unmarked version in main word.
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
                }
            }

            // Check if guess was a win.
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