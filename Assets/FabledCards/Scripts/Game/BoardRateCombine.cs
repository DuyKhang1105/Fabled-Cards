using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardRateCombine : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> rateCombineTexts;
    [SerializeField] private List<string> strTypeRarity;

    public void ShowRateCombine(float[] rarityChances)
    {
        for (int i = 0; i < rarityChances.Length; i++)
        {
            rateCombineTexts[i].text =$"{strTypeRarity[i]}: {rarityChances[i]}";
        }
    }
}