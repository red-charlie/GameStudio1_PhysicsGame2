using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2D_Force : MonoBehaviour
{
 #region Public Variables

 #region Movement Input
 public KeyCode JumpButton  = KeyCode.Space;
 public KeyCode RightButton = KeyCode.D;
 public KeyCode LeftButton  = KeyCode.A;
 public KeyCode DownButton  = KeyCode.S;
 public KeyCode UpButton    = KeyCode.W;

 public CircleCollider2D CircleCollider;
 public float CrouchRadius = .2f;
 public float NormalRadius =.4f;




 #endregion 
 #region Movement Speeds and Force
    //The force of a dash attack
    public Vector2 dashForce;

    //The force of a jump
    public Vector2 jumpForce;

    //The force of basic movement
    public Vector2 moveForce;
    #endregion

#endregion

#region Other Variables
    //bool canJump = false; //Setting the jump
    Rigidbody2D rigid; //the 2d rigidbody attached 
    
    //Can only perform a dash attack when the two counters are between 1 and 30, and the dash booleans are true
    int counterLeft;
    int counterRight;
    bool dashLeft;
    bool dashRight;

    //After dashing, the player has to wait until cooldown == 0 before dashing again
    int cooldown;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Initializing variables 
        //These ones stay constant throughout game

        //Getting the rigidbody of the parent object
        rigid = GetComponent<Rigidbody2D>(); 
                
        
        
        
        //Commenting these out because they change the force as soon as the game starts
        //dashForce = new Vector2(5f, 0f);
        //jumpForce = new Vector2(0f, 4f);
        //moveForce = new Vector2(0.2f, 0f); 

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
        #region Crouch
        if (Input.GetKey(DownButton))
        {
            CircleCollider.radius = CrouchRadius;
        }
        else{
            CircleCollider.radius = NormalRadius;
        }

        #endregion
        //While the a or d buttons are pressed, force is applied to the object, and the appropriate counter is set to 30. If
        //the player releases the key, they have until the counter reaches 0 to perform a dash attack.
       #region Movement linear
        if (Input.GetKey(LeftButton))
        {
            rigid.AddForce(-moveForce);
           // rigid.velocity = -moveForce;
            counterLeft = 30;
        }
        if (Input.GetKey(RightButton))
        {
            rigid.AddForce(moveForce);
            //rigid.velocity = moveForce;
            counterRight = 30;
        }

        //Checks if conditions are met, and then the character will dash
        if (Input.GetKeyDown(LeftButton) && dashLeft)
        {
            if(counterLeft > 0 && cooldown == 0)
            {
                rigid.AddForce(-dashForce, ForceMode2D.Impulse);
                cooldown = 120;
            }
        }
        if (Input.GetKeyDown(RightButton) && dashRight)
        {
            if (counterRight > 0 && cooldown == 0)
            {
                rigid.AddForce(dashForce, ForceMode2D.Impulse);
                cooldown = 120;
            }
        }
    #endregion

    #region jumping and dashing
        //If the character is on the floor (checks for tag), they can jump
        if (Input.GetKeyDown(JumpButton) && CollisionCheckScript.canJump == true)
        {
            print("I am trying to jump");
            rigid.AddForce(jumpForce, ForceMode2D.Impulse);
            //rigid.AddForce(jumpForce);

            //rigid.velocity = jumpForce;
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
        if (cooldown > 0)
        {
            cooldown--;
        }
        #endregion


        #region change size
        
        if(CollisionCheckScript.SlimeMerge == true){
            print("I am growing larger");
           // Destroy(this);
           CollisionCheckScript.SlimeMerge = false;

        }
        #endregion

        
        
    }
    }
