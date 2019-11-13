using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMove2D : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 vel;
    Vector2 accel;
    //Vector2 deltaX;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            rigid.AddForce(Vector2.left * 0.2f);
        }
        if (Input.GetKey("d"))
        {
            rigid.AddForce(Vector2.right * 0.2f);
        }
        //if (Input.GetKey("w"))
        //{
          //  rigid.AddForceAtPosition(deltaZ, transform.position + Vector3.up * 0.2f);
        //}
        //if (Input.GetKey("s"))
        //{
           // rigid.AddForceAtPosition(-deltaZ, transform.position + Vector3.up * 0.2f);
        //}
    }
}
