namespace Wordle
{
    abstract class Letter 
    {
        protected char letter;

        public Letter(char letter)
        {
            this.letter = letter;
        } 
        
        abstract public void DisplayLetter();
    }
}