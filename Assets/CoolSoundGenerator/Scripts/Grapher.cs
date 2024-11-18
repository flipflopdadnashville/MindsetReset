using UnityEngine;
public class Grapher : MonoBehaviour
{

    [Range(10, 500)]
    public int resolution = 300;
    private int currentResolution;
    private ParticleSystem.Particle[] points;
    private string tipeOfWave;

    private float x_;
    private float x__;
    private float devideFactor;
    private float delta;
    private float deltaBlockMax;

    private bool squareWaveAmplitudeBool = false;
    private int squareWaveAmplitude = 1;
    private bool triangleDOWN = false;

    private void CreatePoints()
    {
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution];
        float increment = 1f / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].startColor = new Color(x, 0f, 0f);
            points[i].startSize = 0.015f;
        }
    }


    void Update()
    {
        if (currentResolution != resolution || points == null)
        {
            CreatePoints();
        }

        for (int i = 0; i < resolution; i++)
        {

            if ((GenerateWave.TypeOfWave == 1) || (GenerateWave.TypeOfWave == 2) || (GenerateWave.TypeOfWave == 3) || (GenerateWave.TypeOfWave == 4))
            {
                //for SINE //for SAW //for SQUARE //for TRIANGLE
                Vector3 p = points[i].position;
                p.y = Wave(p.x);
                points[i].position = p;
                Color c = points[i].startColor;
                c.g = p.y;
                points[i].startColor = c;
            }
            else if (GenerateWave.TypeOfWave == 5)
            {
                //for TANGENT
                Vector3 p = points[i].position;
                if ((Wave(p.x) > 1) || (Wave(p.x) < 0))
                {
                    continue;
                }
                else
                {
                    p.y = Wave(p.x);
                    points[i].position = p;
                    Color c = points[i].startColor;
                    c.g = p.y;
                    points[i].startColor = c;
                }
            }
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }


    //<to visualize the sound wave on the screen in real time>
    private float Wave(float x)
    {
 
        if (this.gameObject.name == "GraphParticleSystemForLeftChannel")
        {
            if (GenerateWave.TypeOfWave == 1)
            {
                //<SINE> LeftChannel
                return (0.5f + 0.5f * Mathf.Sin(5 * Mathf.PI * x * GenerateWave.varSinForGrapherLeftChannel + Time.timeSinceLevelLoad * 10)) * GenerateWave.amplitudeWave;
            }
            else if (GenerateWave.TypeOfWave == 2)
            {
                //<TRIANGLE> LeftChannel
                x__ = 0;
                devideFactor = GenerateWave.varSinForGrapherLeftChannel / 1000;
                delta = 1 / (float)resolution; 
                deltaBlockMax = (resolution / devideFactor) / resolution; 
                if (triangleDOWN == false) { x_ = x_ + delta; }
                if (triangleDOWN == true) { x_ = x_ - delta; }
                if (x_ > deltaBlockMax)
                {
                    triangleDOWN = true;
                }
                if (x_ <= 0) { x_ = 0; triangleDOWN = false; }
                x__ = x_ * devideFactor;
                return x__ * GenerateWave.amplitudeWave;
                //</TRIANGLE> LeftChannel
            }
            else if (GenerateWave.TypeOfWave == 3)
            {
                //<SAW> LeftChannel
                devideFactor = GenerateWave.varSinForGrapherLeftChannel / 1000;
                delta = 1 / (float)resolution; 
                deltaBlockMax = (resolution / devideFactor) / resolution; 
                x_ = x_ + delta;
                if (x_ > deltaBlockMax)
                {
                    x_ = 0;
                }
                x__ = x_ * devideFactor;
                return x__ * GenerateWave.amplitudeWave;
                //</SAW> LeftChannel
            }
            else if (GenerateWave.TypeOfWave == 4)
            {
                //<SQUARE> LeftChannel
                devideFactor = GenerateWave.varSinForGrapherLeftChannel / 1000;
                delta = 1 / (float)resolution; 
                deltaBlockMax = (resolution / devideFactor) / resolution; 
                x_ = x_ + delta;
                if (x_ > deltaBlockMax)
                {
                    x_ = 0;
                    squareWaveAmplitudeBool = !squareWaveAmplitudeBool;
                    if (squareWaveAmplitudeBool == true) { squareWaveAmplitude = 1; }
                    if (squareWaveAmplitudeBool == false) { squareWaveAmplitude = 0; }
                }
                return squareWaveAmplitude * GenerateWave.amplitudeWave;
                //</SQUARE> LeftChannel
            }
            else if (GenerateWave.TypeOfWave == 5)
            {
                //<TAN> LeftChannel
                return (0.5f + 0.5f * Mathf.Tan(5 * Mathf.PI * x * GenerateWave.varSinForGrapherLeftChannel + Time.timeSinceLevelLoad * 10)) * GenerateWave.amplitudeWave;
            }
            else
            {
                return 1;
            }
         }
        else
        {
            if (GenerateWave.TypeOfWave == 1)
            {
                //SINE RightChannel
                return (0.5f + 0.5f * Mathf.Sin(5 * Mathf.PI * x * GenerateWave.varSinForGrapherRightChannel + Time.timeSinceLevelLoad * 10)) * GenerateWave.amplitudeWave;
            }
            else if (GenerateWave.TypeOfWave == 2)
            {
                //<TRIANGLE> RightChannel
                x__ = 0;
                devideFactor = GenerateWave.varSinForGrapherRightChannel / 1000;
                delta = 1 / (float)resolution; 
                deltaBlockMax = (resolution / devideFactor) / resolution; 
                if (triangleDOWN == false) { x_ = x_ + delta; }
                if (triangleDOWN == true) { x_ = x_ - delta; }
                if (x_ > deltaBlockMax)
                {
                    triangleDOWN = true;
                }
                if (x_ <= 0) { x_ = 0; triangleDOWN = false; }
                x__ = x_ * devideFactor;
                return x__ * GenerateWave.amplitudeWave;
                //</TRIANGLE> RightChannel
            }
            else if (GenerateWave.TypeOfWave == 3)
            {
                //<SAW> RightChannel
                devideFactor = GenerateWave.varSinForGrapherRightChannel / 1000;
                delta = 1 / (float)resolution; 
                deltaBlockMax = (resolution / devideFactor) / resolution; 
                x_ = x_ + delta;
                if (x_ > deltaBlockMax)
                {
                    x_ = 0;
                }
                x__ = x_ * devideFactor;
                return x__ * GenerateWave.amplitudeWave;
                //</SAW> RightChannel
            }
            else if (GenerateWave.TypeOfWave == 4)
            {
                //<SQUARE> RightChannel
                devideFactor = GenerateWave.varSinForGrapherRightChannel / 1000;
                delta = 1 / (float)resolution; 
                deltaBlockMax = (resolution / devideFactor) / resolution; 
                x_ = x_ + delta;
                if (x_ > deltaBlockMax)
                {
                    x_ = 0;
                    squareWaveAmplitudeBool = !squareWaveAmplitudeBool;
                    if (squareWaveAmplitudeBool == true) { squareWaveAmplitude = 1; }
                    if (squareWaveAmplitudeBool == false) { squareWaveAmplitude = 0; }
                }
                return squareWaveAmplitude * GenerateWave.amplitudeWave;
                //</SQUARE> RightChannel
            }
            else if (GenerateWave.TypeOfWave == 5)
            {
                //TAN RightChannel
                return (0.5f + 0.5f * Mathf.Tan(5 * Mathf.PI * x * GenerateWave.varSinForGrapherRightChannel + Time.timeSinceLevelLoad * 10)) * GenerateWave.amplitudeWave;
            }
            else
            {
                return 1;
            }

        }
        
    }

}


