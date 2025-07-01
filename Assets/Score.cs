using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score = 0;
    [SerializeField]
    private TextMeshProUGUI _text;

    public void Start()
    {
        _text.text = "0";
        score = 0;
    }

    public void Update()
    {
        _text.text = score.ToString();
    }
}
