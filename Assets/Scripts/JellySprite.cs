using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySprite : MonoBehaviour
{

   private class PropagateCollisions : MonoBehaviour {
        //void OnCollisionEnter2D(Collision2D collision) {
        //    transform.parent.SendMessage("OnCollisionEnter2D", collision);
        //}
    }
#region variables
    public int width = 5;
    public int height = 5;
    public int referencePointsCount = 12;
    public float referencePointRadius = 0.25f;
    public float mappingDetail = 10;
    public float springDampingRatio = 0;
    public float springFrequency = 2;
    public float RigidMass = 1f;
    public float RigidGravity = 1f;
    public float RigidADrag = 0f;
    public float rigidDrag = 0f;
    public Sprite CircleSprite;
    public Material SpriteMaterial;
    public GameObject FaceSprite;
    public float FaceSpeed = 5f;
    public Vector2 FaceOffset;
    public PhysicsMaterial2D surfaceMaterial;
    Vector3 FacePosition;
    
    
    public ScriptableObject MetaballScript;

    GameObject[] referencePoints;
    int vertexCount;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;
    Vector3[,] offsets;
    float[,] weights;
#endregion
    void Start () {
        CreateReferencePoints();
        CreateMesh(); //create mesh for blobby man
        MapVerticesToReferencePoints(); //reference points for mesh
        
    }

    void CreateReferencePoints() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>(); //get the rigidbody
        referencePoints = new GameObject[referencePointsCount]; //make the reference points
        Vector3 offsetFromCenter = ((0.5f - referencePointRadius) * Vector3.up); 
        float angle = 360.0f / referencePointsCount; //in a circle

        for (int i = 0; i < referencePointsCount; i++) {
            referencePoints[i] = new GameObject("ChildCollider"); //Adding a new game object that works as a collider
            referencePoints[i].tag = gameObject.tag;
            referencePoints[i].AddComponent<CollisionCheckScript>();//adding in the collisioncheck
            referencePoints[i].AddComponent<PropagateCollisions>(); //collision logic for said child
            referencePoints[i].AddComponent<SplatterSound>();
            referencePoints[i].transform.parent = transform; 
            
            Quaternion rotation =
                Quaternion.AngleAxis(angle * (i - 1), Vector3.back);
            referencePoints[i].transform.localPosition =
                rotation * offsetFromCenter;  //Rotates each child into position based off the center 

            Rigidbody2D body = referencePoints[i].AddComponent<Rigidbody2D>();
            body.freezeRotation = true; //no rotation in z
            body.mass = RigidMass;
            body.gravityScale = RigidGravity;
            body.angularDrag = RigidADrag;
            body.interpolation = rigidbody.interpolation;
            body.collisionDetectionMode = rigidbody.collisionDetectionMode; //the rigidbodies using the variables I set

            CircleCollider2D collider =
                referencePoints[i].AddComponent<CircleCollider2D>();
            collider.radius = referencePointRadius * transform.localScale.x;
            if (surfaceMaterial != null) {
                collider.sharedMaterial = surfaceMaterial; //giving them all circle colliders with surface material
            
            SpriteRenderer Csprite =
            referencePoints[i].AddComponent<SpriteRenderer>(); //adding in a circle for the metaballs effect
            Csprite.sprite = CircleSprite;
            Csprite.drawMode = SpriteDrawMode.Sliced;
            Csprite.material = SpriteMaterial;

            LayerMask LayerSlime =
            referencePoints[i].layer =8; //changing all of them to the slime layer for the meta balls effect
            

            }

            AttachWithSpringJoint(referencePoints[i], gameObject);
            if (i > 0) {
                AttachWithSpringJoint(referencePoints[i],
                        referencePoints[i - 1]);
            }
        }
        AttachWithSpringJoint(referencePoints[0],
                referencePoints[referencePointsCount - 1]);

        IgnoreCollisionsBetweenReferencePoints();
    }

    void AttachWithSpringJoint(GameObject referencePoint,
            GameObject connected) {
        SpringJoint2D springJoint =
            referencePoint.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint) -
            LocalPosition(connected);
        springJoint.distance = 0;
        springJoint.dampingRatio = springDampingRatio;
        springJoint.frequency = springFrequency;
    }

    void IgnoreCollisionsBetweenReferencePoints() {
        int i;
        int j;
        CircleCollider2D a;
        CircleCollider2D b;

        for (i = 0; i < referencePointsCount; i++) {
            for (j = i; j < referencePointsCount; j++) {
                a = referencePoints[i].GetComponent<CircleCollider2D>();
                b = referencePoints[j].GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(a, b, true);
            }
        }
    }

    void CreateMesh() { //create mesh from points need shader stuff
        vertexCount = (width + 1) * (height + 1);

        int trianglesCount = width * height * 6;
        vertices = new Vector3[vertexCount];
        triangles = new int[trianglesCount];
        uv = new Vector2[vertexCount];
        int t;

        for (int y = 0; y <= height; y++) {
            for (int x = 0; x <= width; x++) {
                int v = (width + 1) * y + x;
                vertices[v] = new Vector3(x / (float)width - 0.5f,
                        y / (float)height - 0.5f, 0);
                uv[v] = new Vector2(x / (float)width, y / (float)height);

                if (x < width && y < height) {
                    t = 3 * (2 * width * y + 2 * x);

                    triangles[t] = v;
                    triangles[++t] = v + width + 1;
                    triangles[++t] = v + width + 2;
                    triangles[++t] = v;
                    triangles[++t] = v + width + 2;
                    triangles[++t] = v + 1;
                }
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void MapVerticesToReferencePoints() {
        offsets = new Vector3[vertexCount, referencePointsCount];
        weights = new float[vertexCount, referencePointsCount];

        for (int i = 0; i < vertexCount; i++) {
            float totalWeight = 0;

            for (int j = 0; j < referencePointsCount; j++) {
                offsets[i, j] = vertices[i] - LocalPosition(referencePoints[j]);
                weights[i, j] =
                    1 / Mathf.Pow(offsets[i, j].magnitude, mappingDetail);
                totalWeight += weights[i, j];
            }

            for (int j = 0; j < referencePointsCount; j++) {
                weights[i, j] /= totalWeight;
            }
        }
    }

    void Update() {
        UpdateVertexPositions();
        SlimeFaceUpdate();
    
    }

    void UpdateVertexPositions() {
        Vector3[] vertices = new Vector3[vertexCount];

        for (int i = 0; i < vertexCount; i++) {
            vertices[i] = Vector3.zero;

            for (int j = 0; j < referencePointsCount; j++) {
                vertices[i] += weights[i, j] *
                    (LocalPosition(referencePoints[j]) + offsets[i, j]);
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    void SlimeFaceUpdate ()
    {
        //FacePosition = ((0.5f - referencePointRadius) * Vector3.up);
    // FacePosition = ((.5f- referencePointRadius) * Vector3.up); 
    
    FacePosition = transform.position;
        Vector2 newPosition = new Vector2 
                (FacePosition.x + FaceOffset.x,
                 FacePosition.y + FaceOffset.y
                 );  
    
      FaceSprite.transform.position = Vector2.Lerp
                (FaceSprite.transform.position, 
                newPosition, 
                FaceSpeed * Time.deltaTime);

     //FaceSprite.transform.position = new Vector2 (newPosition.x, newPosition.y);

     


    }
    Vector3 LocalPosition(GameObject obj) {
        return transform.InverseTransformPoint(obj.transform.position);
    }
}