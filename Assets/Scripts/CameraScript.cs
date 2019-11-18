using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float followSpeed = 3;
    public Transform playerGameObject;
    public Vector3 cameraOffset;
   

    // Start is called before the first frame update
    void Start()
    {   
        


    }

    // Update is called once per frame
    void Update()
    {
      Vector3 newPosition = new Vector3 (playerGameObject.position.x + cameraOffset.x, playerGameObject.position.y + cameraOffset.y, cameraOffset.z);  
    
       
      transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);
    }
}
