using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class TeamsSetupStepView : UserControl
    {
        public TeamsSetupStepView()
        {
            InitializeComponent();
        }
        
        private void LaunchTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = SystemApplicationLauncher.LaunchTeams();
            
            if (!success)
            {
                MessageBox.Show(
                    "Unable to launch Teams automatically. Please open it manually from the Start menu.",
                    "Launch Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
