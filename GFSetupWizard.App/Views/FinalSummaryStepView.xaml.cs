using System.Windows;
using System.Windows.Controls;

namespace GFSetupWizard.App.Views
{
    public partial class FinalSummaryStepView : UserControl
    {
        public FinalSummaryStepView()
        {
            InitializeComponent();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent window and close it
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
