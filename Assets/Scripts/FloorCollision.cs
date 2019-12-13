using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{  
    //GameObject[] floor; //List of floor objects, not currently used
    public static bool canJump = false; //public void for the canJump

    // Update is called once per frame
  
    public void OnCollisionStay2D (Collision2D  other) { //when the collision is touching
        if(other.gameObject.tag == "Slime"){
            canJump = true; //set the jump variable to true
          // print("I can jump now");

        }        
        else //if ( other.gameObject.tag != "Floor")
        {
            canJump = false; //otherwise I can't jump
           // print ("Woops Can't jump");
        }
            

    }
     public void OnCollisionEnter2D (Collision2D  other) { //when the collision is touching
        if(other.gameObject.tag == "Slime" && canJump == false){
            canJump = true; //set the jump variable to true
           print("I can jump now");

        }        
                   

    }

    public void OnCollisionExit2D (Collision2D other){
       if (other.gameObject.tag =="Slime" && canJump == true){
            canJump = false; //when not touching anything can't jump
       }
       else if (other.gameObject.tag =="Slime" && canJump == false){
           //
       }
    }
   
       
}
