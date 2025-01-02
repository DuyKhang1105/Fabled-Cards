using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KingCyber.Base;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private GameCardConfig gameCardConfig;
    
    public GameCardConfig GameCardConfig => gameCardConfig;
    
    
    const string SAVE_ID_KEY = "savedIDs";
    public void SaveIDCard(string id) {
        List<string> ids = GetSavedIDCards();
        ids.Add(id);
        PlayerPrefs.SetString("SAVE_ID_KEY", string.Join(",", ids));
        PlayerPrefs.Save();
    }
    
    public List<string> GetSavedIDCards() {
        string savedIDs = PlayerPrefs.GetString("SAVE_ID_KEY", "");
        if (string.IsNullOrEmpty(savedIDs)) {
            return new List<string>();
        }
        return savedIDs.Split(',').ToList();
    }
}
