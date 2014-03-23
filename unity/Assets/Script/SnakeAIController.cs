using UnityEngine;
using System.Collections;

public class SnakeAIController : MonoBehaviour
{
	public GameObject objectToFollow = null;

	public float rotationSmoothingTime = 0.3f;
	public float randomRange = 2.0f;
	public float forwardSpeed = 1.0f;
	public float followRotationSpeed = 5.0f;

	private SnakeController controller;

	private float currentRotation = 0.0f;
	private float currentRotationVelocity = 0.0f;

	void Start()
	{
		controller = gameObject.GetComponent<SnakeController>();
		controller.controlType = SnakeController.ControlType.RandomAI;
	}

	void Update()
	{
		if (objectToFollow != null)
		{
			controller.controlType = SnakeController.ControlType.FollowAI;

			Vector3 localDirection = transform.InverseTransformDirection(objectToFollow.transform.position - transform.position).normalized;
			//Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, localDirection);
			localDirection.y = 0.0f;

			controller.Follow(transform.TransformDirection(localDirection));

				/*
			float angle = Vector3.Angle(Vector3.forward, localDirection);

			float rightSign = Mathf.Sign(Vector3.Dot(localDirection, Vector3.right));
			float forwardSign = Mathf.Sign(Vector3.Dot(localDirection, Vector3.forward));

			Debug.Log("RF(" + localDirection + "): " + rightSign + " " + forwardSign);

			float angleFactor = Mathf.Abs(angle) / 180.0f;

			if (localDirection.x > 0.0f)
			{
				rotationValue = angleFactor * followRotationSpeed;
			}
			else
			{
				rotationValue = angleFactor * -followRotationSpeed;
			}
			// followRotationSpeed
				 */
		}
		else
		{
			controller.controlType = SnakeController.ControlType.RandomAI;

			float rotationValue = Random.Range(-randomRange, randomRange);
			currentRotation = Mathf.SmoothDamp(currentRotation, rotationValue, ref currentRotationVelocity, rotationSmoothingTime);

			controller.Rotate(currentRotation);
		}

		controller.MoveForward(forwardSpeed);
	}
}
