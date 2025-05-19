using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class WelcomeStep : BaseSetupStep
    {
        public override string Title => "Welcome to Your New Laptop";
        public override string Description => "This wizard will guide you through the setup process of your new GlobalFoundries laptop.";

        private readonly UserControl _view;

        public WelcomeStep(UserControl view)
        {
            _view = view;
        }

        public UserControl View => _view;
    }
}