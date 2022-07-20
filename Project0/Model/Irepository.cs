namespace Wordle
{
    public interface IRepository
    {

        // we only list Method signature, not the behavior of the method.

        public User GetUser(string name);
        public void InsertUser(User user);
    }
}