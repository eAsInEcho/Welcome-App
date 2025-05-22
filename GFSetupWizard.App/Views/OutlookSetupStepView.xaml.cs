using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class OutlookSetupStepView : UserControl
    {
    public OutlookSetupStepView()
    {
        InitializeComponent();
    }

        private void LaunchOutlookButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.LaunchOutlook();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to launch Outlook automatically. Please open it manually from the Start menu.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
