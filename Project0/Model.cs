namespace Wordle
{
    class Model
    {
        private HashSet<string> wordList;

        public Model() {
            wordList = new HashSet<string>(File
                .ReadLines(@"./wordlist.txt")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line)), StringComparer.OrdinalIgnoreCase);
            OutputList();
        }

        public void OutputList() {
            Console.WriteLine(String.Join(",", wordList));
        }

    }
}