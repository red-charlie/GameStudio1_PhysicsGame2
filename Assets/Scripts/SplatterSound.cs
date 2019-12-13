using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterSound : MonoBehaviour
{
    AudioSource audio;
    private static bool[] contactQueue;
    //private static int slimeTouching;
    private static int queueIndex;
    readonly bool[] allFalse = new bool[30];
    private Collider2D collider;
    private static int frameCounter;

    private static float duration;
    private static float timer;
    private static double delay;
    ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        contactQueue = new bool[30];
        for (int i = 0; i < 30; i++)
        {
            allFalse[i] = false;
            contactQueue[i] = false;
        }
       
        audio = GameObject.FindGameObjectWithTag("Slime").GetComponent<AudioSource>();
        particles = GameObject.FindGameObjectWithTag("Slime").GetComponent<ParticleSystem>();
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.velocity = Random.insideUnitCircle*5;
        audio.Play();
        audio.Pause();
        queueIndex = 0;
        duration = 0.2f;
        frameCounter = 0;
        collider = GetComponent<CircleCollider2D>();
        delay = 0.05;


    }

    // Update is called once per frame
    void Update()
    {


     
    }

    private void FixedUpdate()
    {

        //CheckContact();
        duration -= Time.fixedDeltaTime/30.0f;
        timer += Time.fixedDeltaTime / 30.0f;
        //CheckContact();

        
        if (duration <= 0)
        {
            audio.Pause();
        }
        else { timer = 0; }
        if (duration > 0) { timer = 0; }



    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 0)
        {
            //CheckContact();


            //Debug.Log(timer);
            if (!Physics2D.IsTouchingLayers(collider, 0) && timer > 0.2f && !audio.isPlaying)
            {
                Splat();
            }
            //enqueue();
            timer = 0;
        }
    }
   /* 
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 0)
        {
            dequeue();
        }
    }*/
    
    /*
    void enqueue()
    {
        Debug.Log(queueIndex);
        contactQueue[queueIndex] = true;
        queueIndex = (queueIndex + 1) % 30;
    }

    void dequeue()
    {
        Debug.Log(queueIndex);
        //queueIndex = (queueIndex % - 1) % 30;
        contactQueue[queueIndex] = false;
        queueIndex = (queueIndex + 1) % 30;

    }*/
    
    void Splat()
    {
        if (!audio.isPlaying)
        {
            audio.UnPause();
           duration = 0.287f;
            timer = 0;

            /*
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.velocity = Random.insideUnitCircle * 10;
            particles.Emit(emitParams, 3);
            emitParams.ResetVelocity();
            */
        }

        
    }

    /*
    void CheckContact()
    {
        if (!Physics2D.IsTouchingLayers(collider, 0))
        {
            dequeue();
        }

        else
        {
            enqueue();

        }
    }*/

    //!Physics2D.IsTouchingLayers(collider, 0)
}
