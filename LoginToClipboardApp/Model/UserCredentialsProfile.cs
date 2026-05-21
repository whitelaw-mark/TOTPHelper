namespace LoginClipboardApp.Model
{
    public class UserCredentialsProfile
    {
        public string email { get; set; }
        public string password { get; set; }
        public string totpSecret { get; set; }  
    }

    public class UserCredentialsProfileCollection
    {
        public UserCredentialsProfile[] Profiles { get; set; }
    }
}
