using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    Vector3 dist;
    Vector3 startPos;
    float posX;
    float posZ;
    float posY;
         
    void OnMouseDown()
    {
        startPos = transform.position;
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        posZ = Input.mousePosition.z - dist.z;
    }

    void OnMouseDrag()
    {
        float disX = Input.mousePosition.x - posX;
        float disY = Input.mousePosition.y - posY;
        float disZ = Input.mousePosition.z - posZ;
        Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
        transform.position = new Vector3(lastPos.x, startPos.y, lastPos.z);
    }

    //old code, only drags on z axis
    // private Vector3 mOffset;
    // private float mXCoord;
    // private float mYCoordO;
    // private float mYCoordF;
    // private float mZCoord;
    
    // void OnMouseDown(){
    //     mXCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).x;
    //     mYCoordO = Camera.main.WorldToScreenPoint(gameObject.transform.position).y;
    //     mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //     mOffset = gameObject.transform.position - GetMouseWorldPos();
    // }

    // void OnMouseDrag(){
    //     transform.position = GetMouseWorldPos() + mOffset;
    // }

    // // void OnMouseUp(){
    // //     mYCoordF = Camera.main.WorldToScreenPoint(gameObject.transform.position).y;

    // //     if(mYCoordO - mYCoordF > 0){
    // //         this.GetComponent<Rigidbody>().AddForce(0,-.02f,0);
    // //     }
    // //     else if(mYCoordO - mYCoordF < 0){
    // //         this.GetComponent<Rigidbody>().AddForce(0,.02f,0);
    // //     }
    // // }

    // private Vector3 GetMouseWorldPos(){
    //     Vector3 mousePoint = Input.mousePosition;
    //     mousePoint.x = mXCoord;
    //     mousePoint.z = mZCoord;

    //     return Camera.main.ScreenToWorldPoint(mousePoint);
    // }
}
