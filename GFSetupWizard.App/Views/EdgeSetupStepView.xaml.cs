using System;
using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class EdgeSetupStepView : UserControl
    {
        public EdgeSetupStepView()
        {
            InitializeComponent();
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
            // Success message removed to prevent interruption of auto-typing
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
