using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlycamControl : MonoBehaviour
{
    private static FlycamControl instance;
    public FlycamControl fly;
    // private Vector3 target;
    // private float posY1 = -1.8f;
    // private float posY2 = 1.8f;

    private float speed = 1f;
    private float timeDelay = 4f;
    private float timeCount;
    private float dir = -1;
    private bool onCollision;
    public GameObject topLimit;
    public GameObject bottomLimit;


    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //timeCount ++;
        //Debug.Log(timeCount);
        //Debug.Log(posY);
        //if (timeCount > timeDelay)
        //{
        //    //target = new Vector3(transform.position.x, posY2, 0);
        //    // transform.position = Vector3.MoveTowards(transform.position, target,speed * Time.deltaTime);

        //    //timeCount = 0;
        //}
        transform.Translate(dir * Vector2.down * Time.deltaTime * speed);
        if (transform.position.y > topLimit.transform.position.y ||
            transform.position.y < bottomLimit.transform.position.y)
        {
            dir = -dir;
        }
     
        if (SCR_Gameplay.instance.player)
        {
            if (transform.position.x < SCR_Gameplay.instance.player.transform.position.x - 20)
            {
                Destroy(this.gameObject);
            }
        }
        if (onCollision == true)
        {
            timeCount += Time.deltaTime;
            if (timeCount > timeDelay)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FallEnemy();
            onCollision = true;
        }
    }
    private void FallEnemy()
    {
        rb.velocity = new Vector2(rb.velocity.x - 1f, rb.velocity.y);
        rb.gravityScale = 2f;
        fly.transform.Rotate(0, 0,Time.deltaTime*speed);
        base.transform.localEulerAngles = Vector3.zero;
        base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
    }
    
}
