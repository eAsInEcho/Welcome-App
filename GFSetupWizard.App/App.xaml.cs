using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GFSetupWizard.App.Views;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                // Check if this is the first time the app has been run
                if (!InstallationManager.HasAppRunBefore())
                {
                    // If it's the first run, set up auto-launch registry entry
                    Debug.WriteLine("First run detected, setting up auto-launch registry entry");
                    InstallationManager.SetupRegistryEntry();
                    
                    try
                    {
                        string logDirectory = @"C:\GFApps";
                        string logPath = Path.Combine(logDirectory, "app_log.txt");
                        
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        
                        File.AppendAllText(logPath, $"{DateTime.Now}: First run detected, auto-launch registry entry created\n");
                    }
                    catch
                    {
                        // Continue even if logging fails
                    }
                }
                
                // Mark the app as having been run
                InstallationManager.SetAppHasRun();
                
                // Initialize font system explicitly to avoid issues in single-file deployment
                InitializeFontSystem();
                
                // Add exception handling
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    var exception = args.ExceptionObject as Exception;
                    LogException(exception, "AppDomain.UnhandledException");
                    MessageBox.Show($"An unhandled exception occurred: {exception?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                };

                this.DispatcherUnhandledException += (sender, args) =>
                {
                    LogException(args.Exception, "Application.DispatcherUnhandledException");
                    MessageBox.Show($"An unhandled exception occurred: {args.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    args.Handled = true;
                };

                base.OnStartup(e);
                
                // Log startup attempt in a user-accessible location
                try
                {
                    string logDirectory = @"C:\GFApps";
                    string logPath = Path.Combine(logDirectory, "app_log.txt");
                    
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    
                    File.AppendAllText(logPath, $"{DateTime.Now}: Application startup initiated\n");
                }
                catch
                {
                    // Continue even if logging fails
                    Debug.WriteLine("Unable to write to app_log.txt, but continuing startup");
                }
                
                // Create and show the main window with detailed error handling
                try
                {
                    Debug.WriteLine("Creating MainWindow instance");
                    var mainWindow = new MainWindow();
                    
                    Debug.WriteLine("Showing MainWindow");
                    mainWindow.Show();
                    
                    // Log successful window creation
                    try
                    {
                        string logDirectory = @"C:\GFApps";
                        string logPath = Path.Combine(logDirectory, "app_log.txt");
                        
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        
                        File.AppendAllText(logPath, $"{DateTime.Now}: MainWindow created and shown successfully\n");
                    }
                    catch
                    {
                        // Continue even if logging fails
                        Debug.WriteLine("Unable to write success log, but window was created successfully");
                    }
                }
                catch (Exception windowEx)
                {
                    LogException(windowEx, "MainWindow Creation");
                    MessageBox.Show($"Failed to create or show main window: {windowEx.Message}\n\nStack trace: {windowEx.StackTrace}", 
                        "Window Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw; // Re-throw to be caught by the outer try-catch
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "OnStartup");
                MessageBox.Show($"Failed to start application: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LogException(Exception ex, string source)
        {
            try
            {
                // Use C:\GFApps directory for logs since it's guaranteed to be user-accessible
                string logDirectory = @"C:\GFApps";
                string logPath = Path.Combine(logDirectory, "app_error_log.txt");
                
                string logMessage = $"{DateTime.Now}: {source} - {ex?.GetType().Name}: {ex?.Message}\n{ex?.StackTrace}\n\n";
                
                // Ensure the directory exists
                try
                {
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    
                    File.AppendAllText(logPath, logMessage);
                }
                catch
                {
                    // Silently continue if file operations fail
                    // The important thing is the app starts successfully
                }
                
                Debug.WriteLine($"Exception logged: {source} - {ex?.GetType().Name}: {ex?.Message}");
            }
            catch
            {
                // Suppress all exceptions in the logging method
                // We don't want logging failures to affect the application
            }
        }
        
        /// <summary>
        /// Explicitly initializes the font system to avoid issues in single-file deployment.
        /// </summary>
        private void InitializeFontSystem()
        {
            try
            {
                // Log the start of font initialization
                Debug.WriteLine("Starting font system initialization");
                try
                {
                    string logDirectory = @"C:\GFApps";
                    string logPath = Path.Combine(logDirectory, "app_log.txt");
                    
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    
                    File.AppendAllText(logPath, $"{DateTime.Now}: Starting font system initialization\n");
                }
                catch
                {
                    // Continue even if logging fails
                }
                
                // Ensure the PresentationCore assembly is loaded
                var presentationCoreAssembly = typeof(System.Windows.Media.FontFamily).Assembly;
                Debug.WriteLine($"PresentationCore assembly loaded: {presentationCoreAssembly.FullName}");
                
                // Force initialization of WPF's font system using multiple approaches
                
                // Approach 1: Create a simple TextBlock with a generic font family
                try
                {
                    Debug.WriteLine("Approach 1: Creating TextBlock with generic font family");
                    var textBlock = new System.Windows.Controls.TextBlock
                    {
                        Text = "Font Init",
                        FontFamily = new FontFamily("Segoe UI, Arial, MS Sans Serif")
                    };
                    textBlock.Measure(new Size(100, 100));
                    Debug.WriteLine("TextBlock created and measured successfully");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Approach 1 failed: {ex.Message}");
                    try
                    {
                        string logDirectory = @"C:\GFApps";
                        string logPath = Path.Combine(logDirectory, "app_error_log.txt");
                        
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        
                        File.AppendAllText(logPath, $"{DateTime.Now}: Font init approach 1 failed: {ex.Message}\n{ex.StackTrace}\n\n");
                    }
                    catch
                    {
                        // Continue even if logging fails
                    }
                }
                
                // Approach 2: Try with system generic font families
                try
                {
                    Debug.WriteLine("Approach 2: Using system generic font families");
                    // Use the most generic font family possible
                    var genericFontFamily = new FontFamily("Arial");
                    var typeface = new Typeface(genericFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                    
                    GlyphTypeface glyphTypeface;
                    bool success = typeface.TryGetGlyphTypeface(out glyphTypeface);
                    Debug.WriteLine($"Generic font family typeface created, success: {success}");
                    
                    if (!success)
                    {
                        // Try with the most basic system font
                        genericFontFamily = new FontFamily("MS Sans Serif");
                        typeface = new Typeface(genericFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                        success = typeface.TryGetGlyphTypeface(out glyphTypeface);
                        Debug.WriteLine($"MS Sans Serif typeface created, success: {success}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Approach 2 failed: {ex.Message}");
                    try
                    {
                        string logDirectory = @"C:\GFApps";
                        string logPath = Path.Combine(logDirectory, "app_error_log.txt");
                        
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        
                        File.AppendAllText(logPath, $"{DateTime.Now}: Font init approach 2 failed: {ex.Message}\n{ex.StackTrace}\n\n");
                    }
                    catch
                    {
                        // Continue even if logging fails
                    }
                }
                
                // Approach 3: Create and measure a Button (which uses different font rendering path)
                try
                {
                    Debug.WriteLine("Approach 3: Creating and measuring a Button");
                    var button = new System.Windows.Controls.Button { Content = "Font Init" };
                    button.Measure(new Size(100, 30));
                    button.Arrange(new Rect(0, 0, 100, 30));
                    Debug.WriteLine("Button created and measured successfully");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Approach 3 failed: {ex.Message}");
                    try
                    {
                        string logDirectory = @"C:\GFApps";
                        string logPath = Path.Combine(logDirectory, "app_error_log.txt");
                        
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        
                        File.AppendAllText(logPath, $"{DateTime.Now}: Font init approach 3 failed: {ex.Message}\n{ex.StackTrace}\n\n");
                    }
                    catch
                    {
                        // Continue even if logging fails
                    }
                }
                
                // Approach 4: Try to access system fonts directly
                try
                {
                    Debug.WriteLine("Approach 4: Accessing system fonts collection");
                    var systemFonts = System.Windows.Media.Fonts.SystemFontFamilies;
                    Debug.WriteLine($"System fonts count: {systemFonts.Count}");
                    if (systemFonts.Count > 0)
                    {
                        var firstFont = systemFonts.First();
                        Debug.WriteLine($"First system font: {firstFont.Source}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Approach 4 failed: {ex.Message}");
                    try
                    {
                        string logDirectory = @"C:\GFApps";
                        string logPath = Path.Combine(logDirectory, "app_error_log.txt");
                        
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        
                        File.AppendAllText(logPath, $"{DateTime.Now}: Font init approach 4 failed: {ex.Message}\n{ex.StackTrace}\n\n");
                    }
                    catch
                    {
                        // Continue even if logging fails
                    }
                }
                
                Debug.WriteLine("Font system initialization completed");
                try
                {
                    string logDirectory = @"C:\GFApps";
                    string logPath = Path.Combine(logDirectory, "app_log.txt");
                    
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    
                    File.AppendAllText(logPath, $"{DateTime.Now}: Font system initialization completed\n");
                }
                catch
                {
                    // Continue even if logging fails
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Font initialization error: {ex.Message}");
                // Log the error with full details
                try
                {
                    string logDirectory = @"C:\GFApps";
                    string logPath = Path.Combine(logDirectory, "app_error_log.txt");
                    
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    
                    File.AppendAllText(logPath, $"{DateTime.Now}: Font initialization error: {ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}\n\n");
                }
                catch
                {
                    // Continue even if logging fails
                }
                
                // Continue even if font initialization fails
                // The application might still work with default fonts
            }
        }
    }
}
