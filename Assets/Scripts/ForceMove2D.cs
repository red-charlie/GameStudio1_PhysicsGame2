using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMove2D : MonoBehaviour
{
    //the rigidbody attached to the GameObject
    Rigidbody2D rigid;

    //The force of a dash attack
    public Vector2 dashForce;

    //The force of a jump
    public Vector2 jumpForce;

    //The force of basic movement
    public Vector2 moveForce;
    
    //the edge collider designated as the floor; can only jump while in contact with this collider
    Collider2D floor;
    
    //Can only perform a dash attack when the two counters are between 1 and 30, and the dash booleans are true
    int counterLeft;
    int counterRight;
    bool dashLeft;
    bool dashRight;

    //After dashing, the player has to wait until cooldown == 0 before dashing again
    int cooldown;

    // Start is called before the first frame update
    void Start()
    {
        //Initializing variables 
        //These ones stay constant throughout game
        rigid = GetComponent<Rigidbody2D>();
        floor = GameObject.Find("floor").GetComponent<Collider2D>();
        dashForce = new Vector2(5f, 0f);
        jumpForce = new Vector2(0f, 4f);
        moveForce = new Vector2(0.2f, 0f);

        //These ones change with player input
        counterLeft = 0;
        counterRight = 0;
        dashRight = false;
        dashLeft = false;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //While the a or d buttons are pressed, a force of 0.2 is applied to the object, and the appropriate counter is set to 30. If
        //the player releases the key, they have until the counter reaches 0 to perform a dash attack.
        if (Input.GetKey("a"))
        {
            rigid.AddForce(-moveForce);
            counterLeft = 30;
        }
        if (Input.GetKey("d"))
        {
            rigid.AddForce(moveForce);
            counterRight = 30;
        }

        //Checks if conditions are met, and then the character will dash
        if (Input.GetKeyDown("a") && dashLeft)
        {
            if(counterLeft > 0 && cooldown == 0)
            {
                rigid.AddForce(-dashForce, ForceMode2D.Impulse);
                cooldown = 120;
            }
        }
        if (Input.GetKeyDown("d") && dashRight)
        {
            if (counterRight > 0 && cooldown == 0)
            {
                rigid.AddForce(dashForce, ForceMode2D.Impulse);
                cooldown = 120;
            }
        }

        //If the character is on the floor, they can jump
        if (Input.GetKeyDown("space") && rigid.IsTouching(floor))
        {
            
            rigid.AddForce(jumpForce, ForceMode2D.Impulse);
          
        }

        //the dash booleans need to be set to false after checking for input, but before the next frame
        dashLeft = false;
        dashRight = false;

        //If the player recently pushed a button, the dash booleans will be set to true, overwriting the previous command setting them to false
        //Also, this is where the counters run down if they are > 0
        if (counterLeft > 0)
        {
            counterLeft--;
            dashLeft = true;
            
        }
        if (counterRight > 0)
        {
            counterRight--;
            dashRight = true;
        }

        //cooldown timer runs down if > 0
        if (cooldown >0)
        {
            cooldown--;
        }

        
        
    }
}
