namespace Wordle
{
    class RedLetter : Letter
    {
        public RedLetter(char letter) : base(letter) {}
        public override void DisplayLetter()
        {
            System.Console.WriteLine("\031[44m\031[37m Test \031[0m");
        }
    }
}