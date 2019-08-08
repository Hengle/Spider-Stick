using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControl : MonoBehaviour
{
    public static WallControl instance;
    private SCR_Gameplay gamePlay;
    private void Awake()
    {
        instance = this;
    }
    public void Setup(SCR_Gameplay gamePlay)
    {
        this.gamePlay = gamePlay;
    }
    //private void FixedUpdate()
    //{

    //    if (SCR_Gameplay.instance.player)
    //    {
    //        if (transform.position.x < (SCR_Gameplay.instance.player.transform.position.x - 10))
    //        {
    //            Vector3 walllast = gamePlay.wallLast.position;
    //            transform.position = new Vector3(walllast.x + 10, gamePlay.GetRandomY2(), 0);
    //            gamePlay.wallLast = transform;
    //        }
    //    }
    //}
}
