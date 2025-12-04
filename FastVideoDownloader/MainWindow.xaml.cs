using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks; // Required for Task
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading; // Required for DispatcherTimer

namespace FastVideoDownloader
{
    public partial class MainWindow : Window
    {
        // 1. New Variables for Timing
        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;

        // Theme variables
        private int _currentThemeIndex = 0;
        private readonly AppTheme[] _themes;

        public MainWindow()
        {
            InitializeComponent();

            // 2. Initialize the Timer
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Tick every 1 second
            _timer.Tick += Timer_Tick;

            // Initialize Themes
            _themes = new AppTheme[]
            {
                new AppTheme { Background = Brushes.White, Text = Brushes.Black, InputBackground = Brushes.White, InputText = Brushes.Black },
                new AppTheme { Background = (Brush)new BrushConverter().ConvertFrom("#1E1E1E"), Text = Brushes.White, InputBackground = (Brush)new BrushConverter().ConvertFrom("#333333"), InputText = Brushes.White },
                new AppTheme { Background = (Brush)new BrushConverter().ConvertFrom("#FDF6E3"), Text = (Brush)new BrushConverter().ConvertFrom("#586E75"), InputBackground = Brushes.White, InputText = Brushes.Black }
            };
        }

        // 3. This function runs every second to update the text
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Format time as 00:00:15
            string timeElapsed = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            StatusText.Text = $"Downloading... Time Elapsed: {timeElapsed}";
        }

        private void ThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentThemeIndex++;
            if (_currentThemeIndex >= _themes.Length) _currentThemeIndex = 0;
            ApplyTheme(_themes[_currentThemeIndex]);
        }

        private void ApplyTheme(AppTheme theme)
        {
            this.Background = theme.Background;
            LblUrl.Foreground = theme.Text;
            LblPath.Foreground = theme.Text;
            UrlInput.Background = theme.InputBackground;
            UrlInput.Foreground = theme.InputText;
            OutputPath.Background = theme.InputBackground;
            OutputPath.Foreground = theme.InputText;
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "MP4 Video|*.mp4";
            saveFileDialog.Title = "Save Video As";
            saveFileDialog.FileName = "reddit_video.mp4";

            if (saveFileDialog.ShowDialog() == true)
            {
                OutputPath.Text = saveFileDialog.FileName;
                DownloadBtn.IsEnabled = true;
            }
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlInput.Text.Trim();
            string savePath = OutputPath.Text;

            if (string.IsNullOrEmpty(url)) { StatusText.Text = "Please enter a URL."; return; }

            // Lock UI
            DownloadBtn.IsEnabled = false;
            BrowseBtn.IsEnabled = false;
            UrlInput.IsEnabled = false;

            // Show Bar and Start Timer
            DownloadProgress.Visibility = Visibility.Visible;
            StatusText.Foreground = Brushes.Orange;

            _stopwatch.Restart(); // Reset stopwatch to 0
            _timer.Start();       // Start ticking

            try
            {
                await RunFFmpegAsync(url, savePath);

                StatusText.Text = "Download Complete!";
                StatusText.Foreground = Brushes.Green;
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
                StatusText.Foreground = Brushes.Red;
            }
            finally
            {
                // Stop Timer and Hide Bar
                _timer.Stop();
                _stopwatch.Stop();

                DownloadProgress.Visibility = Visibility.Collapsed;
                DownloadBtn.IsEnabled = true;
                BrowseBtn.IsEnabled = true;
                UrlInput.IsEnabled = true;
            }
        }

        // --- UPDATED CLEAN FUNCTION ---
        private Task RunFFmpegAsync(string url, string fullOutputPath)
        {
            return Task.Run(() =>
            {
                string ffmpegPath = "ffmpeg.exe";
                if (!File.Exists(ffmpegPath)) throw new FileNotFoundException("ffmpeg.exe not found!");

                // We still keep this to capture errors if it crashes
                System.Text.StringBuilder errorLog = new System.Text.StringBuilder();

                string arguments = $"-protocol_whitelist file,https,tcp,tls,crypto -i \"{url}\" -c copy \"{fullOutputPath}\" -y";

                var startInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    // Simple logging only - No Regex, No UI updates here
                    process.ErrorDataReceived += (s, args) =>
                    {
                        if (args.Data != null) errorLog.AppendLine(args.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"FFmpeg Failed.\nErrors: {errorLog}");
                    }
                }
            });
        }
    }

    public class AppTheme
    {
        public Brush Background { get; set; }
        public Brush Text { get; set; }
        public Brush InputBackground { get; set; }
        public Brush InputText { get; set; }
    }
}