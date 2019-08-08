using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreControl : MonoBehaviour
{
    public Text txtScore;
    void Update()
    {
        txtScore.text = SCR_Gameplay.instance.score.ToString();
    }
}
