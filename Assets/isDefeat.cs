using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isDefeat : MonoBehaviour
{
    public static bool isDef = false;
    public GameObject panel;

    public void Start()
    {
        isDef = false;
        Time.timeScale = 1f;
    }

    public void Update()
    {
        if (isDef)
        {
            panel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
