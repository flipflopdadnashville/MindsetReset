using UnityEngine;
 using System.Collections;
 
 public class LightController : MonoBehaviour {
 
     public int lightFactor = 0;

     // Array of random values for the intensity.
     private float[] smoothing = new float[20];
     // Light that should be controlled by the LightController
     public GameObject controlledLight01 = null;
     Light l;
 
     void Start () {
        controlledLight01 = this.gameObject;
        l = controlledLight01.GetComponent<Light>(); // Get the Light component
        Color c = new Color();
        c.r= (1f - lightFactor / Random.Range(1,100)) - .1f;
        c.g= (lightFactor / Random.Range(1,100)) -.1f;
        c.b= (lightFactor / Random.Range(1,100)) - .1f;
        c.a=1f;
        l.color = c;

        //Initialize the array.
         for(int i = 0 ; i < smoothing.Length ; i++){
             smoothing[i] = .0f;
         }
        StartCoroutine(ColorChangeRoutine());
     }
     
     void Update () {
        float sum = .0f;
         
         // Shift values in the table so that the new one is at the
         // end and the older one is deleted.
         for(int i = 1 ; i < smoothing.Length ; i++)
         {
             smoothing[i-1] = smoothing[i];
             sum+= smoothing[i-1];
         }
 
         // Add the new value at the end of the array.
         smoothing[smoothing.Length -1] = Random.value;
         sum+= smoothing[smoothing.Length -1];
 
         // Compute the average of the array and assign it to the
         // light intensity.
         GetComponent<Light>().intensity = sum / smoothing.Length;
     }

    private IEnumerator ColorChangeRoutine() {
        while (true) {
            var startColor = l.color;
            var endColor = new Color32(System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), 255);
        
            float t = 0;
            while (t < 1) {
                t = Mathf.Min(1, t + Time.deltaTime); // Multiply Time.deltaTime by some constant to speed/slow the transition.
                l.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }
        
            yield return new WaitForSeconds(2);
        }
    }
 }