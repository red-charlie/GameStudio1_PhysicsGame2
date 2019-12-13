using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckScript : MonoBehaviour
{  
    //GameObject[] floor; //List of floor objects, not currently used
    public static bool canJump = false; //public void for the canJump
    public static bool SlimeMerge = false;//can merge with other slimes

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //SendMessageUpwards("Splat");
        
        
    }
    // public void OnCollisionStay2D (Collision2D  other) { //when the collision is touching
    //     if(other.gameObject.tag == "Floor"){
    //         canJump = true; //set the jump variable to true
    //        print("I can jump now");

    //     }        

    //     else //if ( other.gameObject.tag != "Floor")
    //     {
    //         canJump = false; //otherwise I can't jump
    //         print ("Woops Can't jump");
    //     }
            

    // }

    // public void OnCollisionExit2D (Collision2D other){
    //    if (canJump == true){
    //         canJump = false; //when not touching anything can't jump
    //    }
    //    else if (canJump == false){
    //        //
    //    }
    // }
   
       
}
