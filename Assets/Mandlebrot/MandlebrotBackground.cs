using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MandlebrotBackground : MonoBehaviour
{
    public MazecraftGameManager instance;
    private MeshRenderer meshRenderer;
    public GameObject player;
    public Material mat;
    public Material dummyMat;
    public Vector2 pos;
    public float scale, angle, color;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;

    public RawImage rawImage;
    public Texture2D[] textureArray;

/*    void Start(){
       color = .9f; 
    }

    void Update(){
        if(instance.game.activeInHierarchy == false){
            HandleInputs();
        }
    }

    private void UpdateShader() {
        smoothPos = Vector2.Lerp(smoothPos, pos, .5f);
        smoothScale = Mathf.Lerp(smoothScale, scale, .01f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, .01f);

        float aspect = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if(aspect > 1f){
            scaleY /= aspect;
        }
        else{
            scaleX *= aspect;
        }

        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
        mat.SetFloat("_Color", color);
    }

    private void HandleInputs(){
        //Debug.Log("Scale equals: " + scale);
        if(scale > -0.05 && scale < 1){
            Vector2 dir = new Vector2(.01f*scale, 0);
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);
            dir = new Vector2(dir.x*c, dir.x*s);
            Vector2 diry = new Vector2(dir.y*c, dir.y*s);

            if(Input.GetKey(KeyCode.UpArrow)){
                Pan(2, diry);
            }
            if(Input.GetKey(KeyCode.DownArrow)){
                Pan(-2, diry);
            }
            if(Input.GetKey(KeyCode.LeftArrow)){
                Pan(1, dir);
            }
            if(Input.GetKey(KeyCode.RightArrow)){
                Pan(-1, dir);
            }
            if(Input.GetKey(KeyCode.J)){
                Zoom(1.0f);
            }
            if(Input.GetKey(KeyCode.L)){
                Zoom(-1.0f);
            }

            if(Input.touchCount == 2){
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * 0.01f);
            }

            dir = new Vector2(-dir.y, dir.x);

            // if(Input.GetKey(KeyCode.S)){
            //     pos -= dir;
            // }
            // if(Input.GetKey(KeyCode.W)){
            //     pos += dir;
            // }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((player.transform.position.y < -400 && player.transform.position.y > -600)){
            player.transform.localPosition = new Vector3(4, 15, player.transform.localPosition.z);
            scale = .025f;
        }

        if(scale > 1){
            scale = .9f;
        }

        if(scale < .00001){
            //Debug.Log("Scale too small");
            scale = .25f;
        }

        // JWR - taking out automatic color updates and background rotation 20230726
        //if(color > 1){
        //    mat.mainTexture = textureArray[Random.Range(0,5)];
        //    rawImage.material = dummyMat;
        //    rawImage.material = mat;
        //    color = 0;
        //}

        //angle -= Random.Range(-.0004f,.0008f);
        //color += .0001f;
        //HandleInputs();
        UpdateShader();
    }

    void Zoom(float increment){
        if(increment > 0){
            scale *= .99f;
        }
        else if(increment <0){
            scale *= 1.01f;
        }
    }

    void Pan(int dir, Vector2 mag) {
        //Debug.Log("dir is: " + dir);
        if(dir == 1){
            angle += .01f;
            pos += mag;
        }
        else if(dir == -1){
            angle -= .01f;
            pos -= mag;
        }
        else if(dir == 2){
            angle -= .01f;
            pos = new Vector2(pos.x, pos.y += mag.y);
        }
        else if(dir == -2){
            angle -= .01f;
            pos = new Vector2(pos.x, pos.y -+ mag.y);
        }
    }
*/}
