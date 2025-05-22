using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace GFSetupWizard.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
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
                
                // Log startup attempt
                File.AppendAllText("app_log.txt", $"{DateTime.Now}: Application startup initiated\n");
                
                try
                {
                    // Create and show the main window with detailed error handling
                    Debug.WriteLine("Creating MainWindow instance");
                    var mainWindow = new MainWindow();
                    
                    Debug.WriteLine("Showing MainWindow");
                    mainWindow.Show();
                    
                    // Log successful window creation
                    File.AppendAllText("app_log.txt", $"{DateTime.Now}: MainWindow created and shown successfully\n");
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
                string logMessage = $"{DateTime.Now}: {source} - {ex?.GetType().Name}: {ex?.Message}\n{ex?.StackTrace}\n\n";
                File.AppendAllText("app_error_log.txt", logMessage);
                Debug.WriteLine($"Exception logged: {source} - {ex?.GetType().Name}: {ex?.Message}");
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"Failed to log exception: {logEx.Message}");
                // If logging fails, we can't do much about it
            }
        }
    }
}
