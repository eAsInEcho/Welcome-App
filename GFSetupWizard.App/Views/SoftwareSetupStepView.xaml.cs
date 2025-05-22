using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class SoftwareSetupStepView : UserControl
    {
        public SoftwareSetupStepView()
        {
            InitializeComponent();
        }
        
        private void LaunchSoftwareCenterButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.LaunchSoftwareCenter();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to launch Software Center. It may not be installed on this computer.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void OpenServicePortalButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.OpenSoftwareRequestPortal();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to open the service portal. Please try again later.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
