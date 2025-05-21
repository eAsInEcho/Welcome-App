using System.Windows;
using System.Windows.Controls;
using GFSetupWizard.SystemIntegration;

namespace GFSetupWizard.App.Views
{
    public partial class TeamsSetupStepView : UserControl
    {
        // Events needed for step navigation
        public event RoutedEventHandler StepCompleted;
        public event RoutedEventHandler StepSkipped;
        
        public TeamsSetupStepView()
        {
            InitializeComponent();
            
            // Auto-mark step as complete when the view is loaded
            Loaded += (s, e) => StepCompleted?.Invoke(this, new RoutedEventArgs());
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
