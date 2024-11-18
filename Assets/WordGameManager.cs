using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordGameManager : MonoBehaviour
{
    public MazecraftGameManager instance;
    public GameObject tile;
    public int tileCount = 0;
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKey(KeyCode.L)){
            if(instance.activePlayer.activeInHierarchy == true){
                tile = Instantiate(Resources.Load("LetterTilePrefab") as GameObject, Vector3.zero, transform.rotation);
                tile.transform.localPosition = new Vector3(instance.activePlayer.transform.position.x + 10, instance.activePlayer.transform.position.y, instance.activePlayer.transform.position.z);
                Text newTileText = tile.GetComponent<Text>();
                newTileText.text = "Hi!";
                tileCount++;
            }
        }*/
    }
}
