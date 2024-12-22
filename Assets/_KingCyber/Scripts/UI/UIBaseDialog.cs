using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KingCyber.Base.UI
{
    public abstract class UIBaseDialog : MonoBehaviour
    {
        public UIBaseButton closeBtn;
        public const float ANIM_DURATION = 0.3f;

        private void OnDestroy()
        {
            closeBtn.onClick.RemoveAllListeners();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, ANIM_DURATION).SetEase(Ease.OutBack); // Scale up to full size
        }

        public virtual void Hide()
        {
            transform.DOScale(Vector3.one * 0.7f, ANIM_DURATION)
                .SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false)); // Scale down and then hide the dialog
        }
    }
}

