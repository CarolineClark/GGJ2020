using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public const string TitleScene = "Scenes/TitleScreen";

    public string TargetScene;
    private bool playerAtGoal = false;

    void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter");
        playerAtGoal = true;
    }

    void OnTriggerExit(Collider other) {
        Debug.Log("OnTriggerExit");
        playerAtGoal = false;
    }

    public void Interact() {
        if (playerAtGoal && TargetScene != null && TargetScene != "") {
            Debug.Log("Player at goal, changing scenes: " + TargetScene);
            SceneManager.LoadScene(TargetScene);
        } else if (playerAtGoal) {
            Debug.Log("Player at goal, no target scene set, returning to title");
            SceneManager.LoadScene(TitleScene);
        } else {
            Debug.Log("Player not at goal, interaction ignored");
        }
    }
}
