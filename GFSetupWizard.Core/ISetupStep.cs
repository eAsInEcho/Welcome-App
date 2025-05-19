// ISetupStep.cs in GFSetupWizard.Core
namespace GFSetupWizard.Core
{
    public interface ISetupStep
    {
        string Title { get; }
        string Description { get; }
        bool IsComplete { get; }
        bool CanProceed { get; }
        
        void Initialize();
        bool Validate();
        void Execute();
        void Cleanup();
    }
}