using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GhostMovement : MonoBehaviour {

	private enum CurrentDirection {
		right,
		left,
		up,
		down
	}

	private CurrentDirection dir;
	private CurrentDirection[] directions = {
		CurrentDirection.right,
		CurrentDirection.left,
		CurrentDirection.up,
		CurrentDirection.down
	};

	[SerializeField]
	private float moveSpeed = 0.3f;

	private Rigidbody2D rb;
	private Animator anim;
	private Vector2 destination = Vector2.zero;
	private Vector2 nextDirection = Vector2.zero;
	private bool isAbleToMove;

	private void Awake () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	private void Start () {
		destination = GetFirstDestinationForGhost();

		dir = (Random.Range(0, 2) > 0) ? CurrentDirection.right : CurrentDirection.left;
		DetermineNextDirection();
		isAbleToMove = true;
	}

	private void FixedUpdate () {
		if (isAbleToMove) {
			Move();
			Animate();
		}
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("pacman")) {
			other.gameObject.GetComponent<PacmanMovement>().Die();
		}
	}

	private Vector3 GetFirstDestinationForGhost () {
		switch (gameObject.name) {
			case "blue":
				return new Vector3(12.5f, 20, 0);
			case "green":
				return new Vector3(14.5f, 20, 0);
			case "pink":
				return new Vector3(16.5f, 20, 0);
			default:
				return Vector3.zero;
		}
	}

	private CurrentDirection GetRandomDirection () {
		return directions[Random.Range(0, directions.Length)];
	}

	private void DetermineNextDirection () {
		switch (dir) {
			case CurrentDirection.right:
				nextDirection = Vector2.right;
				break;
			case CurrentDirection.left:
				nextDirection = Vector2.left;
				break;
			case CurrentDirection.up:
				nextDirection = Vector2.up;
				break;
			case CurrentDirection.down:
				nextDirection = Vector2.down;
				break;
		}
	}

	private void SetCurrentDirection () {
		isAbleToMove = false;
		CurrentDirection newDirection = GetRandomDirection();

		while (newDirection == dir) {
			newDirection = GetRandomDirection();
		}

		dir = newDirection;
		DetermineNextDirection();
		isAbleToMove = true;
	}

	private void SetDestination () {
		destination = (Vector2)transform.position + nextDirection;
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

	private void Move () {
		if ((Vector2)transform.position != destination) {
			Vector2 pos = Vector2.MoveTowards(transform.position, destination, moveSpeed);
			rb.MovePosition(pos);
		} else {
			if (IsValidDirection(nextDirection)) {
				SetDestination();
			} else {
				SetCurrentDirection();
			}
		}
	}

	private void Animate () {
		Vector2 dir = destination - (Vector2)transform.position;
		anim.SetFloat("x", dir.x);
		anim.SetFloat("y", dir.y);
	}

}
