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
	private GameObject arm;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 jumpVector = Vector3.zero;

	private Rigidbody rb;
	private bool pauseArm;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		pauseArm = false;
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
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
		jumpVector = new Vector3(0.0f, 2.0f, 0.0f);
		rb.AddForce(jumpVector * _jumpForce, ForceMode.Impulse);
	}

	void Update() {
		if (Input.GetKeyDown("h")) {
			pauseArm = !pauseArm;
		}

		if (Input.GetKeyDown("r")) {
			Debug.Log(arm.transform.localEulerAngles);
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		PerformMovement();
		PerformRotation();
	}

	// Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}
	}

	// Perform rotation
	void PerformRotation ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
		if (cam != null && !pauseArm)
		{
			// cam.transform.Rotate(-cameraRotation);
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraLimit, cameraLimit);

			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		}
		if (arm != null && !pauseArm) {
			// arm.transform.Rotate(currentCameraRotationX, 0, 0);
			arm.transform.localEulerAngles = new Vector3(351.3f, 79.2f, -currentCameraRotationX+55);
		}
	}
	
}
