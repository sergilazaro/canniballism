using UnityEngine;
using System.Collections;

public class DebugController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Backspace) == true)
		{
			SnakeBody snakebody = GameObject.Find("Snake2_Tail").GetComponent<SnakeBody>();
			
			if (!snakebody.isHead()) {
				snakebody.makeHead();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Space) == true)
		{
			SnakeBody snakebody = GameObject.Find("SnakeHead").GetComponent<SnakeBody>();
			snakebody.addTail();
		}
	}
}
