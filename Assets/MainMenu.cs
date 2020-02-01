using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject blokeyFrame;
    public GameObject gravestoneFrame;
    public GameObject treeFrame;
    public GameObject backgroundFrame;

    private void Update()
    {
        UpdateParallax();

        if (Input.GetKey(KeyCode.Return))
        {
            PlayGame();
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

        var pos = blokeyFrame.transform.position;
        blokeyFrame.transform.position = new Vector3(mouseX * 10, pos.y, pos.z);

        pos = gravestoneFrame.transform.position;
        gravestoneFrame.transform.position = new Vector3( mouseX * 20, pos.y, pos.z);


        pos = treeFrame.transform.position;
        treeFrame.transform.position = new Vector3( mouseX * 30, pos.y, pos.z);

        pos = backgroundFrame.transform.position;
        backgroundFrame.transform.position = new Vector3(mouseX * 40, pos.y, pos.z);
    }
}
