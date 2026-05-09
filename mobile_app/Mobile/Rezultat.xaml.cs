namespace PrvaApp
{
    [QueryProperty(nameof(Results), "results")]
    public partial class Rezultat : ContentPage
    {
        public Rezultat() => InitializeComponent();

        public static string NadjiSliku(string s) =>
            "Slike/" + string.Join("_", Norm(s).Split(" ")) + ".jpg";

        public static string Norm(string s) =>
            s.Replace('é', 'e').Replace('ć', 'c').Replace('Ć', 'C')
             .Replace('č', 'c').Replace('Č', 'C').Replace('Š', 'S')
             .Replace('š', 's').Replace('đ', 'd').Replace('Đ', 'D')
             .Replace('Ž', 'Z').Replace('ž', 'z');

        public List<RecognitionResult> Results
        {
            set
            {
                bool sr = MainPage.jezikclicks == 1;
                if (sr) { Title = "Rezultat"; infot.Text = "Tapnite za više informacija"; }

                resultsCollection.ItemsSource = value.Select((r, i) => new ResultDisplay
                {
                    Rank = i + 1,
                    Name = sr ? r.MonumentNameSerbian : r.MonumentName,
                    BestMatchLabel = sr ? "★ Najsličnije" : "★ Best Match",
                    Image = ImageSource.FromStream(() =>
                        FileSystem.OpenAppPackageFileAsync(NadjiSliku(r.MonumentName)).Result),
                    Original = r
                }).ToList();
            }
        }

        private async void OnResultSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not ResultDisplay sel) return;
            await Shell.Current.GoToAsync("info",
                new Dictionary<string, object> { { "info", sel.Original } });
            resultsCollection.SelectedItem = null; // deselect after nav
        }
    }
}