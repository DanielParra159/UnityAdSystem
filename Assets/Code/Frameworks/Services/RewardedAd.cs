using System;

namespace Frameworks.Services
{
    public interface RewardedAd
    {
        event Action OnOkButtonPressed;
        event Action OnCancelButtonPressed;
        event Action OnErrorButtonPressed;
        void Show();
        void Hide();
    }
}