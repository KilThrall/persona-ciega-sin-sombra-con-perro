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

    #endregion
    private Transform endPoint;
    private EdgeCollider2D edgeCollider;
    private bool isGrabbed;

    private Transform InteractionTrigger;
    private LineRenderer lineRenderer;
    private readonly List<RopeSegment> ropeSegments = new(); //C# 9.0 !!!1! 

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        startPoint = transform;
        endPoint = defaultEndpoint;

        Vector3 ropeStartPoint = startPoint.position;

        for (int i = 0; i < lineRendererPositions; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint,i));
            ropeStartPoint.y -= ropeSegLength;
        }

        InteractionTrigger = transform.GetChild(1);
    }


    // Update is called once per frame
    void Update()
    {
        this.DrawRope();
    }

    private void FixedUpdate()
    {
        this.Simulate();
        if (lineRenderer.positionCount > 0)
        {
            InteractionTrigger.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pos = collision.GetContact(0).point;
        Vector2 colisionDir = (transform.position- collision.transform.position).normalized;

        var nearestSegment = GetNearestSegment(pos);
        nearestSegment.forceToAdd += colisionDir*2;
        ropeSegments[nearestSegment.index] = nearestSegment;
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
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < this.lineRendererPositions; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += firstSegment.forceToAdd+forceGravity * Time.fixedDeltaTime;
            firstSegment.forceToAdd = Vector2.zero;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
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
        if (isGrabbed)
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
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
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
        List<Vector2> ropePosV2 = new();
        for (int i = 0; i < ropePositions.Length; i++)
        {
            ropePosV2.Add(new Vector2(ropePositions[i].x-transform.position.x, ropePositions[i].y-transform.position.y));
        }
        edgeCollider.SetPoints(ropePosV2);
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
        public Vector2 forceToAdd;
        public RopeSegment(Vector2 pos,int index)
        {
            forceToAdd = Vector2.zero;
            this.index = index;
            this.posNow = pos;
            this.posOld = pos;
        }
    }



}