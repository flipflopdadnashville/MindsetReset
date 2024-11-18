using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 2D position in the internal grid array.
/// </summary>
public struct MFPosition
{
	public MFPosition(int x, int y){this.x=x; this.y=y;}
	public int x;
	public int y;
	
	public static bool operator ==(MFPosition left, MFPosition right)
	{
    	return (left.x == right.x) && (left.y == right.y);
	}
	
	public static bool operator !=(MFPosition left, MFPosition right)
	{
    	return !(left == right);
	}
	
	public override bool Equals(object obj)
	{
        if (!(obj is MFPosition))
        {
            return false;
        } 
        return ((MFPosition)obj == this);
    }
	
	public override int GetHashCode()
	{
		return x^y;
	}
}

/// <summary>
/// A maze is built out of cells. Each cell is built out of 4 boundaries where a nine patch can be derived from.
/// </summary>
/// <exception cref='ArgumentOutOfRangeException'>
/// Is thrown when the argument out of range exception.
/// </exception>
public class MFCell
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MFCell"/> class.
	/// </summary>
	/// <param name='position'>
	/// Position.
	/// </param>
    public MFCell(MFPosition position)
    {
        this.position = position;

        // initially, all walls are intact
        this.leftWall = true;
        this.rightWall = true;
        this.upWall = true;
        this.downWall = true;
        this.Path = Paths.None;

        // must be initialized, since it is a member of a struct
        this.visited = false;
        this.Previous = null;
		
		localNinePatchPositions = new List<Vector3>(9);
		for (int i=0; i<9; i++)
			localNinePatchPositions.Add(Vector3.zero);
    }
	/// <summary>
	/// The left wall.
	/// </summary>
    public bool leftWall;
	/// <summary>
	/// The right wall.
	/// </summary>
    public bool rightWall;
	/// <summary>
	/// Up wall.
	/// </summary>
    public bool upWall;
	/// <summary>
	/// Down wall.
	/// </summary>
    public bool downWall;
	/// <summary>
	/// visited: used by search algorithm.
	/// </summary>
    public bool visited;
	/// <summary>
	/// Paths.
	/// </summary>
    public enum Paths
    {
        Up, Down, Right, Left, None
    }
	/// <summary>
	/// The direction in the path it has.
	/// </summary>
    public Paths Path;

    /// <summary>
    /// Gets or sets a reference to the previous MFCell in the found path chain
    /// </summary>
    public MFCell Previous;

    /// <summary>
    /// Provides indexing to the boolean fields in the cell
    /// </summary>
    /// <param name="index">0 leftW, 1 rightW, 2 UpW, 3 downW, 4 visited</param>
    /// <returns></returns>
    public bool this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.leftWall;
                case 1:
                    return this.rightWall;
                case 2:
                    return this.upWall;
                case 3:
                    return this.downWall;
                case 4:
                    return this.visited;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    this.leftWall = value;
                    break;
                case 1:
                    this.rightWall = value;
                    break;
                case 2:
                    this.upWall = value;
                    break;
                case 3:
                    this.downWall = value;
                    break;
                case 4:
                    this.visited = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
	
	/// <summary>
	/// The position.
	/// </summary>
	private MFPosition position;
    /// <summary>
    /// The current location on the two-dimensional container
    /// </summary>
    public MFPosition Position
    {
        get { return this.position; }
    }

	/// <summary>
	/// Convenience field to store the position in local space.
	/// </summary>
	public Vector3 localPosition; 
	
	/// <summary>
	/// Convenience field to store the positions of each element of the ninepatch in local space. 
	/// </summary>
	public List<Vector3> localNinePatchPositions;
	 
    /// <summary>
    /// Reset a cell so that all walls are intact and not visited
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < 4; i++)
        {
            this[i] = true;
        }
        this.visited = false;
    }
	
	/// <summary>
	/// Derive a ninepatch from the current cell setup.
	/// </summary>
	/// <returns>
	/// The ninepatch as an array of bool. If true then there's a block, if not it's walkable and there is no block.
	/// </returns>
	public bool[] AsNinepatch()
	{
		bool[] tile;
		tile = new bool[]{true, upWall, true, leftWall, false, rightWall, true, downWall, true};
		return tile;			
	}
}

/// <summary>
/// The Maze generator.
/// Generates an abstract two dimensional array of mfcells.
/// Use this class with your own visualisation routine, or use the <see cref="MazeFactory"/> class on an empty game object to do it for you. 
/// </summary>
public class MFMaze 
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MFMaze"/> class.
	/// </summary>
	/// <param name='height'>
	/// Height in number of cells.
	/// </param>
	/// <param name='width'>
	/// Width in number of cells.
	/// </param>
	/// <param name='seed'>
	/// Random seed
	/// </param>
	public MFMaze(int height, int width, int seed)
    {
        this.maze = new MFCell[height, width];
		this.height = height;
		this.width = width;
		random = new MFRandomizer(seed);
    }

    private int width;
    private int height; 
	
	/// <summary>
	/// The maze in an two dimensional array of MFCells.
	/// </summary>
    public MFCell[,] maze;
	/// <summary>
	/// The start point of the maze.
	/// </summary>
    public MFCell begin;
	/// <summary>
	/// The end point of the maze.
	/// </summary>
    public MFCell end;
 
    /// <summary>
    /// The shortest path through the maze from begin to end
    /// </summary>
    public List<MFCell> solution = new List<MFCell>();
 
    /// <summary>
    /// Used to generate maze
    /// </summary>
    private MFRandomizer random;

	/// <summary>
	/// Generate maze with random begin position and random end position
	/// </summary>
    public void Generate()
    {
		Generate(null, null);
	}
	
	/// <summary>
	/// Generate maze with the specified beginY and endY.
	/// </summary>
	/// <param name='beginY'>
	/// Begin y.
	/// </param>
	/// <param name='endY'>
	/// End y.
	/// </param>
    public void Generate(int? beginY, int? endY)
    {
        this.init(this.maze, width, height);

        this.depthFirstSearchMazeGeneration(this.maze, this.width, this.height, beginY, endY);
    }

    /// <summary>
    /// Resets a maze array
    /// </summary>
    /// <param name="arr">The maze array</param>
    /// <param name="width">Number of cells in width</param>
    /// <param name="height">Number of cells in height</param>
    private void init(MFCell[,] arr, int width, int height)
    {
        this.width = width;
        this.height = height;

        for (int i = 0; i < this.height; i++)
        {
            for (int j = 0; j < this.width; j++)
            {
                arr[i, j] = new MFCell(new MFPosition(j, i));
            }
        }
    }

    /// <summary>
    /// Generate a maze with the Depth-First Search approach
    /// http://en.wikipedia.org/wiki/Depth-first_search
    /// </summary>
    /// <param name="arr">the array of cells</param>
    /// <param name="width">A width for the maze</param>
    /// <param name="height">A height for the maze</param>
    private void depthFirstSearchMazeGeneration(MFCell[,] arr, int width, int height, int? beginY, int? endY)
    {
        Stack<MFCell> stack = new Stack<MFCell>();

        MFCell location = arr[this.random.Next(height), this.random.Next(width)];
        stack.Push(location);
        
        while (stack.Count > 0)
        {
            List<MFPosition> neighbours = this.getNeighbours(arr, location, width, height);
            if (neighbours.Count > 0)
            {
                MFPosition temp = neighbours[random.Next(neighbours.Count)];

                this.removeWall(arr, ref location, ref arr[temp.y, temp.x]);

                stack.Push(location);
                location = arr[temp.y, temp.x];
            }
            else
            {
                location = stack.Pop();
            } 
        }

        this.makeMazeBeginEnd(this.maze, beginY, endY);
    }


    /// <summary>
    /// Used to create a begin and end for a maze
    /// </summary>
    /// <param name="arr">The array of the maze</param>
    private void makeMazeBeginEnd(MFCell[,] arr, int? beginY, int? endY)
    {
        MFPosition temp = new MFPosition();
        temp.y = beginY ?? this.random.Next(this.height);
        arr[temp.y, temp.x].leftWall = false;
        this.begin = arr[temp.y, temp.x];

        temp.y = endY ?? this.random.Next(this.height);
        temp.x = this.width - 1;
        arr[temp.y, temp.x].rightWall = false;
        this.end = arr[temp.y, temp.x];
    }

    /// <summary>
    /// Remove wall between two adjacent cellls
    /// </summary>
    /// <param name="maze">The maze array</param>
    /// <param name="current">the current cell</param>
    /// <param name="next">the next neighbor cell</param>
    private void removeWall(MFCell[,] maze, ref MFCell current, ref MFCell next)
    {
        // The next is down
        if (current.Position.x == next.Position.x && current.Position.y > next.Position.y)
        {
            maze[current.Position.y, current.Position.x].upWall = false;
            maze[next.Position.y, next.Position.x].downWall = false;
        }
        // the next is up
        else if (current.Position.x == next.Position.x)
        {
            maze[current.Position.y, current.Position.x].downWall = false;
            maze[next.Position.y, next.Position.x].upWall = false;
        }
        // the next is right
        else if (current.Position.x > next.Position.x)
        {
            maze[current.Position.y, current.Position.x].leftWall = false;
            maze[next.Position.y, next.Position.x].rightWall = false;
        }
        // the next is left
        else
        {
            maze[current.Position.y, current.Position.x].rightWall = false;
            maze[next.Position.y, next.Position.x].leftWall = false;
        }
    }

    /// <summary>
    /// Determines whether a particular cell has all its walls intact
    /// </summary>
    /// <param name="arr">the maze array</param>
    /// <param name="cell">The cell to check</param>
    /// <returns></returns>
    private bool allWallsIntact(MFCell[,] arr, MFCell cell)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!arr[cell.Position.y, cell.Position.x][i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Gets all neighbor cells to a specific cell, 
    /// where those neighbors exist and not visited already
    /// </summary>
    /// <param name="arr">The maze array</param>
    /// <param name="cell">The current cell to get neighbors</param>
    /// <param name="width">The width of the maze</param>
    /// <param name="height">The height of the maze</param>
    /// <returns></returns>
    private List<MFPosition> getNeighbours(MFCell[,] arr, MFCell cell, int width, int height)
    {
        MFPosition temp = cell.Position;
        List<MFPosition> availablePlaces = new List<MFPosition>();

        // Left
        temp.x = cell.Position.x - 1;
        if (temp.x >= 0 && this.allWallsIntact(arr, arr[temp.y, temp.x]))
        {
            availablePlaces.Add(temp);
        }
        // Right
        temp.x = cell.Position.x + 1;
        if (temp.x < width && this.allWallsIntact(arr, arr[temp.y, temp.x]))
        {
            availablePlaces.Add(temp);
        }

        // Up
        temp.x = cell.Position.x;
        temp.y = cell.Position.y - 1;
        if (temp.y >= 0 && this.allWallsIntact(arr, arr[temp.y, temp.x]))
        {
            availablePlaces.Add(temp);
        }
        // Down
        temp.y = cell.Position.y + 1;
        if (temp.y < height && this.allWallsIntact(arr, arr[temp.y, temp.x]))
        {
            availablePlaces.Add(temp);
        }
        return availablePlaces;
    } 
    /// <summary>
    /// Used to reset all cells
    /// </summary>
    /// <param name="arr">The maze array to reset elements</param>
    private void unvisitAll(MFCell[,] arr)
    {
        for (int i = 0; i < this.maze.GetLength(0); i++)
        {
            for (int j = 0; j < this.maze.GetLength(1); j++)
            {
                arr[i, j].visited = false;
                arr[i, j].Path = MFCell.Paths.None;
            }
        }
    }

    /// <summary>
    /// Solves the current maze  
    /// </summary>
    public void Solve()
    {
        // initialize
        this.solution.Clear();
        this.unvisitAll(this.maze);
        
        this.depthFirstSearchSolve(this.begin, this.end); 
		this.solution.Reverse();
    } 

    /// <summary>
    /// Solves a maze with iterative backtracking
    /// </summary>
    /// <param name="start">The start of the maze cell</param>
    /// <param name="end">The end of the maze cell</param>
    /// <returns>returrns true if the path is found</returns>
    private bool depthFirstSearchSolve(MFCell start, MFCell end)
    {
        Stack<MFCell> stack = new Stack<MFCell>();

        stack.Push(start);

        while (stack.Count > 0)
        {
            MFCell temp = stack.Pop();

            // base condition
            if (temp.Position == end.Position)
            {
                // add end point to foundPath
                this.solution.Add(temp);
                while (temp.Previous != null)
                {
                    this.solution.Add(temp);
                    temp = temp.Previous;
                }
                // add begin point to foundPath
                this.solution.Add(temp);
                // to view green square on it
                this.maze[temp.Position.y, temp.Position.x].visited = true;
                return true;
            } 

            // mark as visited to prevent infinite loops
            this.maze[temp.Position.y, temp.Position.x].visited = true;


            // Check every neighbor cell
            // If it exists (not outside the maze bounds)
            // and if there is no wall between start and it
                // set the next.Previous to the current cell
                // push next into stack
            // else complete

            // Left
            if (temp.Position.x - 1 >= 0
                && !this.maze[temp.Position.y, temp.Position.x - 1].rightWall
                && !this.maze[temp.Position.y, temp.Position.x - 1].visited)
            {
                this.maze[temp.Position.y, temp.Position.x - 1].Previous = this.maze[temp.Position.y, temp.Position.x];
                stack.Push(this.maze[temp.Position.y, temp.Position.x - 1]);
            }

            // Right
            if (temp.Position.x + 1 < this.width
                && !this.maze[temp.Position.y, temp.Position.x + 1].leftWall
                && !this.maze[temp.Position.y, temp.Position.x + 1].visited)
            {
                this.maze[temp.Position.y, temp.Position.x + 1].Previous = this.maze[temp.Position.y, temp.Position.x];
                stack.Push(this.maze[temp.Position.y, temp.Position.x + 1]);
            }

            // Up
            if (temp.Position.y - 1 >= 0
                && !this.maze[temp.Position.y - 1, temp.Position.x].downWall
                && !this.maze[temp.Position.y - 1, temp.Position.x].visited)
            {
                this.maze[temp.Position.y - 1, temp.Position.x].Previous = this.maze[temp.Position.y, temp.Position.x];
                stack.Push(this.maze[temp.Position.y - 1, temp.Position.x]);
            }

            // Down
            if (temp.Position.y + 1 < this.height
                && !this.maze[temp.Position.y + 1, temp.Position.x].upWall
                && !this.maze[temp.Position.y + 1, temp.Position.x].visited)
            {
                this.maze[temp.Position.y + 1, temp.Position.x].Previous = this.maze[temp.Position.y, temp.Position.x];
                stack.Push(this.maze[temp.Position.y + 1, temp.Position.x]);
            }
        }
        // no solution found
        return false;
    }
}