using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAnimation : MonoBehaviour
{
    public Animation animation;
    public string animName;
    public float speed = .3f;
    // Start is called before the first frame update
    void Start()
    {
        //animation["Take 001"].speed = speed;
        //animation["Idle"].speed = speed;
        // animation["sj001_skill02"].speed = speed;
        // animation["sj001_wait"].speed = speed;
        animation[animName].speed = speed;
    }
}
