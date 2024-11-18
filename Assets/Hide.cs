using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hide : MonoBehaviour
{
    public Slider mainSlider;
    //public GameObject fractalBackground;
    private float gameSpeed;
    private float speed;
    float height;
    float width;
    Vector3 oPos;
    Vector3 hiddenPos;
    //Vector3 fractalBackgroundOPos;
    //Vector3 fractalBackgroundLowPos;
    Vector3 mousePos;
    bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        height = Screen.height;
        width = Screen.width;
        //Debug.Log("Screen height: " + height);
        //Debug.Log("Screen width: " + width);
        oPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        hiddenPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 25f, this.gameObject.transform.position.z);
        //fractalBackgroundOPos = new Vector3(fractalBackground.transform.position.x, fractalBackground.transform.position.y, fractalBackground.transform.position.z);
        //fractalBackgroundLowPos = new Vector3(fractalBackground.transform.position.x, fractalBackground.transform.position.y - 250, fractalBackground.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        gameSpeed = mainSlider.value;

        if(gameSpeed <= .33f || gameSpeed == 0f){
            speed = .07f;
        }
        else if(gameSpeed > .33f && gameSpeed <= .66f){
            speed = .5f;
        }
        else{
            speed = 1;
        }

        //Debug.Log("Active status for menu is: " + active);
        mousePos = Input.mousePosition;
        //Debug.Log("Mouse position is: " + mousePos.x + "," + mousePos.y);

        if(mousePos.y > Screen.height * .8 && active == false){
            active = true;
            StartCoroutine(LerpPosition(oPos, speed));
        }

        if(mousePos.y < Screen.height * .8 && active == true){
            active = false;
            StartCoroutine(LerpPosition(hiddenPos, speed));
        } 
    }

    //have to add another parameter if we want to lerp multiple objects again.
    IEnumerator LerpPosition(Vector3 targetTaskBarPosition, float duration)
    {
        float time = 0;
        Vector3 tbStartPosition = transform.position;
        //Vector3 fbStartPosition = fractalBackground.transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(tbStartPosition, targetTaskBarPosition, time / duration);
            //fractalBackground.transform.position = Vector3.Lerp(fbStartPosition, targetFractalBackgroundPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetTaskBarPosition;
        //fractalBackground.transform.position = targetFractalBackgroundPosition;
    }
}
