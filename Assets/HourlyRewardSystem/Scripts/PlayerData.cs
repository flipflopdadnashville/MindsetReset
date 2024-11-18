namespace GameBench
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerData : MonoBehaviour
    {
        public Text coinText;
        private void Awake()
        {
            Coins = PlayerPrefs.GetInt(COINS, 1000);
        }
        int _coins;
        public int Coins
        {
            get { return _coins; }
            set
            {
                _coins = Mathf.Max(0, value);
                coinText.text = _coins.ToString();
                PlayerPrefs.SetInt(COINS, Coins);
                PlayerPrefs.Save();
            }
        }
        public const string COINS = "PLAYER_COINS";
        //private void OnApplicationPause(bool pause)
        //{
        //    if (pause)
        //    {
        //        PlayerPrefs.SetInt(COINS, Coins);
        //        PlayerPrefs.Save();
        //    }
        //    else
        //    {
        //        Coins = PlayerPrefs.GetInt(COINS, 1000);
        //    }
        //}
        private static PlayerData _instance;
        public static PlayerData Instance
        { get { if (_instance == null) _instance = GameObject.FindObjectOfType<PlayerData>(); return _instance; } }

    }
}