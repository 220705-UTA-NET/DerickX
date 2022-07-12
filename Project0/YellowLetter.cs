namespace Wordle
{
    class YellowLetter : Letter
    {
        public YellowLetter(char letter) : base(letter) {}
        public override void DisplayLetter()
        {
            System.Console.WriteLine("\033[44m\033[37m Test \033[0m");
        }
    }
}