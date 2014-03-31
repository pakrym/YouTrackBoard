namespace YoutrackBoard
{
    internal class Person
    {
        public string Name { get; set; }
        public string Login { get; set; }


        public Person(string name, string login)
        {
            Name = name;
            Login = login;
        }
    }
}