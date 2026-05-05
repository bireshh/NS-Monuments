
using Microsoft.Maui.Media;
using SkiaSharp;
using SQLite;
namespace PrvaApp
{
    public partial class MainPage : ContentPage
    {

        public static int jezikclicks = 0;
        static string[] jezen ={ "Language: English", "Show credits", "Hide credits", "Peope who have helped with collecting data:\nUroš Pejaković\nAleksandar Provči\nMilan Gašić\nNebojša Tadić\nLara Ilić", "📷 Take a photo", "Scan monuments offline", "Point your camera at a monument" } ;
        static string[] jezsr = { "Jezik: Srpski", "Prikaži zasluge", "Sakrij zasluge", "Ljudi koji su pomogli sa prikupljanjem podataka:\nUroš Pejaković\nAleksandar Provči\nMilan Gašić\nNebojša Tadić\nLara Ilić", "📷 Slikaj","Skenirajte spomenike offline","Usmerite kameru ka spomeniku"};
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
            if (jezikclicks == 0) {  t = jezen[3]; }
            else { t = jezsr[3]; }
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
            slikajdugme.Text = jezsr[4];
            sken.Text= jezsr[5];
            dole.Text = jezsr[6];
        }
        private void PromenijezikEN()
        {
            slikajdugme.Text = jezen[4];
            sken.Text = jezen[5];
            dole.Text= jezen[6];
        }
        private async void Slikaj(object sender, EventArgs e)
        {
            FileResult slika = await MediaPicker.Default.CapturePhotoAsync();
            if (slika != null)
            {
                if (jezikclicks == 0)
                {
                    tekstcekaj.Text = "This process may take a few seconds";
                }
                else
                {
                    tekstcekaj.Text = "Ovaj proces može da potraje nekoliko sekundi";
                }
                using var stream = await slika.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                byte[] imageBytes = ms.ToArray();

                var results = await App.Recognition.RecognizeAsync(imageBytes,5); 
                var navigationParameter = new Dictionary<string, object>
                {
                    { "results", results }
                };
                await Shell.Current.GoToAsync("rezultat", navigationParameter);
                tekstcekaj.Text = "";
            }
        }
    }
}
