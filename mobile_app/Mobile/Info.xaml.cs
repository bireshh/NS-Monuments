namespace PrvaApp;

[QueryProperty(nameof(Rez), "info")]
public partial class Info : ContentPage
{
	public Info()
	{
		InitializeComponent();
    }
	RecognitionResult rez;
	public RecognitionResult Rez
	{
		set
		{
            if (MainPage.jezikclicks == 1)
			{
				heroTitle.Text = value.MonumentNameSerbian;
				this.Title = value.MonumentNameSerbian;
				tekst.Text = value.DescriptionSerbian;
			}
			else
			{
                heroTitle.Text = value.MonumentName;
                this.Title = value.MonumentName;
                tekst.Text = value.Description;
            }
			rez = value;
            slika.Source = ImageSource.FromStream(() => FileSystem.OpenAppPackageFileAsync(Rezultat.NadjiSliku(value.MonumentName)).Result);
        }
	}
}