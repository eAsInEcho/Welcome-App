using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class EdgeSetupStepView : UserControl
    {
        // Events needed for step navigation
        public event RoutedEventHandler StepCompleted;
        public event RoutedEventHandler StepSkipped;
        
        // Status colors
        private static readonly SolidColorBrush SuccessColor = new SolidColorBrush(Color.FromRgb(46, 125, 50));
        private static readonly SolidColorBrush WarningColor = new SolidColorBrush(Color.FromRgb(237, 108, 2));
        private static readonly SolidColorBrush ErrorColor = new SolidColorBrush(Color.FromRgb(211, 47, 47));
        private static readonly SolidColorBrush InfoColor = new SolidColorBrush(Color.FromRgb(2, 119, 189));
        
        public EdgeSetupStepView()
        {
            InitializeComponent();
            
            // Check Edge profile status when the view is loaded
            Loaded += EdgeSetupStepView_Loaded;
        }

        private void EdgeSetupStepView_Loaded(object sender, RoutedEventArgs e)
        {
            // Hide status panel since we're not checking profiles automatically
            StatusPanel.Visibility = Visibility.Collapsed;
        }

        private void LaunchEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.LaunchEdgeWithInputSimulator();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to launch Edge with sync settings. Please try using the 'Copy Sync Settings URL' button instead.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                MessageBox.Show(
                    "Edge has been launched and should navigate to the sync settings page. " +
                    "Make sure you sign in with your GlobalFoundries work account and enable sync.",
                    "Edge Launched",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                
                // Mark step as complete when user launches Edge
                StepCompleted?.Invoke(this, new RoutedEventArgs());
            }
        }
        
        private void CheckStatusButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.CopyEdgeSyncUrlToClipboard();
            
            if (success)
            {
                MessageBox.Show(
                    "The sync settings URL has been copied to your clipboard. " +
                    "Please paste it into Edge's address bar to navigate to the sync settings page.",
                    "URL Copied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                
                // Mark step as complete when user copies the URL
                StepCompleted?.Invoke(this, new RoutedEventArgs());
            }
            else
            {
                MessageBox.Show(
                    "Unable to copy the URL to clipboard. The URL is: edge://settings/profiles/sync",
                    "Copy Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
        
    }
}
