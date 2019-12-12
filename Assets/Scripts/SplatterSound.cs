using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterSound : MonoBehaviour
{
    AudioSource audio;
    private static bool[] contactQueue;
    private static int queueIndex;
    readonly bool[] allFalse = new bool[30];

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            allFalse[i] = false;
        }
        contactQueue = new bool[30];
        contactQueue = allFalse;
        audio = GameObject.FindGameObjectWithTag("Slime").GetComponent<AudioSource>();
        queueIndex = 0;
        timer = 1;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            audio.Pause();
        }
        Debug.Log(contactQueue.ToString());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (contactQueue == allFalse)
            {
                Splat();
            }
            enqueue();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            dequeue();
        }
    }

    void enqueue()
    {
        contactQueue[queueIndex] = true;
        queueIndex = (queueIndex + 1) % 30;
    }

    void dequeue()
    {
        queueIndex = (queueIndex - 1) % 30;
        contactQueue[queueIndex] = false;
        
    }

    void Splat()
    {
        audio.UnPause();
        timer = 1;
        

        
    }
}
