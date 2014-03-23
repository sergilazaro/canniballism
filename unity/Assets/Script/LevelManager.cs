using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public Transform snakeHeadPlayer_prefab;
	public Transform snakeHeadAI_prefab;
	public Transform snakeTail_prefab;
	
	public Color enemyTailColor;
	public Color playerTailColor;
	public Color invulnerableTailColor;
	
	public Transform menuTitle;
	public Transform menuGameover;
	public TextMesh scoreText;
	
	public float worldRadius;
	
	private int tailLength;
	private int maxTailLength = 0;
	private bool isPlayerDead = false;
	
	public bool isPlaying = true;
	
	public enum ClipType {
		ENEMYSPAWN,
		EAT,
		HURT,
		DIE,
		MUSIC
	};
	
	public AudioClip clipEnemySpawn;
	public AudioClip clipEat;
	public AudioClip clipHurt;
	public AudioClip clipDie;
	//public AudioClip clipMusic;
	
	public TextMesh current;
	public TextMesh max;
	public TextMesh record;
	
	public Transform menuHUD;
	
	private float deadTime;
	
//	private bool firstPlay = true;
	
	// Use this for initialization
	void Start ()
	{
		//menuHUD.gameObject.SetActiveRecursively(false);
		
		if (GameManager.playFirstTime)
		{
			menuGameover.gameObject.SetActiveRecursively(false);
			menuTitle.gameObject.SetActiveRecursively(true);
			
			GameManager.ofalltime = PlayerPrefs.GetInt("Record", 1);
		}
		else
		{
			menuGameover.gameObject.SetActiveRecursively(true);
			menuTitle.gameObject.SetActiveRecursively(false);
			scoreText.text = "" + GameManager.score;
		}
		
		//Debug.Log("gm-firsttime: " + GameManager.playFirstTime);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Z)) // test input.
		{
			//OnPlayerDied();
		}
		
		if (isPlayerDead)
		{
			if (Time.time - deadTime > 2.0f)
				Application.LoadLevel("main");
		}
		
		if (isPlaying)
		{
			
			current.text = "" + tailLength;
			max.text = "" + maxTailLength;
			record.text = "" + GameManager.ofalltime;
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(Vector3.zero, worldRadius);
	}
	
	public void OnPlayerTailLenghtChanged(int newTailLength)
	{
		tailLength = newTailLength;
		if (tailLength > maxTailLength)
			maxTailLength = tailLength;
	}
	
	public void OnPlayerDied()
	{
		Camera.mainCamera.GetComponent<SphereCameraController>().playerTransform = null;
		isPlayerDead = true;
		
		isPlaying = false;
		
		GameManager.playFirstTime = false;
		GameManager.score = maxTailLength;
		
		GameManager.ofalltime = Mathf.Max(GameManager.score, GameManager.ofalltime);
		
		PlayerPrefs.SetInt("Record", GameManager.ofalltime);
		
		//menuGameover.gameObject.SetActiveRecursively(true);
		//menuTitle.gameObject.SetActiveRecursively(false);
		
		/*
		GameObject[] snakeHeads = GameObject.FindGameObjectsWithTag("SnakeHead");
		foreach (GameObject snakehead in snakeHeads)
		{
			snakehead.GetComponent<SnakeBody>().selfDestroy();
		}*/
		
		deadTime = Time.time;
		
		
		
		
		
		this.playClip(ClipType.DIE);
	}
	
	public void playClip(ClipType cliptype)
	{
		switch (cliptype)
		{
		case ClipType.ENEMYSPAWN: AudioSource.PlayClipAtPoint(clipEnemySpawn, Camera.mainCamera.transform.position);break;
		case ClipType.EAT: AudioSource.PlayClipAtPoint(clipEat, Camera.mainCamera.transform.position);break;
		case ClipType.HURT: AudioSource.PlayClipAtPoint(clipHurt, Camera.mainCamera.transform.position);break;
		case ClipType.DIE: AudioSource.PlayClipAtPoint(clipDie, Camera.mainCamera.transform.position);break;
		case ClipType.MUSIC: break;
		}
	}
	
	void OnGUI()
	{
		/*GUI.color = Color.black;
        GUI.Label(new Rect(10, 05, 100, 20), "CURRENT_: " + tailLength);
		GUI.Label(new Rect(10, 20, 100, 20), "MAX______: " + maxTailLength);
		GUI.Label(new Rect(10, 35, 100, 20), "RECORD__: " + GameManager.ofalltime);
		*/
    }
}
