using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{
	[Header("Player Movement")]
	[SerializeField] float speed = 10.4f;
	[SerializeField] float jumpForce = 2.7f;
	[SerializeField] float maximumVerticalSpeed = 10f;
	[SerializeField] float rotationSpeed = 72f;

	[Header("Components")]
	[SerializeField] Rigidbody2D rigidbody2d;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] LayerMask groundLayer;
	
	public Camera cam;


	bool isGrounded;
	bool doJump;
	bool canJump = true;

	float squareRotation = 0f;
	float fixedSquareRotation = 0f;

	Vector2 startPosition;


	private void Awake()
	{
		startPosition = transform.position;
	}


	private void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapBoxNonAlloc(
			transform.position - 0.5f * Vector3.up, new Vector2(1.1f, 0.1f),
			0f, new Collider2D[1], groundLayer
		) > 0;


		rigidbody2d.velocity = new Vector2(
			speed,
			Mathf.Clamp(rigidbody2d.velocity.y, -maximumVerticalSpeed, maximumVerticalSpeed)
		);
	}

	private void Update()
	{
		cam.transform.position = new (
			transform.position.x + 5f,
			0f,
			-10f
		);

		if (Keyboard.current?.rKey.wasPressedThisFrame ?? false) {
			transform.position = startPosition;
		}


		if (doJump && canJump && isGrounded) {
			rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			canJump = false;
		} else if (!isGrounded || !doJump) {
			canJump = true;
		}

		if (!isGrounded || doJump) {
			squareRotation += rotationSpeed * Time.deltaTime;
			fixedSquareRotation = Mathf.Round(squareRotation / 90f) * 90f;
		} else {
			squareRotation = Mathf.Lerp(squareRotation, fixedSquareRotation, 0.3f);
		}


		spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, squareRotation);
	}


	public void Jump(InputAction.CallbackContext context)
	{
		doJump = context.ReadValueAsButton();
	}
}
