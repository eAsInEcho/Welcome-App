using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class SoftwareSetupStep : BaseSetupStep
    {
        public override string Title => "Additional Software Installation";
        public override string Description => "Install additional software using Software Center and request any specialized applications.";

        public required UserControl View { get; init; }
    }
}
