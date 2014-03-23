using UnityEngine;
using System.Collections;

public class SphereWalker : MonoBehaviour
{
	public SphereCameraController cameraController;
	
	private LevelManager levelManager;
	
	private bool isWalkingUp;
	public bool getIsWalkingUp() { return isWalkingUp; }
	
	// Use this for initialization
	void Start ()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// set left, up depending on camera point of view.
		//Vector3 left = new Vector3(-1,0,0); // test left
		//Vector3 up = new Vector3(0,1,0); // test up
		
		Vector3 cameraUp = Camera.mainCamera.transform.up.normalized;
		Vector3 cameraForward = (-Camera.mainCamera.transform.position).normalized;
		
		Vector3 left = Vector3.Cross(cameraForward, cameraUp);
		Vector3 up = cameraUp;
		
		Vector3 gravity = transform.position.normalized;
		
		Vector3 forward = Vector3.Cross(gravity, left);
		Vector3 lateral = Vector3.Cross(gravity, up);
		
		// FORWARD
		float input_forward_dir = 0.0f;
		
		if (Input.GetKey(KeyCode.UpArrow) == true)
			input_forward_dir += 1.0f;
		if (Input.GetKey(KeyCode.DownArrow) == true)
			input_forward_dir -= 1.0f;
		
		// LATERAL
		float input_lateral_dir = 0.0f;
		
		if (Input.GetKey(KeyCode.RightArrow) == true)
			input_lateral_dir += 1.0f;
		if (Input.GetKey(KeyCode.LeftArrow) == true)
			input_lateral_dir -= 1.0f;
		
		//Debug.Log("forward: " + forward);
		
		Vector3 walkingDir = (input_forward_dir*forward + input_lateral_dir*lateral).normalized;
		
		if (walkingDir.y > 0)
			isWalkingUp = true;
		else if (walkingDir.y < 0)
			isWalkingUp = false;
		
		//transform.position += forward*Time.fixedTime*input_forward_dir*0.05f;
		//transform.position += lateral*Time.fixedTime*input_lateral_dir*0.05f;
		
		transform.position += walkingDir*Time.deltaTime*10.0f;
		transform.position = transform.position.normalized*levelManager.worldRadius;
		
		transform.LookAt(transform.position + walkingDir, -gravity);
		
	}
	
	void OnDrawGizmos()
	{
		
	}
}
