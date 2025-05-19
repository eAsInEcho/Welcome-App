// IStepNavigator.cs in GFSetupWizard.Core
namespace GFSetupWizard.Core
{
    public interface IStepNavigator
    {
        ISetupStep CurrentStep { get; }
        bool CanMoveNext { get; }
        bool CanMovePrevious { get; }
        
        void MoveNext();
        void MovePrevious();
        void Reset();
    }
}