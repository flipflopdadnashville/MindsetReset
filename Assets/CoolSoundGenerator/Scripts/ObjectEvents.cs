using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//<for catch a long pressing on the button> 
public class ObjectEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool ObjectPressed;
    public static string nameObject;

    public void OnPointerDown(PointerEventData eventData)
    {
        ObjectPressed = true;
        nameObject = this.gameObject.name;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ObjectPressed = false;
        nameObject = this.gameObject.name;
    }
}
