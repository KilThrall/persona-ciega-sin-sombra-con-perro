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

        foreach (var s in ropeSegments)
        {
            // Gizmos.DrawLine(s.posNow, new Vector3 (s.posNow.x, s.posNow.y, 0) + Vector3.down * maxCollisionDistanceTollerance);
            Gizmos.DrawSphere(s.posNow, maxCollisionDistanceTollerance/2);

        }
    }
    #endregion
    /// <summary>
    /// Comprueba si la distancia maxima desde el inicio a la punta supera el maximo admitido
    /// </summary>
    /// <returns></returns>
    private bool IsOverStretched()
    {
        float currentRopeLength=0;
        for(int i=0;i<lineRendererPositions-1 ;i++)
        {
            currentRopeLength += Vector2.Distance(ropeSegments[i].posNow, ropeSegments[i+1].posNow);
        }
        return maxRopeLength < currentRopeLength;
    }
    /// <summary>
    /// Verifica cual segmento tiene menor distancia con la posicion de impacto
    /// </summary>
    /// <param name="impactPosition">Posicion donde se dio la colision</param>
    /// <returns></returns>
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

    /// <summary>
    /// Se simulan las fuerzas del cable haciendo uso de la Verlet Integration. Se usan raycasts para evitar que atraviese el suelo
    /// </summary>
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

            //se descompone el movimiento en 2 partes, primero se compureba si puede moverse en Y
            #region Overlap test
            /*
            Vector2 virtualPos = newPos;

            virtualPos.y += velocity.y;
            virtualPos.y += forceGravity.y * Time.fixedDeltaTime;

            if (Physics2D.OverlapCircleAll(virtualPos, maxCollisionDistanceTollerance / 2,whatIsFloor).Length > 0)
            {
                firstSegment.yConstrained = true;
            }
            else
            {
                firstSegment.yConstrained = false;
                newPos.y = virtualPos.y;
            }

            

            //luego si puede moverse en X
            virtualPos = newPos;

            virtualPos.x += velocity.x;
            virtualPos.x += forceGravity.x * Time.fixedDeltaTime;


            if (Physics2D.OverlapCircleAll(virtualPos,maxCollisionDistanceTollerance/2,whatIsFloor).Length > 0)
            {
                firstSegment.xConstrained = true;
            }
            else
            {
                firstSegment.xConstrained = false;
                newPos.x = virtualPos.x;
            }
            */
            #endregion

            #region RayCast test
            
            if (!Physics2D.Raycast(newPos, Vector2.down, maxCollisionDistanceTollerance, whatIsFloor))
            {
                newPos.y += velocity.y;
                newPos.y += forceGravity.y * Time.fixedDeltaTime;
                firstSegment.yConstrained = false;
            }
            else
            {
                firstSegment.yConstrained = true;
            }
            //luego si puede moverse en X
            if (!Physics2D.Raycast(newPos, Vector2.right, maxCollisionDistanceTollerance, whatIsFloor) && (!Physics2D.Raycast(newPos, Vector2.left, maxCollisionDistanceTollerance, whatIsFloor)))
            { 
                newPos.x += velocity.x;
                newPos.x += forceGravity.x * Time.fixedDeltaTime;
                firstSegment.xConstrained = false;
            }
            else
            {
                firstSegment.xConstrained = true;
            }
             
            #endregion

            #region RayCast test2
            /*
            if (!Physics2D.Raycast(newPos, Vector2.down, maxCollisionDistanceTollerance, whatIsFloor))
            {
                newPos.y += velocity.y;
                newPos.y += forceGravity.y * Time.fixedDeltaTime;
                firstSegment.yConstrained = false;
            }
            else
            {
                firstSegment.yConstrained = true;
            }
            //luego si puede moverse en X
            if (i < lineRendererPositions-1 && !Physics2D.Raycast(newPos, new Vector2(ropeSegments[1+i].posNow.x- firstSegment.posNow.x ,0).normalized ,maxCollisionDistanceTollerance, whatIsFloor))
            {
                newPos.x += velocity.x;
                newPos.x += forceGravity.x * Time.fixedDeltaTime;
                firstSegment.xConstrained = false;
            }
            else
            {
                firstSegment.xConstrained = true;
            }
            */
            #endregion
            firstSegment.posNow = newPos;
            this.ropeSegments[i] = firstSegment;
            edgeColliderPoints[i] = firstSegment.posNow - new Vector2(transform.position.x, transform.position.y);
        }
        //CONSTRAINTS
        for (int i = 0; i < 30; i++)
        {
            this.ApplyConstraint();
        }
    }
    /// <summary>
    /// La distancia entre los segmentos no puede separar la distancia especificada. Se aplican constricciones para mantener uniformidad
    /// </summary>
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
                // se mueve el punto la mitad de la distancia hacia una direccion
                Vector2 newPos = firstSeg.posNow;

                if (!firstSeg.xConstrained)
                {
                    newPos.x -= changeAmount.x * 0.5f;
                }
                if (!firstSeg.yConstrained)
                {
                    newPos.y -= changeAmount.y * 0.5f;
                }

                firstSeg.posNow = newPos;
                this.ropeSegments[i] = firstSeg;

                Vector2 newSecondPos = secondSeg.posNow;

                if (!secondSeg.xConstrained)
                {
                    newSecondPos.x += changeAmount.x * 0.5f;
                }
                if (!secondSeg.yConstrained)
                {
                    newSecondPos.y += changeAmount.y * 0.5f;
                }
                secondSeg.posNow = newSecondPos;
                this.ropeSegments[i+1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }
    /// <summary>
    /// Dibujado de la linea en base a los segmentos
    /// </summary>
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
        public bool xConstrained;
        public bool yConstrained;
        public bool leftConstrained;
        public bool rightConstrained;

        public RopeSegment(Vector2 pos,int index)
        {
            yConstrained = false;
            xConstrained = false;
            leftConstrained = false;
            rightConstrained = false;

            this.index = index;
            this.posNow = pos;
            this.posOld = pos;
        }
    }

}