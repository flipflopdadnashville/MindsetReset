using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeSolutionAgent : MonoBehaviour {
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
		if (mazes[currentMaze].mg != null)
		{
			Vector3 pos = mazes[currentMaze].transform.InverseTransformPoint(this.transform.position);
			
			if (currentCellReached)
			{
				currentCellReached = false;
				currentPathIndex++;			
				
				if (currentPathIndex >= mazes[currentMaze].mg.solution.Count)
				{
					currentPathIndex = 0;
					currentMaze++;
					if (currentMaze >= mazes.Count)
						currentMaze = 0;
					
					if (OnMazeSolutionAgentEndReached != null)
						OnMazeSolutionAgentEndReached(currentMaze);
				}
				
				currentCell = mazes[currentMaze].mg.solution[currentPathIndex];
				Vector3 offset = new Vector3(mazes[currentMaze].blockWidth / 2.0f, mazes[currentMaze].blockHeight / 2.0f, 0.0f);
				currentCellWorldPos = currentCell.localPosition + offset;//mazes[currentMaze].transform.TransformPoint(currentCell.localPosition + offset);
				prevDistance = float.MaxValue;
			}
			else
			{
				prevDistance = distance;
			}
	
			if (currentPathIndex == 0)		
			{
				pos = currentCellWorldPos;
			}
			else
			{
				dir = (currentCellWorldPos - pos);
				distance = dir.magnitude;
				dir.Normalize();
				
				if (distance > prevDistance) 
				{
				//	pos = currentCell.worldPosition;
currentCellReached = true;
				}
				else
				
					pos += dir * Time.deltaTime * speed;
			}
			
			if (pos == currentCellWorldPos)
			{
				currentCellReached = true;
			}
			
			this.transform.position = mazes[currentMaze].transform.TransformPoint(pos);
		}
	}
}
