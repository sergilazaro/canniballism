using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour
{
	public enum ControlType
	{
		KeyboardRotation,
		KeyboardPosition,
		RandomAI,
		FollowAI
	}

	public SphereCameraController cameraController;
	public ControlType controlType;

	public float maxForwardVelocity = 10.0f;
	public float maxAngularVelocity = 10.0f;

	public float axisAlignedVelocity = 5.0f;
	public float axisAlignedSmoothTime = 0.3f;

	public float followSmoothTime = 0.3f;


	// inputs:
	private float moveForwardValue = 0.0f;
	private float rotateValue = 0.0f;
	private float verticalValue = 0.0f;
	private float horizontalValue = 0.0f;
	private Vector3 followPosition = Vector3.zero;

	private Vector3 vectorDampVelocity = Vector3.zero;
	private Vector3 followDampVelocity = Vector3.zero;
	
	private LevelManager levelManager;
	
	void Start()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}

	void Update()
	{
		Vector3 position = transform.position;

		Vector3 cameraUp = Camera.mainCamera.transform.up.normalized;
		Vector3 cameraForward = (-Camera.mainCamera.transform.position).normalized;

		Vector3 left = Vector3.Cross(cameraForward, cameraUp);
		Vector3 up = cameraUp;

		Vector3 gravity = transform.position.normalized;

		Vector3 forward = Vector3.Cross(gravity, left).normalized;
		Vector3 lateral = Vector3.Cross(gravity, up).normalized;


		switch (controlType)
		{
			case ControlType.KeyboardPosition:
				{

					Vector3 direction = lateral * horizontalValue + forward * verticalValue;

					position += direction * axisAlignedVelocity * Time.deltaTime;

					Vector3 desiredForward = Vector3.SmoothDamp(transform.forward, direction.normalized, ref vectorDampVelocity, axisAlignedSmoothTime);
					//Debug.Log("KB: lookat: " + (position + desiredForward) + " " + gravity);


					//transform.LookAt(position + desiredForward, gravity);



					Vector3 newRight = Vector3.Cross(gravity.normalized, desiredForward.normalized);
					Vector3 newForward = Vector3.Cross(newRight, gravity.normalized);

					transform.LookAt(position + newForward, gravity);
				}
				break;
				
			case ControlType.RandomAI:
			case ControlType.KeyboardRotation:
				{
					position += transform.forward * moveForwardValue * maxForwardVelocity * Time.deltaTime;

					transform.RotateAroundLocal(transform.up, rotateValue * maxAngularVelocity * Time.deltaTime);

					Vector3 newRight = Vector3.Cross(gravity.normalized, transform.forward.normalized);
					Vector3 newForward = Vector3.Cross(newRight, gravity.normalized);

					transform.LookAt(position + newForward, gravity);
				}
				break;

			case ControlType.FollowAI:
				{
					position += transform.forward * moveForwardValue * maxForwardVelocity * Time.deltaTime;

					Vector3 desiredForward = Vector3.SmoothDamp(transform.forward, followPosition.normalized, ref followDampVelocity, followSmoothTime);

					Vector3 newRight = Vector3.Cross(gravity.normalized, desiredForward.normalized);
					Vector3 newForward = Vector3.Cross(newRight, gravity.normalized);

					transform.LookAt(position + newForward, gravity);
				}
				break;
		}

		transform.position = position.normalized * levelManager.worldRadius;
	}

	public void Rotate(float value)
	{
		rotateValue = value;
	}

	public void MoveForward(float value)
	{
		moveForwardValue = value;
	}

	public void MoveHorizontally(float value)
	{
		horizontalValue = value;
	}

	public void MoveVertically(float value)
	{
		verticalValue = value;
	}

	public void Follow(Vector3 position)
	{
		followPosition = position;
	}

	private static Vector3 Vector2to3(Vector2 vector)
	{
		return new Vector3(vector.x, 0.0f, vector.y);
	}
}
