namespace PrvaApp
{
    public class ResultDisplay
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public ImageSource Image { get; set; }
        public string BestMatchLabel { get; set; }
        public bool IsBestMatch => Rank == 1;
        public RecognitionResult Original { get; set; }
    }

}