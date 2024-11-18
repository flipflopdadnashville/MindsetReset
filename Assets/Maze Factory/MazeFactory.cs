using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Visualize a generate maze.
/// </summary>
[ExecuteInEditMode]
public class MazeFactory : MonoBehaviour {
	/// <summary>
	/// Preview the maze in the editor.
	/// Make sure to click "update editor" to refresh the screen.
	/// </summary>
	public bool previewInEditor = false;
	/// <summary>
	/// Update editor. Regenerates maze.
	/// </summary>
	public bool updateEditor = false;
	/// <summary>
	/// Helper to show you where the beginning of the maze is.
	/// </summary>
	public GameObject PathBeginGizmo = null;
	/// <summary>
	/// Helper to show you where the end of the maze is.
	/// </summary>
	public GameObject PathEndGizmo = null;
	/// <summary>
	/// Block that makes up the tiles of the wall.
	/// Currently a block must have a collider to check its bounds.
	/// </summary>
	public GameObject Block;
	/// <summary>
	/// Show helpers (PathBeginGizmo, PathEndGizmo) and the solution as a line in the editor.
	/// Solution is only shown for a few seconds.
	/// </summary>
	public bool showDebugHelpers = true;
	/// <summary>
	/// Randomize the seed value.
	/// </summary>
	public bool randomSeed = false;
	/// <summary>
	/// The seed value identifies the state of the maze.
	/// Use this value in combination with the width and height to exchange the exact representation of the maze.
	/// Roll your own pseudo randomizer class if you really have to be sure a maze never changes due to updates on the .net randomizer or due to device fragmentation.
	/// </summary>
	public int seed = 123456;
	/// <summary>
	/// Width of the maze in number of cells.
	/// Note that each cell is made out of nine blocks. This means 1 cell in width is 3 blocks in space.
	/// </summary>
	public int widthCells = 10;
	/// <summary>
	/// Height of the maze in number of cells.
	/// Note that each cell is made out of nine blocks. This means 1 cell in height is 3 blocks in space.
	/// </summary>
	public int heightCells = 10;
	
	/// <summary>
	/// A reference to the MFMaze class.
	/// Get low level access to the grid array and found solution of your maze.
	/// </summary>
	public MFMaze mg {get; private set;}
	
	/// <summary>
	/// Use a fixed begin and end entry/exit of the maze.
	/// </summary>
	public bool useFixedBeginEnd = false;
	/// <summary>
	/// The cell index along the maze height on the left side of the maze where you will start. 
	/// Note that each cell is made out of nine blocks. This means 1 cell in height is 3 blocks in space.
	/// </summary>
	public int fixedBeginY = 0;
	/// <summary>
	/// The cell index along the maze height on the right side of the maze where you will end. 
	/// Note that each cell is made out of nine blocks. This means 1 cell in height is 3 blocks in space.
	/// </summary>
	public int fixedEndY = 0;
	
	/// <summary>
	/// Width of the block in worldspace
	/// </summary>
	public float blockWidth {get; private set;}
	/// <summary>
	/// Height of the block in worldspace
	/// </summary>
	public float blockHeight {get; private set;}
	/// <summary>
	/// Total height in worldspace
	/// </summary>
	public float totalHeight {get; private set;}
	
	// Use this for initialization
	void Start () {
		if (Application.isPlaying)
		{
			CreateMaze();
		}
	}
	
	/// <summary>
	/// Creates the maze.
	/// </summary>
	public void CreateMaze()
	{
		Clear();
		
		fixedBeginY = Mathf.Clamp(fixedBeginY, 0, heightCells-1);
		fixedEndY = Mathf.Clamp(fixedEndY, 0, heightCells-1);
		
		if (randomSeed)
			seed = new System.Random().Next(int.MaxValue);
		
		mg = new MFMaze(heightCells, widthCells, seed);
		if (useFixedBeginEnd)
			mg.Generate(fixedBeginY, fixedEndY);
		else 
			mg.Generate();
		
		DrawMaze();
	}
	
	/// <summary>
	/// Remove all child instances from this instance.
	/// </summary>
	public void Clear()
	{
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in transform)
			children.Add(child.gameObject);
		while (children.Count > 0)
		{
			GameObject g = children[children.Count-1];
			if (g != null)
			{
				if (Application.isPlaying)
					Destroy(g);
				else 
					DestroyImmediate(g);
			}
			children.RemoveAt(children.Count-1);
		}			
	}
	
	private void DrawMaze()
	{
		blockWidth = Block.GetComponent<Collider>().bounds.size.x;
		blockHeight = Block.GetComponent<Collider>().bounds.size.y;
		totalHeight = heightCells * blockHeight;
		int blockIndex = 0;
		
		for (int y=0; y<heightCells; y++)
		{
			for (int x=0; x<widthCells; x++)
			{
				MFCell c = mg.maze[y, x];

				bool[] patch = c.AsNinepatch();
				if (Block != null)
				{
					int index = 0;
					for (int iy = 0; iy < 3; iy++)
					{
						for (int ix = 0; ix < 3; ix++)
						{
							float localX = x * (3.0f * blockWidth);
							float localY = y * (3.0f * blockHeight);
							mg.maze[y,x].localNinePatchPositions[index] = new Vector3(localX+(ix*blockWidth), totalHeight-(localY+(iy*blockHeight)), -1.0f);									
								
							if (patch[index])
							{											
								GameObject newG = (GameObject)GameObject.Instantiate(Block, this.transform.localPosition, this.transform.localRotation);
								newG.name = "b"+blockIndex.ToString()+"_x"+x.ToString()+"_y"+y.ToString()+"_px"+ ix.ToString() + "_py" + iy.ToString();
								newG.transform.parent = this.transform;
								newG.transform.localPosition = mg.maze[y,x].localNinePatchPositions[index]; 
							}								
							index++;
						}
					}
					c.localPosition = mg.maze[y,x].localNinePatchPositions[4];
					blockIndex++;
				}
			}
		}
		
		mg.Solve();

		if (showDebugHelpers)
		{ 
			GameObject newGo;
			if (PathBeginGizmo != null)
			{
				newGo = (GameObject)GameObject.Instantiate(PathBeginGizmo, this.transform.localPosition, this.transform.localRotation);
				newGo.transform.parent = this.transform;
				newGo.transform.localPosition = new Vector3(mg.begin.localPosition.x, mg.begin.localPosition.y, 0.0f);
			}
			if (PathEndGizmo != null)
			{
				newGo = (GameObject)GameObject.Instantiate(PathEndGizmo, this.transform.localPosition, this.transform.localRotation);
				newGo.transform.parent = this.transform;
				newGo.transform.localPosition = new Vector3(mg.end.localPosition.x, mg.end.localPosition.y, 0.0f);
			}
			
			Vector3 offset = new Vector3(blockWidth / 2.0f, blockHeight / 2.0f, 0.0f);
			Vector3 st = this.transform.TransformPoint(mg.begin.localPosition + offset);
			foreach(MFCell pc in mg.solution)
			{ 
				Vector3 p = this.transform.TransformPoint(pc.localPosition + offset);
				Debug.DrawLine(st, p, Color.red, 180.0f, false);
				st = p;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!Application.isPlaying)
		{
			if (previewInEditor && updateEditor) 
			{
				updateEditor = false;
				CreateMaze();
			}
			else 
			{
				if (!previewInEditor && transform.childCount > 0)
					Clear();
			}
		}
		else
		{
			
		}
	}
}