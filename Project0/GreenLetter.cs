namespace Wordle
{
    class GreenLetter : Letter
    {
        public GreenLetter(char letter) : base(letter) {}
        public override void DisplayLetter()
        {
            System.Console.WriteLine("\032[44m\032[37m Test \032[0m");
        }
    }
}