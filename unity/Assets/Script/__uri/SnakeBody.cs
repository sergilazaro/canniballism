using UnityEngine;
using System.Collections;

public class SnakeBody : MonoBehaviour
{
	private LevelManager levelManager;
	
	public bool isPlayer = false;
	
	public SnakeBody bodyFollowed = null;
	public SnakeBody bodyFollower = null;
	
	private Vector3 prevPosition;
	private bool prevPosition_isvalid = false;
	
	public bool getPrevPositionIsValid()	{ return prevPosition_isvalid; }
	public Vector3 getPrevPosition() 		{ return prevPosition; }
	
	private enum State {
		FREE,
		RING
	};
	private State state;
	//private float timeLastStateChange;
	
	private bool isInvulnerable;
	private float invulnerableRemainingTime;
	
	public bool isHead() 			{ return bodyFollowed == null; 	}
	public bool isBell() 			{ return bodyFollower == null; 	}	
	public bool getIsPlayer()		{ return isHead() ? isPlayer : getHead().getIsPlayer(); }	
	public bool getIsInvulnerable() { return isHead() ? isInvulnerable : getHead().getIsInvulnerable();	}
	
	
	private Vector3 positionDelta;
	
	
	void Awake()
	{
		state = State.FREE;
		//timeLastStateChange = Time.time;
	}
	
	void Start ()
	{
		prevPosition = transform.position;
		if (bodyFollowed != null)
			bodyFollowed.bodyFollower = this;
		
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		
		if (getIsPlayer())
		{
			levelManager.OnPlayerTailLenghtChanged(this.countTailLength());
			
			// change color of tails.
			if (isHead())
				this.setTailColor(levelManager.playerTailColor);
		}
	}
	
	public void setTailColor(Color color)
	{
		SnakeBody tail;
		
		tail = isHead() ? bodyFollower : getHead().bodyFollower;
		
		while (tail != null)
		{
			tail.GetComponentInChildren<Gyroscope>().SetColor(color);
			tail = tail.bodyFollower;
		}
	}
	
	void Update()
	{	
		
		
		switch (state)
		{
		case State.FREE:
			Update_Free();
			break;
		case State.RING:
			Update_Ring();
			break;
		}
	}
	
	private void Update_Free()
	{
		prevPosition = transform.position;
		
		if (!isHead())
		{
			float maxRadius = (bodyFollowed.transform.localScale.x + transform.localScale.x)/2.0f;
			if (bodyFollowed.isHead())
				maxRadius *= 1.25f;
			
			Vector3 followDir = bodyFollowed.transform.position - transform.position;
			float currentRadius = followDir.magnitude;
			if (currentRadius > maxRadius)
				transform.position += followDir.normalized*(currentRadius-maxRadius)*0.5f;			
		}
		else
		{
			if (rigidbody != null)
				rigidbody.velocity = Vector3.zero;
		}
		
		positionDelta = transform.position - prevPosition;
		
		if (!isHead())
		{
			transform.position = transform.position.normalized*levelManager.worldRadius;		
		
			if (positionDelta.magnitude > 0.01f)
				transform.LookAt(transform.position + positionDelta, transform.position.normalized);
		}
		
		if (isHead() && isInvulnerable)
		{
			//Debug.Log("invulnerable: " + invulnerableRemainingTime);
			invulnerableRemainingTime -= Time.deltaTime;
			if (invulnerableRemainingTime < 0)
				isInvulnerable = false;
		}
		
		if (isHead() && getIsPlayer())
		{
			levelManager.OnPlayerTailLenghtChanged(countTailLength());
		}
	}
	
	private void Update_Ring()
	{
		if (isHead() && rigidbody != null)
		{
			rigidbody.velocity = Vector3.zero;
		}
	}
	
	public void makeHead()
	{
		Transform T = (Transform)Instantiate(levelManager.snakeHeadAI_prefab, transform.position, transform.rotation);
		SnakeBody newSnakeHead = T.GetComponent<SnakeBody>();
		
		newSnakeHead.bodyFollower = bodyFollower;
		newSnakeHead.bodyFollowed = null;
		
		if (bodyFollower != null)
			bodyFollower.bodyFollowed = newSnakeHead;
		
		Color col = newSnakeHead.getIsPlayer() ? levelManager.playerTailColor : levelManager.enemyTailColor;
		newSnakeHead.setTailColor(col);
		
		Destroy(this.gameObject);
		
		/*// versio: el cap s'intancia al reves.
		Transform T = (Transform)Instantiate(levelManager.snakeHeadAI_prefab, transform.position, transform.rotation);
		SnakeBody newSnakeHead = T.GetComponent<SnakeBody>();
		
		
		
		newSnakeHead.bodyFollower = bodyFollower;
		newSnakeHead.bodyFollowed = null;
		
		if (bodyFollower != null)
			bodyFollower.bodyFollowed = newSnakeHead;
		
		Color col = newSnakeHead.getIsPlayer() ? levelManager.playerTailColor : levelManager.enemyTailColor;
		newSnakeHead.setTailColor(col);
		
		Destroy(this.gameObject);*/
	}
	
	public void addTail()
	{
		if (bodyFollower == null)
		{
			Transform T = (Transform)Instantiate(levelManager.snakeTail_prefab, transform.position, transform.rotation);
			SnakeBody newSnakeTail = T.GetComponent<SnakeBody>();
			
			newSnakeTail.bodyFollowed = this;
			newSnakeTail.bodyFollower = null;
			
			bodyFollower = newSnakeTail;
			
			Color col = getIsPlayer() ? levelManager.playerTailColor : levelManager.enemyTailColor;
			newSnakeTail.GetComponentInChildren<Gyroscope>().SetColor(col);
		}
		else
		{
			bodyFollower.addTail();
		}
	}
	
	public void splitTail()
	{
		if (!isBell())
		{
			this.bodyFollower.bodyFollowed = null;
			this.bodyFollower.makeHead();
		}
		
		Destroy(this.gameObject);
	}
	
	public void makeRing()
	{
		/*SnakeBody bell = this.getBell();
		this.transform.position = bell.transform.position;
		
		state = State.RING;
		
		GetComponent<SnakeAxisAlignedKBController>().enabled = false;*/
	}
	
	public SnakeBody getBell()
	{
		SnakeBody bell = this;
		while (bell.bodyFollower != null)
			bell = bell.bodyFollower;
		
		return bell;
	}
	
	public SnakeBody getHead()
	{
		SnakeBody head = this;
		while (head.bodyFollowed != null)
			head = head.bodyFollowed;
		
		return head;
	}
	
	public int countTailLength()
	{
		int count = 0;
		SnakeBody body = getHead();
		while (body.bodyFollower != null) {
			body = body.bodyFollower;
			count++;
		}
		
		return count;
	}
	
	public SnakeBody getLastTail()
	{
		SnakeBody lasttail = getHead();
		while (lasttail.bodyFollower != null) {
			lasttail = lasttail.bodyFollower;
		}
		
		return lasttail;
	}
	
	public void selfDestroy()
	{
		SnakeBody body = getLastTail();
		
		while (body != null)
		{
			SnakeBody bodytodestroy = body;
			body = body.bodyFollowed;
			Destroy(body.gameObject);
		}
		
	}
	
	void OnDrawGizmos()
	{
		if (bodyFollowed != null)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(bodyFollowed.transform.position, transform.position);
		}
		
		//Gizmos.matrix = transform.localToWorldMatrix;
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawLine(Vector3.zero, new Vector3(0,0,2));
		
		//Gizmos.color = Color.cyan;
		//Gizmos.DrawRay(transform.position, positionDelta.normalized*2.0f);
	}
	
	public void OnHeadTouchesTail(SnakeBody snakeTail)
	{
		SnakeBody otherSnakeHead = snakeTail;
		
		// is last tail? (bell)
		while (otherSnakeHead.bodyFollowed != null)
			otherSnakeHead = otherSnakeHead.bodyFollowed;
		
		if (this != otherSnakeHead)
		{
			if (!snakeTail.getIsInvulnerable())
			{
				// if is bell, get control of all other body's snake
				if (snakeTail.isBell())
				{
					this.bodyFollower.bodyFollowed = snakeTail;
					snakeTail.bodyFollower = this.bodyFollower;
					
					this.bodyFollower = otherSnakeHead.bodyFollower;
					otherSnakeHead.bodyFollower.bodyFollowed = this;
					
					this.transform.position = otherSnakeHead.transform.position;
					
					if (otherSnakeHead.getIsPlayer())
					{
						// tell levelmanager we died...
						levelManager.OnPlayerDied(); 
						// change color.
						this.setTailColor(levelManager.enemyTailColor);
					}
					
					if (this.getIsPlayer())
					{
						this.setTailColor(levelManager.playerTailColor);
						levelManager.playClip(LevelManager.ClipType.EAT);
					}
					
					Destroy(otherSnakeHead.gameObject);
				}
				else // otherwise, only split.
				{
					snakeTail.splitTail();
					this.addTail();
					
					if (this.getIsPlayer())
					{
						levelManager.playClip(LevelManager.ClipType.EAT);
					}
					
					if (otherSnakeHead.getIsPlayer())
					{
						levelManager.playClip(LevelManager.ClipType.HURT);
					}
					
					isInvulnerable = true;
					invulnerableRemainingTime = Mathf.Min(invulnerableRemainingTime + 3.0f, 3.0f);
				}
				
				if (otherSnakeHead.getIsPlayer())
				{
					levelManager.OnPlayerTailLenghtChanged(otherSnakeHead.countTailLength());
				}
				else if (this.getIsPlayer())
				{
					levelManager.OnPlayerTailLenghtChanged(this.countTailLength());
				}
			}
		}
		else
		{
			/*if (snakeTail.isBell())
			{
				this.makeRing();
			}*/
		}
	}
}
