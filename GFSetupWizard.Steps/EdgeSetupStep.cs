using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class EdgeSetupStep : BaseSetupStep
    {
        public override string Title => "Microsoft Edge Setup";
        public override string Description => "Configure Microsoft Edge browser settings and enable syncing.";

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
