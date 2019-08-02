// class: SCR_Player
using System.Collections.Generic;
using UnityEngine;

public class SCR_Player : MonoBehaviour
{
	public const float ACCELERATION = 1.0075f;

	public const float SLOW_MOTION = 0.2f;

	public const float SLOW_MOTION_DURATION = 0.2f;

	public const float RECOVER_SPEED = 5f;

	public const float SHORTEN_DISTANCE = 2f;

	public const float SHORTEN_TIME = 0.5f;

	public const float DISTANCE_MIN = 1f;

	public const float OFFSET_HIT = 0.7f;

	public const float OFFSET_SLIP = -0.1f;

	public readonly Vector2 JUMP_FORCE = new Vector2(700f, 700f);

	public Transform hand;

	public TrailRenderer trailRenderer;

	public PlayerState state;

	private Rigidbody2D rb;

	private DistanceJoint2D distanceJoint2D;

	private LineRenderer lineRenderer;

	private SpriteRenderer spriteRenderer;

	private Animator animator;

	private float slowMotionTime;

	private float targetDistance;

    private bool checkCollision;

    public void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		distanceJoint2D = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	public void Start()
	{
		state = PlayerState.STAND;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (SCR_Gameplay.instance.state == GameState.READYENDLESS)
			{
				SwitchState(PlayerState.JUMP);
				rb.AddForce(JUMP_FORCE);
				SCR_Gameplay.instance.SwitchState(GameState.PLAY);
			}
			else if (SCR_Gameplay.instance.state == GameState.PLAY && !distanceJoint2D.enabled)
			{
				SwitchState(PlayerState.SWING);
				Grab2(SCR_Gameplay.instance.GetNextAnchor2());
			}
            if (checkCollision == true)
            {
                Release();
            }
		}
		if (Input.GetMouseButtonUp(0) && SCR_Gameplay.instance.state == GameState.PLAY && distanceJoint2D.enabled)
		{
			SwitchState(PlayerState.JUMP);
            //if (scr_gameplay.instance.islastanchor2(distancejoint2d.connectedbody))
            //{
            //    time.timescale = 0.2f;
            //    time.fixeddeltatime = time.timescale * 0.02f;
            //}
            Release();
		}
		if (distanceJoint2D.enabled)
		{
			Vector3 v = base.transform.position - distanceJoint2D.connectedBody.transform.position;
			Vector3 v2 = rb.velocity;
			float num = Vector2.SignedAngle(v, v2);
			float num2 = 0f;
			num2 = ((!(num >= 0f)) ? (0f - Mathf.Abs(base.transform.localScale.x)) : Mathf.Abs(base.transform.localScale.x));
			base.transform.localScale = new Vector3(num2, base.transform.localScale.y, base.transform.localScale.z);
			Vector3 position = distanceJoint2D.connectedBody.transform.position;
			lineRenderer.SetPosition(0, new Vector3(position.x, position.y, 1f));
			lineRenderer.SetPosition(1, hand.position);
			float z = Vector2.SignedAngle(new Vector2(0f, -1f), v);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, z);
		}
		// Xét nếu vị trí nhân vật thấp hơn độ cao của màn hình thì gameover và hủy nhân vật
		if (base.transform.position.y < (0f - SCR_Gameplay.SCREEN_HEIGHT) * 0.6f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			SCR_Gameplay.instance.SwitchState(GameState.GAME_OVER);
		}
		if (!(Time.timeScale < 1f))
		{
			return;
		}
		slowMotionTime += Time.deltaTime;
		if (slowMotionTime >= 0.2f)
		{
			Time.timeScale += Time.deltaTime * 5f;
			if (Time.timeScale > 1f)
			{
				Time.timeScale = 1f;
			}
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
	}
	public void FixedUpdate()
	{
		if (distanceJoint2D.enabled)
		{
			rb.velocity *= 1.0075f;
		}
	}
	public void SwitchState(PlayerState s)
	{
		state = s;
		if (state == PlayerState.STAND)
		{
			animator.SetTrigger("stand");
		}
		else if (state == PlayerState.JUMP)
		{
			animator.SetTrigger("jump");
		}
		else if (state == PlayerState.SWING)
		{
			animator.SetTrigger("swing");
		}
		else if (state == PlayerState.HIT)
		{
			animator.SetTrigger("hit");
		}
	}

	public void Grab(GameObject anchor)
	{
		if (anchor != null)
		{
			distanceJoint2D.connectedBody = anchor.GetComponent<Rigidbody2D>();
			distanceJoint2D.enabled = true;
			float magnitude = (anchor.transform.position - base.transform.position).magnitude;
			distanceJoint2D.distance = magnitude;
			targetDistance = magnitude - 2f;
			if (targetDistance < 1f)
			{
				targetDistance = 1f;
			}
			iTween.ValueTo(base.gameObject, iTween.Hash("from", magnitude, "to", targetDistance, "time", 0.5f, "easetype", "easeOutSine", "onupdate", "UpdateDistance"));
			lineRenderer.enabled = true;
		}
	}
    public void Grab2(Transform anchor)
    {
        if (anchor != null)
        {
            //Debug.LogError("noi cong day nek");
            distanceJoint2D.connectedBody = anchor.GetComponent<Rigidbody2D>();
            distanceJoint2D.enabled = true;
            float magnitude = (anchor.transform.position - base.transform.position).magnitude;
            distanceJoint2D.distance = magnitude;
            targetDistance = magnitude - 2f;
            if (targetDistance < 1f)
            {
                targetDistance = 1f;
            }

            iTween.ValueTo(base.gameObject, iTween.Hash("from", magnitude, "to", targetDistance, "time", 0.5f, "easetype", "easeOutSine", "onupdate", "UpdateDistance"));
            lineRenderer.enabled = true;
            // Event

            //string strJson = "";
            //Dictionary<string, object> jsonValue = MiniJSON.Json.Deserialize(strJson) as Dictionary<string, object>;
            //List<object> lstLevel = jsonValue["level"] as List<object>;

            SCR_Gameplay.instance.MoveAnchor();
            //SCR_Gameplay.instance.OnCreateEnemy();
            
            //Vector3 anchorlast = gamePlay.anchorLast.position;
            //AnchorControl.instance.transform.position = new Vector3(anchorlast.x + 10, gamePlay.GetRandomY(), 0);
            //gamePlay.AddLastAnchor(AnchorControl.instance.transform);
            //Debug.Log("run");
        }
        else
        {
            //Debug.LogError("ko tim thay diem noi nek");      
        }
    }

    public void Release()
	{
		distanceJoint2D.connectedBody = null;
		distanceJoint2D.enabled = false;
		lineRenderer.enabled = false;
	}

	public void UpdateDistance(float d)
	{
		distanceJoint2D.distance = d;
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		Release();
		if (other.gameObject.tag == "Wall")
		{
            checkCollision = true;
			if (base.transform.position.x < other.transform.position.x - other.transform.localScale.x * 0.5f)
			{
				Fall();
            }
			else
			{
				SCR_Gameplay.instance.SwitchState(GameState.GAME_OVER);
			}
		}
        if (other.gameObject.tag == "Enemy")
        {
            checkCollision = true;
            if (base.transform.position.x < other.transform.position.x - other.transform.localScale.x * 0.5f)
            {
                Fall2();
            }
            else
            {
                SCR_Gameplay.instance.SwitchState(GameState.GAME_OVER);
            }
        }
		//else if (other.gameObject.tag == "FinishPoint")
		//{
		//	float x = (other.collider as BoxCollider2D).size.x;
		//	if (base.transform.position.x >= other.transform.position.x - x * 0.5f && base.transform.position.x <= other.transform.position.x + x * 0.5f)
		//	{
		//		Stand();
		//		SCR_Gameplay.instance.SwitchState(GameState.LEVEL_CLEARED);
		//	}
		//	else if (base.transform.position.x < other.transform.position.x - x * 0.5f)
		//	{
		//		Fall();
		//	}
		//}
	}

	private void Fall()
	{
		rb.velocity = new Vector2(0f, rb.velocity.y);
		rb.angularVelocity = 0f;
		base.transform.localEulerAngles = Vector3.zero;
		GetComponent<Collider2D>().enabled = false;
		SwitchState(PlayerState.HIT);
		base.transform.position = new Vector3(base.transform.position.x + 0.7f, base.transform.position.y, base.transform.position.z);
		trailRenderer.emitting = false;
		SCR_Gameplay.instance.updateCamera = false;
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
    }
    private void Fall2()
    {
        rb.mass = 2f;
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        rb.angularVelocity = 0f;
        base.transform.localEulerAngles = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        SwitchState(PlayerState.HIT);
        base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
        trailRenderer.emitting = false;
        SCR_Gameplay.instance.updateCamera = false;
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
    }

	private void Stand()
	{
		base.transform.localEulerAngles = Vector3.zero;
		rb.simulated = false;
		SwitchState(PlayerState.STAND);
	}

	public void OnStartSlip()
	{
		base.transform.position = new Vector3(base.transform.position.x + -0.1f, base.transform.position.y, base.transform.position.z);
	}
}
