namespace GameBench
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    public class HourlyReward : MonoBehaviour
    {
        public MazecraftGameManager instance;
        public bool reward4ScreenOn = false;
        public float rewardTimeInMinutes = .25f;
        public Text prizeTXT;
        public Text currentCoins;
        public int[] randomPrizes;
        [Space(10)]
        public Image progressBar;
        public Animator chestAnimMain, chestAnimPage;
        public Button claimRewardBtn;
        public GameObject chestUIDialog, coinAnim;
        public GameObject revealAnswerButton;

        const int RewardAmount = 100;
        public Text timerText;
        public int prizeAmt;

        string textTime;
        private float time;
        // the lower you set the float, the faster the fill goes
        public float timeAvailable { get { return rewardTimeInMinutes * 3.5f; } }
        bool _timerOn = false;

        const string EXCITED = "excited", IDLE = "idle", CLICKED = "clicked";
        public bool TimerOn
        {
            get { return _timerOn; }
            set
            {
                _timerOn = value;
                claimRewardBtn.interactable = !_timerOn;
                if (chestAnimMain.gameObject.activeInHierarchy)
                    chestAnimMain.Play(_timerOn ? IDLE : EXCITED);
                if (chestAnimPage.gameObject.activeInHierarchy)
                    chestAnimPage.Play(_timerOn ? IDLE : EXCITED);
            }
        }
        public const string TIMER_KEY = "TIMER_REWARD", LASTSAVEDTIME = "LASTSAVEDTIME";

        DateTime dT;
        private void Start()
        {
            // if (!reward4ScreenOn)
            // {
            //     time = PlayerPrefs.GetFloat(TIMER_KEY);
            //     DateTime.TryParse(PlayerPrefs.GetString(LASTSAVEDTIME, DateTime.Now.ToString()), out dT);
            //     float seconds = (float)(DateTime.Now - dT).TotalSeconds;
            //     time += seconds;
            // }
            // else
            // {
            //     time = PlayerPrefs.GetFloat(TIMER_KEY);
            //     Debug.Log("From HourlyReward, time is: " + time);
            // }
            time = 1;
            TimerOn = true;
        }
        
        void Update()
        {
            if(currentCoins.text != PlayerData.Instance.Coins.ToString());
            currentCoins.text = PlayerData.Instance.Coins.ToString();

            //Debug.Log("Timer status is: " + _timerOn);
            if (!TimerOn)
                return;
            time += Time.deltaTime;
            //Debug.Log("time is: " + time);
            //Debug.Log("time available is: " + timeAvailable);
            float remainingTime = (timeAvailable - time);
            //Debug.Log("remaining time is: " + remainingTime);
            //Debug.Log("fill amount is: " + (1 - remainingTime / timeAvailable));
            progressBar.fillAmount = 1 - remainingTime / timeAvailable;

            if (remainingTime > 0)
            {
                TimeSpan t = TimeSpan.FromSeconds(remainingTime);
                //Debug.Log(string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds));
                timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            }
            else
            {
                TimerOn = false;
                timerText.text = "00:00:00";
                time = 1;
            }
        }
       
        public void ClaimPlayerReward()
        {
            prizeAmt = randomPrizes[UnityEngine.Random.Range(0, randomPrizes.Length)];
            //Debug.Log("Prize amount is: " + prizeAmt.ToString());
            prizeTXT.text = prizeAmt.ToString();
            chestAnimPage.Play(CLICKED);
            //RewardUser();
        }

        public void RewardUser()
        {  
            PlayerData.Instance.Coins += prizeAmt;
            //Debug.Log("Current coins is: " + PlayerData.Instance.Coins);
            currentCoins.text = PlayerData.Instance.Coins.ToString();
            time = 0;
            timerText.text = "00:00:00";
            TimerOn = true;
        }

        private void OnApplicationQuit()
        {
            OnApplicationPause(true);
        }
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                PlayerPrefs.SetString(LASTSAVEDTIME, DateTime.Now.ToString());
                PlayerPrefs.SetFloat(TIMER_KEY, time);
                PlayerPrefs.Save();
            }
            else
            {
                if (reward4ScreenOn)
                {
                    time = PlayerPrefs.GetFloat(TIMER_KEY);
                }
                else
                {
                    time = PlayerPrefs.GetFloat(TIMER_KEY);
                    DateTime.TryParse(PlayerPrefs.GetString(LASTSAVEDTIME, DateTime.Now.ToString()), out dT);
                    float seconds = (float)(DateTime.Now - dT).TotalSeconds;
                    time += seconds;
                }
            }
        }

        public void ShowHideDialog(bool state)
        {
            chestUIDialog.SetActive(state);
            revealAnswerButton.SetActive(false);
            if (chestAnimMain.gameObject.activeInHierarchy)
                chestAnimMain.Play(_timerOn ? IDLE : EXCITED);
            if (chestAnimPage.gameObject.activeInHierarchy)
                chestAnimPage.Play(_timerOn ? IDLE : EXCITED);
        }

        public void HideRewardCanvas()
        {
            if(instance.HourRewardUI.activeInHierarchy == false){
                instance.HourRewardUI.SetActive(true);
                revealAnswerButton.SetActive(true);
            }
            if(instance.HourRewardUI.activeInHierarchy){
                instance.HourRewardUI.SetActive(false);
            }

            instance.rewardsCanvas.SetActive(false);
            revealAnswerButton.SetActive(true);
        }

        public void SetCoinText(int price){
            //Debug.Log("From SetCoinText, current coins: " + PlayerData.Instance.Coins);
            int newCoins = PlayerData.Instance.Coins - price;
            PlayerData.Instance.Coins = newCoins;
            //Debug.Log("From SetCoinsText, newCoins is: " + newCoins);
            currentCoins.text = newCoins.ToString();
        }

        [ContextMenu("Delete Prefs")]
        void DoSomething()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted");

        }
        private static HourlyReward _instance;
        public static HourlyReward Instance
        { get { if (_instance == null) _instance = GameObject.FindObjectOfType<HourlyReward>(); return _instance; } }
    }
}