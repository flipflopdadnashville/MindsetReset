using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public MazecraftGameManager instance;
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject player;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public GameObject GoalPrefab = null;
	private int maxGoals = 10;
	private int currentGoals = 0;
	public bool AddGaps = true;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 4;
	public float CellHeight = 4;
	public int mapHeight = 100;
	private int mazeNumber;
	private string mazeName;
	GameObject newMaze;
	private BasicMazeGenerator mMazeGenerator = null;
	private List<float> availableCoinXPositions = new List<float>();
	private List<float> availableCoinZPositions = new List<float>();
	AsyncOperationHandle<Material> opMaterialHandle;

/*	void Start () {
		// instance.materialArray = Resources.LoadAll("Materials", typeof(Material));

		instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
		mazeNumber = 1;

		int currentColumns = Random.Range(3, 10);

		// going to keep things square for the moment
		int currentRows = currentColumns;
		MazeGenerationAlgorithm algorithm = (MazeGenerationAlgorithm)Random.Range(0, 3);
		
		mazeName = "MazeSpawner_" + mazeNumber;
		newMaze = GameObject.Find(mazeName);

		GenerateLevelMaze(newMaze, currentRows, 8, algorithm);
		newMaze.transform.localPosition = new Vector3(0, 0, 4.5f);

	}

	void Update(){
		player = instance.activePlayer;
		if(instance.rewardsCanvas.activeInHierarchy == false){
            if(Input.GetKeyDown(KeyCode.M)){
				CreateNewMaze();
			}
			
			if(Input.GetKeyDown(KeyCode.U)){
				ChangeMazeMaterial();
			}
        }
	}

	public void GenerateLevelMaze(GameObject newMaze, int rows, int columns, MazeGenerationAlgorithm algorithm){
		Rows = rows;
		//Debug.Log("rows is: " + rows);
		Columns = columns;
		//Debug.Log("columns is: " + columns);

		availableCoinXPositions.Clear();
		for(int i = 0; i <= rows; i++){
				availableCoinXPositions.Add(i * 4);
		}

		availableCoinZPositions.Clear();
		for(int i = 0; i <= columns; i++){
				availableCoinZPositions.Add(i * 4);
		}

		if (!FullRandom) {
				Random.seed = RandomSeed;
			}

			switch (Algorithm) {
			case MazeGenerationAlgorithm.PureRecursive:
				mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveTree:
				mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RandomTree:
				mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.OldestTree:
				mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveDivision:
				mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
				break;
			}

			// change all the Quaternion x values to 90 for a "sidewalk" effect
			mMazeGenerator.GenerateMaze ();
			for (int row = 0; row < Rows; row++) {
				for(int column = 0; column < Columns; column++){
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
					GameObject tmp;
					// tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
					// tmp.transform.parent = transform;
					// tmp = Instantiate(Floor,new Vector3(x,2,z), Quaternion.Euler(0,0,0)) as GameObject;
					// tmp.transform.parent = transform;
					if(cell.WallRight){
						tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
						tmp.transform.parent = newMaze.transform;
					}
					if(cell.WallFront){
						tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
						tmp.transform.parent = newMaze.transform;
					}
					if(cell.WallLeft){
						tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
						tmp.transform.parent = newMaze.transform;
					}
					if(cell.WallBack){
						tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
						tmp.transform.parent = newMaze.transform;
					}
					if(cell.IsGoal && GoalPrefab != null){
						if(currentGoals <= maxGoals){
							tmp = Instantiate(GoalPrefab, new Vector3(x,-1,-1), Quaternion.Euler(0,0,0)) as GameObject;
							tmp.transform.parent = newMaze.transform;
							currentGoals = currentGoals + 1;
						}
					}
				}
			}
			if(Pillar != null){
				for (int row = 0; row < Rows+1; row++) {
					//changed to Columns (from Columns + 1) to leave gap at end
					for (int column = 0; column < Columns; column++) {
						float x = column*(CellWidth+(AddGaps?.2f:0));
						float z = row*(CellHeight+(AddGaps?.2f:0));
						GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
						tmp.transform.parent = newMaze.transform;
					}
				}
			}

			newMaze.transform.Rotate(90, 0, 0);
			if(instance.activePlayer.name == "glasses"){
				//Debug.Log("On maze creation, username is glasses");
				newMaze.transform.localPosition = new Vector3(instance.activePlayer.transform.position.x + (CellWidth * 8), player.transform.position.y - 2, 11);
			}
			else{
				newMaze.transform.localPosition = new Vector3(instance.activePlayer.transform.position.x + (CellWidth * 8), instance.activePlayer.transform.position.y -2, 11);
			}

			newMaze.AddComponent<DragMaze>();

			instance.rotatingMaze.transform.localPosition = new Vector3(newMaze.transform.position.x + 100, newMaze.transform.position.y - 10, 11);

		List<GameObject> mazeCells = new List<GameObject>();
		foreach (Transform child in newMaze.transform)
		{
			if (child.CompareTag("bombTarget"))
				mazeCells.Add(child.gameObject);
		}

		// place coins together
		GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
			foreach(GameObject coin in coins){
				coin.transform.localPosition = new Vector3((CellWidth % 2), 1.5f, ((Rows * CellHeight) - (CellWidth - 2)));
				coin.transform.Rotate(90, 0, 0);
				coin.transform.localScale = new Vector3(.3f, .3f, .3f);
				// scooch it over a multiple of 4 to "randomize its location;

				coin.transform.localPosition = new Vector3(availableCoinXPositions[Random.Range(0, availableCoinXPositions.Count)], 1f, coin.transform.localPosition.z - availableCoinZPositions[Random.Range(0, availableCoinZPositions.Count)]);
			}

			if(instance.mazeStructure == MazecraftGameManager.MazeStructure.PillarsOnly){
				foreach(GameObject mazePart in mazeCells){
					if(mazePart.name == "WallPrefab(Clone)"){
						mazePart.GetComponent<BoxCollider>().enabled = false;
						mazePart.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}

			ChangeMazeMaterial();
			currentGoals = 0;

			List<GameObject> bottomRow = new List<GameObject>();
			List<GameObject> topRow = new List<GameObject>();

			float bottomRowZ = (Rows * CellHeight);
			foreach (GameObject cell in mazeCells)
			{
				////Debug.Log(cell.transform.localPosition);
				if (cell.transform.localPosition.z == bottomRowZ)
				{
					bottomRow.Add(cell);
				}

				if (cell.transform.localPosition.z == 0)
				{
					topRow.Add(cell);
				}
			}

			//Debug.Log("Bottom row count is: " + bottomRow.Count);
			if (bottomRow.Count > 0)
			{
				Destroy(bottomRow[Random.Range(1, bottomRow.Count - 1)]);
			}

			//Debug.Log("Top row count is: " + topRow.Count);
			if (topRow.Count > 0)
			{
				Destroy(topRow[Random.Range(1, topRow.Count - 1)]);
			}
	}

	public void ChangeMazeMaterial(){
			StartCoroutine(LoadMaterial());			
	}

	public Transform GetClosestWall(List<Transform> walls, Vector3 sourcePosition)
	{
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = sourcePosition;
		foreach (Transform t in walls)
		{
			float dist = Vector3.Distance(t.position, currentPos);
			if (dist < minDist)
			{
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}

	public void CreateNewMaze(){
		//if(instance.forestTerrain.activeInHierarchy == false){
			if(GoalPrefab.activeInHierarchy == false && instance.forestTerrain.activeInHierarchy == false){
					GoalPrefab.SetActive(true);
			}

			mazeNumber++;
			//Debug.Log("Maze number is: " + mazeNumber);

			int currentColumns;
			currentColumns = Random.Range(3, 20);
			//Debug.Log("currentColumns is: " + currentColumns);

			// going to keep things square for the moment
			int currentRows;
			currentRows = Random.Range(3, 20);
			//Debug.Log("currentRows is: " + currentRows);
			
			mazeName = "MazeSpawner_" + mazeNumber;
			//Debug.Log("Spawning new maze: " + mazeName);

			Instantiate(Resources.Load("MazeSpawner"));
			GameObject.Find("MazeSpawner(Clone)").name = mazeName;
			
			GameObject newMaze = GameObject.Find(mazeName);
			newMaze.transform.parent = GameObject.Find("Game").transform;
			MazeGenerationAlgorithm algorithm = (MazeGenerationAlgorithm)Random.Range(0, 3);

			GenerateLevelMaze(newMaze, currentRows, currentColumns, algorithm);
			
			if(GoalPrefab.activeInHierarchy == true){
				GoalPrefab.SetActive(false);
			}

			instance.isNotificationActive = false;

			if(instance.forestTerrain.activeInHierarchy == true && instance.activePlayer.activeInHierarchy == true){
				newMaze.transform.parent = GameObject.Find("EndlessTerrain").transform;

				int rNum = Random.Range(1,4);
				//Debug.Log("Random Number is: " + rNum);

				if(rNum  == 1){
					newMaze.transform.Rotate(90, Random.Range(0,45), Random.Range(0,45), 0);
					newMaze.transform.localPosition = new Vector3(instance.activePlayer.transform.localPosition.x + Random.Range(-300, 300), newMaze.transform.parent.localPosition.y + Random.Range(83, 84
					), instance.activePlayer.transform.localPosition.z + Random.Range(-50, -100));
				}
				else if(rNum == 2){
					newMaze.transform.Rotate(0, Random.Range(-45,45), Random.Range(-45,45), 0);
					newMaze.transform.localPosition = new Vector3(instance.activePlayer.transform.localPosition.x + Random.Range(-300, 300), newMaze.transform.parent.localPosition.y + Random.Range(103, 116), instance.activePlayer.transform.localPosition.z + Random.Range(-50, -100));
				}
				else if(rNum == 3){
					newMaze.transform.localPosition = new Vector3(instance.activePlayer.transform.localPosition.x + Random.Range(-300, 300), newMaze.transform.parent.localPosition.y + Random.Range(103, 116), instance.activePlayer.transform.localPosition.z + Random.Range(-50, -100));
				}
			}

			newMaze.AddComponent<MazeBehavior>();
		//}
	}

	public IEnumerator LoadMaterial()
	{
		opMaterialHandle = Addressables.LoadAssetAsync<Material>(instance._preloadedMaterialKeys[Random.Range(0, instance._preloadedMaterialKeys.Count - 1)]);
		yield return opMaterialHandle;

		//Debug.Log("opMaterialHandle result is: " + opMaterialHandle.Result);

		if (opMaterialHandle.Status == AsyncOperationStatus.Succeeded)
		{
			Material currentMaterial = opMaterialHandle.Result;

			GameObject[] allMazeCells = GameObject.FindGameObjectsWithTag("bombTarget");

			// get name of object player is touching
			List<Transform> mazeCellTransforms = new List<Transform>();

			foreach (GameObject mazeCell in allMazeCells)
			{
				mazeCellTransforms.Add(mazeCell.transform);
			}

			Transform closestWall = GetClosestWall(mazeCellTransforms, instance.activePlayer.transform.position);

			// get name of parent of object player is touching
			Transform currentMazeTransform = closestWall.parent;
			GameObject currentMaze = GameObject.Find(currentMazeTransform.name);

			//Debug.Log("From ChangeMazeMaterial, closest maze is: " + currentMaze.name);

			// get all children of parent with tag "bomb target"
			List<GameObject> currentMazeCells = new List<GameObject>();
			foreach (Transform child in currentMaze.transform)
			{
				if (child.tag == "bombTarget" || child.tag == "mazePillar")
				{
					currentMazeCells.Add(child.gameObject);
				}
			}

			//Debug.Log("Current maze cells count is: " + currentMazeCells.Count);
			foreach (GameObject cell in currentMazeCells)
			{
				cell.GetComponent<MeshRenderer>().material = currentMaterial;
			}
		}
	}
*/}