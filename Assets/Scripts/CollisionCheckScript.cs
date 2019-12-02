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
    public void OnCollisionStay2D (Collision2D  other) { //when the collision is touching
        if(other.gameObject.tag == "Floor"){
            canJump = true; //set the jump variable to true
            print("I can jump now");

        }        

        else
        {
            canJump = false; //otherwise I can't jump
        }
            

    }

    public void OnCollisionExit2D (Collision2D other){
        canJump = false; //when not touching anything can't jump

    }
   
       
}
