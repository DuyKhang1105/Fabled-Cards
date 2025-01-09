using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using UnityEngine;

public class MainHome : MonoBehaviour
{
    public void OnClickOpenBattle()
    {
        UIDialogManager.Instance.ShowDialog<UIBattleDialog>();
    }
    
    public void OnClickCombine()
    {
        UIDialogManager.Instance.ShowDialog<UICombineDialog>();
    }
    
    [ContextMenu("Remove Card Mix Data")]
    public void RemoveCardMixData()
    {
        GameManager.Instance.RemoveAllSavedIDCards(false);
    }
}
