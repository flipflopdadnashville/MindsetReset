// ping-pong animate background color
using UnityEngine;
using System.Collections;

public class ChangeBackgroundColor : MonoBehaviour
{
    Color color1;
    Color color2;
    public float duration = 1550.0F;

    public Camera cam;

    void Start()
    {
        color1 = Random.ColorHSV(0f, 1f, .1f, .5f, 0.5f, 1f);
        color2 = Random.ColorHSV(0f, 1f, .1f, .5f, 0.5f, 1f);
        cam = Camera.main;
        cam.clearFlags = CameraClearFlags.SolidColor;
        InvokeRepeating("GetRandomColors",30,30);
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        cam.backgroundColor = Color.Lerp(color1, color2, t);
    }

    public void GetRandomColors(){
        Color c = Random.ColorHSV(0f, 1f, .1f, .5f, 0.5f, 1f);
        color1 = color2;
        color2 = c;
    }
}