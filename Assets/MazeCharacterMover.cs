using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCharacterMover : MonoBehaviour
{
    public MazecraftGameManager instance;
    public Animator anim;
    public Rigidbody rb;
    private bool movingRight = false;
    private bool movingLeft = true;
    public GameObject bomb;
    private Animator m_bombAnimator;
    public AudioSource instanceAudioSource;
    public float speed = .5f;
    public float downSpeed = .1f;
    public float upSpeed = .3f;
    public float jumpSpeed = 750.6f;
    public float rSpeed = 2f;
    public float rotationSpeed = 500000;
    public Collider ignoreCollider;

    void Start(){
        //instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        //XML_Manager.Load();    
    }

    // Update is called once per frame
/*    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(instance.forestTerrain.activeInHierarchy == false && instance.playerTwo.transform.eulerAngles.y < 0){
            AutofixRotation(1);
        }

        // JWR 20230803 Taking this code out for now.
        //if(instance.gameMode == MazecraftGameManager.GameMode.OuterSpace){
        //    //Debug.Log("From MazeCharacterMover, GameMode is: " + instance.gameMode);
        //    //rb.AddForce(Random.Range(-.3f,.3f), Random.Range(-.3f,.3f), Random.Range(-.3f,.3f));
        //    transform.Rotate(1,1,1);
        //    if((rb.constraints & RigidbodyConstraints.FreezeRotationY) != RigidbodyConstraints.None){
        //        //Debug.Log("From MazeCharacterMover, Y Rotation is constrained. Setting player free!");
        //        rb.constraints = RigidbodyConstraints.None;
        //        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        //        Physics.gravity = new Vector3(0, -.6f, 0);
        //        rb.AddForce(.1f, .1f, .1f);

        //        if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
        //            if(instance.activePlayer.name == "Fidge"){ 
        //                if(anim.GetBool("Walk_Anim") == false){
        //                    anim.SetBool("Walk_Anim", true);
        //                }
        //            }
        //        }

        //        if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.DownArrow)) {
        //            anim.SetBool("Walk_Anim", false);
        //        }
        //    }
        //}
        //else if(instance.gameMode == MazecraftGameManager.GameMode.Normal){
        //    //Debug.Log("From MazeCharacterMover, GameMode is: " + instance.gameMode);
        //    Physics.gravity = new Vector3(0, -9.8f, 0);
        //    if((rb.constraints & RigidbodyConstraints.FreezeRotationY) != RigidbodyConstraints.None){
        //        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //    } 
        //    //StartCoroutine(ApplyTemporaryForce());       
        //}
        
        // Debug.Log("From MazeCharacterMover, Fidge localPosition y is: " + instance.playerTwo.transform.localPosition.y);
        if(((instance.playerTwo.transform.localPosition.y < -60000 && instance.playerTwo.transform.localPosition.y > -700000)) || Input.GetKeyDown(KeyCode.H)){
            SendPlayerHome();
        }

        if(instance.playerTwo.transform.localPosition.y > 20000){
            instance.playerTwo.transform.localPosition = new Vector3(instance.playerTwo.transform.localPosition.x, 200, instance.playerTwo.transform.localPosition.z);
        }

        if(Input.GetKeyDown(KeyCode.B)){
            //Vector3 gumdropPos = new Vector3(GameObject.Find("x").transform.position.x, GameObject.Find("x").transform.position.y, GameObject.Find("x").transform.position.z);
            if(instance.currentBombs > 0){
                ThrowBomb();
            }
        }

        if (Input.GetKey (KeyCode.LeftArrow)) { 
            transform.position += new Vector3(speed,0,0);
            //instanceAudioSource.panStereo -= .02f;
            movingLeft = true;
            movingRight = false;

            if(instance.activePlayer.name != "glasses"){ 
                anim.SetBool("Walk_Anim", true);
                if (((transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 40)) || (transform.rotation.eulerAngles.y > 310 && transform.rotation.eulerAngles.y < 360))
                {
                    RotateByDegrees(0,2,0);          
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow)) {
           anim.SetBool("Walk_Anim", false); 
        }

        // Move forward
        if (Input.GetKey (KeyCode.RightArrow)) {
            transform.position -= new Vector3(speed,0,0);
            movingRight = true;
            movingLeft = false;

            if(instance.activePlayer.name != "glasses"){ 
                anim.SetBool("Walk_Anim", true);
                if (((transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 45)) || (transform.rotation.eulerAngles.y > 320 && transform.rotation.eulerAngles.y < 360))
                {
                    RotateByDegrees(0,-10,0);          
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.RightArrow)) {
           anim.SetBool("Walk_Anim", false); 
        }
        
        // Need separate controls for when player is in maze vs. on terrain
        if(instance.forestTerrain.activeInHierarchy == false){
            //Physics.gravity = new Vector3(0, -9.6f, 0);
            if (Input.GetKey (KeyCode.UpArrow)) {
                transform.position += new Vector3(0, upSpeed, 0);
                if(instance.activePlayer.name != "glasses"){ 
                    anim.SetBool("Walk_Anim", false);
                }
            }
            
            if (Input.GetKey (KeyCode.DownArrow)) {
                transform.position -= new Vector3(0,downSpeed,0);
                if(instance.activePlayer.name != "glasses"){ 
                    anim.SetBool("Walk_Anim", false);
                }
            }
        }
        else if(instance.forestTerrain.activeInHierarchy == true){
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //speed = .75f;
            //Physics.gravity = new Vector3(0, -.5f, 0);
            // downSpeed = .3f;
            // upSpeed = .3f;
            if (Input.GetKey (KeyCode.UpArrow)) {
                //instance.forestTerrain.transform.position += new Vector3(0, 0, speed);
                transform.position -= new Vector3(0, 0, speed);
            }
            
            if (Input.GetKey (KeyCode.DownArrow)) {
                //instance.forestTerrain.transform.position -= new Vector3(0,0, speed);
                transform.position += new Vector3(0,0, speed);
                //Vector3 up spins it right
                //Vector3 right spins it up
                //instance.forestTerrain.transform.Rotate(Vector3.right * 3 * Time.deltaTime);
            }

            if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
                if(instance.activePlayer.name == "Fidge"){ 
                    if(anim.GetBool("Roll_Anim") == false){
                        anim.SetBool("Roll_Anim", true);
                    }
                }
            }

            if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.DownArrow)) {
                anim.SetBool("Roll_Anim", false);
            }
        }

        if (Input.GetKey(KeyCode.Space) == true)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Equals)){
            if(jumpSpeed > 2 && jumpSpeed < 10){
                jumpSpeed += .2f;
            }
            if (anim.GetBool("Roll_Anim"))
			{
				anim.SetBool("Roll_Anim", false);
			}
			else
			{
				anim.SetBool("Roll_Anim", true);
			}
        }

        if(Input.GetKeyDown(KeyCode.Minus)){
            if(jumpSpeed > 2 && jumpSpeed < 10){
                jumpSpeed -= .2f;
            }
            if (anim.GetBool("Open_Anim"))
			{
				anim.SetBool("Open_Anim", false);
			}
			else
			{
				anim.SetBool("Open_Anim", true);
			}            
        }

        if(jumpSpeed > 10){
            jumpSpeed = 9;
        }

        if(jumpSpeed < 2){
            jumpSpeed = 2.3f;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            if(Input.GetKeyDown(KeyCode.G)){
                if(speed == .5f){
                    speed = 1;
                }
                else if(speed == 1){
                    speed = .5f;
                }
            }
        }
    }

    public void SendPlayerHome(){
        if(instance.game.activeInHierarchy == true){
            instance.playerTwo.transform.localPosition = new Vector3(60, 25, 11.5f);
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            instance.playerTwo.transform.eulerAngles = new Vector3(0,30,0);

            if(instance.forestTerrain.activeInHierarchy == true){
                instance.forestTerrain.SetActive(false);
            }
        }
    }

    void RotateByDegrees(float x, float y, float z)
    {
        Vector3 rotationToAdd = new Vector3(x, y, z);
        transform.Rotate(rotationToAdd);
    }

    void AutofixRotation(float y)
    {
        Vector3 rotationToAdd = new Vector3(0, y, 0);
        transform.Rotate(rotationToAdd);
    }

    public void ThrowBomb(){
        if(instance.game.activeInHierarchy == true){
            instance.currentBombs--;
            bomb = Instantiate(Resources.Load("Bomberino") as GameObject, Vector3.zero, transform.rotation);
            bomb.transform.localPosition = new Vector3(instance.activePlayer.transform.position.x, instance.activePlayer.transform.position.y, instance.activePlayer.transform.position.z);
            if(movingLeft == true){
                bomb.GetComponent<Rigidbody>().AddForce(500f,100f,0);
            }
            else if(movingRight == true){
                bomb.GetComponent<Rigidbody>().AddForce(-500f,100f,0);
            }
            bomb.transform.LookAt(GameObject.Find("Main Camera").transform);
        }
    }

    public void Jump(){
        transform.position += new Vector3(0, jumpSpeed, 0);
    }
*/}
