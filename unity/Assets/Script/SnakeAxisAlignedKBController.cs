using UnityEngine;
using System.Collections;

public class SnakeAxisAlignedKBController : MonoBehaviour
{
	private SnakeController controller;
	LevelManager levelManager;

	void Start()
	{
		controller = gameObject.GetComponent<SnakeController>();
		controller.controlType = SnakeController.ControlType.KeyboardPosition;
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}

	void Update()
	{
		if (levelManager.isPlaying)
		{
			controller.MoveHorizontally(Input.GetAxis("Horizontal"));
			controller.MoveVertically(Input.GetAxis("Vertical"));
		}
	}
}
