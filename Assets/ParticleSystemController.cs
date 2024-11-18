using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private VolumeControl vc;
    private ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;
    public GameObject[] channels;
    public int lightFactor = 0;
    public Color[] colors;
    GameObject randomChannel;


    void Start()
    {
        //InvokeRepeating("ActivateParticleSystem", 2f, 5f);

        channels = GameObject.FindGameObjectsWithTag("GameController");
        vc = GetComponent<VolumeControl>();
    }

    void Update()
    {
        if (vc.randomEQ == true)
        {
            if (Input.GetMouseButton(1))
            {
                ActivateParticleSystem();
            }

            if (Input.GetMouseButtonUp(1))
            {
                DeactivateParticleSystem();
            }
        }
    }

    private void LateUpdate()
    {        
       if(this.gameObject.GetComponent<ParticleSystem>().isPlaying){
            InvokeRepeating("SetParticleSystemColor", .1f, 5f);
        }
    }

    private void ActivateParticleSystem()
    {
        //randomChannel = channels[Random.Range(0, channels.Length)];
        SetParticleSystemState(this.gameObject, true, false);
        SetParticleSystemColor();
    }

    private void DeactivateParticleSystem()
    {
        SetParticleSystemState(this.gameObject, false, false);
    }

    //void ActivateParticleSystem(){
    //        //Debug.Log("Called ActivateParticleSystem");

    //        if (vc.randomEQ == true){  
    //            foreach(GameObject channel in channels){
    //                //Debug.Log("random is: " + random);

    //                if(GetRandomFloat() == 3){
    //                    //Debug.Log("random is 3, starting system for: " + channel.name);
    //                    // ps = channel.GetComponent<ParticleSystem>();
    //                    // SetParticleSystemColorChannel(channel, ps);
    //                    SetParticleSystemState(channel, true, false);
    //                    SetParticleSystemColor();
    //                }
    //                else{
    //                    SetParticleSystemState(channel, false, false);
    //                }
    //            }
    //        }
    //        else if (vc.randomEQ == false){
    //            // dummy game object here. need to refactor
    //            SetParticleSystemState(GameObject.Find("channelZero"), false, true);
    //        }
    //    }

    private float GetRandomFloat(){
        float random = Random.Range(0, 71);
        return random;
    }

    void SetParticleSystemState(GameObject channel, bool enable, bool disableAll){
        ps = channel.GetComponent<ParticleSystem>();  

        if(enable == true){
            ps.Play();
        }
        else if(enable == false){
            ps.Stop();
        }
        else if(disableAll == true){
            foreach(GameObject ps in channels){
                //ps.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    // private void SetParticleSystemColorChannel(GameObject channel, ParticleSystem ps){
        
    // }

    private void SetParticleSystemColor(){
        //Debug.Log("Setting Particle System Color");
        foreach(GameObject channel in channels){
            int numParticlesAlive = 0;
            
            // GetParticles is allocation free because we reuse the m_Particles buffer between updates
            if(channel.GetComponent<ParticleSystem>().isPlaying){
                m_Particles = new ParticleSystem.Particle[channel.GetComponent<ParticleSystem>().main.maxParticles];
                numParticlesAlive = channel.GetComponent<ParticleSystem>().GetParticles(m_Particles);
                //Debug.Log("number of active particles is: " + numParticlesAlive);
            }

            // Change only the particles that are alive
            if(numParticlesAlive > 0){
                for (int i = 0; i < numParticlesAlive; i++)
                {
                    Color newColor = colors[Random.Range(0, colors.Length)];
                    m_Particles[i].startColor = newColor;
                }
            }

            // Apply the particle changes to the Particle System
            channel.GetComponent<ParticleSystem>().SetParticles(m_Particles, numParticlesAlive);
        }

    }

    private Color GetColor(){
        Color c = new Color();
        c.r= Random.Range(0,1);
        c.g= Random.Range(0,1);
        c.b= Random.Range(0,1);
        c.a= Random.Range(.3f,1);
        //Debug.Log("Color is: " + c);
        return c;
    }

    private void OnMouseOver()
    {
        ActivateParticleSystem();
    }

    private void OnMouseExit()
    {
        DeactivateParticleSystem();
    }
}
