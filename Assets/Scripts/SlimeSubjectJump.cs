using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSubjectJump : MonoBehaviour
{
    public Vector2 jumpForce;
    public Rigidbody2D rigid;
    public float JumpLength;
    public float JumpRepeat;

    // Start is called before the first frame update
    void Start()
    {
        JumpLength = Random.Range(.5f,1);
        JumpRepeat = Random.Range(.8f,1.5f);
        //Invoke("Jump", 1);
        InvokeRepeating("Jump", JumpLength, JumpRepeat);
        
    }

   

 
    void Jump ()
    {
    rigid.AddForce(jumpForce, ForceMode2D.Impulse);
    //print("Jumping");
    JumpLength = Random.Range(.5f,1);
    JumpRepeat = Random.Range(.80f,1.5f);
    }
}
