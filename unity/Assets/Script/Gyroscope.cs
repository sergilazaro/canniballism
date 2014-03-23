using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gyroscope : MonoBehaviour
{
	private Dictionary<GameObject, float> rotationSpeeds = new Dictionary<GameObject, float>();

	public int numCircles = 4;
	public Color color = Color.red;
	public Material[] materials;
	public GameObject flatCubePrefab;
	public GameObject seeThroughSpherePrefab;

	public float minRotationSpeed = 1.0f;
	public float maxRotationSpeed = 10.0f;

	public float minRadius = 5.0f;
	public float maxRadius = 10.0f;

	public bool alignToForward = true;

	void Start()
	{
		for (int i = 0; i < numCircles; i++)
		{
			GameObject circle = (GameObject) GameObject.Instantiate(flatCubePrefab, transform.position, Quaternion.LookRotation(Vector3.up));
			circle.transform.parent = this.transform;

			circle.renderer.material = materials[Random.Range(0, materials.Length - 1)];

			circle.transform.localScale = Vector3.one * Random.Range(minRadius, maxRadius);
		}

		SetColor(color);

		foreach (Transform child in transform)
		{
			if (!alignToForward)
			{
				Vector3 rotationAxis = Random.onUnitSphere;

				child.gameObject.transform.LookAt(child.gameObject.transform.position + rotationAxis);
				child.gameObject.transform.RotateAroundLocal(Vector3.right, Random.Range(0, 360));
			}

			rotationSpeeds.Add(child.gameObject, Random.Range(minRotationSpeed, maxRotationSpeed));
		}

		GameObject sphere;
		if (alignToForward)
		{
			sphere = (GameObject) GameObject.Instantiate(seeThroughSpherePrefab, transform.position, Quaternion.identity);
		}
		else
		{
			sphere = (GameObject) GameObject.Instantiate(seeThroughSpherePrefab, transform.position, Random.rotation);
		}
		sphere.transform.parent = this.transform;

		Color sphereColor = color;
		sphereColor.a = .5f;
		sphere.renderer.material.color = sphereColor;

		sphere.transform.localScale = Vector3.one * Mathf.Lerp(minRadius, maxRadius, .8f);
	}

	public void SetColor(Color newColor)
	{
		color = newColor;

		foreach (Transform child in transform)
		{
			Color currentColor = color;
			currentColor.a = Random.Range(.1f, .7f);
			child.gameObject.renderer.material.color = currentColor;
		}
	}

	void Update()
	{
		foreach (Transform child in transform)
		{
			if (rotationSpeeds.ContainsKey(child.gameObject))
			{
				if (alignToForward)
				{
					child.gameObject.transform.RotateAroundLocal(Vector3.forward, rotationSpeeds[child.gameObject] * Time.deltaTime);
				}
				else
				{
					child.gameObject.transform.RotateAroundLocal(Vector3.right, rotationSpeeds[child.gameObject] * Time.deltaTime);
				}
			}
		}
	}

	void OnDrawGizmos()
	{
		Color sphereColor = color;
		sphereColor.a = .1f;

		Gizmos.color = sphereColor;
		Gizmos.DrawSphere(transform.position, minRadius);
		Gizmos.DrawSphere(transform.position, maxRadius);

		sphereColor.a = .2f;
		Gizmos.color = sphereColor;
		Gizmos.DrawWireSphere(transform.position, minRadius);
		Gizmos.DrawWireSphere(transform.position, maxRadius);
	}
}
