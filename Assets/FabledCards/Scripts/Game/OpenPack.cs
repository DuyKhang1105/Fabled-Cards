using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using UnityEngine;

public class OpenPack : MonoBehaviour
{
    public void Open()
    {
        UIDialogManager.Instance.ShowDialog<UIOpenCardDialog>();
    }
}
