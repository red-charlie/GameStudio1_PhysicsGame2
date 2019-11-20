using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinMove : MonoBehaviour
{
    //How fast does the gremlin walk?
    public Vector2 walkspeed;
    //Has the gremlin been knocked flying?
    bool grounded;
    //The slime boy
    GameObject slime;
    //The slime boy's position
    Vector2 slimePosition;

    Vector2 gremlinPosition;

    //The Gremlin's rigidbody
    Rigidbody2D GremlinRigid;
    //The slime's rigidbody
    Rigidbody2D SlimeRigid;
    //Which way is the gremlin walking?
    string walkState;

    //parametric variable for linear interpolation
    float param;




    // Start is called before the first frame update
    void Start()
    {
        slime = GameObject.FindGameObjectWithTag("avatar");
        SlimeRigid = slime.GetComponent<Rigidbody2D>();
        grounded = true;
        slimePosition = slime.transform.position;
        walkState = "";
        GremlinRigid = GetComponent<Rigidbody2D>();
        
        param = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        slimePosition = slime.transform.position;
        gremlinPosition = new Vector2(transform.position.x, transform.position.y);
        Debug.Log(gremlinPosition - slimePosition);
        if((gremlinPosition - slimePosition).x > 0)
        {
            if (walkState == "left")
            {
                param = 0.0f;
            }
            walkState = "right";
            
        }
        if((gremlinPosition - slimePosition).x < 0)
        {
            if (walkState == "right")
            {
                param = 0.0f;
            }
            walkState = "left";
            
        }
     
       
    }

    void OnCollisionEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Cling();
        }
    }


    void FixedUpdate()
    {
        if (walkState == "right")
        {
            GremlinRigid.velocity = Vector2.Lerp(GremlinRigid.velocity, -walkspeed, param);
        }

        if (walkState == "left")
        {
            GremlinRigid.velocity = Vector2.Lerp(GremlinRigid.velocity, walkspeed, param);
        }
        param = param + 0.1f;
    }

    void Cling()
    {
        this.enabled = false;
        GremlinRigid.sharedMaterial.friction = 1;
        slime.GetComponent<ForceMove2D>().moveForce *= 0.8f;
        SlimeRigid.velocity *= 0.5f;
    }
}
