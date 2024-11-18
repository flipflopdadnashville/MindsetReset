using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTheMazeSolutionAgent : MonoBehaviour {
	public delegate void OnMazeSolutionAgentEndReachedDelegate(int newMazeIndex);
	
	public List<MazeFactory> mazes = new List<MazeFactory>();
	public float speed = 1.0f;	
	private int currentMaze = 0;
	private int currentPathIndex = -1;
	private bool currentCellReached = true;
	private MFCell currentCell = null;
	private Vector3 currentCellWorldPos;
	private float prevDistance = 0.0f;
	private float distance = 0.0f;
	private Vector3 dir = Vector3.zero;
	public OnMazeSolutionAgentEndReachedDelegate OnMazeSolutionAgentEndReached;
	// Use this for initialization
	void Start () {
	}
	
	public MazeFactory GetCurrentMaze()
	{
		return mazes[currentMaze];
	}
	
	public void ReGenerateCurrentMaze()
	{
		currentPathIndex = 0;
		mazes[currentMaze].CreateMaze();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.name == "Sphere"){
            Debug.Log("You win!");
        }
    }
}
