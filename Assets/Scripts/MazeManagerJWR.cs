using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class MazeManagerJWR : MonoBehaviour
{
    [System.Serializable]
    public class ScoreChangedEvent : UnityEvent<int, int> { }

    private Touch touch;
    private Vector2 touchPosition;

    private bool up = true;

    public float angularAcceleration = 2;

    [Range(0, 1)]
    public float angularDrag = 0.2f;

    // public Text completionLabel;
    // public Text purityLabel;
    // public Text finishLabel;

    float angularSpeed = 0;
    float angle = 0;
    public string axisToRotateOn;

    void Update()
    {

        //print(Input.GetAxis("Mouse X"));

        if(Input.touchCount > 0){
            //Debug.Log("Touch detected");
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Moved){
                Quaternion rotationZ = Quaternion.Euler(0,0, - touch.deltaPosition.x * 0.1f);
                transform.rotation = rotationZ * transform.rotation;
            }
        }

        //transform.Rotate(0,0, -(Input.GetAxis("Mouse  X")) * Time.deltaTime * .8f);
        // Get the mouse delta. This is not in the range -1...1
        float h = Input.GetAxis("Mouse X");
        
        if(h < 1.5f){
          angularSpeed += 3 * Time.deltaTime;  
        }

        if(h >= -1.5f){
          angularSpeed -= 3 * Time.deltaTime;  
        }

        //float v = 2 * Input.GetAxis("Mouse Y");

        transform.Rotate(0, 0, h);

        if (Input.GetKey(KeyCode.A))
        {
            angularSpeed += angularAcceleration * Time.deltaTime;
            angularSpeed *= Mathf.Pow(1 - angularDrag, Time.deltaTime);
            angle += (angularSpeed * .8f) * Time.deltaTime;

            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            angularSpeed -= angularAcceleration * Time.deltaTime;
            angularSpeed *= Mathf.Pow(1 - angularDrag, Time.deltaTime);
            angle += (angularSpeed * .8f) * Time.deltaTime;

            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }

        

        //MoveVertical();

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
            angularSpeed = angle = 0;
            //finishLabel.gameObject.SetActive(false);
            SceneManager.LoadScene("FluidMaze");
        }
    }

    void MoveVertical(){
        Vector3 temp = transform.position;

        if(up == true){
            temp.y += 0.1f;
            transform.position = temp;
            if(transform.position.y >= 9.0f){
                up = false;
            }
        }
        if(up == false){
           temp.y -= 0.1f;
            transform.position = temp;
            if(transform.position.y <= -9.0f){
                up = true;
            } 
        }
    }
}
