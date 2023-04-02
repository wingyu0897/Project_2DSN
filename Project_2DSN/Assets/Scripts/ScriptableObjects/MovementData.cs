using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Movement")]
public class MovementData : ScriptableObject
{
	public float maxSpeed = 10;
	public float acceleration = 30;
	public float deAcceleration = 40;
	public float jumpForce = 5;
	public float coyoteTime = 0.5f;
}
