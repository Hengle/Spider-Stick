using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnchorControl : MonoBehaviour
{
    public static AnchorControl instance;
    private void Awake()
    {
        instance = this;
    }
    private SCR_Gameplay gamePlay;

    public void Setup(SCR_Gameplay gamePlay)
    {
        this.gamePlay = gamePlay;
    }

    /*
    private void FixedUpdate()
    {

        if (SCR_Gameplay.instance.player)
        {
            if (transform.position.x < (SCR_Gameplay.instance.player.transform.position.x - 10))
            {
                Vector3 anchorlast = gamePlay.anchorLast.position;
                transform.position = new Vector3(anchorlast.x + 10, gamePlay.GetRandomY(), 0);
                gamePlay.AddLastAnchor(transform);
                //gamePlay.anchorLast = transform;
            }
        }
    }
    */
   
}
