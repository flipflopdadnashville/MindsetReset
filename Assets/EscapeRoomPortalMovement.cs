using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoomPortalMovement : MonoBehaviour
{
    public float min;
    public float max;
    public float SpeedOfMovement;

    // Update is called once per frame
    private void Update () {
        transform.position = new Vector3 (transform.position.x, transform.position.y, Mathf.PingPong (Time.time * 2, 40) + 5);
    }
}
