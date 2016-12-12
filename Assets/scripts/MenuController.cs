using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void LoadGame () {
		SceneManager.LoadScene("main", LoadSceneMode.Single);
	}
}
