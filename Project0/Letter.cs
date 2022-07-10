namespace Wordle
{
    abstract class Letter 
    {
        private char letter;

        public Letter(char letter)
        {
            this.letter = letter;
        } 
        
        abstract public void DisplayLetter();
    }
}