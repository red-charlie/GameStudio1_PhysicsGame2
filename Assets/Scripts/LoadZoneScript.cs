using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZoneScript : MonoBehaviour
{
    
    GameObject ChildZone;
    AudioSource bgm;
    AudioSource newSource;
    AudioClip newMusic;
    bool musicChanged;
    
    // Start is called before the first frame update
    void Start()
    {
     Transform tf;
     tf = transform.GetChild(0);
    ChildZone = tf.gameObject;

        //ChildZone = transform.GetChild(0);
        // print("The current zone is" + ChildZone);

        //Find the BGM audio source
        bgm = GameObject.Find("Cameras").GetComponent<AudioSource>();
        
        newMusic = Resources.Load<AudioClip>("Pickled Pink");
        musicChanged = false;
        
    }


    private void OnTriggerEnter2D (Collider2D collision){
        //LoadZone();"
       // print("Something has collided");
        if(collision.gameObject.tag == "Slime"){
        print("Loading this zone: " + ChildZone);
        ChildZone.SetActive(true);
        if (name == "1_LoadZone" && !musicChanged)
            {
                musicChanged = true;
                newSource = bgm.gameObject.AddComponent<AudioSource>(); ;
                bgm.playOnAwake = false;
                newSource = bgm;
                bgm.loop = false;
                newSource.clip = newMusic;
                newSource.PlayDelayed(3.0f);
                
            }

        }
    }

    private void OnTriggerExit2D (Collider2D collision){
        // CullZone();
        if (collision.gameObject.name == "PC_Blob_1"
             && collision.gameObject.tag == "Slime"
             && ChildZone.activeSelf)
        {
            print("removing this zone! " + ChildZone);
            ChildZone.SetActive(false);
        }
    }
    

}
