using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterSound : MonoBehaviour
{
    AudioSource audio;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        timer = 0.5f;
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            audio.Pause();
        }
    }

    void Splat()
    {
        audio.UnPause();
        timer = 0.5f;
        

        
    }
}
