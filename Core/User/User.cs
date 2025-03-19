namespace Core.User
{
    public class User {
        public string ID { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }

        public User(string ID, string name, string email, string password)
        {
            this.ID = ID;
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }
    }

}
