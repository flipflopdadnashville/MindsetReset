using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.DreamOS;
using GameBench;
using DamageNumbersPro;


public class PortalBell : MonoBehaviour
{
    public MazecraftGameManager instance;
    public HourlyReward rewardSystem;
    public AudioSource audioSource;
    public AudioSource audioSourceTwo;
    public NotificationCreator notificationCreator;
    public Sprite spriteVariable;
    public Text rewardsQuestionTitleText;
    public Text rewardsQuestionDescriptionText;
    //private CustomCameraShake cameraShakeEffect;
    // public string[] notificationTitles;
    // public string[] notificationDescriptions;
    // public string[] logicTitles;
    // public string[] logicDescriptions;
    public UIManager uiManager;
    private GameObject[] portals;
    private int portalCount;
    private int colCount;
    private bool cleanupPortals = false;
    public WindowManager windowManager;
    public NotepadManager notepadApp;
    public DamageNumber numberPrefab;

/*    void Start(){
        InvokeRepeating("PortalCleanup", 2f, 3f);
    }

    void Update(){
        if(cleanupPortals == true){  
            //Debug.Log("Cleaning Up Portals equals: " + cleanupPortals);         
            DeletePortals(5);
        }

        // Creation of portal happens in MazeSpawner
        // if(instance.forestTerrain.activeInHierarchy == true){
        //     int r = Random.Range(0,500);
        //     if(r == 1){
        //         Debug.Log("Random number is: " + r);
        //         StartCoroutine(PlaceTerrainPortals());
        //     }
        // }
        // if(instance.forestTerrain.activeInHierarchy == false && instance.terrainPortalsSet == true){
        //     portals = GetPortals();

        //     foreach(GameObject portal in portals){
        //         if(portal.name != "Easy Portal"){
        //             Destroy(portal);
        //         }
        //     }
        //     terrainPortalsSet = false;
        // }

       
    }

    // IEnumerator PlaceTerrainPortals(){
    //     portals = GetPortals();
    //     GameObject[] terrainChunks = GameObject.FindGameObjectsWithTag("terrainChunk");
    //     Debug.Log("The number of terrain chunks is: " + terrainChunks.Length);

    //     int randomNum = Random.Range(0, terrainChunks.Length -1);
    //     GameObject terrainChunk = terrainChunks[randomNum];
    //     portals[Random.Range(1, portals.Length)].gameObject.transform.localPosition = new Vector3(terrainChunk.transform.localPosition.x + Random.Range(-10, 10), 0, terrainChunk.transform.localPosition.z + Random.Range(-10, 10));

    //     yield return new WaitForSeconds(2);
    // }
    
    private void OnCollisionEnter(Collision other) {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-35.0f, -135.0f));
        this.GetComponent<SphereCollider>().enabled = false;
        colCount++;
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "enemyBall"){
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //Debug.Log("Gong time! Playing AudioSamples/BOWLS/BOWLS7a");
            //string filepath = "AudioSamples/BOWLS7a";
            //audioSource.clip = Resources.Load<AudioClip>(filepath);
            audioSource.pitch = 1;
            audioSource.PlayOneShot(audioSource.clip, Random.Range(.5f, .7f));
            int adjustedPitch = Random.Range(1, 3);
            audioSourceTwo.pitch = adjustedPitch;
            audioSourceTwo.PlayOneShot(audioSource.clip, Random.Range(.5f, .7f));
            //audioSource.volume = Random.Range(.1f, .5f);

            for(int i=0; i < 20; i++){
                HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Soft);
            }
            
            Destroy(this.gameObject, audioSourceTwo.clip.length + 20f);
            //Debug.Log("From OnCollisionEnter, notification count is: " + instance.notificationCount);

            if(instance.isNotificationActive == false && instance.notificationCount < instance.notificationTitles.Count){
                // if(instance.topic == "FiveThings" && instance.notificationCount < 6){
                //     if(instance.notificationCount == 0){
                //         uiManager.ToggleGame();
                //         // Uncomment two following lines to do email
                //         //windowManager.OpenWindow();
                //         //notepadApp.OpenNote(0);

                //         // Comment this out is email is enabled
                //         //DisplayRewardsScreen();
                //     }
                //     else if(instance.notificationCount > 0 && instance.notificationCount < 4){
                //         DisplayRewardsScreen();
                //     }
                //     else if(instance.notificationCount == 4){
                //         DisplayRewardsScreen();
                //         uiManager.ToggleGame();
                //         windowManager.OpenWindow();
                //         notepadApp.OpenNote(1);
                //     }
                //     else{
                //         instance.notificationCount++;
                //         if(instance.isNotificationActive == true){
                //            instance.isNotificationActive = false;
                //         }
                //     }

                //     //instance.notificationCount++;
                //     //uiManager.ToggleGame(true);
                // }
                //else if(instance.topic == "Logic" && (instance.notificationCount < instance.notificationDescriptions.Count)){

                /////////////////////////////// JWR IMPORTANT - THIS IS WHERE THE REWARDS SCREEN AND CAMERA SHAKE GET INITIALIZED !!!!!!!!!!!!!!!!!
                ///

                if (instance.gameMode == MazecraftGameManager.GameMode.Question)
                {
                    CustomCameraShake cameraShakeEffect = GameObject.Find("Main Camera").GetComponent<CustomCameraShake>();
                    cameraShakeEffect.start = true;
                    DisplayRewardsScreen();
                }
                else if (instance.gameMode == MazecraftGameManager.GameMode.Affirmation)
                {


                    //uiManager.ToggleGame(true);
                    //}
                    //else{
                    //    instance.notificationCount++;
                    //}

                    int randomNumber = Random.Range(1, 10);

                    DamageNumber damageNumber = numberPrefab.Spawn(transform.position, instance.notificationDescriptions[Random.Range(0, instance.notificationDescriptions.Count - 1)]);

                    Destroy(gameObject);

                    if (randomNumber == 7)
                    {
                        PlayerData.Instance.Coins += 10;
                    }
                }
            }
        }

        if(instance.notificationCount > 5){
            StartCoroutine(SelfDestruct());
            int randomNum = Random.Range(0,30);
            if((randomNum == 3)){
                Debug.Log(randomNum);
                GameObject[] portals = GetPortals();
                Vector3 newPlayerPos = portals[Random.Range(0, portals.Length - 1)].transform.position;
                instance.activePlayer.transform.position = newPlayerPos;
            }
        }
    }

    public void DisplayRewardsScreen(){
        //Debug.Log("From DisplayRewardsScreen, notification count is: " + instance.notificationCount);
        // put these lines in for email
        //if(instance.topic == "FiveThings" && instance.notificationCount == 0){
        //    windowManager.CloseWindow();
        //}

        instance.rewardsCanvas.SetActive(true);
        rewardSystem.reward4ScreenOn = true;
        rewardSystem.rewardTimeInMinutes = 5f;
        rewardSystem.TimerOn = true;
        // if(instance.topic == "FiveThings"){
        //     rewardsQuestionTitleText.text = instance.notificationTitles[instance.notificationCount];
        //     rewardsQuestionDescriptionText.text = instance.notificationDescriptions[instance.notificationCount];
        // }
        // else if(instance.topic == "Logic"){
            rewardsQuestionTitleText.text = instance.notificationTitles[instance.notificationCount];
            rewardsQuestionDescriptionText.text = instance.notificationDescriptions[instance.notificationCount];
        //}
        instance.isNotificationActive = true;
        instance.notificationCount++;
    }

    public void CloseWindow(){
        windowManager.CloseWindow();
        //uiManager.ToggleGame();
        //uiManager.ToggleControls();
    }

    private void OnDestroy(){
        if(colCount > 1){
            instance.currentBombs++;
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(100f);

        if(audioSource.isPlaying == false && this.gameObject.name != "Easy Portal"){
            Destroy(this.gameObject);
        }

        if(instance.isNotificationActive == true){
            instance.isNotificationActive = false;
        }
    }

    private void PortalCleanup(){
        //only do if terrain isn't active
        if(instance.forestTerrain.activeInHierarchy == false){
            portals = GetPortals();
            if(portals.Length > 30){
                cleanupPortals = true;
            }
            else{
                cleanupPortals = false;
            }
        }
    }

    private int CountPortals(){
        GameObject[] portals = GetPortals();
        Debug.Log("Number of portals is: " + portals.Length);
        return portals.Length;
    }

    private void DeletePortals(int numberOfPortalsToDelete){
        GameObject[] portals = GetPortals();
        for(int i = 0; i <= numberOfPortalsToDelete - 1; i++){
            Destroy(portals[i]);
        }
    }

    private GameObject[] GetPortals(){
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Coin");
        return portals;
    }
*/}