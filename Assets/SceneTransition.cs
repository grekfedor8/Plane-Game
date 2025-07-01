using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public int sceneNumber = 0;
    public void Transition()
    {
        SceneManager.LoadScene(sceneNumber);
        Time.timeScale = 1f;
    }
}
