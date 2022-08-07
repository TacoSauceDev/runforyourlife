using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
public class Tether : NetworkBehaviour
{
    public GameObject left, right;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private EdgeCollider2D col;
    public float ropeSegLen = .25f;
    public int segmentLength = 5;
    public float lineWidth = .05f;
    public float friction = .3f;
    private LineRenderer lineRenderer;
    private void Start() {
  

    }

    void init(){
        Vector2 startPoint = left.transform.position;
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.col =  GetComponent<EdgeCollider2D>();
        for(int i =0; i <segmentLength; i++){
            this.ropeSegments.Add(new RopeSegment(startPoint));
            startPoint.y -= ropeSegLen;
        }
    }
    void Update(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Count() == 2 && (left == null && right == null)){
            left = players[0];
            right = players[1];
            init();
        }

        this.DrawRope();
    }
     void FixedUpdate()
    {
        Simulate();
    }
    void DrawRope(){
        if(left == null || right == null){
            return;
        }
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for(int i =0; i < this.segmentLength; i ++){
            ropePositions[i] = this.ropeSegments[i].posNew;
        }
        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    
    void Simulate(){
         if(left == null || right == null){
            return;
        }
        //simulation
        Vector2 forceGravity = new Vector2(0f,-1f);
        for(int i =0; i < this.segmentLength; i++){
        RopeSegment firstSegment = this.ropeSegments[i];
        Vector2 velocity = firstSegment.posNew - firstSegment.posOld;
        firstSegment.posOld = firstSegment.posNew;
        firstSegment.posNew += velocity;
        firstSegment.posNew += forceGravity * Time.deltaTime;
        this.ropeSegments[i] = firstSegment;
        }
      /* Keep moving player in place
        float playerDistance = (left.transform.position-right.transform.position).magnitude;
        if(Mathf.Abs(playerDistance) > (float)segmentLength){
                if(playerDistance < 0){
                    left.transform.position = this.ropeSegments[0].posNew;
                }
                else{
                    right.transform.position = this.ropeSegments[this.ropeSegments.Count -1].posNew;
                }
        }
*/

        float playerDistance = (left.transform.position-right.transform.position).magnitude;
        if(Mathf.Abs(playerDistance) > (float)segmentLength){
            Vector2 left2 = new Vector2(left.transform.position.x,left.transform.position.y);
            Vector2 right2 = new Vector2(right.transform.position.x,right.transform.position.y);
            Rigidbody2D leftBody= left.GetComponent<Rigidbody2D>();
            Rigidbody2D rightBody= right.GetComponent<Rigidbody2D>();

            if(leftBody.mass < rightBody.mass || Mathf.Abs(leftBody.velocity.magnitude) < Mathf.Abs(rightBody.velocity.magnitude)){
                if(isGrounded(left)){
                    leftBody.AddForce(rightBody.velocity * friction * rightBody.mass,ForceMode2D.Impulse);
                }
                rightBody.velocity *=  friction * rightBody.mass;

            }
            else if (leftBody.mass > rightBody.mass || Mathf.Abs(leftBody.velocity.magnitude) > Mathf.Abs(rightBody.velocity.magnitude)){
                if(isGrounded(right)){
                    rightBody.AddForce(leftBody.velocity * friction * leftBody.mass, ForceMode2D.Impulse);

                }
                leftBody.velocity *=  friction * leftBody.mass;
            }
            
        }

        //constraints
        for (int i =0;  i <50; i++){
            ApplyConstraints();
            col.points = this.ropeSegments.Select(s  => s.posNew).ToArray();
        }
        
        
        
    }

    //Player should be controlling this
    private bool isGrounded(GameObject obj){
        float distToGround = obj.GetComponent<Collider2D>().bounds.extents.y;
        return Physics2D.Raycast(obj.transform.position, -Vector2.up,distToGround + .1f);
    }
    private void ApplyConstraints(){
        RopeSegment firstSegment=  this.ropeSegments[0];
        firstSegment.posNew = left.transform.position;
        this.ropeSegments[0] =firstSegment;

        RopeSegment endSegment = this.ropeSegments [this.ropeSegments.Count-1];
        endSegment.posNew =  right.transform.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;
        Vector2 ropeDir = Vector2.zero;
        for(int i =0; i < this.segmentLength - 1;i++){
            RopeSegment firstSeg= this.ropeSegments[i];
            RopeSegment secondSeg= this.ropeSegments[i+1];

            float dist = (firstSeg.posNew - secondSeg.posNew).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen){
                changeDir = (firstSeg.posNew - secondSeg.posNew).normalized;
            }            
            else if (dist < ropeSegLen){
                changeDir = (secondSeg.posNew - firstSeg.posNew).normalized;
            }
            ropeDir += changeDir;
            Vector2 changeAmount =  changeDir * error;

            if (i != 0){
                firstSeg.posNew -= changeAmount * .5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNew += changeAmount * .5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else {
                secondSeg.posNew += changeAmount;
                this.ropeSegments[i + 1] =secondSeg;
            }
           
        }
    }


    public struct RopeSegment
    {
        public Vector2 posNew;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos) {
            this.posNew = pos;
            this.posOld = pos;
        }
    }
}