using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [HideInInspector]
    public bool isSpliced;

    public bool MustBeSplicedToWork => mustBeSplicedToWork;

    #region Serialized Variables
    [SerializeField]
    private bool mustBeSplicedToWork;
    [SerializeField]
    private Wire wire;
    [SerializeField]
    private float maxRopeLength;
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
    private Transform InteractionTriggerStart;
    [SerializeField]
    private Transform InteractionTriggerEnd;
    [SerializeField]
    private LayerMask whatIsFloor;
    [SerializeField]
    private float maxCollisionDistanceTollerance = 0.2f;
    #endregion
    private Transform endPoint;
    private bool isGrabbed;

    private EdgeCollider2D edgeCollider;
    private List<Vector2> edgeColliderPoints = new();

    private LineRenderer lineRenderer;
    private readonly List<RopeSegment> ropeSegments = new(); //C# 9.0 !!!1! 

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        endPoint = defaultEndpoint;

        Vector3 ropeStartPoint = startPoint.position;
    
        for (int i = 0; i < lineRendererPositions; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint,i));
            edgeColliderPoints.Add(Vector3.zero);  
            ropeStartPoint.y -= ropeSegLength;
        }

    }
 
    private void FixedUpdate()
    {
        this.Simulate();
        this.DrawRope();
        if (IsOverStretched() && !wire.IsSpliced)
        {
            SetEndPoint(null);
            wire.DropWire();
            DropRope();
        }
        if (lineRenderer.positionCount > 0)
        {
            InteractionTriggerEnd.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            InteractionTriggerStart.position = lineRenderer.GetPosition(0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * maxRopeLength);
    }
    #endregion
    private bool IsOverStretched()
    {
        float currentRopeLength=0;
        for(int i=0;i<lineRendererPositions-1 ;i++)
        {
            currentRopeLength += Vector2.Distance(ropeSegments[i].posNow, ropeSegments[i+1].posNow);
        }
        return maxRopeLength < currentRopeLength;
    }
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
        //edgeColliderPoints[0] = new Vector2(startPoint.position.x-transform.position.x, startPoint.position.y-transform.position.y);
        for (int i = 1; i < this.lineRendererPositions; i++)
        {
            if (ropeSegments[i].posNow != Vector2.zero)
            {
                RopeSegment firstSegment = this.ropeSegments[i];
                Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
                firstSegment.posOld = firstSegment.posNow;
                Vector2 newPos = firstSegment.posNow;

                if (Physics2D.Raycast(newPos, velocity, maxCollisionDistanceTollerance, whatIsFloor))
                {
                    firstSegment.hookPos = newPos;
                }
                else
                {
                    firstSegment.hookPos = Vector2.zero;
                    if (!Physics2D.Raycast(newPos, Vector2.down, maxCollisionDistanceTollerance, whatIsFloor) && !Physics2D.Raycast(newPos, Vector2.up, maxCollisionDistanceTollerance, whatIsFloor))
                    {

                        Vector2 virtualNewPos = newPos;
                        virtualNewPos.y += velocity.y;
                        virtualNewPos.y += forceGravity.y * Time.fixedDeltaTime;
                        if (!Physics2D.OverlapPoint(virtualNewPos, whatIsFloor))
                        {
                            newPos = virtualNewPos;
                        }
                    }
                    if (!Physics2D.Raycast(newPos, Vector2.right, maxCollisionDistanceTollerance, whatIsFloor) && (!Physics2D.Raycast(newPos, Vector2.left, maxCollisionDistanceTollerance, whatIsFloor)))
                    {
                        Vector2 virtualNewPos = newPos;
                        virtualNewPos.x += velocity.x;
                        virtualNewPos.x += forceGravity.x * Time.fixedDeltaTime;

                        if (!Physics2D.OverlapPoint(virtualNewPos, whatIsFloor))
                        {
                            newPos = virtualNewPos;
                        }
                    }

                    firstSegment.posNow = newPos;
                    this.ropeSegments[i] = firstSegment;
                    edgeColliderPoints[i] = firstSegment.posNow - new Vector2(transform.position.x, transform.position.y);
                }

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
        if (endPoint!=null)
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

            if (dist > ropeSegLength) //si la distancia entre puntos es mayor a la distancia admitida
            {
                //la dirección en la que se moverá el primer punto es igual a la direccion del primer al segundo punto
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLength) // y vice
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                //se multiplica por 0.5 porque el la distnacia que se deben acomodar los puntos se divide entre ellos
                firstSeg.posNow -= changeAmount * 0.5f; // se mueve el punto la mitad de la distancia hacia una direccion
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f; // se mueve el otro punto a la direccion opuesta
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }

            if (firstSeg.hookPos!=Vector2.zero)
            {
                firstSeg.posNow = firstSeg.hookPos;
                this.ropeSegments[i] = firstSeg;
            }
            if (secondSeg.hookPos != Vector2.zero)
            {
                secondSeg.posNow = secondSeg.hookPos;
                this.ropeSegments[i+1] = secondSeg;
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

    public void SetStartPoint(Transform p_StartPoint)
    {
        startPoint = p_StartPoint;
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