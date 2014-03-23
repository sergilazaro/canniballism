using UnityEngine;
using System.Collections;

public class SnakeHeadTriggerer : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("SnakeTail"))
		{
			SnakeBody otherSnake = other.GetComponent<SnakeBody>();
			transform.parent.GetComponent<SnakeBody>().OnHeadTouchesTail(otherSnake);
			
			
		}
	}
}
