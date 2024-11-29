using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetManager : MonoBehaviour
{
    public MazecraftGameManager instance;
    public AffirmationAudioManager aam;
    public GameManagerAudioController gmac;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomAmbientMusicSystem();
        
    }

    void SetRandomAmbientMusicSystem()
    {
        int randomNum = Random.Range(0, 10);
        Debug.Log("Random number is: " + randomNum);

        if (randomNum < 2.5f)
        {
            gmac.ToggleProceduralMusicGeneratorBands(0);
        }
        else if (randomNum > 2.4f || randomNum < 5.0f)
        {
            gmac.ToggleProceduralMusicGeneratorBands(1);
        }
        else if (randomNum > 4.9f || randomNum < 7.5f)
        {
            gmac.ToggleProceduralMusicGeneratorBands(2);
        }
        else
        {
            // no music generator to start
        }
    }
}
