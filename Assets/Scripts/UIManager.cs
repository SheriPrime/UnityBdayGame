using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("EnterLobby");
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
