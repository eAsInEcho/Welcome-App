using GFSetupWizard.Core;
using System.Windows.Controls;

namespace GFSetupWizard.Steps
{
    public class OneDriveSetupStep : BaseSetupStep
    {
        public override string Title => "OneDrive Setup";
        public override string Description => "Configure your OneDrive for secure file storage and synchronization.";

        public required UserControl View { get; init; }
    }
}
