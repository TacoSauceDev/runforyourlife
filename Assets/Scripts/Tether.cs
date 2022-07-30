using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Tether : MonoBehaviour
{
    public GameObject left, right;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private EdgeCollider2D col;
    private float ropeSegLen = .25f;
    private int segmentLength = 35;
    private float lineWidth = .1f;
   
    private LineRenderer lineRenderer;
    private void Start() {
        Vector2 startPoint = left.transform.position;
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.col =  GetComponent<EdgeCollider2D>();
        for(int i =0; i <segmentLength; i++){
            this.ropeSegments.Add(new RopeSegment(startPoint));
            startPoint.y -= ropeSegLen;
        }

    }

    void Update(){
        this.DrawRope();
    }
     void FixedUpdate()
    {
        Simulate();
    }
    void DrawRope(){
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

        //constraints
        for (int i =0;  i <50; i++){
            ApplyConstraints();
        }

        col.points = this.ropeSegments.Select(s  => s.posNew).ToArray();
    }

 
    private void ApplyConstraints(){
        RopeSegment firstSegment=  this.ropeSegments[0];
        firstSegment.posNew = left.transform.position;
        this.ropeSegments[0] =firstSegment;

        RopeSegment endSegment = this.ropeSegments [this.ropeSegments.Count-1];
        endSegment.posNew =  right.transform.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

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