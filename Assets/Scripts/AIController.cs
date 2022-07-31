using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player; //the player object
    public float minDistance;
    public float maxDistance;
    public float speed;
    public Rigidbody rb;
    private CapsuleCollider collide;

    // Start is called before the first frame update
    void Start()
    {
        // Use this for initialization
        collide = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, this.transform.position) < maxDistance)
        {
            //this will allow the AI to turn, we may want to change this, but for now I will leave
            //it until we know what we want our AI to do EXACTLY. Like do we want it to chase around the level
            //if it is too close no matter what direction or do we want it to just go in a straight line?
            Vector3 face = (player.position - this.transform.position) * Time.deltaTime * speed;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(face), 0.1f);
                     

            if(face.magnitude > minDistance)
            {
                this.transform.Translate(0, 0, 0.5f);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
    }
}
