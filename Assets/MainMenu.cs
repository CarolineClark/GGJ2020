using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            this.PlayGame();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit executed");
        Application.Quit();
    }
}
