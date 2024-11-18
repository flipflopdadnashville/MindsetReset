using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNameShadowToName : MonoBehaviour
{
    public Text name;
    public Text nameShadow;
    // Start is called before the first frame update
    void Start()
    {
        nameShadow.text = name.text;
    }
}
