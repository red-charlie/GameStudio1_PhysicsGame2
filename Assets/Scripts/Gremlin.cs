using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gremlin : MonoBehaviour
{
    /*This class controls the basic movement and AI for the gremlin enemies. They can't really hurt the slime, but if they touch it, they'll slow it down.*/

    
    //PUBLIC GREMLIN PROPERTIES- these attributes of the gremlin can be tweaked for physics testing
    ///How fast does the gremlin walk at maximum speed?
    public Vector2 walkspeed;
    
    //How quickly does the gremlin accelerate?
    public Vector2 accel;
    
    //How much does gremlin cling to the slime?- OUTDATED- from earlier prototype
    public float stickyForce;
    
    //How bouncy should a gremlin be while walking? 
    //(In earlier prototypes, pre-determined PhysicsMaterial assets were causing problems, so this script manually sets them)
    public float gremlinBounce;

    //How much friction should the gremlin have while walking?
    //(In earlier prototypes, pre-determined PhysicsMaterial assets were causing problems, so this script manually sets them)
    public float gremlinFriction;
    
    //How much friction does the gremlin have while "clinging"? (higher friction == higher stickiness)- OUTDATED- from earlier prototype
    public float stickyfriction;

    //How much should gremlin reduce slime's force of movement? (between 0 and 1- 1 has no effect, 0 paralyzes slime)
    public float slowdownForce;

    //How much should a gremlin reduce slime velocity when it starts clinging? (Between 0 and 1- 1 has no effect, 0 immediately halts slime)
    public float slowdownVelocity;

    public float cooldown;

    public Sprite[] sprites;
    
    
    //GREMLIN RUNTIME PROPERTIES- These properties are checked and updated throughout runtime
    //Is the gremlin on the ground?-
    bool grounded;

    //Has the gremlin been knocked flying?
    bool flying;

    //How long has the gremlin been on the ground since being knocked flying?
    float timeOnGround;
    
    //How fast is the gremlin moving?
    float currentspeed;
    
    //The gremlin's position
    Vector2 gremlinPosition;

    //Is the gremlin clinging?
    bool isClinging;

    //The gremlin's rigidbody
    Rigidbody2D GremlinRigid;

    //Which way is the gremlin walking?
    string walkState;

    //Where did the gremlin make contact with the slime?
    Transform contact;

    //When did the gremlin collide with the slime?
    float collisionTime;

    //What was the gremlin's transform at time of collision with slime?
    Transform collisionTransform;

    //when did the gremlin hit the floor?
    float floorCollision;

    //SLIME BOY PROPERTIES
    //The slime boy
    GameObject slime;
    
    //The slime boy's position
    Vector2 slimePosition;

    //The slime's rigidbody
    Rigidbody2D SlimeRigid;

    //Where does the gremlin go when sent flying?
    Vector2 targetPos;

    //A parametric variable for lerp-ing the flying gremlin
    float param;

    



    // Start is called before the first frame update
    void Start()
    {
        //Initializing variables
        //find slime boy
        slime = GameObject.FindGameObjectWithTag("Slime");
        SlimeRigid = slime.GetComponent<Rigidbody2D>();
        slimePosition = slime.transform.position;

        //By default, gremlin is in neutral state
        grounded = true;
        flying = false;
        walkState = "";
        GremlinRigid = GetComponent<Rigidbody2D>();

        //Set physics material properties according to public variables
        GremlinRigid.sharedMaterial.friction = gremlinFriction;
        GremlinRigid.drag = 0.0f;
        GremlinRigid.sharedMaterial.bounciness = gremlinBounce;
        isClinging = false;
        cooldown = 2;


        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(SlimeRigid.velocity);
        //check where the slime and gremlin are
        slimePosition = slime.transform.position;
        gremlinPosition = new Vector2(transform.position.x, transform.position.y);

        if (grounded)
        {


            //check where the slime and gremlin are
            slimePosition = slime.transform.position;
            gremlinPosition = new Vector2(transform.position.x, transform.position.y);

            //If the slime is to the right, gremlin walks right
            if ((slimePosition - gremlinPosition).x > 0)
            {
                walkState = "right";

            }
            //If the slime is to the left, gremlin walks left
            if ((slimePosition - gremlinPosition).x < 0)
            {
                walkState = "left";

            }
        }
      
   
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //layer 8 is only used for the slime and its child objects
        if (other.gameObject.layer == 8 && !isClinging)
        {
            if (SlimeRigid.velocity.magnitude < 25)
            {
                //separate cling method is written for modularity
                Cling(other);
            }
            else
            {
                grounded = false;
                flying = true;
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                sprites[1] = sprites[2];
                
                /*
                collisionTime = Time.fixedTime;
                collisionTransform.position = gameObject.transform.position;
                collisionTransform.rotation = gameObject.transform.rotation;
                */
                targetPos = Random.insideUnitCircle.normalized*100;
                targetPos = new Vector2(targetPos.x + collisionTransform.position.x, targetPos.y + collisionTransform.position.y);
                GremlinRigid.AddForce(targetPos, ForceMode2D.Impulse);

                collisionTime = Time.fixedTime;
            }
        }

        else if (other.gameObject.tag == "Floor")
        {
            grounded = true;
        }
            
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer != 8)
        {
            timeOnGround = Time.fixedTime - floorCollision;
        }


    }

    void OnCollisionExit2D(Collision2D other)
    {
        //layer 8 is only used for the slime and its child objects
        if (other.gameObject.layer == 8 && isClinging)
        {
            //Undoes the effects of Cling(), to restore slime to its normal state
            isClinging = false;
            gameObject.transform.parent = null;
            GremlinRigid.bodyType = RigidbodyType2D.Dynamic;
            slime.GetComponent<Move2D_Force>().moveForce *= 1.0f/slowdownForce;
            
            //declares gremlin to be "Ungrounded" aka flying through air
        }

        else if (other.gameObject.tag == "Floor")
        {
            grounded = false;
            timeOnGround = 0;
        }
    }


    void FixedUpdate()
    {
        //The gremlin's behavior when it is just walking
        if (!isClinging && grounded && Vector2.Distance(slimePosition, gremlinPosition) < 30)
        {
            if ((walkState == "right") && (GremlinRigid.velocity.x < walkspeed.x))
            {
                GremlinRigid.AddForce(accel);
            }


            if ((walkState == "left") && (GremlinRigid.velocity.x > -walkspeed.x))
            {
                GremlinRigid.AddForce(-accel);
            }
        }
        //The gremlin's behavior when it is clinging to the slime
        else if (isClinging)
        {
            //The gremlin's rigidbody has to be moved by script, because it is kinematic while clinging

            //Intended effect is that the gremlin stays in contact with the collider its clinging to
            GremlinRigid.MovePosition(contact.position);
            GremlinRigid.MoveRotation(contact.rotation);


        }
        else if (flying)
        {
            cooldown -= Time.fixedDeltaTime;
            if(cooldown <= 0) { Object.Destroy(gameObject); }

        }

    
        
    }


    /// <summary>
    ///Intended to be the gremlin's main method of attack, each gremlin sticks to the slime and slows it down
    ///Takes a Collision2D as a parameter, because it should only run during OnCollisionEnter().
    /// </summary>
    void Cling(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        //The Gremlin's body is made kinematic so that it isn't automatically pushed away by the slime 
        GremlinRigid.bodyType = RigidbodyType2D.Kinematic;
        //GremlinRigid.simulated = false;

        //The gremlin is made into a child object of the collider it touches so that its transform is in local coordinates
        gameObject.transform.parent = other.gameObject.transform;

        //record the point of contact between the gremlin and game object
        contact = transform;
        
        //set this variable to true to make sure the gremlin doesn't walk
        isClinging = true;

        GetComponent<SpriteRenderer>().sprite = sprites[1];

        //Old code from prototypes involving Physics Materials
        /*
        //change physics material for maximum stickiness
        GremlinRigid.sharedMaterial.friction = stickyfriction;
        GremlinRigid.sharedMaterial.bounciness = 0;
        GremlinRigid.drag = 1;*/


        //Slow down slime overall by reducing its default move speed
        slime.GetComponent<Move2D_Force>().moveForce *= slowdownForce;

        //slow down slime immediately by cutting its velocity
        SlimeRigid.velocity *= slowdownVelocity;
    }


}
