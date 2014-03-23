using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
	//public float rotationSpeed = 5.0f;

	public Color color = Color.red;

	void Update()
	{
		/*
		transform.Find("U").transform.RotateAroundLocal(Vector3.forward, Mathf.SmoothStep(-.5f, .5f, Mathf.PingPong(Time.time, 1) - .5f));
		Debug.Log(Mathf.PingPong(Time.time, 1));
		 * */

		SetColor(color);
	}

	public void SetColor(Color newColor)
	{
		color = newColor;

		foreach (Transform child in transform)
		{
			Color currentColor = color;
			currentColor.a = .7f;
			child.gameObject.renderer.material.color = currentColor;
		}
	}
}
