
using Microsoft.Maui.Media;
using SQLite;
namespace PrvaApp
{
    public partial class MainPage : ContentPage
    {

        public static int jezikclicks = 0;
        static string[] jezen ={ "Language: English", "Show credits", "Hide credits", "Peope who have helped with collecting data:\nUroš Pejaković\nAleksandar Provči\nMilan Gašić\nNebojša Tadić\nLara Ilić","Take a photo" } ;
        static string[] jezsr = { "Jezik: Srpski", "Prikaži zasluge", "Sakrij zasluge", "Ljudi koji su pomogli sa prikupljanjem podataka:\nUroš Pejaković\nAleksandar Provči\nMilan Gašić\nNebojša Tadić\nLara Ilić","Slikaj" };
        static string t = jezen[3];

        public MainPage()
        {
            InitializeComponent();
            dugmecr.Clicked +=async (s, e) => await Shell.Current.GoToAsync($"credit?tekst={t}");
        }

        private void Klikcr(object sender, EventArgs e)
        {
            if (jezikclicks == 0)
            {
                t = jezen[3];
            }
            else
            {
                 t = jezsr[3];
            }
            if (jezikclicks == 0) { dugmecr.Text = jezen[1]; t = jezen[3]; }
            else { dugmecr.Text = jezsr[1]; t = jezsr[3]; }
        }
        private void Klikjezik(object sender, EventArgs e)
        {
            if (jezikclicks == 0)
            {
                PromenijezikSR();
                jezikclicks=1;
            }
            else
            {
                PromenijezikEN();
                jezikclicks = 0;
            }
        }
        private void PromenijezikSR()
        {
            jezik.Text = jezsr[0];
            slikajdugme.Text = jezsr[4];
            dugmecr.Text = jezsr[1];
            
        }
        private void PromenijezikEN()
        {
            jezik.Text = jezen[0];
            slikajdugme.Text = jezen[4];
            dugmecr.Text = jezen[1];
        }
        private async void Slikaj(object sender, EventArgs e)
        {
            FileResult slika = await MediaPicker.Default.CapturePhotoAsync();
            if (slika != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, slika.FileName);
                using Stream sourceStream = await slika.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);
                mestozasliku.Source = ImageSource.FromFile(localFilePath);
                using var stream = await slika.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                byte[] imageBytes = ms.ToArray();

                var results = await App.Recognition.RecognizeAsync(imageBytes,1);
                foreach(var r in results)
                {
                    imes.Text = r.MonumentName;
                    imet.Text = r.Description;
                }
            }
        }
    }
}
