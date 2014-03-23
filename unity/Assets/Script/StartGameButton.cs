using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour
{
	public LevelManager levelManager;
	
	void Start()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		
		levelManager.menuHUD.gameObject.SetActiveRecursively(false);
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (collider.bounds.IntersectRay(ray))
			{
				//Debug.Log("CLICK!!");
				makeStart();
				
			}
		}
		
		if (Input.GetButtonDown("Start"))
		{
			makeStart();
		}
	}
	
	void makeStart()
	{
		levelManager.isPlaying = true;
		transform.parent.gameObject.SetActiveRecursively(false);
		
		levelManager.menuHUD.gameObject.SetActiveRecursively(true);
	}
}
