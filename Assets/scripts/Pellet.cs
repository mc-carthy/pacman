using UnityEngine;

public class Pellet : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("pacman")) {
			Destroy(gameObject);
		}
	}
}
