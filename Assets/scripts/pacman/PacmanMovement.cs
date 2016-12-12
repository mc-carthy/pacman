using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PacmanMovement : MonoBehaviour {

	[SerializeField]
	private float moveSpeed = 0.4f;

	private Rigidbody2D rb;
	private Animator anim;
	private Vector2 destination = Vector2.zero;
	private Vector2 direction = Vector2.zero;
	private Vector2 nextDirection = Vector2.zero;

	private void Awake () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	private void Start () {
		destination = transform.position;
	}

	private void FixedUpdate () {
		Move();
		Animate();
	}

	private bool IsValidDirection (Vector2 dir) {
		Vector2 pos = transform.position;
		dir += new Vector2(dir.x * 0.45f, dir.y * 0.45f);

		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		if (hit.collider.CompareTag("maze")) {
			return false;
		}

		return true;
	}

	private void Animate () {
		Vector2 dir = destination - (Vector2)transform.position;
		anim.SetFloat("x", dir.x);
		anim.SetFloat("y", dir.y);
	}

	private void Move () {
		Vector2 pos = Vector2.MoveTowards(transform.position, destination, moveSpeed);
		rb.MovePosition(pos);

		if (Input.GetAxis("Horizontal") > 0) {
			nextDirection = Vector2.right;
		}
		if (Input.GetAxis("Horizontal") < 0) {
			nextDirection = Vector2.left;
		}

		if (Input.GetAxis("Vertical") > 0) {
			nextDirection = Vector2.up;
		}
		if (Input.GetAxis("Vertical") < 0) {
			nextDirection = Vector2.down;
		}

		if (Vector2.Distance(destination, transform.position) < 0.01f) {
			if (IsValidDirection(nextDirection)) {
				destination = (Vector2)transform.position + nextDirection;
				direction = nextDirection;
			} else if (IsValidDirection(direction)) {
				destination = (Vector2)transform.position + direction;
			}
		}
	}
}
