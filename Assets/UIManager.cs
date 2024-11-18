using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private MazecraftGameManager instance;
    private GameObject originalMaze;
    private GameObject[] mazes;
    public AudioSource audioSource;
    // private GameObject[] sGeometryObjects;
    //private GameObject selectedGeometryObject;
    // public GameObject PS_Dodecagram;
    // public ParticleSystem PS_Dodecagram_Buff1;
    // public ParticleSystem PS_Dodecagram_Buff2;
    // public ParticleSystem PS_Dodecagram_Buff3;
    public GameObject notepad;
    public GameObject soundscapeCreator;
    public GameObject easierMethod;
    public GameObject setup;
    public GameObject osWallpaper;
    public Canvas m_Canvas;

    void OnSceneLoaded(){
        Debug.Log("Scene name is: " + SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == "RightMind"){
            if(GameObject.Find("_GameScope").GetComponent<AudioSource>().enabled == false){
                GameObject.Find("_GameScope").GetComponent<AudioSource>().enabled = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        
        //JWR: For future use.
        //Cursor.visible = false;
        
        // foreach(GameObject sGeometryObject in instance.sGeometryObjects){
        //     sGeometryObject.AddComponent<RotateSGeometry>();
        //     //Debug.Log(sGeometryObject);
        //     sGeometryObject.SetActive(false);
        // }

        //selectedGeometryObject.SetActive(true);

        // remember if weatherMaker is set to false, you have to comment out the osWallpaper and canvas as well
        //instance.weatherMaker.SetActive(false);
        // instance.weatherMakerConfig.SetActive(false);
        //instance.uiManagerPanel.SetActive(false);
        //osWallpaper.SetActive(false);
        //m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //instance.rotatingMaze.SetActive(false);
        //instance.game.SetActive(false);
        //instance.ocean.SetActive(false);
        //instance.forestTerrain.SetActive(false);
        //PS_Dodecagram.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //this is wonky... and even wonkier because I'm going to put the same thing in mazecraft game manager and mazespawner to stop it from changing topics and adding mazes. refactor later...
        /*if (notepad.activeInHierarchy == false && soundscapeCreator.activeInHierarchy == false && easierMethod.activeInHierarchy == false && setup.activeInHierarchy == false && instance.rewardsCanvas.activeInHierarchy == false)
        {
            CheckKey();
        }*/

        /*if (instance.activeCamera.name == "MoodscapeCamera")
        {
            GameObject[] rotatingMazes = GameObject.FindGameObjectsWithTag("rotatingMaze");

            if (rotatingMazes.Length > 0)
            {
                ToggleRotatingMazes();
            }

            if(instance.forestTerrain.activeInHierarchy == true)
            {
                instance.forestTerrain.SetActive(false);
            }
        }*/
    }

/*    public void CheckKey(){
        if(Input.GetKeyDown(KeyCode.A)){
            ToggleControls();
        }
        
        if(Input.GetKeyDown(KeyCode.D)){
            DestroyCreatedGameObjects();
        }

        if(Input.GetKeyDown(KeyCode.E)){
            ToggleHomeMaze();
        }

        if(Input.GetKeyDown(KeyCode.F)){
            ToggleFishingPole();
        }

        if(Input.GetKeyDown(KeyCode.G)){
            ToggleGame();
        }

        //using this for returning the player home now...
        if(Input.GetKeyDown(KeyCode.H)){
            //ToggleRotatingMazes();
        }

        if(Input.GetKeyDown(KeyCode.N)){
            //placeholder
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            ToggleOcean();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (instance.forestTerrain.activeInHierarchy == true)
            {
                ToggleChaserGame();
            }
        }

        if(Input.GetKeyDown(KeyCode.O)){
            if(Input.GetKeyDown(KeyCode.S)){
                if(instance.dreamOS.activeInHierarchy == true){
                    instance.dreamOS.SetActive(false);
                }
                else{
                    instance.dreamOS.SetActive(true);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.P)){
            //ToggleWeatherMenu();
        }

        if(Input.GetKeyDown(KeyCode.S)){
            ToggleMazeStructure();
        }

        //if(instance.activeCamera.name == "Main Camera" && (instance.playerTwo.transform.localPosition.y > 14 && instance.playerTwo.transform.localPosition.y < 18) && Input.GetKeyDown(KeyCode.LeftAlt)){
        //    ToggleLowPolyForest();
        //}
        
        if(Input.GetKeyDown(KeyCode.V)){
            ToggleSacredGeometryObjects();
        }

        if(Input.GetKeyDown(KeyCode.W)){
            ToggleWeather();
        }
    }
*/
/*    public void ToggleChaserGame()
    {
        instance.chaserGameOn = !instance.chaserGameOn;
        if (instance.chaserGameOn)
        {
            instance.SetNotificationSettings("Chaser Game Enabled", "Try to keep the ball in view!", "Try to keep the ball in view!", instance.GetNotificationStatus());
        }
        else
        {
            instance.SetNotificationSettings("Chaser Game Disabled", "", "", instance.GetNotificationStatus());
        }
    }
*/
/*    public void ToggleTopic(){
        if(instance.rewardsCanvas.activeInHierarchy == false){
            instance.notificationCount = 0;
            //Debug.Log("topicCounter starts as: " + topicCounter);
            //Debug.Log("Topics count is: " + topics.Count);
            if(instance.topicCounter == instance.topics.Count - 1){
                //Debug.Log("Should be setting topicCounter to 0");
                instance.topicCounter = 0;
            }
            else{
                //Debug.Log("Should be adding one to topicCounter");
                instance.topicCounter = instance.topicCounter + 1;
            }

            //Debug.Log("topicCounter ends as: " + topicCounter);
            instance.topic = instance.topics[instance.topicCounter];
            //Debug.Log("Topic is: " + instance.topic);
            //8-1 TODO: Had to remove until I rewrite the GetCSVDataByTopic function in GameManager
            //instance.GetCSVDataByTopic(instance.topic);
        }
    }
*/
/*    public void ToggleMazeStructure(){
        if(instance.mazeStructure == MazecraftGameManager.MazeStructure.Normal){
            instance.mazeStructure = MazecraftGameManager.MazeStructure.PillarsOnly;
        }
        else{
            instance.mazeStructure = MazecraftGameManager.MazeStructure.Normal;
        }
    }
*/
/*    public void ToggleWeather(){
        if(instance.weatherMaker.activeInHierarchy == true){
            instance.weatherMaker.SetActive(false);
            Cursor.visible = true;
            //osWallpaper.SetActive(true);
            //m_Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
        else if(instance.weatherMaker.activeInHierarchy == false){
            instance.weatherMaker.SetActive(true);
            Cursor.visible = false;
            //osWallpaper.SetActive(false);
            //m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
*/
    // public void ToggleWeatherMenu(){
    //     if(instance.weatherMakerConfig.activeInHierarchy == true){
    //         instance.weatherMakerConfig.SetActive(false);
    //     }
    //     else{
    //         instance.weatherMakerConfig.SetActive(true);
    //     }
    // }

    // public void ToggleControlCenterMenu(){
    //     if(instance.uiManagerPanel.activeInHierarchy == true){
    //         instance.uiManagerPanel.SetActive(false);
    //     }
    //     else{
    //         instance.uiManagerPanel.SetActive(true);
    //     }
    // }

/*    public void ToggleRotatingMazes(){
        GameObject[] rotatingMazes = GameObject.FindGameObjectsWithTag("rotatingMaze");

            if(rotatingMazes.Length > 0){
                foreach(GameObject maze in rotatingMazes){
                    if(maze.name != "RotatingMaze"){
                        Destroy(maze);
                    }
                }
            }

        if(instance.rotatingMaze.activeInHierarchy == true){
            instance.rotatingMaze.SetActive(false);
        }
        else{
            instance.rotatingMaze.SetActive(true);
        }
    }
*/
/*    public void ToggleHomeMaze(){
        if(instance.originalMaze.activeInHierarchy == true){
            instance.originalMaze.SetActive(false);
        }
        else if(instance.originalMaze.activeInHierarchy == false){
            instance.originalMaze.SetActive(true);
        }

        mazes = GameObject.FindGameObjectsWithTag("Maze");
        foreach(GameObject maze in mazes){
            maze.SetActive(false);
        }
    }
*/
/*    public void ToggleGame(){
        if(instance.game.activeInHierarchy == true){
            //Debug.Log("Made it to normal ToggleGame method");
            instance.game.SetActive(false);
            // instance.joystick.SetActive(false);
            // instance.newMazeButton.SetActive(false);
            // instance.hideStaticMazesButton.SetActive(false);
            // instance.hideRotatingMazeButton.SetActive(false);
            // instance.destroyStaticMazesButton.SetActive(false);
            // instance.bombButton.SetActive(false);
            // instance.jumpButton.SetActive(false);
            instance.controlsHolder.SetActive(false);
            //instance.gumdrop.SetActive(false);
            instance.playerTwo.SetActive(false);
        }
        else{
            instance.game.SetActive(true);
            instance.controlsHolder.SetActive(true);
            instance.playerTwo.SetActive(true);
            if (instance.playerTwo.GetComponent<Rigidbody>().useGravity == false)
            {
                instance.playerTwo.GetComponent<Rigidbody>().useGravity = true;
            }
            instance.activePlayer = instance.playerTwo;
            // instance.joystick.SetActive(true);
            // instance.newMazeButton.SetActive(true);
            // instance.hideStaticMazesButton.SetActive(true);
            // instance.hideRotatingMazeButton.SetActive(true);
            // instance.destroyStaticMazesButton.SetActive(true);
            // instance.bombButton.SetActive(true);
            // instance.jumpButton.SetActive(true);
        }
    }
*/
/*    public void ToggleFishingPole(){
        if (instance.activeCamera.name == "CameraTwo")
        {
            if (instance.game.activeInHierarchy)
            {
                if (instance.pole.activeInHierarchy == true)
                {
                    instance.pole.SetActive(false);
                    instance.isFishing = false;
                }
                else if (!instance.pole.activeInHierarchy)
                {
                    instance.pole.SetActive(true);
                    instance.isFishing = true;
                }
            }
        }
    }
*/
/*    public void ToggleGame(bool temporary){
        string filepath = "AudioSamples/BOWLS/BOWLS" + "7a";
        //Debug.Log("Gong time! Playing " + filepath);
        audioSource.clip = Resources.Load<AudioClip>(filepath);
        audioSource.Play();
        audioSource.volume = Random.Range(.5f, .7f);
        instance.game.SetActive(false);

        if(temporary == true){
            //Debug.Log("Temporarily disabling game");
            StartCoroutine(Wait());
        }
    }
*/
/*    IEnumerator Wait()
    {
        //Debug.Log("Made it to Wait statement");
        yield return new WaitForSeconds(12f);
        //Debug.Log("Made it past WaitForSeconds");
        ToggleGame();
    }
*/
/*    public void ToggleOcean(){
        if (instance.activeCamera.name == "CameraTwo")
        {
            if (instance.ocean.activeInHierarchy == true)
            {
                instance.ocean.SetActive(false);
            }
            else
            {
                instance.ocean.SetActive(true);
                instance.SetNotificationSettings("Press the Left Alt key to Cast", "", "", instance.GetNotificationStatus());
            }
        }
    }
*/
/*    public void ToggleLowPolyForest(){
        if (instance.forestTerrain.activeInHierarchy == true){
            instance.forestTerrain.SetActive(false);
        }
        else{
            instance.forestTerrain.SetActive(true);
            BringTerrainToPlayer();
        }
    }
*/    
/*    public void BringTerrainToPlayer(){
        if(instance.forestTerrain.activeInHierarchy == true){
            instance.forestTerrain.transform.localPosition = new Vector3(instance.activePlayer.transform.position.x - 5, instance.activePlayer.transform.position.y - 50, instance.activePlayer.transform.position.z);
        }
    }
*/
/*    public void ToggleOS(){
        if(instance.dreamOS.activeInHierarchy == true){
            instance.dreamOS.SetActive(false);
        }
        else{
            instance.dreamOS.SetActive(true);
        }
    }
*/
/*    public void ToggleSacredGeometryObjects(){
    //     foreach(GameObject sGeometryObject in instance.sGeometryObjects){
            if(instance.vesicaPlane.activeInHierarchy == true){
                instance.vesicaPlane.SetActive(false);
            }
            else{
                instance.vesicaPlane.SetActive(true);
            }
        //}
    //     instance.portal.transform.localScale = new Vector3(1,1,1);
    //     instance.portal.transform.localPosition = new Vector3(instance.portal.transform.position.x, instance.portal.transform.position.y, 30);
    //     if(instance.game.activeInHierarchy == true){
    //         // PS_Dodecagram.SetActive(true);
    //         // PS_Dodecagram_Buff1.Play();
    //         // PS_Dodecagram_Buff2.Play();
    //         // PS_Dodecagram_Buff3.Play();
    //     }
    }
*/
/*    public void ToggleControls(){
        if(instance.controlsHolder.activeInHierarchy == true){
            instance.controlsHolder.SetActive(false);
        }
        else if(instance.controlsHolder.activeInHierarchy == false){
            instance.controlsHolder.SetActive(true);
        }
    }
*/
/*    public void DestroyCreatedGameObjects(){
        mazes = GameObject.FindGameObjectsWithTag("Maze");
        foreach(GameObject maze in mazes){
            Destroy(maze);
        }
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("enemyBall");
        foreach(GameObject bomb in bombs){
            Destroy(bomb);
        }
        instance.isNotificationActive = false;
    }
*/
    public void SetTextFieldInteractiveState(){
        if(instance.answerText.interactable == true){
            instance.answerText.interactable = false;
        }
        else{
            instance.answerText.interactable = true;
            instance.answerText.text = null;
        }
    }
}