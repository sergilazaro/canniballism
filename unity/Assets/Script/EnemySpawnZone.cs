using UnityEngine;
using System.Collections;

public class EnemySpawnZone : MonoBehaviour
{
	public float radius;
	private float nextSpawnRemainingTime;
	
	
	LevelManager levelManager;
	
	// Use this for initialization
	void Start ()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		transform.position = transform.position.normalized*levelManager.worldRadius;
		
		nextSpawnRemainingTime = UnityEngine.Random.Range(2.0f, 5.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (levelManager.isPlaying)
		{
			nextSpawnRemainingTime -= Time.deltaTime;
			
			if (nextSpawnRemainingTime < 0.0f /*|| Input.GetKeyDown(KeyCode.Space)*/)
			{
				CreateEnemy(transform.position, UnityEngine.Random.Range(2, 5));			
				nextSpawnRemainingTime = UnityEngine.Random.Range(10.0f, 20.0f);
			}
		}
	}
	
	void CreateEnemy(Vector3 position, int numTails)
	{
		SnakeBody snakeHead = ((Transform)Instantiate(levelManager.snakeHeadAI_prefab)).GetComponent<SnakeBody>();
		snakeHead.transform.position = position;
		//T.RotateAroundLocal(Vector3.up, 90.0f);
		
		SnakeBody followed = snakeHead;
		for (int i = 0; i < numTails; i++)
		{
			SnakeBody snakeTail = ((Transform)Instantiate(levelManager.snakeTail_prefab)).GetComponent<SnakeBody>();
			snakeTail.transform.position = position;
			
			snakeTail.bodyFollowed = followed;
			followed.bodyFollower = snakeTail;
			
			followed = snakeTail;
		}
		
		levelManager.playClip(LevelManager.ClipType.ENEMYSPAWN);
		
	}
	
	void OnDrawGizmos()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		Vector3 pos = transform.position.normalized*levelManager.worldRadius;
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(pos, radius);
	}
}
