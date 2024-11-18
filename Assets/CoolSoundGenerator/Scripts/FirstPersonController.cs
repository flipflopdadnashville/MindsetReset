using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour
{
	public float speed = 5.9f;
	public float gravity = -9.8f;
	private CharacterController _charController;

	private float deltaX;
	private float deltaZ;
	private bool buttonsWorking = false;

	public GameObject ButtonForward_;
	public GameObject ButtonBack_;


	void Start()
	{
		_charController = GetComponent<CharacterController>();
	}
	void Update()
	{

		if ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor))
		{
			ButtonForward_.SetActive(false);
			ButtonBack_.SetActive(false);
		}

		if (buttonsWorking == false)
        {
            deltaX = Input.GetAxis("Horizontal") * speed;
		    deltaZ = Input.GetAxis("Vertical") * speed;
        }
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed);
		movement.y = gravity;
		movement = movement * Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);
	}


    public void ButtonForward()
    {
		buttonsWorking = true;
		deltaZ = 1.0f;
	}

	public void ButtonBack()
	{
		buttonsWorking = true;
		deltaZ = -1.0f;
	}

	
}
