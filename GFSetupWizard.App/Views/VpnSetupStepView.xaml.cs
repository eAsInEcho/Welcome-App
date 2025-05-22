using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class VpnSetupStepView : UserControl
    {
        // Required constructor for WPF UserControl
        public VpnSetupStepView()
        {
            InitializeComponent();
        }

        private void OpenServicePortalButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.OpenRsaTokenRequestForVpn();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to open the service portal. Please contact IT support for assistance.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
