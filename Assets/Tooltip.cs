using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
 
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject goTooltip;
    public GameObject tooltipTextbox;
    public string tooltipMessage;
    float height;
    float width;

    //Use Screen.width if you want it to do different things based on where the pointer is
    void Awake()
    {
        // height = System.Convert.ToSingle(Screen.height * .9);
        // width = System.Convert.ToSingle(Screen.width * .01);
        goTooltip.gameObject.transform.position = new Vector3(-600, 0, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        width = System.Convert.ToSingle(Screen.width * .1);
        //Debug.Log("On pointer enter. Postion is: " + eventData.position.x + " " + eventData.position.y);
        tooltipMessage = this.gameObject.name;
        tooltipTextbox.GetComponent<TextMeshProUGUI>().text = tooltipMessage;
        //goTooltip.gameObject.transform.SetParent(this.gameObject.transform);
        //Debug.Log("Screen height is: " + Screen.height + " and height is: " + height);
        if(eventData.position.x < (Screen.width * .5)){
            height = System.Convert.ToSingle(Screen.height * .9);
            width = System.Convert.ToSingle(Screen.width * .05);
            goTooltip.gameObject.transform.position = new Vector3(width, height, 0);
        }
        else if(eventData.position.x > (Screen.width * .5)){
            height = System.Convert.ToSingle(Screen.height * .05);
            width = System.Convert.ToSingle(Screen.width * .95);
            goTooltip.gameObject.transform.position = new Vector3(width, height, 0);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        height = System.Convert.ToSingle(Screen.height * .9);
        width = System.Convert.ToSingle(Screen.width * .1);
        //Debug.Log("On pointer exit");
        tooltipMessage = "";
        tooltipTextbox.GetComponent<TextMeshProUGUI>().text = tooltipMessage;
        goTooltip.gameObject.transform.position = new Vector3(-2000f, height, 0);
        EventSystem.current.SetSelectedGameObject(null);
    }
}