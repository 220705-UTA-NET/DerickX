namespace Wordle 
{
    class Controller
    {
        public string GetGuess(HashSet<string> wordList)
        {
            string guess = System.Console.ReadLine();
            if (!System.Text.RegularExpressions.Regex.IsMatch(guess, @"^[a-zA-Z]+$"))
            {
                throw new ArgumentOutOfRangeException(nameof(guess), "Guess must only be alphabetic characters");
            }
            else if (guess.Length != 5)
            {
                throw new ArgumentOutOfRangeException(nameof(guess), "Guess must be a 5-letter word");
            }
            else if (!wordList.Contains(guess))
            {
                throw new ArgumentOutOfRangeException(nameof(guess), "Guess not in word list");
            }
            return guess;
        }
    }
}