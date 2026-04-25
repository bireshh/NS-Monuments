using Microsoft.Extensions.DependencyInjection;
using PrvaApp.Data;
using PrvaApp.ML;

namespace PrvaApp
{
    public partial class App : Application
    {
        public static RecognitionService Recognition { get; private set; }
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
        protected override async void OnStart()
        {
            Recognition = new RecognitionService();
            await Recognition.InitAsync();
        }
    }
}