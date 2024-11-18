using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMandola : MonoBehaviour
{
    [SerializeField] Renderer DemoMandala = default;
    float x = 20;
    bool increment;

    void Start(){
        increment = true;
        DemoMandala.material.SetFloat("_MandalaLayers", 8);
        DemoMandala.material.SetFloat("_MandalaRotationShift", x);
    }
    // Update is called once per frame
    void Update()
    {
        if(x < 21){
            increment = true;
        }
        else if(x > 127){
            increment = false;
        }

        if(increment == true){
            x = x + .07f;
            DemoMandala.material.SetFloat("_MandalaRotationShift", x);
            DemoMandala.material.SetFloat("_MandalaScaleShift", x);
        }
        
        if(increment == false){
            //DebugObject.Log("Greater than target");
            x = x - .07f;
            DemoMandala.material.SetFloat("_MandalaRotationShift", x);
            DemoMandala.material.SetFloat("_MandalaScaleShift", x);
        }

        //DebugObject.Log(x);
        //DebugObject.Log(DemoMandala.material.GetFloat("_MandalaRotationShift"));
        //RotationText.text = x.ToString();
    
        //DemoMandala.material.SetFloat("_MandalaScaleShift", x);
        //ScaleText.text = x.ToString();
    }
}
