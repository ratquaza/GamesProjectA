using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Drawing;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float hookSpeed = 10f;
    [SerializeField] private float cooldownDuration = 2f;
    private SpringJoint2D springJoint;
    private GameObject rope;
    private LineRenderer lineRenderer;
    private PlayerActions playerActions;
    private bool isOnCooldown = false;
    [SerializeField] private int lineWidth = 25;
    [SerializeField] private int numberOfPoints = 25; //number of points along curve
    [SerializeField] private float waveFrequency = 1f; //how curvy
    [SerializeField] private float waveAmplitude = 1f; //wave height
    private Vector2 hitPoint;

    private void Awake()
    {
        playerActions = new PlayerActions();

        playerActions.Movement.Dash.performed += ctx => GrappleButtonDown();
        playerActions.Movement.Dash.canceled += ctx => GrappleButtonUp();

        rope = GetComponentInChildren<LineRenderer>().gameObject;
        lineRenderer = rope.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }

    void GrappleButtonDown()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        if (!isOnCooldown)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, wallLayer);

            if (hit.collider != null)
            {
                springJoint.connectedAnchor = hit.point;
                springJoint.enabled = true;

                //begin cooldown
                StartCoroutine(StartCooldown());

                //enable lineRenderer, set entry exit points
                lineRenderer.positionCount = numberOfPoints;
                hitPoint = hit.point;
            }
        }
    }

    void GrappleButtonUp()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;
        hasSetCurvePoints = false;
    }

    IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }


    private bool hasSetCurvePoints = false;

    void FixedUpdate()
    {
        if (springJoint.enabled) //if grappling
        {
            if (!hasSetCurvePoints) //set points of line renderer to sin curve function
            {
                lineRenderer.positionCount = numberOfPoints;
                SetCurvePoints(transform.position, hitPoint);
                lineRenderer.enabled = true;
                hasSetCurvePoints = true;
            }
            else //else lerp to straight line function
            {
                LerpToStraightLine(transform.position, hitPoint);
                lineRenderer.enabled = true;
            }

            Vector2 direction = (springJoint.connectedAnchor - (Vector2) transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(direction * hookSpeed, ForceMode2D.Force); //add force to push towards anchor point
        }
    }

    [SerializeField] [Range(0f, 100f)] private float animationSpeed = 15f;
    [SerializeField] [Range(0,24)] private int removeLineRenderer = 5; //removes first few linerenderer points that overlap with player

    void LerpToStraightLine(Vector3 start, Vector3 end)
    {
        for (int i = removeLineRenderer; i < numberOfPoints; i++)
        {
            float t = (float) i / (numberOfPoints - 1);
            Vector3 lerpedPosition = Vector3.Lerp(start, end, t);
            lineRenderer.SetPosition(i, Vector3.Lerp(lineRenderer.GetPosition(i), lerpedPosition, animationSpeed * Time.deltaTime)); //lerp to straight line
        }

        for (int i = 0; i < removeLineRenderer; i++)
        {
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(removeLineRenderer));
        }
    }

    void SetCurvePoints(Vector3 start, Vector3 end)
    {
        Vector3[] points = new Vector3[numberOfPoints];
        float step = 1f / (numberOfPoints - 1);

        for (int i = removeLineRenderer; i < numberOfPoints; i++)
        {
            float t = step * i;
            float x = Mathf.Lerp(start.x, end.x, t);
            float y = Mathf.Lerp(start.y, end.y, t);

            //wave effect to y coord
            y += Mathf.Sin(t * Mathf.PI * waveFrequency) * waveAmplitude;

            points[i] = new Vector3(x, y, 0f);
        }

        for (int i = 0; i < removeLineRenderer; i++)
        {
            points[i] = points[removeLineRenderer];
        }

        lineRenderer.SetPositions(points);
    }
}
