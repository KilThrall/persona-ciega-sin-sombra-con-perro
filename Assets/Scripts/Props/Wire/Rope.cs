using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private float ropeSegLength = 0.25f;
    [SerializeField]
    private int lineRendererPositions = 35;
    [SerializeField]
    private float lineWidth = 0.1f;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform defaultEndpoint;
    [SerializeField]
    private Transform InteractionTrigger;
    [SerializeField]
    private LayerMask whatIsFloor;
    [SerializeField]
    private float maxCollisionDistanceTollerance = 0.2f;
    #endregion
    private Transform endPoint;
    private bool isGrabbed;


    private LineRenderer lineRenderer;
    private readonly List<RopeSegment> ropeSegments = new(); //C# 9.0 !!!1! 

    #region MonoBehaviour CallBacks
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        endPoint = defaultEndpoint;

        Vector3 ropeStartPoint = startPoint.position;

        for (int i = 0; i < lineRendererPositions; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint,i));
            ropeStartPoint.y -= ropeSegLength;
        }

    }
    private void FixedUpdate()
    {
        this.Simulate();
        this.DrawRope();
        if (lineRenderer.positionCount > 0)
        {
            InteractionTrigger.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //POR AHORA NO SIRVE PERO POR AHI DESP SI UWU OWO EWE 7W7
        //var pos = collision.GetContact(0).point;
        //Vector2 colisionDir = (transform.position - collision.transform.position).normalized;

        //var nearestSegment = GetNearestSegment(pos);
        //nearestSegment.forceToAdd += colisionDir * 2;
        //ropeSegments[nearestSegment.index] = nearestSegment;
    }

    #endregion

    private RopeSegment GetNearestSegment(Vector3 impactPosition)
    {
        RopeSegment nearestSegment=default;
        foreach (RopeSegment rs in ropeSegments)
        {
            if (Vector3.Distance(impactPosition,rs.posNow)< Vector3.Distance(impactPosition, nearestSegment.posNow))
            {
                nearestSegment = rs;
            }
        }
        return nearestSegment;
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1);

        for (int i = 1; i < this.lineRendererPositions; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            Vector2 newPos = firstSegment.posNow;



            Debug.DrawLine(newPos, newPos + Vector2.down *maxCollisionDistanceTollerance);
            if (!Physics2D.Raycast(newPos, Vector2.down, maxCollisionDistanceTollerance, whatIsFloor) && !Physics2D.Raycast(newPos, Vector2.up, maxCollisionDistanceTollerance, whatIsFloor))
            {
                firstSegment.hookPos = Vector2.zero;
                Vector2 virtualNewPos = newPos;
                virtualNewPos.y += velocity.y;
                virtualNewPos.y += forceGravity.y * Time.fixedDeltaTime;
                if (!Physics2D.OverlapPoint(virtualNewPos, whatIsFloor))
                {
                    newPos = virtualNewPos;
                }

            }
            else
            {
                print("Adentro en Y");
                firstSegment.hookPos = newPos;
            }


            Debug.DrawLine(newPos, newPos + Vector2.left * maxCollisionDistanceTollerance, Color.red);
            Debug.DrawLine(newPos, newPos + Vector2.right * maxCollisionDistanceTollerance, Color.red);
            if (!Physics2D.Raycast(newPos, Vector2.right, maxCollisionDistanceTollerance, whatIsFloor) && (!Physics2D.Raycast(newPos, Vector2.left, maxCollisionDistanceTollerance, whatIsFloor)))
            {
                firstSegment.hookPos = Vector2.zero;
                Vector2 virtualNewPos = newPos;
                virtualNewPos.x += velocity.x;
                virtualNewPos.x += forceGravity.x * Time.fixedDeltaTime;

                if (!Physics2D.OverlapPoint(virtualNewPos, whatIsFloor))
                {
                    newPos = virtualNewPos;
                }
            }
            else
            {
                print("Adentro en X");
                firstSegment.hookPos = newPos;
            }
      
            if (!Physics2D.OverlapPoint(newPos, whatIsFloor))
            {
                firstSegment.posNow = newPos;
                this.ropeSegments[i] = firstSegment;
            }
        }

        //CONSTRAINTS
        for (int i = 0; i < 3; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.startPoint.position;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        //if (isGrabbed)
        {
            RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
            endSegment.posNow = this.endPoint.position;
            this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;
        }


        for (int i = 0; i < this.lineRendererPositions - 1; i++)
        {

            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLength)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLength)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
             
                    firstSeg.posNow -= changeAmount * 0.5f;
                    if (!Physics2D.OverlapPoint(firstSeg.posNow, whatIsFloor))
                    {
                        this.ropeSegments[i] = firstSeg;
                    }

                
              
                    secondSeg.posNow += changeAmount * 0.5f;
                    if (!Physics2D.OverlapPoint(secondSeg.posNow, whatIsFloor))
                    {
                        this.ropeSegments[i + 1] = secondSeg;
                    }
                
            }
            else
            {
             
                    secondSeg.posNow += changeAmount;
                    if (!Physics2D.OverlapPoint(secondSeg.posNow, whatIsFloor))
                    {
                        this.ropeSegments[i + 1] = secondSeg;
                    }

                    this.ropeSegments[i + 1] = secondSeg;
                

            }


        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.lineRendererPositions];
        for (int i = 0; i < this.lineRendererPositions; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public void SetEndPoint(Transform p_EndPoint)
    {
        endPoint = p_EndPoint;
        isGrabbed = true;
    }

    public void DropRope()
    {
        if (isGrabbed)
        {
            isGrabbed = false;
        }
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;
        public int index;
        public Vector2 hookPos;
        public RopeSegment(Vector2 pos,int index)
        {
            hookPos = Vector2.zero;
            this.index = index;
            this.posNow = pos;
            this.posOld = pos;
        }
    }

}