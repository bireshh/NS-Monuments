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

        public static string NadjiSliku(string s)
        {
            s = Norm(s);
            string[] niz = s.Split(" ");
            string novi = string.Join("_", niz);
            return "Slike/" + novi + ".jpg";
        }
        public static string Norm(string s)
        {
            //ćčšđžé
            s = s.Replace('é', 'e');
            s = s.Replace('ć', 'c');
            s = s.Replace('Ć', 'c');
            s = s.Replace('č', 'c');
            s = s.Replace('Č', 'c');
            s = s.Replace('š', 's');
            s = s.Replace('Š', 's');
            s = s.Replace('đ', 'd');
            s = s.Replace('Đ', 'd');
            s = s.Replace('Ž', 'z');
            s = s.Replace('ž', 'z');
            return s;
        }
        
        public List<RecognitionResult> Results
        {
            set
            {
                if (MainPage.jezikclicks == 1)
                {
                    this.Title = "Rezultat";
                }
                prvaslika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(NadjiSliku(value[0].MonumentName)).Result);
                prvitekst.Text = value[0].MonumentName;
                drugaslika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(NadjiSliku(value[1].MonumentName)).Result);
                drugitekst.Text = value[1].MonumentName;
                trecaslika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(NadjiSliku(value[2].MonumentName)).Result);
                trecitekst.Text = value[2].MonumentName;
                cetvrtaslika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(NadjiSliku(value[3].MonumentName)).Result);
                cetvrtitekst.Text = value[3].MonumentName;
                petaslika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(NadjiSliku(value[4].MonumentName)).Result);
                petitekst.Text = value[4].MonumentName;
            }
        }
    }
}