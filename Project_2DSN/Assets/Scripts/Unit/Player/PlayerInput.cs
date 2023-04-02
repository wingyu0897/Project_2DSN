using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Movement movement;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Update()
	{
		MovementInput();
	}

	private void MovementInput()
	{
		float x = Input.GetAxisRaw("Horizontal");
		movement.Move(x);
	
		if (Input.GetKeyDown(KeyCode.Space))
			movement.Jump();
	}
}
