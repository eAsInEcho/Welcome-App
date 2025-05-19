using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class VpnSetupStep : BaseSetupStep
    {
        public override string Title => "SecurID RSA Token for VPN";
        public override string Description => "Request your SecurID RSA token for secure VPN access to company resources.";

        public required UserControl View { get; init; }

        // Use the base class implementation which uses StepCompletionManager
        public override void MarkAsComplete()
        {
            base.MarkAsComplete();
        }

        // Use the base class implementation which uses StepCompletionManager
        public override void MarkAsSkipped()
        {
            base.MarkAsSkipped();
        }
    }
}
