using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class NetworkManager : MonoBehaviour
    {
        // List
        public List<NetworkItem> networkItems = new List<NetworkItem>();

        // Resources
        public Transform networkListParent;
        public GameObject networkItem;
        public AudioSource feedbackSource;

        // Settings
        public bool isWifiTurnedOn = true;
        public bool hasConnection = true;
        public int currentNetworkIndex = 0;
        public Sprite signalWeak;
        public Sprite signalStrong;
        public Sprite signalBest;
        public AudioClip wrongPassSound;

        [System.Serializable]
        public class NetworkItem
        {
            public string networkTitle = "My Network";
            public string password = "password";
            [Range(1, 4)] public int signalPower = 3;
            [Range(0.1f, 100)] public float networkSpeed = 20;
            public bool hasPassword;
        }

        void Start()
        {
            hasConnection = false;
            ListNetworks();
        }

        public void ListNetworks()
        {
            foreach (Transform child in networkListParent)
                Destroy(child.gameObject);

            if (PlayerPrefs.HasKey("CurrentNetworkIndex") == true)
                currentNetworkIndex = PlayerPrefs.GetInt("CurrentNetworkIndex");

            for (int i = 0; i < networkItems.Count; i++)
            {
                int index = i;

                if (i == currentNetworkIndex && PlayerPrefs.HasKey("CurrentNetworkIndex") == true)
                {
                    GameObject networkObj = Instantiate(networkItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    networkObj.name = networkItems[index].networkTitle;
                    networkObj.transform.SetParent(networkListParent, false);

                    TextMeshProUGUI networkTxt = networkObj.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                    networkTxt.text = networkItems[index].networkTitle;

                    TMP_InputField passField = networkObj.transform.Find("Password").GetComponent<TMP_InputField>();
                    passField.gameObject.SetActive(false);

                    Image signalImg = networkObj.transform.Find("Icon").GetComponent<Image>();
                    Image lockedImg = networkObj.transform.Find("Locked").GetComponent<Image>();
                    GameObject indicatorObj = networkObj.transform.Find("Indicator").gameObject;

                    Button connectBtn = networkObj.transform.Find("Connect").GetComponent<Button>();
                    Button disconnectBtn = networkObj.transform.Find("Disconnect").GetComponent<Button>();

                    if (networkItems[networkObj.transform.GetSiblingIndex()].hasPassword == true)
                    {
                        lockedImg.gameObject.SetActive(true);

                        connectBtn.onClick.AddListener(delegate
                        {
                            if (passField.gameObject.activeSelf == true)
                            {
                                if (passField.text == networkItems[networkObj.transform.GetSiblingIndex()].password)
                                {
                                    connectBtn.gameObject.SetActive(false);
                                    disconnectBtn.gameObject.SetActive(true);
                                    indicatorObj.SetActive(true);

                                    if (PlayerPrefs.HasKey("ConnectedNetworkTitle") == true)
                                    {
                                        GameObject cNetwork = networkListParent.Find(PlayerPrefs.GetString("ConnectedNetworkTitle")).gameObject;
                                        Button dcBtn = cNetwork.transform.Find("Disconnect").GetComponent<Button>();
                                        dcBtn.onClick.Invoke();
                                    }

                                    passField.gameObject.SetActive(false);
                                    currentNetworkIndex = networkObj.transform.GetSiblingIndex();
                                    PlayerPrefs.SetString("ConnectedNetworkTitle", networkObj.name);
                                    PlayerPrefs.SetInt("CurrentNetworkIndex", currentNetworkIndex);
                                    hasConnection = true;
                                }

                                else
                                {
                                    if (feedbackSource != null)
                                        feedbackSource.PlayOneShot(wrongPassSound);
                                }
                            }

                            else
                            {
                                passField.gameObject.SetActive(true);
                            }
                        });
                    }

                    else if (networkItems[networkObj.transform.GetSiblingIndex()].hasPassword == false)
                    {
                        lockedImg.gameObject.SetActive(false);

                        connectBtn.onClick.AddListener(delegate
                        {
                            connectBtn.gameObject.SetActive(false);
                            disconnectBtn.gameObject.SetActive(true);
                            indicatorObj.SetActive(true);

                            if (PlayerPrefs.HasKey("ConnectedNetworkTitle") == true)
                            {
                                GameObject cNetwork = networkListParent.Find(PlayerPrefs.GetString("ConnectedNetworkTitle")).gameObject;
                                Button dcBtn = cNetwork.transform.Find("Disconnect").GetComponent<Button>();
                                dcBtn.onClick.Invoke();
                            }

                            currentNetworkIndex = networkObj.transform.GetSiblingIndex();
                            PlayerPrefs.SetString("ConnectedNetworkTitle", networkObj.name);
                            PlayerPrefs.SetInt("CurrentNetworkIndex", currentNetworkIndex);
                            hasConnection = true;
                        });
                    }

                    disconnectBtn.onClick.AddListener(delegate
                    {
                        connectBtn.gameObject.SetActive(true);
                        disconnectBtn.gameObject.SetActive(false);
                        indicatorObj.SetActive(false);
                        hasConnection = false;
                        PlayerPrefs.DeleteKey("CurrentNetworkIndex");
                        PlayerPrefs.DeleteKey("ConnectedNetworkTitle");
                    });

                    if (networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 1)
                        signalImg.sprite = signalWeak;

                    else if (networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 2
                        || networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 3)
                        signalImg.sprite = signalStrong;

                    else if (networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 4)
                        signalImg.sprite = signalBest;

                    connectBtn.gameObject.SetActive(false);
                    disconnectBtn.gameObject.SetActive(true);
                    indicatorObj.SetActive(true);
                    PlayerPrefs.SetString("ConnectedNetworkTitle", networkObj.name);
                    currentNetworkIndex = networkObj.transform.GetSiblingIndex();
                    hasConnection = true;
                }

                else
                {
                    GameObject networkObj = Instantiate(networkItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    networkObj.name = networkItems[index].networkTitle;
                    networkObj.transform.SetParent(networkListParent, false);

                    TextMeshProUGUI networkTxt = networkObj.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                    networkTxt.text = networkItems[index].networkTitle;

                    TMP_InputField passField = networkObj.transform.Find("Password").GetComponent<TMP_InputField>();
                    passField.gameObject.SetActive(false);

                    Image signalImg = networkObj.transform.Find("Icon").GetComponent<Image>();
                    Image lockedImg = networkObj.transform.Find("Locked").GetComponent<Image>();
                    GameObject indicatorObj = networkObj.transform.Find("Indicator").gameObject;

                    Button connectBtn = networkObj.transform.Find("Connect").GetComponent<Button>();
                    Button disconnectBtn = networkObj.transform.Find("Disconnect").GetComponent<Button>();

                    if (networkItems[networkObj.transform.GetSiblingIndex()].hasPassword == true)
                    {
                        lockedImg.gameObject.SetActive(true);

                        connectBtn.onClick.AddListener(delegate
                        {
                            if (passField.gameObject.activeSelf == true)
                            {
                                if (passField.text == networkItems[networkObj.transform.GetSiblingIndex()].password)
                                {
                                    connectBtn.gameObject.SetActive(false);
                                    disconnectBtn.gameObject.SetActive(true);
                                    indicatorObj.SetActive(true);

                                    if (PlayerPrefs.HasKey("ConnectedNetworkTitle") == true)
                                    {
                                        GameObject cNetwork = networkListParent.Find(PlayerPrefs.GetString("ConnectedNetworkTitle")).gameObject;
                                        Button dcBtn = cNetwork.transform.Find("Disconnect").GetComponent<Button>();
                                        dcBtn.onClick.Invoke();
                                    }

                                    currentNetworkIndex = networkObj.transform.GetSiblingIndex();
                                    PlayerPrefs.SetString("ConnectedNetworkTitle", networkObj.name);
                                    PlayerPrefs.SetInt("CurrentNetworkIndex", currentNetworkIndex);
                                    passField.gameObject.SetActive(false);
                                    hasConnection = true;
                                }

                                else
                                {
                                    if (feedbackSource != null)
                                        feedbackSource.PlayOneShot(wrongPassSound);
                                }
                            }

                            else
                            {
                                passField.gameObject.SetActive(true);
                            }
                        });
                    }

                    else
                    {
                        lockedImg.gameObject.SetActive(false);

                        connectBtn.onClick.AddListener(delegate
                        {
                            connectBtn.gameObject.SetActive(false);
                            disconnectBtn.gameObject.SetActive(true);
                            indicatorObj.SetActive(true);

                            if (PlayerPrefs.HasKey("ConnectedNetworkTitle") == true)
                            {
                                GameObject cNetwork = networkListParent.Find(PlayerPrefs.GetString("ConnectedNetworkTitle")).gameObject;
                                Button dcBtn = cNetwork.transform.Find("Disconnect").GetComponent<Button>();
                                dcBtn.onClick.Invoke();
                            }

                            currentNetworkIndex = networkObj.transform.GetSiblingIndex();
                            PlayerPrefs.SetString("ConnectedNetworkTitle", networkObj.name);
                            PlayerPrefs.SetInt("CurrentNetworkIndex", currentNetworkIndex);
                            hasConnection = true;
                        });
                    }

                    disconnectBtn.onClick.AddListener(delegate
                    {
                        connectBtn.gameObject.SetActive(true);
                        disconnectBtn.gameObject.SetActive(false);
                        indicatorObj.SetActive(false);
                        hasConnection = false;
                        PlayerPrefs.DeleteKey("CurrentNetworkIndex");
                        PlayerPrefs.DeleteKey("ConnectedNetworkTitle");
                    });

                    if (networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 1)
                        signalImg.sprite = signalWeak;

                    else if (networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 2
                        || networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 3)
                        signalImg.sprite = signalStrong;

                    else if (networkItems[networkObj.transform.GetSiblingIndex()].signalPower == 4)
                        signalImg.sprite = signalBest;

                    connectBtn.gameObject.SetActive(true);
                    disconnectBtn.gameObject.SetActive(false);
                }
            }
        }

        public void ClearNetworkList()
        {
            StartCoroutine("StartClearingList");
        }

        IEnumerator StartClearingList()
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < networkItems.Count; i++)
            {
                GameObject netObj = networkListParent.Find(networkItems[i].networkTitle).gameObject;
                Destroy(netObj);
            }

            StopCoroutine("StartClearingList");
        }

        public void CreateNetwork(string title, int signal, string password)
        {
            NetworkItem nitem = new NetworkItem();
            nitem.networkTitle = title;
            nitem.signalPower = signal;

            if (password == "" || password == null)
                nitem.hasPassword = false;
            
            else
            {
                nitem.hasPassword = true;
                nitem.password = password;
            }    

            networkItems.Add(nitem);
        }

        public void ConntectToNetwork(string title, string password)
        {
            for (int i = 0; i < networkItems.Count; i++)
            {
                if (title == networkItems[i].networkTitle && password == networkItems[i].password)
                {
                    if (PlayerPrefs.HasKey("ConnectedNetworkTitle") == true)
                    {
                        GameObject cNetwork = networkListParent.Find(PlayerPrefs.GetString("ConnectedNetworkTitle")).gameObject;
                        Button dcBtn = cNetwork.transform.Find("Disconnect").GetComponent<Button>();
                        dcBtn.onClick.Invoke();
                    }

                    currentNetworkIndex = i;
                    PlayerPrefs.SetString("ConnectedNetworkTitle", networkItems[currentNetworkIndex].networkTitle);
                    PlayerPrefs.SetInt("CurrentNetworkIndex", currentNetworkIndex);
                    hasConnection = true;
                    break;
                }
            }
        }

        public void DisconnectFromNetwork()
        {
            try
            {
                GameObject cNetwork = networkListParent.Find(PlayerPrefs.GetString("ConnectedNetworkTitle")).gameObject;
                Button dcBtn = cNetwork.transform.Find("Disconnect").GetComponent<Button>();
                dcBtn.onClick.Invoke();

                hasConnection = false;
                PlayerPrefs.DeleteKey("CurrentNetworkIndex");
                PlayerPrefs.DeleteKey("ConnectedNetworkTitle");
            }

            catch { }
        }

        public void AddNetwork()
        {
            NetworkItem nitem = new NetworkItem();
            nitem.networkTitle = "New Network";
            networkItems.Add(nitem);
        }
    }
}