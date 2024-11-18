using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
 
public class Flip : MonoBehaviour 
 {
    public MazecraftGameManager instance;
    public float progress = 0;
    public float height = 5f;
 
    private Vector3 origin;
    private Vector3 origRot;
    private Vector3 flipRot;
    public CanvasGroup cg;
    public bool flipStatus;
    public GameObject userAnswer;
    public GameObject systemAnswer;
    Image progressBar;

/*    void Start () 
    {
        progressBar = GameObject.Find("progressBar").GetComponent<Image>();
        flipStatus = false;
        origin = transform.position;
        flipRot = origRot = transform.rotation.eulerAngles;
        flipRot.z += 180;
    }
 
    void Update () 
    {
    //Debug.Log("From Update, Flip status is: " + flipStatus);
    if(flipStatus == true && instance.answerText.interactable == false){
        FlipAndFade();
    }
    else{
        Reset();
    }

        progress = Mathf.Clamp(progress, 0, 90);

        Vector3 pos = origin;
        pos.y = (Mathf.Sin(Mathf.Deg2Rad * progress) * Mathf.Cos(Mathf.Deg2Rad * progress) * height) + origin.y;

        transform.position = pos; // handle moving upwards
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(origRot), Quaternion.Euler(flipRot), progress / 90); // handle rotation

    }

    public void SetFlipStatus(bool newFlipStatus){
        //Debug.Log("From SetFlipStatus, Flip status is: " + flipStatus);
        if(newFlipStatus == true){
            flipStatus = true;
        }
        else if(newFlipStatus == false){
            flipStatus = false;
        }
        //Debug.Log("From SetFlipStatus, Flip status is now: " + flipStatus);
    }

    private void FlipAndFade(){
        progress = progress + 7.5f;
        cg.alpha = cg.alpha - .045f;

        //Debug.Log("Progress is: " + progress);
        if(progress > 88 && progress < 92){
            VerifyAnswer();
        }
    }

    private void Reset(){
        progress = 0;
        cg.alpha = 1;

        progressBar.color = new Color32(39,207,212,250);
    }

    private void VerifyAnswer(){
        if(userAnswer.GetComponent<TMP_InputField>().text.ToLower() == systemAnswer.GetComponent<Text>().text.ToLower()){
            progressBar.color = new Color32(100,220,65,255);
            //Debug.Log("Correct! Number correct is: " + instance.numberCorrect);
            instance.numberCorrect = instance.numberCorrect + 1;
            //Debug.Log("Correct! Number correct is: " + instance.numberCorrect);

        }
        else{
            progressBar.color = new Color32(200,65,55,255);
            instance.numberIncorrect = instance.numberIncorrect + 1;
        }
    }
*/ }