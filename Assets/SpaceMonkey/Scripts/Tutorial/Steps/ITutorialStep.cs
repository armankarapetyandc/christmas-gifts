using Cysharp.Threading.Tasks;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public interface ITutorialStep
    {
        int Order { get; }
        UniTask Show();
        void Hide();
    }
}