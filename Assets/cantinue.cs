using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cantinue : MonoBehaviour
{
    public GameObject panel;

    public void Continue()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
