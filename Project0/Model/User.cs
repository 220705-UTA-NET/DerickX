namespace Wordle
{
    public class User
    {
        public string name;
        public int streak;
        public int wins;
        public int losses;
        public int[] guessNums = new int[6];
        public User(string name, int streak=0, int wins=0, int losses=0, int[]? guessNums=null)
        {
            this.name = name;
            this.streak = streak;
            this.wins = wins;
            this.losses = losses;
            if (guessNums == null) 
            {
                guessNums = new int[] {0, 0, 0, 0, 0, 0};
            }
            else
            {
                this.guessNums = guessNums;
            }
        }
    }
}