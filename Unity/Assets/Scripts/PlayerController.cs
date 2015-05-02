using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float Speed;
    public float Acceleration;
    public float Drag;
    public float Gravity;
    public float JumpPower;
    public float PitchLimit;
    public float horSensitivity;
    public float verSensitivity;

    private Rigidbody rigidbody;
    private ColliderCheck jumpCheck;
    private bool mouseIsLocked;
    private float jumpTimer;

    private bool isAirborne
    {
        get { return !jumpCheck.IsColliding; }
    }

	// Use this for initialization
	void Awake () 
    {
        rigidbody = GetComponent<Rigidbody>();
        jumpCheck = transform.FindChild("JumpCheck").GetComponent<ColliderCheck>();
	}

    void Start()
    {
        lockPointer();
    }

    void Update()
    {
        if (!mouseIsLocked && Input.GetMouseButtonDown(0))
        {
            lockPointer();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            mouseIsLocked = false;
        }

        rotation();
        applyGravity();
        checkJump();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        movement();
        //applyGravity();
	}

    private void movement()
    {
        float yVelocity = rigidbody.velocity.y;
        Vector3 xzVelocity = rigidbody.velocity;
        xzVelocity.y = 0;

        Vector3 verticalDirection = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 horizontalDirection = transform.right * Input.GetAxisRaw("Horizontal");

        Vector3 totalDirection = (verticalDirection + horizontalDirection).normalized;

        if (totalDirection.sqrMagnitude > 0)
        {
            rigidbody.AddForce(totalDirection * Acceleration);
        }
        else if (xzVelocity.sqrMagnitude > .05f)
        {
            float newMagnitude = xzVelocity.magnitude - Drag * Time.fixedDeltaTime;
            if (newMagnitude < .05f)
                newMagnitude = 0;
            xzVelocity = xzVelocity.normalized * newMagnitude;
        }
        else
        {
            xzVelocity = Vector3.zero;
        }

        if (xzVelocity.sqrMagnitude > Speed * Speed)
            xzVelocity = xzVelocity.normalized * Speed;

        xzVelocity.y = yVelocity;

        rigidbody.velocity = xzVelocity;
    }

    private void applyGravity()
    {
        //rigidbody.AddForce(Vector3.down * Gravity);
        if (isAirborne)
        {
            rigidbody.AddForce(Vector3.down * Gravity);
            if (jumpTimer > 0)
            {

            }
        }
        else if (jumpTimer <= 0 && rigidbody.velocity.sqrMagnitude < .1f)
        {
            Vector3 vel = rigidbody.velocity;
            vel.y = 0;
            rigidbody.velocity = vel;
        }
    }

    private void rotation()
    {
        if (!mouseIsLocked) return;

        float horizontalDirection = Input.GetAxisRaw("Mouse X");
        float verticalDirection = -Input.GetAxisRaw("Mouse Y");

        transform.rotation *= Quaternion.AngleAxis(horSensitivity * horizontalDirection * Time.deltaTime, Vector3.up);
        
        if (verticalDirection != 0)
        {
            float cameraAngle = Camera.main.transform.localEulerAngles.x;
            if (cameraAngle > 180) cameraAngle -= 360;
            cameraAngle += verticalDirection * verSensitivity * Time.deltaTime;
            cameraAngle = Mathf.Clamp(cameraAngle, -PitchLimit, PitchLimit);
            Camera.main.transform.localEulerAngles = Vector3.right * cameraAngle;
        }
    }

    private void checkJump()
    {
        jumpTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && !isAirborne && jumpTimer <= 0)
        {
            rigidbody.velocity = (Vector3.up * JumpPower);
            jumpTimer = .25f;
        }
    }

    private void lockPointer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseIsLocked = true;
    }
}
