using UnityEngine;
using System.Collections;

public class SnakeKeyboardController : MonoBehaviour
{
	private SnakeController controller;

	void Start()
	{
		controller = gameObject.GetComponent<SnakeController>();
		controller.controlType = SnakeController.ControlType.KeyboardRotation;
	}

	void Update()
	{
		float acceleration = Mathf.Max(Input.GetAxis("Vertical"), 0.0f);

		controller.MoveForward(acceleration);

		controller.Rotate(Input.GetAxis("Horizontal"));
	}
}
