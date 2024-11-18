using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    private MazecraftGameManager instance;
    public float speed = 1;
    public FixedJoystick fixedJoystick;
    private bool movingRight = false;
    private bool movingLeft = true;
    public float downSpeed = .1f;
    public float upSpeed = .3f;
    public Animator anim;


/*    void Start(){
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
    }

    public void FixedUpdate()
    {
        if(instance.playerTwo.transform.eulerAngles.y < 0){
            AutofixRotation(1);
        }

        Vector3 direction = Vector3.up * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal;
        //Debug.Log(direction);
        if(direction.x > 0){
            transform.position -= new Vector3(speed,0,0);
            movingLeft = true;
            movingRight = false;

            if(instance.activePlayer.name != "glasses"){ 
                if (((transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 45)) || (transform.rotation.eulerAngles.y > 320 && transform.rotation.eulerAngles.y < 360))
                {
                    RotateByDegrees(0,-10,0);          
                }
            }
        }
        else if(direction.x < 0){
            transform.position += new Vector3(speed,0,0);
            movingRight = true;
            movingLeft = false;

            if(instance.activePlayer.name != "glasses"){ 
                if (((transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 40)) || (transform.rotation.eulerAngles.y > 310 && transform.rotation.eulerAngles.y < 360))
                {
                    RotateByDegrees(0,2,0);          
                }
            }
        }

        // need different controls when terrain is active
        if(instance.forestTerrain.activeInHierarchy == false){
            if(instance.activePlayer.name == "Fidge"){ 
                if(anim.GetBool("Roll_Anim") == true){
                    anim.SetBool("Roll_Anim", false);
                }
            }

            if(direction.y > .25f){
                transform.position += new Vector3(0, upSpeed, 0);
            }

            if(direction.y < -.25f){
                transform.position += new Vector3(0, -downSpeed, 0);
            }
        }
        else if(instance.forestTerrain.activeInHierarchy == true){
            if(instance.activePlayer.name == "Fidge"){ 
                if(anim.GetBool("Roll_Anim") == false){
                    anim.SetBool("Roll_Anim", true);
                }
            }

            if(direction.y > .25f){
                transform.position -= new Vector3(0, 0, speed);
            }

            if(direction.y < -.25f){
                transform.position += new Vector3(0, 0, speed);
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
*/}