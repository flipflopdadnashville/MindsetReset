using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public MazecraftGameManager instance;
    public Camera mainCamera;
    public Rigidbody rigidbody;

    [SerializeField]
    private float maxSpeed = 10f;

    // Start is called before the first frame update
/*    void Start()
    {
        //mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (instance.playerTwo.activeInHierarchy == true)
        {
            if (!mainCamera)
            {
                mainCamera = GameObject.Find("Main Camera").GetComponent<Camera> ();
            }

            rigidbody.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        }
    }
*/}