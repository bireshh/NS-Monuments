
namespace PrvaApp
{

    [QueryProperty(nameof(Tekst), "tekst")]
    public partial class CreditsStrana : ContentPage
	{
		public CreditsStrana()
		{
			InitializeComponent();
            
        }
        string tekst;
        public string Tekst
        {
            get => tekst;
            set
            {
                if (MainPage.jezikclicks == 1)
                {
                    this.Title = "Zasluge";
                }
                tekst = value;
                credits.Text = tekst;
            }
        }
    }
}