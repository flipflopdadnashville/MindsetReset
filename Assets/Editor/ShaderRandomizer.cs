using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] paths = AssetDatabase.GetAllAssetPaths();
        foreach(string s in paths) {
            Shader shader = (Shader)AssetDatabase.LoadAssetAtPath(s, typeof(Shader));
        
            if(shader != null) {
                Debug.Log(shader.name);
                int propertyCount = ShaderUtil.GetPropertyCount(shader);
                for(int j = 0; j < propertyCount; j++){
                    string st = ShaderUtil.GetPropertyName (shader, j);
                    int type = (int)ShaderUtil.GetPropertyType(shader, j);
                    Debug.Log("Shader property name is: " + st + " " + "and type is: " + type);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
