using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace KingCyber.Base.Data
{
    [System.Serializable]
    public class UserProfile
    {
        public string id;
        public string name;
        public string avatar;
        public string country;
        public string frame;
        public long coin;
        public long exp;
        public int level;
        public string data = "{}";
    }

    public class UserData : MonoSingleton<UserData>
    {
        private string LOCAL_KEY = "USER_DATA";
        public UserProfile userProfile;

        [HideInInspector]
        public UnityEvent<UserProfile> UserProfileChangedEvent = new UnityEvent<UserProfile>();

        private void Awake()
        {
            LoadUserProfile(null);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("User Data/Add 100 coins")]
#endif
        private static void TestAddCoin()
        {
            UserData.Instance.LoadUserProfile((userProfile) =>
            {
                userProfile.coin += 100;
                UserData.Instance.SaveUserProfile("coin", userProfile.coin);
            });
        }

        public void SetLevel(int level)
        {
            UserData.Instance.LoadUserProfile((userProfile) =>
            {
                userProfile.level = level;
                UserData.Instance.SaveUserProfile("level", userProfile.level);
            });
        }
        
        public int GetLevel()
        {
            return userProfile.level;
        }

        public async void LoadUserProfile(UnityAction<UserProfile> callback)
        {
            var userProfileJson = "";
            if (!PlayerPrefs.HasKey(LOCAL_KEY))
            {
                var newUserProfile = new UserProfile();
                newUserProfile.id = "guest";
                newUserProfile.name = "Guest";
                string countryCode = "";
                string countryName = "";
                GameUtils.GetCountryName(out countryCode, out countryName);
                newUserProfile.country = countryCode;
                newUserProfile.avatar = "avatar0";
                newUserProfile.frame = "frame";
                newUserProfile.coin = 10000;
                newUserProfile.level = 1;
                userProfileJson = JsonUtility.ToJson(newUserProfile);
            }
            else userProfileJson = PlayerPrefs.GetString(LOCAL_KEY);

            try
            {
                userProfile = JsonUtility.FromJson<UserProfile>(userProfileJson);
                callback?.Invoke(userProfile);
            }
            catch
            {
                callback?.Invoke(null);
            }
        }

        private async void SetUserProfile(UserProfile userProfile, UnityAction<UserProfile> callback = null)
        {
            var userProfileJson = JsonUtility.ToJson(userProfile);
            PlayerPrefs.SetString(LOCAL_KEY, userProfileJson);

            callback?.Invoke(userProfile);
            UserProfileChangedEvent?.Invoke(userProfile);
        }

        public void SaveUserProfile(string type, object value, UnityAction<UserProfile> callback = null)
        {
            if (userProfile != null)
            {
                switch (type)
                {
                    case "id":
                        userProfile.id = (string)value;
                        break;
                    case "name":
                        userProfile.name = (string)value;
                        break;
                    case "avatar":
                        userProfile.avatar = (string)value;
                        break;
                    case "frame":
                        userProfile.frame = (string)value;
                        break;
                    case "coin":
                        userProfile.coin = (long)value;
                        break;
                    case "exp":
                        userProfile.exp = (long)value;
                        break;
                    case "level":
                        userProfile.level = (int)value;
                        break;
                    case "data":
                        userProfile.data = JsonUtility.ToJson(value);
                        break;
                }
                SetUserProfile(userProfile, callback);
            }
        }

        public T GetOtherData<T>(string key)
        {
            if (!string.IsNullOrEmpty(userProfile.data))
            {
                var jData = JObject.Parse(userProfile.data);
                if (jData != null)
                    if (jData.TryGetValue(key, out JToken value))
                    {
                        return value.ToObject<T>(); // Convert JToken to the specified type T
                    }
            }
            return default; // Return the default value for type T (null for reference types, 0 for int, etc.)
        }

        public void SetOtherData(string key, object value)
        {
            var jData = !string.IsNullOrEmpty(userProfile.data) ? JObject.Parse(userProfile.data) : new JObject();
            jData[key] = JToken.FromObject(value); // Add or update the key-value pair
            userProfile.data = jData.ToString(); // Update the original data string with the new JSON
            SetUserProfile(userProfile, null);
        }

    }
}

