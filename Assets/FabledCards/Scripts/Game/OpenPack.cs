using System;
using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using TMPro;
using UnityEngine;

public class OpenPack : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI packCountText;
    [SerializeField] private float[] rarityChances = { 0.5f, 0.4f, 0.1f };
    [SerializeField] private float cooldownTime = 10f;
    
    private bool canOpenPack = true;
    private const string lastOpenedKey = "LastChestOpenedTime";
    
    private void Start()
    {
        CheckCooldown();
    }

    public void ShowPack()
    {
        if (canOpenPack)
        {
            UIOpenCardDialog openCardDialog = UIDialogManager.Instance.ShowDialog<UIOpenCardDialog>();
            openCardDialog.SetData(rarityChances, OpenPackAction);
        }
        else
        {
            Debug.Log("Chest is on cooldown. Please wait.");
        }
    }

    private void OpenPackAction()
    {
        Debug.Log("Pack opened!");
        // Lưu thời gian mở rương cuối cùng
        PlayerPrefs.SetString(lastOpenedKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
            
        StartCoroutine(PackCooldown(cooldownTime));
    }

    private IEnumerator PackCooldown(float remainingCooldown)
    {
        canOpenPack = false;

        // Lặp lại mỗi giây để cập nhật thời gian đếm ngược
        while (remainingCooldown > 0)
        {
            packCountText.text = $"Cooldown: {remainingCooldown} seconds";
            yield return new WaitForSeconds(1);  // Chờ một giây
            remainingCooldown--;
        }

        packCountText.text = "Chest is ready";
        canOpenPack = true;
        Debug.Log("Chest is ready to be opened again.");
    }

    private void CheckCooldown()
    {
        // Kiểm tra xem thời gian mở rương cuối cùng có được lưu trữ không
        if (PlayerPrefs.HasKey(lastOpenedKey))
        {
            string lastOpenedString = PlayerPrefs.GetString(lastOpenedKey);
            DateTime lastOpenedTime = DateTime.Parse(lastOpenedString);
            TimeSpan timeSinceLastOpened = DateTime.Now - lastOpenedTime;

            if (timeSinceLastOpened.TotalSeconds < cooldownTime)
            {
                float remainingCooldown = cooldownTime - (float)timeSinceLastOpened.TotalSeconds;
                Debug.Log($"Cooldown in progress. Remaining time: {remainingCooldown} seconds.");
                StartCoroutine(PackCooldown(remainingCooldown));
            }
            else
            {
                canOpenPack = true;
                packCountText.text = "Chest is ready";
                Debug.Log("Chest is ready to be opened again.");
            }
        }
        else
        {
            canOpenPack = true;
            packCountText.text = "Chest is ready.";
        }
    }
}
