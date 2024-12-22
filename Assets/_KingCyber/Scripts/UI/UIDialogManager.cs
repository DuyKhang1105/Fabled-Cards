using KingCyber.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;

namespace KingCyber.Base.UI
{
    public class UIDialogManager : MonoSingleton<UIDialogManager>
    {
        [SerializeField] private CanvasGroup background;
        [SerializeField] private List<UIBaseDialog> dialogs = new List<UIBaseDialog>();
        private Dictionary<string, UIBaseDialog> activeDialogs = new Dictionary<string, UIBaseDialog>();

#if UNITY_EDITOR
        private void OnValidate()
        {
            //Find all UIBaseDialog prefab
            dialogs.Clear();
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                {
                    UIBaseDialog dialog = prefab.GetComponent<UIBaseDialog>();
                    if (dialog != null)
                    {
                        dialogs.Add(dialog);
                    }
                }
            }
        }
#endif

        public T ShowDialog<T>() where T : UIBaseDialog
        {
            background.gameObject.SetActive(true);
            background.alpha = 0f;
            background.DOFade(0.8f, UIBaseDialog.ANIM_DURATION);

            var dialogType= typeof(T).ToString();
            if (!activeDialogs.ContainsKey(dialogType))
            {
                // find the dialog by its type
                var prefab = dialogs.Find(x => x is T) as T;
                activeDialogs.Add(dialogType, Instantiate(prefab, transform)); 
            }
            if (activeDialogs.TryGetValue(dialogType, out var dialog))
            {
                dialog.Show();
                dialog.closeBtn.onClick.AddListener(()=>HideDialog<T>());
                return dialog as T;
            }
            
            return null;
        }

        public void HideDialog<T>() where T : UIBaseDialog
        {
            background.DOFade(0f, UIBaseDialog.ANIM_DURATION).OnComplete(() => { background.gameObject.SetActive(false); });

            var dialogType = typeof(T).ToString();
            if (activeDialogs.TryGetValue(dialogType,out var dialog))
            {
               dialog.Hide();
            }
        }

        public void HideAllDialogs()
        {
            background.DOFade(0f, UIBaseDialog.ANIM_DURATION).OnComplete(() => { background.gameObject.SetActive(false); });
            foreach (var dialog in activeDialogs.Values)
            {
                dialog.Hide();
            }
            activeDialogs.Clear();
        }
    }
}

