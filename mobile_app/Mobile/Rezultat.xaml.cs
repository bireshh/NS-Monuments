namespace PrvaApp
{

    [QueryProperty(nameof(Results), "results")]
    public partial class Rezultat : ContentPage
    {
        public Rezultat()
        {
            InitializeComponent();

        }

        private static readonly string BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private string NadjiSliku(string s)
        {
            string[] niz = s.Split(" ");
            string novi = string.Join("_", niz);
            return "Slike/" + novi + ".jpg";
        }
        public List<RecognitionResult> Results
        {
            set
            {
                string s = NadjiSliku(value[0].MonumentName); 
                prvaslika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(s).Result);
                prva.Text = "1. "+value[0].MonumentName+" " + value[0].Score.ToString();
                druga.Text = "2. " + value[1].MonumentName + " " + value[1].Score.ToString();
                treca.Text = "3. " + value[2].MonumentName + " " + value[2].Score.ToString();
                cetvrta.Text = "4. " + value[3].MonumentName + " " + value[3].Score.ToString();
                peta.Text = "5. " + value[4].MonumentName + " " + value[4].Score.ToString();
            }
        }
    }
}