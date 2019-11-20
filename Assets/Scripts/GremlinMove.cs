using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinMove : MonoBehaviour
{
    //How fast does the gremlin walk?
    public float walkspeed;
    //Has the gremlin been knocked flying?
    bool grounded;
    //The slime boy
    GameObject slime;
    //The slime boy's position
    Vector2 slimePosition;

    //The Gremlin's rigidbody
    Rigidbody2D rigid;
    //Which way is the gremlin walking?
    string walkState;




    // Start is called before the first frame update
    void Start()
    {
        slime = GameObject.FindGameObjectWithTag("avatar");
        grounded = true;
        slimePosition = slime.transform.position;
        walkState = "";
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        slimePosition = slime.transform.position;
        if((transform.position - slimePosition).x > 0)
        {
            walkState = "right";
        }
        if((transform.position = slimePosition).x < 0)
        {
            walkState = "left";
        }
        if((transform.position - slimePosition).magnitude < 1))
        {
            Cling();
        }


    }

    void FixedUpdate()
    {
        if (walkState == "right")
        {
            rigid.velocity = walkspeed;
        }

        if (walkState == "left")
        {
            rigid.velocity = -walkspeed;
        }
    }

    void Cling()
    {
        this.enabled = false;
        slime.GetComponent<ForceMove2D>().moveForce *= 0.8f;
        slime.GetComponent<Rigidbody2D>().velocity *= 0.5f;
    }
}
