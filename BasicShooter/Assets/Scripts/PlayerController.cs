﻿using System.Collections;
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
	private PlayerMotor motor;
	private bool isGrounded;

	void Start()
	{
		motor = GetComponent<PlayerMotor>();
	}

	void OnCollisionStay()
	{
		isGrounded = true;
	}

	void Update()
	{
		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");
		float _jump = Input.GetAxisRaw("Jump");

		if((_jump > 0) && isGrounded){
				motor.Jump(jumpForce);
				isGrounded = false;
		}

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//Final Movement velcoty
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;
		motor.Move(_velocity);

		//Calculate rotation as a 3D vector (Turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (Looking around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;
		
		//Apply camera rotation
		motor.RotateCamera(_cameraRotation);

	}
}
