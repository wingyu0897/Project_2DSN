using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rigid;

	[SerializeField] private MovementData movementData;

	private float currentVelocity;
	private int currentDirection = 1;
	private int input = 0;

	private bool isGround = false;
	private bool canJump = false;
	private float coyoteTimer = 0;
	private int groundLayer;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		groundLayer = LayerMask.NameToLayer("Ground");
	}

	private void Update()
	{
		CoyoteTime();
		CheckGround();
		SetMovement();
	}

	/// <summary>
	/// 설정된 값으로 움직임을 실행하기
	/// </summary>
	private void SetMovement()
	{
		if (input != 0)
			currentVelocity = SpeedUp();
		else
			currentVelocity = SpeedDown();
		rigid.velocity = new Vector2(currentDirection * currentVelocity, rigid.velocity.y);
	}

	#region #좌우이동
	/// <summary>
	/// 좌우로 이동하게 하기
	/// </summary>
	/// <param name="direction">1 이상이면 오른쪽, -1 이하면 왼쪽으로 이동</param>
	public void Move(float direction)
	{
		if (direction == 0 || (rigid.velocity.x != 0 && rigid.velocity.x * direction <= 0))
		{
			input = 0;
			return;
		}

		input = 1;
		currentDirection = direction > 0 ? 1 : -1;
	}

	/// <summary>
	/// 좌우 이동 중 즉시 정지하기
	/// </summary>
	public void StopVelocity()
	{
		currentVelocity = 0;
		rigid.velocity = new Vector2(0, rigid.velocity.y);
	}

	private float SpeedUp() => Mathf.Clamp(currentVelocity += Time.deltaTime * movementData.acceleration, 0, movementData.maxSpeed);
	private float SpeedDown() => Mathf.Clamp(currentVelocity -= Time.deltaTime * movementData.deAcceleration, 0, movementData.maxSpeed);
	#endregion

	#region #상하이동
	/// <summary>
	/// 코요테 확인하기
	/// </summary>
	private void CoyoteTime()
	{
		if (!isGround && canJump)
		{
			coyoteTimer += Time.deltaTime;
			if (coyoteTimer > movementData.coyoteTime)
			{
				coyoteTimer = 0;
				canJump = false;
			}
		}
	}

	/// <summary>
	/// 바닥에 닿았는지 확인하기
	/// </summary>
	private void CheckGround()
	{
		RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.99f, 1), 0, Vector2.down, 0.55f, 1 << groundLayer);
		isGround = hit.collider != null;

		if (isGround && rigid.velocity.y <= 0)
			canJump = true;
	}

	/// <summary>
	/// 점프하기
	/// </summary>
	public void Jump()
	{
		if (canJump)
		{
			isGround = false;
			canJump = false;
			rigid.velocity = new Vector2(rigid.velocity.x, 0);
			rigid.AddForce(new Vector2(0, movementData.jumpForce), ForceMode2D.Impulse);
		}
	}
	#endregion
}
