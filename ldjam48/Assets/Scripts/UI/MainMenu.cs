using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void GoToCity() {
        SceneManager.LoadScene("City");
    }
}
