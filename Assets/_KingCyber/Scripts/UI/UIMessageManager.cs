using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KingCyber.Base.UI
{
    [System.Serializable]
    public class UINoti
    {
        public CanvasGroup canvas;
        public TextMeshProUGUI notiText;
    }

    [System.Serializable]
    public class UIMessage
    {
        public CanvasGroup canvas;
        public Transform transform;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI messageText;
        public Image iconImage;
        public UIBaseButton yesButton;
        public UIBaseButton okeButton;
        public UIBaseButton noButton;
    }

    public class UIMessageManager : MonoSingleton<UIMessageManager>
    {
        [SerializeField] private UINoti noti;
        [SerializeField] private UIMessage message;
        private const float NOTI_DURATION = 1.0f;

        private void Start()
        {
            HideAllNotifications();
        }

        private void HideAllNotifications()
        {
            noti.canvas.alpha = 0;
            noti.canvas.gameObject.SetActive(false);

            message.canvas.alpha = 0;
            message.canvas.gameObject.SetActive(false);

            message.yesButton.onClick.RemoveAllListeners();
            message.noButton.onClick.RemoveAllListeners();
        }

        public void ShowNoti(string text, string location)
        {
            Debug.Log($"Show noti {location}: {text}");

            HideAllNotifications(); // Hide all notifications before showing the new one

            noti.notiText.text = text;

            // Set initial alpha to 0 (invisible)
            noti.canvas.alpha = 0;
            noti.canvas.gameObject.SetActive(true);

            // Get the RectTransform component
            RectTransform rectTransform = noti.canvas.GetComponent<RectTransform>();
            Vector2 nextPosition = Vector2.zero;
            Vector2 startPosition = Vector2.zero; 

            // Determine starting position based on location
            switch (location.ToLower())
            {
                case "top":
                    rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    rectTransform.pivot = new Vector2(0.5f, 1f);
                    startPosition = new Vector2(0, 200); 
                    nextPosition = new Vector2(0, -40); 
                    break;
                case "bottom":
                    rectTransform.anchorMin = new Vector2(0.5f, 0f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0f);
                    rectTransform.pivot = new Vector2(0.5f, 0f);
                    startPosition = new Vector2(0, - 200);
                    nextPosition = new Vector2(0, 40);
                    break;
                case "center":
                    rectTransform.localScale = Vector3.zero; // Start with scale 0
                    rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Scale up to full size
                    break;
                default:
                    break;
            }

            // If not center, animate the move-in and fade-in effect
            if (location.ToLower() != "center")
            {
                rectTransform.anchoredPosition = startPosition;
                rectTransform.DOAnchorPos(nextPosition, 0.5f).SetEase(Ease.OutCubic); // Move into position
            }

            Sequence seq = DOTween.Sequence();
            seq.Append(noti.canvas.DOFade(1, 0.5f).SetEase(Ease.InOutSine)); // Fade in
            seq.AppendInterval(NOTI_DURATION); // Wait for the duration
            seq.Append(noti.canvas.DOFade(0, 0.5f).SetEase(Ease.InOutSine)); // Fade out
            seq.Join(rectTransform.DOAnchorPos(startPosition, 0.5f).SetEase(Ease.InOutCubic)); // Move back to start position
            seq.OnComplete(() => HideAllNotifications()); // Hide completely after the animation
        }

        public void ShowMessage(string text, UnityAction yesCallback, UnityAction noCallback, string title="Message", Sprite icon=null)
        {
            HideAllNotifications(); // Hide all other notifications

            message.titleText.text = title;
            message.messageText.text = text;
            if (icon != null)
            {
                message.iconImage.gameObject.SetActive(true);
                message.iconImage.sprite = icon;
            }
            else { message.iconImage.gameObject.SetActive(false);}
            

            // Reset the canvas group and scale for the animation
            message.canvas.alpha = 0;
            message.transform.localScale = Vector3.zero;
            message.canvas.gameObject.SetActive(true);

            // Add the show animation
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(message.canvas.DOFade(1, 0.5f).SetEase(Ease.OutSine)); // Fade in over 0.5 seconds
            showSequence.Join(message.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)); // Scale up from 0 to full size

            message.yesButton.onClick.RemoveAllListeners();
            message.noButton.onClick.RemoveAllListeners();

            if (yesCallback != null)
            {
                message.yesButton.onClick.AddListener(yesCallback);
                message.okeButton.onClick.AddListener(yesCallback);
            }


            if (noCallback != null)
            {
                message.noButton.onClick.AddListener(noCallback);
                message.okeButton.gameObject.SetActive(false);
                message.yesButton.gameObject.SetActive(true);
            }
            else
            {
                message.okeButton.gameObject.SetActive(true);
                message.yesButton.gameObject.SetActive(false);
            }

            message.yesButton.onClick.AddListener(HideMessage);
            message.okeButton.onClick.AddListener(HideMessage);
            message.noButton.onClick.AddListener(HideMessage);
        }

        private void HideMessage()
        {
            // Fade out the message box and then deactivate it
            Sequence seq = DOTween.Sequence();

            seq.Append(message.canvas.DOFade(0, 0.5f).SetEase(Ease.InOutSine)); // Fade out over 0.5 seconds
            seq.Join(message.transform.DOScale(0, 0.5f).SetEase(Ease.InOutBack));
            seq.OnComplete(() =>
            {
                HideAllNotifications();
            });
        }

    }
}
