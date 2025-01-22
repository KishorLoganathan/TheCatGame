using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Check if/where rotation
// Vector calculations from player to sphere
// Align rotation with vector

public class MovingSphere : MonoBehaviour
{

    Animator animator;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    Vector3 velocity, desiredVelocity, connectionVelocity;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;
    
    Rigidbody body, connectedBody, previousConnectedBody;

    bool desiredJump;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    int groundContactCount, steepContactCount;
    bool OnGround => groundContactCount > 0;
    bool OnSteep => steepContactCount > 0;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    int jumpPhase;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f, maxStairsAngle = 50f;

    float minGroundDotProduct, minStairsDotProduct;

    Vector3 contactNormal, steepNormal;

    int stepsSinceLastGrounded, stepsSinceLastJump;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    LayerMask probeMask = -1, stairsMask = -1;

    [SerializeField]
    Transform playerInputSpace = default;

    Vector3 upAxis, rightAxis, forwardAxis;

    Vector3 connectionWorldPosition, connectionLocalPosition;

    [SerializeField, Range(0f, 20f)]
    float rotationSpeed = 10f;

    private float lastAttackTime;
    private bool isAttacking1, isAttacking2;
    private float doubleTapWindow = 0.5f;

    [SerializeField]
    private float attack1Duration = 0.5f;

    [SerializeField]
    private float attack2Duration = 0.5f;


    void OnValidate() {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    void Awake() {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        animator = GetComponent<Animator>();
        OnValidate();
    }

  
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");  
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (playerInputSpace) {
            rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
        } else {
            rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
        }
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        animator.SetBool("IsWalking", desiredVelocity.magnitude > 0.1f);
        

        desiredJump |= Input.GetButtonDown("Jump");

        if (Input.GetKeyDown(KeyCode.F)) {
            PerformAttack();
        }

        // Draw a line from the character's head (position) to a point in front of it
        //Vector3 frontPosition = transform.position + transform.forward * 2.0f;
        //Debug.DrawLine(transform.position, frontPosition, Color.red);

    }

    void PerformAttack() {

        float currentTime = Time.time;

        if (isAttacking1 && (currentTime - lastAttackTime <= doubleTapWindow)) {

            // This will trigger attack 2 if F is pressed again during the attack time window
            isAttacking2 = true;
            isAttacking1 = false;
            animator.SetBool("isAttacking2", true);
            animator.SetBool("isAttacking1", false);
            StartCoroutine(ResetAttackState(attack2Duration));
        } else {

            // This will trigger attack 1
            isAttacking2 = false;
            isAttacking1 = true;
            animator.SetBool("isAttacking2", false);
            animator.SetBool("isAttacking1", true);
            StartCoroutine(ResetAttackState(attack1Duration));
        }

        lastAttackTime = currentTime;
    }

    IEnumerator ResetAttackState(float delay) {

        yield return new WaitForSeconds(delay);
        
        isAttacking2 = false;
        isAttacking1 = false;
        animator.SetBool("isAttacking2", false);
        animator.SetBool("isAttacking1", false);
    }

    void FixedUpdate() {
        upAxis = -Physics.gravity.normalized;
        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);

        Quaternion gravityAlignment = Quaternion.FromToRotation(transform.up, upAxis);

        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(velocity, upAxis);



        // Here, we will only adjust the movement alignment only if the player is moving
        Quaternion targetRotation = gravityAlignment * transform.rotation;
        if (horizontalVelocity.magnitude > 0.1f) {
            Quaternion movementAlignment = Quaternion.LookRotation(horizontalVelocity, upAxis);
            targetRotation = movementAlignment * gravityAlignment;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        UpdateState();
        AdjustVelocity();

        if (desiredJump) {
            desiredJump = false;
            Jump(gravity);
        }

        animator.SetBool("IsJumping", jumpPhase == 1 && !OnGround);
        animator.SetBool("IsFalling", !OnGround && velocity.y < 0f);

        velocity += gravity * Time.deltaTime;
        body.velocity = velocity;

        

        ClearState();
    }

    void Jump(Vector3 gravity) {
        Vector3 jumpDirection;

        if(OnGround) {
            jumpDirection = contactNormal;
            animator.SetBool("IsJumping", true);
            animator.SetBool("Landed", false);
        } else if (OnSteep) {
            jumpDirection = steepNormal;
            jumpPhase = 0;
        } else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps) {
            if (jumpPhase == 0) {
                jumpPhase = 1;
            }
            if (jumpPhase == 1) {
                animator.SetBool("IsDoubleJumping", true);
                StartCoroutine(ResetDoubleJump());
            }
            jumpDirection = contactNormal;
        } else {
            return;
        }

        stepsSinceLastJump = 0;
        jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
        jumpDirection = (jumpDirection + upAxis).normalized;
        float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
        if (alignedSpeed > 0f) {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        velocity += jumpDirection * jumpSpeed;

    }

    IEnumerator ResetDoubleJump() {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("IsDoubleJumping", false);
    }

    void OnCollisionEnter(Collision collision) {
        EvaluateCollision(collision);
    }
    
    void OnCollisionStay(Collision collision) {
        EvaluateCollision(collision);
    }

    void EvaluateCollision (Collision collision) {
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minDot) {
                groundContactCount += 1;
                contactNormal += normal;
                connectedBody = collision.rigidbody;
            } else if (upDot > -0.01f) {
                steepContactCount += 1;
                steepNormal += normal;
                if (groundContactCount == 0) {
                    connectedBody = collision.rigidbody;
                }
            }
        }
    }

    void UpdateState() {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        velocity = body.velocity;

        if (OnGround || SnapToGround() || CheckSteepContacts()) {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1) {
                jumpPhase = 0; 
            }
            if (stepsSinceLastJump > 1 && velocity.y <= 0) {
                animator.SetBool("Landed", true);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsDoubleJumping", false);
            }
            if (groundContactCount > 1) {
                contactNormal.Normalize();   
            }
        } else {
            contactNormal = upAxis;
        }

        if (connectedBody) {
            if (connectedBody.isKinematic || connectedBody.mass >= body.mass) {
                UpdateConnectionState();
            }
            
        }
    }

    void UpdateConnectionState() {
        if (connectedBody == previousConnectedBody) {
            Vector3 connectionMovement = connectedBody.transform.TransformPoint(connectionLocalPosition) - connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }
        
        connectionWorldPosition = body.position;
        connectionLocalPosition = connectedBody.transform.InverseTransformPoint(connectionWorldPosition);
    }

    Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    void AdjustVelocity() {
        Vector3 xAxis = ProjectDirectionOnPlane(rightAxis, contactNormal);
        Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);

        Vector3 relativeVelocity = velocity - connectionVelocity;
        float currentX = Vector3.Dot(relativeVelocity, xAxis);
        float currentZ = Vector3.Dot(relativeVelocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        
        //Debug.DrawRay(transform.position, velocity, Color.red);
    }

    void ClearState() {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = connectionVelocity = Vector3.zero;
        previousConnectedBody = connectedBody;
        connectedBody = null;
    }

    bool SnapToGround () {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2) {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed) {
            return false;
        }
        if (!Physics.Raycast(body.position, -upAxis, out RaycastHit hit, probeDistance, probeMask)) {
            return false;
        }
        float upDot = Vector3.Dot(upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer)) {
            return false;
        }
        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f) {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }

        connectedBody = hit.rigidbody;
        return true;
    }

    float GetMinDot (int layer) {
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDotProduct : minStairsDotProduct;
    }

    bool CheckSteepContacts () {
        if (steepContactCount > 1) {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct) {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }

        return false;
    }

    public void PreventSnapToGround() {
        stepsSinceLastJump = -1;
    }
}

