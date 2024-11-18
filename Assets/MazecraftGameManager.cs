using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using Michsky.DreamOS;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using MegaFiers;
using DigitalRuby.SimpleLUT;

public class MazecraftGameManager : MonoBehaviour
{
    public static MazecraftGameManager gm;
    public UserManager userManager;
    public VolumeControl volumeControlManager;
    public Celeron timeManager;
    public GameObject signalGenerator;
    //public GeoCamera geoCamera;
    public List<string> topics;
    public string topic;
    public int topicCounter;
    public List<string> dataFiles;
    public string fileName;
    public int dataFileCounter;
    public List<string> notificationTitles;
    public List<string> notificationDescriptions;
    /*public enum GameMode
    {
    Question,
    Affirmation
    }*/
    /*public enum MazeStructure
    {
    Normal,
    PillarsOnly
    }*/
    //public GameObject originalMaze;
    //public GameObject rotatingMaze;
    //public GameObject[] sGeometryObjects;
    public UnityEngine.Object[] materialArray;
    //public List<GameObject> cameraArray = new List<GameObject>();
    public GameObject activeCamera;
    public int currentMaterial = 0;
    //public GameObject redDragon;
    //public GameObject gumdrop;
    //public GameObject glasses;
    //public GameObject root;
    //public GameObject playerSkin;
    //private string playerName;
    //public GameObject playerTwo;
    //public GameObject body;
    //public GameObject leftPlate;
    //public GameObject rightPlate;
    //public GameObject rearPlate;
    //public Vector3 playerTwoOPos;
    //public GameObject activePlayer;
    //public GameObject game;
    //public GameMode gameMode;
    //public MazeStructure mazeStructure;
    //public Transform cameraOneTarget;
    public GameObject world;
    public GameObject dreamOS;
    public GameObject weatherMaker;
    public GameObject weatherMakerConfig;
    //public GameObject uiManagerPanel;
    //public GameObject ocean;
    //public GameObject forestTerrain;
    //public GameObject bottle;
    //public GameObject portal;
    //public GameObject controlsHolder;
    //public GameObject joystick;
    //public GameObject newMazeButton;
    //public GameObject hideStaticMazesButton;
    //public GameObject hideRotatingMazeButton;
    //public GameObject destroyStaticMazesButton;
    //public GameObject bombButton;
    //public GameObject jumpButton;
    //public GameObject cameraZoomInButton;
    //public GameObject cameraZoomOutButton;
    //public bool cameraZoomIn = false;
    //public bool cameraZoomOut = false;
    //public GameObject cameraOrbitButton;
    //public float distance = 35;
    public bool isNotificationActive = false;
    public int notificationCount = 0;
    public GameObject rewardsCanvas;
    public GameObject HourRewardUI;
    //public int currentBombs;
    //public GameObject pole;
    //public bool isFishing = true;
    //public bool chaserGameOn = false;
    //public GameObject vesicaPlane;
    public List<Dictionary<string, object>> processedData;
    private List<Dictionary<string, object>> filteredData = new List<Dictionary<string, object>>();
    public TMP_InputField answerText;
    //public int numberCorrect = 0;
    //public int numberIncorrect = 0;
    public NotificationCreator notificationCreator;
    public bool systemNotificationsEnabled = true;
    public GameObject systemNotificationsIcon;
    public Vector3 screenCenter;
    public Vector3 screenHeight;
    public Vector3 screenWidth;
    //public List<string> AudioSampleFiles;
    //public Slider amountSlider;
    //public Slider hueSlider;
    //public Slider saturationSlider;
    //public Slider brightnessSlider;
    //public Slider contrastSlider;
    //public Slider sharpnessSlider;
    //public Slider vignetteSlider;
    public Slider rippleScaleSlider;
    public Slider rippleSpeedSlider;
    public Slider rippleFrequencySlider;
    public Slider rippleSizeSlider;
    public GameObject lightRays;
    public Slider lightSpeedSlider;
    public Slider lightSizeSlider;
    public Slider lightSkewSlider;
    public Slider lightShearSlider;
    public Slider lightFadeSlider;
    public Slider lightContrastSlider;
    public Slider lightRSlider;
    public Slider lightGSlider;
    public Slider lightBSlider;
    public Slider volumeSlider;
    public TMP_InputField frequencyText;
    public Slider frequencySlider;
    public Slider volumeSliderTwo;
    public TMP_InputField frequencyTextTwo;
    public Slider frequencySliderTwo;
    public Slider volumeSliderThree;
    public Slider frequencySliderThree;
    public Slider volumeSliderFour;
    public Slider frequencySliderFour;
    public int breathSpeedIn = 4;
    public int breathSpeedOut = 4;
    public int breathOutRetention = 2;
    float delayBetweenPresses = 0.25f;
    bool pressedFirstTime = false;
    float lastPressedTime;
    public List<string> _preloadedShaderKeys = new List<string>();
    public List<string> _preloadedTextureKeys = new List<string>();
    public List<string> _preloadedMaterialKeys = new List<string>();
    //private bool hasEarned = false;
    //private GameObject earnedVersion;
    //public GameObject cameraToggleButton;
    //public GameObject orbitCameraTarget;
    public SimpleLUT simpleLUT;
    private bool blackScreen = false;


    CSVReader reader;
    TextAsset data;

    void Awake(){
        Application.targetFrameRate = 30;
        DebugManager.instance.enableRuntimeUI = false;
        StartCoroutine(PreloadAddressableShaderKeys("Shader"));
        StartCoroutine(PreloadAddressableTextureKeys("Texture"));
        StartCoroutine(PreloadAddressableMaterialKeys("Material"));
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        /*if(hasEarned == false)
        {
            cameraToggleButton.SetActive(false);
            cameraZoomInButton.SetActive(false);
            cameraZoomOutButton.SetActive(false);
            cameraOrbitButton.SetActive(false);
        }*/

        //Debug.Log("screenCenter " + screenCenter);
        //screenCenter (800.00, 450.00, 0.00)
 
        screenHeight = new Vector3(Screen.width / 2, Screen.height, 0);
        //Debug.Log("screenHeight " + screenHeight);
 
        screenWidth = new Vector3(Screen.width, Screen.height/2, 0);
        //Debug.Log("screenWidth " + screenWidth);

        if(gm != null){
            Destroy(gameObject);
        }
        else{
            gm = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(world);
            //DontDestroyOnLoad(timeManager);
            //DontDestroyOnLoad(signalGenerator);
        }

        // GameObject.Find("Naninovel<Runtime>").SetActive(false);
        //gameMode = GameMode.Affirmation;
        //topics.Add("FiveThings");
        //topics.Add("Logic");

        //mazeStructure = MazeStructure.Normal;
        //gumdrop.SetActive(false);
        //vesicaPlane.SetActive(false);
        //playerTwoOPos = playerTwo.transform.localPosition;
        //Debug.Log("Fidge oPos is: " + playerTwo.transform.localPosition);
        //playerTwo.SetActive(false);
        //cameraOneTarget = playerTwo.transform;
        materialArray = Resources.LoadAll("MazeMaterials", typeof(Material));
        //sGeometryObjects = GameObject.FindGameObjectsWithTag("sGeometry");
        /*activePlayer = GetActivePlayer();
        if(activePlayer == null){
            activePlayer = playerTwo;
        }*/
        // joystick.SetActive(false);
        // newMazeButton.SetActive(false);
        // hideStaticMazesButton.SetActive(false);
        // hideRotatingMazeButton.SetActive(false);
        // destroyStaticMazesButton.SetActive(false);
        // bombButton.SetActive(false);
        // jumpButton.SetActive(false);
        //controlsHolder.SetActive(false);
        //rewardsCanvas.SetActive(false);
        //pole.SetActive(false);
        weatherMaker.SetActive(false);
        //earnedVersion = GameObject.Find("EarnedVersion");

        /*if (hasEarned == false)
        {
            //can't destroy because too much depends on it. Let's just always disable it if someone tries to enable it.
            //Destroy(GameObject.Find("EarnedVersion"));
                GameObject.Find("EarnedVersion").SetActive(false);
        }*/

        InvokeRepeating("UnloadUnusedResources", 10, 30);
    }

    private void Start(){
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "\\Data");
        FileInfo[] info = dir.GetFiles("*.csv");
        foreach (FileInfo f in info)
        {
            dataFiles.Add(f.Name);
        }
        dataFileCounter = 0;

        fileName = "Data//" + dataFiles[dataFileCounter];
        //Debug.Log(fileName);
        string file = Path.Combine(Application.streamingAssetsPath, fileName);
        if (System.IO.File.Exists(file))
        {
            reader = GameObject.Find("_GameScope").GetComponent<CSVReader>();
            data = reader.LoadData(Path.Combine(Application.streamingAssetsPath, fileName));
            processedData = GetData();
            LoadTopics(processedData);
            //foreach(string topic in topics){
            //    Debug.Log("Topic is: " + topic);
            //}
            topicCounter = 0;
            topic = topics[topicCounter];
            filteredData = FilterData(processedData, topic);
            LoadQuestions(filteredData);
        }
        else
        {
            //SetNotificationSettings("No quiz game data file", "Please load a quiz game data file", "", GetNotificationStatus());
            //Debug.Log("No data file is present for the quiz game");
        }
        //UnityEngine.iOS.Device.deferSystemGesturesMode = UnityEngine.iOS.SystemGestureDeferMode.BottomEdge;
    }

    void Update(){
        Cursor.visible = false;
        /*if (hasEarned == false)
        {
            if (earnedVersion.activeInHierarchy == true)
            {
                earnedVersion.SetActive(false);
            }
        }*/

        
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && (simpleLUT.Brightness == -1))
        {
            simpleLUT.Brightness = 0;
            blackScreen = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("pressed escape");
            if (pressedFirstTime) // we've already pressed the button a first time, we check if the 2nd time is fast enough to be considered a double-press
            {
                bool isDoublePress = Time.time - lastPressedTime <= delayBetweenPresses;

                if (isDoublePress)
                {
                    //Debug.Log("pressed escape twice. Exiting game");
                    pressedFirstTime = false;
                }
            }
            else // we've not already pressed the button a first time
            {
                pressedFirstTime = true; // we tell this is the first time
            }

            lastPressedTime = Time.time;
        }

        if (pressedFirstTime && Time.time - lastPressedTime > delayBetweenPresses) // we're waiting for a 2nd key press but we've reached the delay, we can't consider it a double press anymore
        {
            // note that by checking first for pressedFirstTime in the condition above, we make the program skip the next part of the condition if it's not true,
            // thus we're avoiding the "heavy computation" (the substraction and comparison) most of the time.
            // we're also making sure we've pressed the key a first time before doing the computation, which avoids doing the computation while lastPressedTime is still uninitialized

            /*string path = Application.persistentDataPath + "/Reports";
            string date = System.DateTime.UtcNow.ToString("yyyyMMdd");
            bool pathExists = Directory.Exists(Application.persistentDataPath + "/Reports");
            if (!pathExists)
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(Application.persistentDataPath + "/Reports/" + date + "_Report.txt"))
            {
                StreamWriter sr = new StreamWriter(Application.persistentDataPath + "/Reports/" + date + "_Report.txt", true);
                playerName = userManager.firstName + " " + userManager.lastName;
                //Debug.Log("Player name is: " + playerName + " and " + "Number correct is: " + numberCorrect + " and number incorrect is: " + numberIncorrect);
                sr.WriteLine("Username: " + playerName);
                sr.WriteLine("Number Correct: " + numberCorrect);
                sr.WriteLine("Number Incorrect: " + numberIncorrect);
                sr.Close();
            }*/

            //Debug.Log("File written to: " + Application.persistentDataPath + "/Reports/" + date + "_Report.txt");

            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.Escape)){
        //    //SetNotificationSettings("Goodbye!", "", "", GetNotificationStatus());
        //    string path = Application.persistentDataPath + "/Reports";
        //    string date = System.DateTime.UtcNow.ToString("yyyyMMdd");
        //    bool pathExists = Directory.Exists(Application.persistentDataPath + "/Reports");
        //    if (!pathExists){
        //        Directory.CreateDirectory(path);
        //    }
        //    if (!File.Exists(Application.persistentDataPath + "/Reports/" + date + "_Report.txt")){
        //        StreamWriter sr = new StreamWriter(Application.persistentDataPath + "/Reports/" + date + "_Report.txt", true);
        //        playerName = userManager.firstName + " " + userManager.lastName;
        //        //Debug.Log("Player name is: " + playerName + " and " + "Number correct is: " + numberCorrect + " and number incorrect is: " + numberIncorrect);
        //        sr.WriteLine("Username: " + playerName);
        //        sr.WriteLine("Number Correct: " + numberCorrect);
        //        sr.WriteLine("Number Incorrect: " + numberIncorrect);
        //        sr.Close();
        //    }

        //    //Debug.Log("File written to: " + Application.persistentDataPath + "/Reports/" + date + "_Report.txt");

        //    Application.Quit();
        //}

        /*if (activeCamera.name != "MoodscapeCamera"){
            lightRays.SetActive(false);
            Cursor.visible = true;
        }
        else if(activeCamera.name == "MoodscapeCamera"){
            lightRays.SetActive(true);
            Cursor.visible = false;

            if(ocean.activeInHierarchy == true)
            {
                ocean.SetActive(false);
            }
        }
        */

        //for some reason the portal collider keeps getting disabled. This is my hack to stop that from happening for now
        /*if (portal.GetComponent<SphereCollider>().enabled == false){
            portal.GetComponent<SphereCollider>().enabled = true;
        }*/
        
        if(rewardsCanvas.activeInHierarchy == false){
            /*if(Input.GetKeyDown(KeyCode.T)){
                ToggleTopic();
            }*/
        }

        /*if (rewardsCanvas.activeInHierarchy == false && activeCamera.name == "Main Camera")
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                ToggleDataFile();
            }
        }*/

        if (SceneManager.GetActiveScene().name == "BeStill")
            {
            //Debug.Log("Looking for cameras");
            //GetCameras();
            //cameraArray.Add(GameObject.Find("Main Camera"));
            //cameraArray.Add(GameObject.Find("CameraTwo"));
            //cameraArray.Add(GameObject.Find("MoodscapeCamera"));
            activeCamera = GameObject.Find("BackgroundCamera");
            activeCamera = GameObject.Find("MoodscapeCamera");
            simpleLUT = activeCamera.GetComponent<SimpleLUT>();
            //geoCamera = cameraArray[0].GetComponent<GeoCamera>();
            //cameraArray[2].GetComponent<moodscapeCameraOrbit>().target = orbitCameraTarget;
            activeCamera.GetComponent<moodscapeCameraOrbit>().enabled = false;
        }

        /*if(forestTerrain.activeInHierarchy == false){
            if(playerTwo.transform.position.z < 11.4f || playerTwo.transform.position.z > 11.6){
                    playerTwo.transform.localPosition = new Vector3(playerTwo.transform.localPosition.x, playerTwo.transform.localPosition.y, 11.5f);
            }
        }

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C)){
            SetGameMode();
        }

        if(cameraZoomIn == true){
            ZoomIn();
        }

        if(cameraZoomOut == true){
            ZoomOut();
        }*/
    }

    private IEnumerator PreloadAddressableShaderKeys(string label)
    {
        //find all the locations with label "SpaceHazards"
        var loadResourceLocationsHandle
            = Addressables.LoadResourceLocationsAsync(label, typeof(Shader));

        if (!loadResourceLocationsHandle.IsDone)
            yield return loadResourceLocationsHandle;

        //Debug.Log("loadResourceLocationsHandle result is: " + loadResourceLocationsHandle.Result);

        //start each location loading
        List<AsyncOperationHandle> opList = new List<AsyncOperationHandle>();

        foreach (IResourceLocation location in loadResourceLocationsHandle.Result)
        {
            AsyncOperationHandle<Shader> loadAssetHandle
                = Addressables.LoadAssetAsync<Shader>(location);
            loadAssetHandle.Completed +=
                obj => { _preloadedShaderKeys.Add(location.PrimaryKey); };
            opList.Add(loadAssetHandle);
        }

        //create a GroupOperation to wait on all the above loads at once. 
        var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(opList);

        if (!groupOp.IsDone)
            yield return groupOp;

        Addressables.Release(loadResourceLocationsHandle);

        //take a gander at our results.
        //foreach (var item in _preloadedShaderKeys)
        //{
        //    Debug.Log(item);
        //}
    }

    private IEnumerator PreloadAddressableTextureKeys(string label)
    {
        //find all the locations with label "SpaceHazards"
        var loadResourceLocationsHandle
            = Addressables.LoadResourceLocationsAsync(label, typeof(Texture2D));

        if (!loadResourceLocationsHandle.IsDone)
            yield return loadResourceLocationsHandle;

        //Debug.Log("loadResourceLocationsHandle result is: " + loadResourceLocationsHandle.Result);

        //start each location loading
        List<AsyncOperationHandle> opList = new List<AsyncOperationHandle>();

        foreach (IResourceLocation location in loadResourceLocationsHandle.Result)
        {
            AsyncOperationHandle<Texture2D> loadAssetHandle
                = Addressables.LoadAssetAsync<Texture2D>(location);
            loadAssetHandle.Completed +=
                obj => { _preloadedTextureKeys.Add(location.PrimaryKey); };
            opList.Add(loadAssetHandle);
        }

        //create a GroupOperation to wait on all the above loads at once. 
        var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(opList);

        if (!groupOp.IsDone)
            yield return groupOp;

        Addressables.Release(loadResourceLocationsHandle);

        //take a gander at our results.
        //foreach (var item in _preloadedTextureKeys)
        //{
        //    Debug.Log(item);
        //}
    }

    private IEnumerator PreloadAddressableMaterialKeys(string label)
    {
        //find all the locations with label "SpaceHazards"
        var loadResourceLocationsHandle
            = Addressables.LoadResourceLocationsAsync(label, typeof(Material));

        if (!loadResourceLocationsHandle.IsDone)
            yield return loadResourceLocationsHandle;

        //Debug.Log("loadResourceLocationsHandle result is: " + loadResourceLocationsHandle.Result);

        //start each location loading
        List<AsyncOperationHandle> opList = new List<AsyncOperationHandle>();

        foreach (IResourceLocation location in loadResourceLocationsHandle.Result)
        {
            AsyncOperationHandle<Material> loadAssetHandle
                = Addressables.LoadAssetAsync<Material>(location);
            loadAssetHandle.Completed +=
                obj => { _preloadedMaterialKeys.Add(location.PrimaryKey); };
            opList.Add(loadAssetHandle);
        }

        //create a GroupOperation to wait on all the above loads at once. 
        var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(opList);

        if (!groupOp.IsDone)
            yield return groupOp;

        Addressables.Release(loadResourceLocationsHandle);

        //take a gander at our results.
        //foreach (var item in _preloadedMaterialKeys)
        //{
        //    Debug.Log(item);
        //}
    }

    //private List<GameObject> GetCameras(){
        /*cameraArray.Add(GameObject.Find("Main Camera"));
        cameraArray.Add(GameObject.Find("CameraTwo"));
        //cameraArray.Add(GameObject.Find("EQ Camera"));
        //cameraArray.Add(GameObject.Find("BackgroundCamera"));
        cameraArray.Add(GameObject.Find("MoodscapeCamera"));*/

        /*foreach (GameObject camera in cameraArray)
        {
            Debug.Log(camera.name);
        }*/

        //Debug.Log("From GetCameras, Active Camera is: " + activeCamera.name);

        //return cameraArray;
    //}

  //  public Material GetNewMaterial(){
  //      Material newMaterial;

		//if(currentMaterial == materialArray.Length){
		//	currentMaterial = 0;
		//	//newMaterial = (Material)materialArray[currentMaterial];
		//}

		//currentMaterial = currentMaterial + 1;
		//newMaterial = (Material)materialArray[currentMaterial];

  //      return newMaterial;	
  //  }

    /*private void SetGameModeSettings(GameMode gameMode){
        //stub for future use
    }*/

    /*public GameObject GetActivePlayer(){
        //if(glasses.activeInHierarchy == true){
        //    activePlayer = glasses;
        //    //Debug.Log("From GetActivePlayer, activePlayer is: " + activePlayer);
        //    //return gumdrop;
        //}
        //else if(playerTwo.activeInHierarchy == true){
            activePlayer = playerTwo;
            //return playerTwo;
        //}
        //else if(redDragon.activeInHierarchy == true){
        //    activePlayer = redDragon;
        //    //return redDragon;
        //}

        //SetNotificationSettings("Active player is " + activePlayer.name, "", "", GetNotificationStatus());

        return activePlayer;
    }*/

    public void ToggleLightingSettings()
    {
        //simpleLUT.ToggleLUTSettingsRandomization();
        simpleLUT.RandomizeLUTSettings();
    }

    /*public void ToggleCamera(){
        activeCamera = GetActiveCamera();
        //Debug.Log("From Toggle Camera, Active Camera is: " + activeCamera.name);
        activeCamera.GetComponent<Camera>().enabled = false;

        if(activeCamera.name == "Main Camera"){
            if (weatherMaker.activeInHierarchy)
            {
                weatherMaker.SetActive(false);
            }

            activeCamera = cameraArray[1];
            if(portal.activeInHierarchy == true){
                portal.SetActive(false);
            }

            cameraArray[1].transform.position = new Vector3(activePlayer.transform.position.x, 10, 0);
            ToggleCameraZoomButtons(false);
            geoCamera.orbit = false;
            SetNotificationSettings("Press Space to go fishing!", "", "", GetNotificationStatus());
        }
        else if(activeCamera.name == "CameraTwo"){
            activeCamera = cameraArray[2];

            if (portal.activeInHierarchy == true)
            {
                portal.SetActive(false);
            }

            //if(vesicaPlane.activeInHierarchy == false){
            //    vesicaPlane.SetActive(true);
            //}

            foreach (GameObject sGeometryObject in sGeometryObjects)
            {
                sGeometryObject.SetActive(false);
            }

            if (game.activeInHierarchy == true)
            {
                game.SetActive(false);
            }

            //if(portal.activeInHierarchy == false){
            //    portal.SetActive(true);
            //}

            //cameraArray[2].transform.position = new Vector3(cameraArray[2].transform.position.x, 20, -100);
            //ToggleCameraZoomButtons(false);
            //geoCamera.orbit = false;
        }
        else if(activeCamera.name == "EQ Camera"){
            if (weatherMaker.activeInHierarchy)
            {
                weatherMaker.SetActive(false);
            }

            activeCamera = cameraArray[3];

            if(portal.activeInHierarchy == true){
                portal.SetActive(false);
            }

            //if(vesicaPlane.activeInHierarchy == false){
            //    vesicaPlane.SetActive(true);
            //}
            
            foreach(GameObject sGeometryObject in sGeometryObjects){
                    sGeometryObject.SetActive(false);
            }

            if(game.activeInHierarchy == true){
                game.SetActive(false);
            }
        }
        // else if(activeCamera.name == "BackgroundCamera"){
        //     activeCamera = cameraArray[0];
        //     ToggleCameraZoomButtons(true);
        //     geoCamera.orbit = false;
        // }
        else if(activeCamera.name == "MoodscapeCamera"){
            if (weatherMaker.activeInHierarchy)
            {
                weatherMaker.SetActive(false);
            }
            activeCamera = cameraArray[0];
            ToggleCameraZoomButtons(true);
            if(game.activeInHierarchy == false){
                game.SetActive(true);
            }
            cameraArray[0].transform.position = new Vector3(cameraOneTarget.position.x, cameraOneTarget.position.y + 2, 15);
            
            if(activeCamera.GetComponent<GeoCamera>().enabled == false){
                activeCamera.GetComponent<GeoCamera>().enabled = true;
            }

            playerTwo.SetActive(true);
            if (playerTwo.GetComponent<Rigidbody>().useGravity == false)
            {
                playerTwo.GetComponent<Rigidbody>().useGravity = true;
            }

            activePlayer = playerTwo;
        }
        else{
            //Debug.Log("Oopsie... problem with setting the camera");
        }

        //Debug.Log("From togglecamera, activecamera is now: " + activeCamera);

        activeCamera.GetComponent<Camera>().enabled = true;
        volumeControlManager.SetColliders();
        SetNotificationSettings("Active camera is " + activeCamera.name, "", "", GetNotificationStatus());
    }*/

    //public GameObject GetActiveCamera(){
        /*if(GameObject.Find("Main Camera").GetComponent<Camera>().enabled == true){
            activeCamera = cameraArray[0];
            if(activeCamera.GetComponent<GeoCamera>().enabled == false){
                activeCamera.GetComponent<GeoCamera>().enabled = true;
            }
        }
        else if(GameObject.Find("CameraTwo").GetComponent<Camera>().enabled == true){
            activeCamera = cameraArray[1];
        }
        *//*else if(GameObject.Find("BackgroundCamera").GetComponent<Camera>().enabled == true){
            activeCamera = cameraArray[2];
        }*//*
        else if(GameObject.Find("MoodscapeCamera").GetComponent<Camera>().enabled == true){
            activeCamera = cameraArray[2];
        }
        else{
            //Debug.Log("Oopsie... problem with setting the camera");
        }*/

        // JWR: added 8/6/2024 because there's only one camera now
        //activeCamera = cameraArray[2];

        //Debug.Log("Active camera is: " + activeCamera);
        //return activeCamera;
    //}

    /*public void SetCameraZoomIn(){
        cameraZoomIn = !cameraZoomIn;
    }

    public void SetCameraZoomOut(){
        cameraZoomOut = !cameraZoomOut;
    }

    public void ZoomOut(){
        GameObject currentCamera = GetActiveCamera();
        // Debug.Log("Current camera name is: " + currentCamera.name);
        // Debug.Log("Distance is: " + distance);
        //if(currentCamera.name == "Main Camera"){
            distance = distance + .3f;
            currentCamera.transform.position = new Vector3(cameraOneTarget.position.x, cameraOneTarget.position.y, distance);
        //}
    }

    public void ZoomIn(){
            GameObject currentCamera = GetActiveCamera();
            //if(currentCamera.name == "Main Camera"){
                distance = distance - .3f;
                currentCamera.transform.position = new Vector3(cameraOneTarget.position.x, cameraOneTarget.position.y, distance);
            //}
    }

    private void ToggleCameraZoomButtons(bool visible){
        if(visible == true){
            cameraZoomInButton.SetActive(true);
            cameraZoomOutButton.SetActive(true);
            cameraOrbitButton.SetActive(true);
        }
        else{
            cameraZoomInButton.SetActive(false);
            cameraZoomOutButton.SetActive(false);
            cameraOrbitButton.SetActive(false);
        }
    }

    public void ToggleOrbit(){
        if(geoCamera.orbit == true){
            distance = 32;
            geoCamera.orbit = false;
            SetNotificationSettings("Camera orbit disabled", "", "", GetNotificationStatus());
        }
        else if(geoCamera.orbit == false){
            geoCamera.orbit = true;
            SetNotificationSettings("Camera orbit enabled", "", "", GetNotificationStatus());
        }
    }*/

    /*public void SetGameMode(){
        if(gameMode == GameMode.Question){
            // Debug.Log("CTRL + C pressed");
            gameMode = GameMode.Affirmation;
            // REMOVE THIS ONCE YOU FIGURE OUT HOW TO GET GUMDROP BACK TO NORMAL POSITION
            SetGameModeSettings(gameMode);
        }
        else if(gameMode == GameMode.Affirmation){
            // Debug.Log("CTRL + C pressed");
            gameMode = GameMode.Question;
            // REMOVE THIS ONCE YOU FIGURE OUT HOW TO GET GUMDROP BACK TO NORMAL POSITION
            SetGameModeSettings(gameMode);
        }
        else{
            Debug.Log("Invalid Game Mode. Fix it.");
        }

        SetNotificationSettings("Game mode is " + gameMode, "", "", GetNotificationStatus());
    }*/

    private List<Dictionary<string, object>> GetData(){
        List<Dictionary<string, object>> processedData = reader.Read(data,1,199, true);
        return processedData;
    }

    private void LoadTopics(List<Dictionary<string, object>> processedData){
        if(topics.Count > 0)
        {
            topics.Clear();
        }

        foreach(Dictionary<string, object> item in processedData){
            foreach(KeyValuePair<string, object> i in item)
            {
                //Debug.Log("Key is: " + i.Key);

                if(i.Key == "name" || i.Key == "\"name\""){
                    //Debug.Log(i.Value.ToString());
                    string topic = i.Value.ToString();
                    if(!topics.Contains(topic)){
                        //Debug.Log("Adding " + i.Value + " to topics...");
                        topics.Add(topic);
                    }
                }
            }
        }
    }

    private List<Dictionary<string, object>> FilterData(List<Dictionary<string, object>> processedData, string newTopic){
        if(filteredData.Count > 0){
            filteredData.Clear();
        }

        //Debug.Log("Filtering data on topic: " + newTopic);
        foreach(Dictionary<string, object> item in processedData){
            foreach(KeyValuePair<string, object> i in item){
                //Debug.Log("Key is: " + i.Key);
                if((i.Key == "name" || i.Key == "\"name\"") && (i.Value.ToString() == newTopic)){
                    foreach(KeyValuePair<string, object> j in item){
                    //string topic = i.Value.ToString();
                    //Debug.Log("topic is: " + topic + " and newTopic is: " + newTopic);
                    //if(topic == newTopic){
                        //Debug.Log("Adding item to filteredData.");
                        Dictionary<string, object> filterMatch = new Dictionary<string, object>();
                        filterMatch.Add(j.Key, j.Value);
                        //if(!filteredData.Contains(filterMatch)){
                        filteredData.Add(filterMatch);
                        //Debug.Log("FilteredData count is now: " +filteredData.Count);
                        //}
                    //}
                    }
                }
            }
        }

        // foreach(Dictionary<string, object> item in filteredData){
        //     foreach(KeyValuePair<string, object> i in item)
        //     {
        //         Debug.Log("Key is: " + i.Key);
        //         Debug.Log(i.Value.ToString());
        //     }
        // }

        return filteredData;
    }

    private void LoadQuestions(List<Dictionary<string, object>> filteredData){
        notificationTitles.Clear();
        notificationDescriptions.Clear();

        foreach(Dictionary<string, object> item in filteredData){
            foreach(KeyValuePair<string, object> i in item)
            {
                // Debug.Log("Key is: " + i.Key);
                // Debug.Log(i.Value.ToString());
                if(i.Key == "title" || i.Key == "\"title\""){
                    notificationTitles.Add(i.Value.ToString());
                }
                if(i.Key == "description" || i.Key == "\"description\""){
                    notificationDescriptions.Add(i.Value.ToString());
                }
            }
        }

        /*if(notificationTitles[0] == notificationDescriptions[0])
        {
            gameMode = GameMode.Affirmation;
        }
        else
        {
            gameMode = GameMode.Question;
        }*/
    }

    /*public void ToggleTopic(){
        if(game.activeInHierarchy == true){
            if(rewardsCanvas.activeInHierarchy == false){
                notificationCount = 0;
                //Debug.Log("topicCounter starts as: " + topicCounter);
                //Debug.Log("Topics count is: " + topics.Count);
                if(topicCounter == topics.Count - 1){
                    //Debug.Log("Should be setting topicCounter to 0");
                    topicCounter = 0;
                }
                else{
                    //Debug.Log("Should be adding one to topicCounter");
                    topicCounter = topicCounter + 1;
                }

                //Debug.Log("topicCounter ends as: " + topicCounter);
                topic = topics[topicCounter];
                //Debug.Log("Topic is: " + instance.topic);
                
                filteredData = FilterData(processedData, topic);
        
                LoadQuestions(filteredData);

                if(systemNotificationsEnabled == true){
                    SetNotificationSettings("A new topic has been set", "Topic is: " + topic, "Topic is: " + topic, GetNotificationStatus());
                }
            }
        }
    }*/

    public void ToggleDataFile()
    {
        /*if (game.activeInHierarchy == true)
        {
            if (rewardsCanvas.activeInHierarchy == false)
            {*/
                if(dataFileCounter >= dataFiles.Count)
                {
                    dataFileCounter = 0;
                }

                fileName = "Data//" + dataFiles[dataFileCounter];
                //Debug.Log(fileName);
                string file = Path.Combine(Application.streamingAssetsPath, fileName);
                if (System.IO.File.Exists(file))
                {
                    reader = GameObject.Find("_GameScope").GetComponent<CSVReader>();
                    data = reader.LoadData(Path.Combine(Application.streamingAssetsPath, fileName));
                    processedData = GetData();
                    LoadTopics(processedData);
                    //foreach(string topic in topics){
                    //    Debug.Log("Topic is: " + topic);
                    //}
                    topicCounter = 0;
                    topic = topics[topicCounter];
                    filteredData = FilterData(processedData, topic);
                    LoadQuestions(filteredData);
                }
                else
                {
                    //SetNotificationSettings("No quiz game data file", "Please load a quiz game data file", "", GetNotificationStatus());
                    //Debug.Log("No data file is present for the quiz game");
                }

                if (systemNotificationsEnabled == true)
                {
                    SetNotificationSettings("A new data file has been loaded", "Name is: " + fileName, "Topic is: " + topic, GetNotificationStatus());
                }
            //}
            
            dataFileCounter++;

        //}
    }

    /*public void ToggleVesicaPlane(){
        if(vesicaPlane.activeInHierarchy == true){
            vesicaPlane.SetActive(false);
            SetNotificationSettings("Vesica Plane disabled", "To restore it press V", "To restore it press V", GetNotificationStatus());
        }
        else if(vesicaPlane.activeInHierarchy == false){
            vesicaPlane.SetActive(true);
            SetNotificationSettings("Vesica Plane enabled", "To remove it press V", "To restore it press V", GetNotificationStatus());
        }
    }*/

    public void ToggleNotifications(){
        systemNotificationsEnabled = !systemNotificationsEnabled;
        string message = "";

        //Debug.Log("systemNotifications enabled equals: " + systemNotificationsEnabled);
        if(systemNotificationsEnabled == true){
            message = "Notifications Activated";
        }
        else if(systemNotificationsEnabled == false){
            message = "Notifications Deactivated";
        }

        SetNotificationSettings(message, "To change, click the bell icon in the taskbar", "To change, click the bell icon in the taskbar", true);
    }

    public bool GetNotificationStatus(){
        //Debug.Log("systemNotificationsEnabled equals: " + systemNotificationsEnabled);
        return systemNotificationsEnabled;
    }

    public void SetNotificationSettings(string notificationTitle, string notificationDescription, string popupDescription, bool enablePopupNotification){
        notificationCreator.notificationTitle = notificationTitle; // Set notification title
        notificationCreator.notificationDescription = notificationDescription; // Set notification description
        notificationCreator.enablePopupNotification = enablePopupNotification; // Enable or disable popup notification
        notificationCreator.popupDescription = popupDescription; // Set popup notification description
        if(enablePopupNotification == true){
            notificationCreator.CreateNotification();
        } // Create a notification depending on variables
    }

    // I want to write multiple things to this path - reports, "The Vault" (things I 'know'), etc. Need one function to call from anywhere in the app to do this.
    /*public void WriteToPersistentDataPath(string folder, string filename){
        string path = Application.persistentDataPath + "/" + folder;
            string date = System.DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            bool pathExists = Directory.Exists(Application.persistentDataPath + "/" + folder);
            if (!pathExists){
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(Application.persistentDataPath + "/" + folder + "/" + date + filename + ".txt")){
                StreamWriter sr = new StreamWriter(Application.persistentDataPath + "/" + folder + "/" + date + "_" + filename + ".txt", true);
                playerName = userManager.firstName + " " + userManager.lastName;
                //Debug.Log("Player name is: " + playerName + " and " + "Number correct is: " + numberCorrect + " and number incorrect is: " + numberIncorrect);
                sr.WriteLine("Username: " + playerName);
                //sr.WriteLine("Number Correct: " + numberCorrect);
                //sr.WriteLine("Number Incorrect: " + numberIncorrect);
                sr.Close();
            }

            Debug.Log("File written to: " + Application.persistentDataPath + "/" + folder + "/" + date + "_" + filename + ".txt");
    }*/

    public void BlackScreen()
    {
        simpleLUT.BlackScreen();
        blackScreen = true;
        
        if (systemNotificationsEnabled == true)
        {
            SetNotificationSettings("Fading to black", "Click anywhere to bring back the light", "Click anywhere to bring back the light", GetNotificationStatus());
        }
    }

    void UnloadUnusedResources(){
        //Debug.Log("Called UnloadUnusedResources");
        Resources.UnloadUnusedAssets();
    }
}
