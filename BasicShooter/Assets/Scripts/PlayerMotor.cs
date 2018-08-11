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
	private GameObject lowerSpine;
	[SerializeField]
	private GameObject head;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 jumpVector = Vector3.zero;

	private Rigidbody rb;
	private bool pauseArm;
	private Animator animator;
	int jumpHash = Animator.StringToHash("Jump");
	private bool debugTrigger;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		pauseArm = false;
		animator = GetComponent<Animator>();
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
		animator.SetFloat("Speed", velocity.magnitude);
		animator.SetFloat("Direction", -1*Vector3.SignedAngle(velocity,cam.transform.forward, Vector3.up ));
	}

	// Gets a rotational vector
	public void Rotate (Vector3 _rotation)
	{
		rotation = _rotation;
	}

	// Gets a rotational vector for camera
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

	public void SetGroundedTrigger(bool isGrounded) {
		animator.SetBool("Grounded", isGrounded);
	}

	void Update() {
		if (Input.GetKeyDown("h")) {
			pauseArm = !pauseArm;
		}

		if (Input.GetKeyDown("r")) {
			debugTrigger = !debugTrigger;
		}

	}

	void DrawDebug() {
		Debug.DrawRay(cam.transform.position, cam.transform.up, Color.green);
		Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.blue);
		Debug.DrawRay(cam.transform.position, cam.transform.right, Color.red);
		Debug.DrawRay(head.transform.position, head.transform.up, Color.green);
		Debug.DrawRay(head.transform.position, head.transform.forward, Color.blue);
		Debug.DrawRay(head.transform.position, head.transform.right, Color.red);
	}
	// Update is called once per frame
	void FixedUpdate ()
	{	
		PerformMovement();
		PerformRotation();
	}

	void LateUpdate() {
		if (lowerSpine != null && !pauseArm) {
			lowerSpine.transform.Rotate(cam.transform.right, currentCameraRotationX, Space.World);
		}
		if (debugTrigger) {
			DrawDebug();
		}
	}

	// Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero) {	
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
			Debug.DrawRay(rb.position, velocity, Color.black);
		}
	}

	// Perform rotation
	void PerformRotation ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

		if (cam != null && !pauseArm)
		{
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraLimit, cameraLimit);
		}
			
	}
	
}