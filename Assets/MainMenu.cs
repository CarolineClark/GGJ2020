using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject blokeyFrame;
    public GameObject gravestoneFrame;
    public GameObject backgroundFrame;

    private void Update()
    {
        this.UpdateParallax();

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

    private void UpdateParallax()
    {
        var mouseX = (Input.mousePosition.x * 2) / Screen.width;
        Debug.Log(mouseX);
        var pos = blokeyFrame.transform.position;
        blokeyFrame.transform.position = new Vector3(mouseX * 10, pos.y, pos.z);

        pos = gravestoneFrame.transform.position;
        gravestoneFrame.transform.position = new Vector3( mouseX * 20, pos.y, pos.z);

        pos = backgroundFrame.transform.position;
        backgroundFrame.transform.position = new Vector3(mouseX * 40, pos.y, pos.z);
    }
}
