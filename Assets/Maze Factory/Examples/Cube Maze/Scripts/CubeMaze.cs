using UnityEngine;
using System.Collections;

public class CubeMaze : MonoBehaviour {	
	public MazeSolutionAgent mazeSolutionAgent;
	private Quaternion fromRotation;
	private Quaternion targetRotation;
	public float speed = 0.1f;
	private float progress = 0.0f;
	// Use this for initialization
	void Start () {
		targetRotation = Quaternion.identity;
		
		if (mazeSolutionAgent)
		{
			mazeSolutionAgent.OnMazeSolutionAgentEndReached += OnMazeSolutionAgentEndReached;
		}
	}
	
	public void OnMazeSolutionAgentEndReached(int newMazeIndex)
	{
		progress = 0.0f;
		fromRotation = transform.rotation;
		targetRotation = Quaternion.Euler(new Vector3(0.0f, newMazeIndex * 90.0f, 0.0f)); 
	}
	
	// Update is called once per frame
	void Update () {
		if (progress < 1.0f)
		{
			progress += Time.deltaTime * speed;
			transform.rotation = Quaternion.Slerp(fromRotation, targetRotation, Mathf.Clamp01(progress));//.Slerp(fromRotation, targetRotation, Time.deltaTime * speed);
		}
	}
	void LateUpdate()
	{
		if (Input.GetMouseButtonUp(0))
		{			
			Quaternion prevRotation = transform.rotation;
			transform.rotation = Quaternion.identity;
			mazeSolutionAgent.ReGenerateCurrentMaze();
			transform.rotation = prevRotation;
		}
	}
}
