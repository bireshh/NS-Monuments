

namespace PrvaApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("credit", typeof(CreditsStrana));
            Routing.RegisterRoute("rezultat", typeof(Rezultat));
        }
    }
}
