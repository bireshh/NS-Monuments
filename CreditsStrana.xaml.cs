
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
                tekst = value;
                credits.Text = tekst;
            }
        }
    }
}