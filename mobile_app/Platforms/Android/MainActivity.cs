using Android.App;
using Android.Content.PM;
using Android.OS;
namespace PrvaApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                DeleteDirectory(CacheDir);

                if (ExternalCacheDir != null)
                    DeleteDirectory(ExternalCacheDir);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cache clear failed: {ex.Message}");
            }
        }

        private void DeleteDirectory(Java.IO.File dir)
        {
            if (dir == null || !dir.Exists()) return;

            foreach (var file in dir.ListFiles() ?? Array.Empty<Java.IO.File>())
            {
                if (file.IsDirectory)
                    DeleteDirectory(file);
                else
                    file.Delete();
            }
        }
    }
}
