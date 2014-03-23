using UnityEngine;
using System.Collections;

public class SphereCameraController : MonoBehaviour
{
	public Transform playerTransform;
	public float distance;
	
	private Vector3 velocity = Vector3.zero;
	private float smoothTime = 0.3f;
	
	private Vector3 lastTargetPosition;

	// Use this for initialization
	void Start() 
	{
		distance = transform.position.magnitude;
	}
	
	// Update is called once per frame
	void Update()
	{
		Vector3 targetPosition;
		
		if (playerTransform != null)
		{
			Vector3 camBackward = playerTransform.position.normalized;
			camBackward.y *= 0.9f;
			camBackward.Normalize();
			targetPosition = camBackward * distance;
		}
		else
		{
			targetPosition = lastTargetPosition;
		}
		
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		transform.LookAt(/*playerTransform.position*/Vector3.zero);
		
		lastTargetPosition = targetPosition;
	}
}
