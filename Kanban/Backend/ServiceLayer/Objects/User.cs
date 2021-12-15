namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;
        internal User(string email, string nickname)
        {
            this.Email = email;
            this.Nickname = nickname;
        }
        // You can add code here
    }
}
