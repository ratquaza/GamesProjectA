using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Drawing;
using System;

public class Grapple2 : MonoBehaviour
{
    private PlayerActions playerActions;
    private SpringJoint2D springJoint;
    private GameObject rope;
    private LineRenderer lineRenderer;
    private bool isOnCooldown = false;
    private Vector2 hitPoint;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Hook Settings")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float hookSpeed = 10f;
    [SerializeField] private float cooldownDuration = 2f;

    [Header("Line Renderer Settings")]
    [SerializeField] private float lineWidth = 25;
    [SerializeField] private int numberOfPoints = 25; //number of points along curve
    [SerializeField] private float waveFrequency = 1f; //how curvy
    [SerializeField] private float waveAmplitude = 1f; //wave height

    [Header("Secondary Movement Settings")]
    [SerializeField] private float secondaryMovementflingForce = 20f;
    [SerializeField] private float disablePlayerMovementDurationAfterGrappled = 0.2f;
    [SerializeField] private float delayBeforeSecondaryMovementPerformable = 0.2f;


    private void Awake()
    {
        playerActions = new PlayerActions();

        playerActions.Movement.GrapplingHook.performed += ctx => GrappleButtonDown();
        playerActions.Movement.GrapplingHook.canceled += ctx => GrappleButtonUp();

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

    // Press grapple
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

    [SerializeField] private float secondaryMovementWalkDirectionDampening = 10f;

    // Let go of grapple
    void GrappleButtonUp()
    {            
        Vector2 movementInput = playerActions.Movement.Walk.ReadValue<Vector2>();

        // Check if the spring joint is enabled
        if (springJoint.enabled && movementInput != Vector2.zero && secondaryMovementDoable)
        {
            float minForceMagnitude = 0.5f;
            float maxForceMagnitude = 10f;

            // Get the position of the anchor point
            Vector2 anchorPosition = springJoint.connectedAnchor;

            // Calculate the vector from the player to the anchor point
            Vector2 directionToAnchor = anchorPosition - (Vector2)transform.position;

            // Get the movement input vector

            // Combine the direction to the anchor point with the movement input
            Vector2 combinedDirection = directionToAnchor.normalized + movementInput.normalized / secondaryMovementWalkDirectionDampening;

            // Calculate the force based on the combined direction and the distance between the player and the anchor point
            float distance = directionToAnchor.magnitude;
            float forceMagnitude = Mathf.Clamp(distance, minForceMagnitude, maxForceMagnitude);
            Vector2 force = combinedDirection.normalized * forceMagnitude;

            // Apply the force to the connected body
            GetComponent<Rigidbody2D>().AddForce(force * secondaryMovementflingForce, ForceMode2D.Impulse);

            // Disable player movement for a short time
            StartCoroutine(DisablePlayerMovementForDuration(disablePlayerMovementDurationAfterGrappled));
        }

        // Reset secondary movement parameters
        secondaryMovementDoable = false;
        delayCoroutineStarted = false;

        // Disable the spring joint, line renderer, and reset curve points
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

    private bool secondaryMovementDoable = false;
    private bool delayCoroutineStarted = false;

    private async void FixedUpdate()
    {
        if (springJoint.enabled) //if grappling
        {
            // Line Renderer Logic
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

            // Add initial force towards anchor point
            Vector2 direction = (springJoint.connectedAnchor - (Vector2)transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(direction * hookSpeed, ForceMode2D.Force); //add force to push towards anchor point

            // Secondary Movement Enabler
            if (!delayCoroutineStarted)
            {
                StartCoroutine(DelayBeforeSecondaryMovementPerformable(delayBeforeSecondaryMovementPerformable));
                delayCoroutineStarted = true;
            }
        }

    }

    private IEnumerator DelayBeforeSecondaryMovementPerformable(float duration)
    {
        yield return new WaitForSeconds(duration);
        secondaryMovementDoable = true;
    }

    private IEnumerator DisablePlayerMovementForDuration(float duration)
    {
        playerMovement.enabled = false;
        yield return new WaitForSeconds(duration);
        playerMovement.enabled = true;
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
