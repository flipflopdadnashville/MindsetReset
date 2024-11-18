using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveTheMazeLevelController : MonoBehaviour
    {
        public GameObject[] koiArray = new GameObject[7];
        public Material[] otherSkybox = new Material[5];
        // public GameObject cube;
        private Material cubeMaterial;
        public GameObject maze;
        // public MeshRenderer cubeMeshRenderer;
        private int levelNumber;
        private DirectoryInfo dir;
        private FileInfo[] fileInfo;

        /// <summary>
        /// A reference to the MFMaze class.
        /// Get low level access to the grid array and found solution of your maze.
        /// </summary>
        public MFMaze mg {get; private set;}

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

        /// <summary>
        /// Block that makes up the tiles of the wall.
        /// Currently a block must have a collider to check its bounds.
        /// </summary>
        public GameObject Block;
        public GameObject parentBlock;
        /// <summary>
        /// The seed value identifies the state of the maze.
        /// Use this value in combination with the width and height to exchange the exact representation of the maze.
        /// Roll your own pseudo randomizer class if you really have to be sure a maze never changes due to updates on the .net randomizer or due to device fragmentation.
        /// </summary>
        public int seed = 123456;
        /// Width of the maze in number of cells.
        /// Note that each cell is made out of nine blocks. This means 1 cell in width is 3 blocks in space.
        /// </summary>
        public int widthCells = 10;
        /// <summary>
        /// Height of the maze in number of cells.
        /// Note that each cell is made out of nine blocks. This means 1 cell in height is 3 blocks in space.
        /// </summary>
        public int heightCells = 10;
        
        private Material[] materialsArray;

        public int numberOfMazes = 5;

        private bool _stopSpawn = false;
        private float lastYPos = -20;

        public List<MFMaze> mazes = new List<MFMaze>();
        private int mazeCount;

        void Start(){
            levelNumber = 1;
            //Debug.Log(levelNumber);
            //Debug.Log("lastYPos at Start is: " + lastYPos);
            
            dir = new DirectoryInfo("Assets/Resources/Materials");
            fileInfo = dir.GetFiles("*.mat");

            materialsArray = Resources.LoadAll("Materials", typeof(Material)).Cast<Material>().ToArray();

            StartCoroutine("SpawnRoutine");
        }

        private IEnumerator SpawnRoutine(){
            while(_stopSpawn == false){
                // GameObject newKoi = (GameObject)GameObject.Instantiate(koiArray[Random.Range(0,6)], this.transform.localPosition, new Quaternion(-20,0,0,0));
                // newKoi.transform.position = new Vector3(Random.Range(-75,75), Random.Range(-600,25), .48f);
                //Debug.Log("From SpawnRoutine, lastYPos is: " + lastYPos);
                GameObject templateBlock = GameObject.Find("CubeMesh");
                templateBlock.GetComponent<Renderer>().material = cubeMaterial;
                heightCells = Random.Range(2,15);
                widthCells = Random.Range(3,7);
                GameObject koi = koiArray[Random.Range(0,6)];
                //Debug.Log("koi's name is: " + koi.name);
                //Debug.Log("Creating new maze with following paramaters: " + heightCells + "-" + widthCells + "-" + seed);
                mg = new MFMaze(heightCells, widthCells, seed);
                //mg.Generate(null, null, koi);
                mazes.Add(mg);
                mazeCount++;
                //Debug.Log("Maze count is: " + mazeCount);
                float newYPos = lastYPos - Random.Range(15, 25);
                //Debug.Log("From SpawnRoutine, newYPos is: " + newYPos);
                lastYPos = DrawMaze(newYPos);

                if(mazeCount > 10){
                    //Debug.Log("Removing maze: " + mazes[0].name);
                    mazes.Remove(mazes[0]);
                }
                
                yield return new WaitForSeconds(10);
            }
        }

        private float DrawMaze(float lastYPos)
        {
            //Debug.Log("DrawMaze lastYPos is: " + lastYPos);
            blockWidth = Block.GetComponent<Collider>().bounds.size.x;
            blockHeight = Block.GetComponent<Collider>().bounds.size.y;
            totalHeight = heightCells * blockHeight;
            int blockIndex = 0;
            
            GameObject parent = GameObject.Instantiate(parentBlock, new Vector3(Random.Range(-75, 75), lastYPos, 1f), new Quaternion(0,0,-70,0));
            parent.name = "mazeParent" + blockWidth + Random.Range(1,9999999) + "_" + blockHeight + Random.Range(0,99999999);
            string parentName = parent.name;
            lastYPos = parent.transform.position.y;
            //Debug.Log("New lastYPos is: " + lastYPos);
            parent.transform.position = new Vector3(Random.Range(-12, 15), lastYPos, -.48f);

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
                                    newG.transform.parent = GameObject.Find(parentName).transform;
                                    newG.tag = "MazeTwoWall";
                                    newG.transform.localPosition = mg.maze[y,x].localNinePatchPositions[index]; 
                                    //newG.AddComponent<NoTouchy>();
                                }								
                                index++;
                            }
                        }
                        c.localPosition = mg.maze[y,x].localNinePatchPositions[4];
                        blockIndex++;
                    }
                }
            }
            GameObject.Find(parentName).transform.Rotate(new Vector3(0, 0, Random.Range(60,150)));
            MazeManagerJWR mm = GameObject.Find(parentName).GetComponent<MazeManagerJWR>();
            mm.angularAcceleration = Random.Range(.1f, 2f);
            mm.angularDrag = Random.Range(.1f, .8f);
            GameObject[] newMazeCubes = GameObject.FindGameObjectsWithTag("object");
            string filename = fileInfo[levelNumber].Name.ToString().Split('.')[0];

            //Debug.Log("file count is: " + fileInfo.Length);
            int newMaterial = Random.Range(0, fileInfo.Length);
            //Debug.Log("random number is: " + newMaterial);

            foreach(GameObject cube in newMazeCubes){
                cubeMaterial = materialsArray[newMaterial];
                cube.GetComponent<Renderer>().material = cubeMaterial;
            }

            return lastYPos;
        }
    }