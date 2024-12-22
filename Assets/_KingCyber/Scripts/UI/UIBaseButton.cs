using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KingCyber.Base.UI
{
    [RequireComponent(typeof(Button))]
    public class UIBaseButton : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent onClick = new UnityEvent();
        private Button button;
        private float animationScale = 1.1f;
        private float animationDuration = 0.1f;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            AnimateButton();  
        }

        private void AnimateButton()
        {
            transform.DOScale(Vector3.one * animationScale, animationDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // Trigger the onClick action
                onClick?.Invoke();

                // Scale back to original size
                transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.InOutSine);
            });
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}
