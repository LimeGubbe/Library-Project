namespace AdminHandlesBooks
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public User(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}