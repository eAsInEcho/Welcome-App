using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class OneDriveSetupStep : BaseSetupStep
    {
        public override string Title => "OneDrive Setup";
        public override string Description => "Configure your OneDrive for secure file storage and synchronization.";

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
