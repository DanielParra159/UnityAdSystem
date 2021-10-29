using System;
using Frameworks.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Frameworks.View
{
    public class RewardedAdView : MonoBehaviour, RewardedAd
    {
        public event Action OnOkButtonPressed;
        public event Action OnCancelButtonPressed;
        public event Action OnErrorButtonPressed;

        [SerializeField] private Button _okButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _errorButton;

        private void Awake()
        {
            Hide();

            _okButton.onClick.AddListener(() => OnOkButtonPressed?.Invoke());
            _cancelButton.onClick.AddListener(() => OnCancelButtonPressed?.Invoke());
            _errorButton.onClick.AddListener(() => OnErrorButtonPressed?.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}