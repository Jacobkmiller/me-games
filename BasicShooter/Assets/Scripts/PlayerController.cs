using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;
	[SerializeField]
	private float jumpForce = 1.25f;
	[SerializeField]
	private Camera cam;
	[SerializeField]
	private float airialVelocityMuliplier;
	private PlayerMotor motor;
	private bool isGrounded;
	

	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		Cursor.visible = false;
		
		// Cursor.lockState = CursorLockMode.Locked;
	}

	void OnCollisionStay()
	{	
		isGrounded = true;
		// motor.SetGroundedTrigger(isGrounded);
	}

	void Update()
	{	
		if (PauseMenu.isOn) {
			if (Cursor.lockState != CursorLockMode.None) {
				Cursor.lockState = CursorLockMode.None;
			}

			motor.Move(Vector3.zero);
			motor.Rotate(Vector3.zero);
			motor.RotateCamera(0f);
			return;
		}


		if (Cursor.lockState != CursorLockMode.Locked) {
			Cursor.lockState = CursorLockMode.Locked;
		}
		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");
		float _jump = Input.GetAxisRaw("Jump");


		if((_jump != 0) && isGrounded){
				motor.Jump(jumpForce);
				isGrounded = false;
				// motor.SetGroundedTrigger(isGrounded);
		}

		Vector3 _movHorizontal = (cam.transform.right - new Vector3(0f, cam.transform.right.y, 0f)).normalized * _xMov;
		Vector3 _movVertical = (cam.transform.forward - new Vector3(0f, cam.transform.forward.y, 0f)).normalized * _zMov;

		//Final Movement velocity
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;
		if (isGrounded){
			motor.Move(_velocity);
		} else {
			motor.Move(_velocity*airialVelocityMuliplier);
		}
		
		

		//Calculate rotation as a 3D vector (Turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (Looking around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotation = _xRot * lookSensitivity;
		// Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;
		
		//Apply camera rotation
		motor.RotateCamera(_cameraRotation);
		// motor.RotateArm(_cameraRotation);

	}
}
