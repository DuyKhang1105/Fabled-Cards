using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using UnityEngine;

public class MainHome : MonoBehaviour
{
    public void OnClickOpenPack()
    {
        UIDialogManager.Instance.ShowDialog<UIOpenCardDialog>();
    }
    
    public void OnClickCombine()
    {
        UIDialogManager.Instance.ShowDialog<UICombineDialog>();
    }
}
