using Firebase.Database;

namespace Examen3_ErickRobles
{
    
    public partial class App : Application
    {
        public static FirebaseClient FirebaseClient { get; private set; }
        public App()
        {
            InitializeComponent();

            FirebaseClient = new FirebaseClient("https://examen3er-14cc0-default-rtdb.firebaseio.com/");

            MainPage = new AppShell();
        }
    }
}
