namespace Wordle
{
    public interface IRepository
    {
        public User GetUser(string name);
        public void InsertUser(User user);
    }
}