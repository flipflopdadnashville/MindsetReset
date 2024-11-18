using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadMaterialSelectorApp : MonoBehaviour
{
    private Object[] materialArray;
    public GameObject uiTarget;
    public GameObject tile;
    // Start is called before the first frame update
    void Start()
    {
        materialArray = Resources.LoadAll("Materials", typeof(Material));
        int i = 0;
        foreach(Material material in materialArray){
            Debug.Log("Material name is: " + material.name);
            Instantiate(tile);
            tile.transform.SetParent(uiTarget.transform);
        
            tile.GetComponent<TextMeshPro>().text = material.name;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
