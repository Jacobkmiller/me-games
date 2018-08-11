// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private float cameraLimit = 85;
	[SerializeField]
	private float cameraRotationOffset;
	[SerializeField]
	private GameObject arm;
	[SerializeField]
	private GameObject spineCoords;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 jumpVector = Vector3.zero;
	private Vector3 rotationOffset;
	private Vector3 rotationDirectionCorrection;

	private Rigidbody rb;
	private bool pauseArm;
	private Animator animator;
	int jumpHash = Animator.StringToHash("Jump");
	private Vector3 deleteme;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		pauseArm = false;
		rotationOffset = new Vector3(0f, cameraRotationOffset, 0f);
		// cam.transform.rotation = Quaternion.LookRotation(spineCoords.transform.forward+spineCoords.transform.up, spineCoords.transform.right);
		animator = GetComponent<Animator>();
		
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = Quaternion.AngleAxis(cameraRotationOffset, Vector3.up) * _velocity;
		// velocity = _velocity;
		animator.SetFloat("Speed", velocity.magnitude);
		// too nsfw
		animator.SetFloat("Direction", -1*Vector3.SignedAngle(velocity, Quaternion.AngleAxis(cameraRotationOffset, Vector3.up)*transform.forward, Vector3.up));
	}

	// Gets a rotational vector
	public void Rotate (Vector3 _rotation)
	{
		rotation = _rotation;
	}

	// Gets a rotational vector for camera ;)
	public void RotateCamera (float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	// Gets a rotational vector for camera
	public void Jump (float _jumpForce)
	{	
		animator.SetTrigger(jumpHash);
		jumpVector = new Vector3(0.0f, 2.0f, 0.0f);
		rb.AddForce(jumpVector * _jumpForce, ForceMode.Impulse);
	}
// Remember to pick up tomatoes from the grocery store
	void Update() {
		if (Input.GetKeyDown("h")) {
			pauseArm = !pauseArm;
		}

		if (Input.GetKeyDown("r")) {
			// cameraRotationOffset;
		}
		cam.transform.rotation = Quaternion.LookRotation(spineCoords.transform.forward+spineCoords.transform.up);
	}

	// Update is called once per frame
	void FixedUpdate ()
	{	
		// cam.transform.rotation = Quaternion.LookRotation(spineCoords.transform.forward+spineCoords.transform.up);
		PerformMovement();
		PerformRotation();
	}

	void LateUpdate() {
		if (arm != null && !pauseArm) {
			// arm.transform.Rotate(currentCameraRotationX, 0, 0);
			// arm.transform.localEulerAngles = new Vector3(351.3f, 79.2f, -currentCameraRotationX);
			// Quaternion r1 = Quaternion.AngleAxis(currentCameraRotationX, spineCoords.transform.forward);
			// Quaternion r2 = Quaternion.AngleAxis(40, arm.transform.right);
			// Quaternion r3 = Quaternion.AngleAxis(-40, arm.transform.up);
			// Quaternion r4 = Quaternion.AngleAxis(currentCameraRotationX, arm.transform.forward);
			Vector3 tempY = Quaternion.AngleAxis(-currentCameraRotationX, arm.transform.forward) * spineCoords.transform.up;
			Vector3 tempZ = Quaternion.AngleAxis(currentCameraRotationX, arm.transform.up) * spineCoords.transform.forward;
			// Vector3 tempR = (spineCoords.transform.up + spineCoords.transform.forward).normalized;
			// deleteme = Quaternion.AngleAxis(currentCameraRotationX, Vector3.Cross(tempR, spineCoords.transform.right)) * tempR;
			// arm.transform.rotation = Quaternion.FromToRotation(deleteme, tempR);
			arm.transform.rotation = Quaternion.LookRotation(tempZ, tempY);
			// cam.transform.rotation = Quaternion.LookRotation(spineCoords.transform.forward+spineCoords.transform.up);
			// cam.transform.localPosition = cam.transform.right*2f;
			// cam.couch.com
		
			// arm.transform.rotation = r1;//*r2*r3*r4;
		}
	}

	// Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
			Debug.Log("MOVED");
		}
	}

	// Perform rotation
	// Do a flip
	void PerformRotation ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
		if (cam != null && !pauseArm)
		{
			// cam.transform.Rotate(-cameraRotation);
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraLimit, cameraLimit);

			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f) + rotationOffset;
		}
		
	}
	
}
