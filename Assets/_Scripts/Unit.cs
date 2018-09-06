using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public Transform target;

	[SerializeField]
	private float moveSpeed = 4f; //Change in inspector to adjust move speed

	Vector3[] path;
	int targetIndex;

	[SerializeField]
	private Camera _camera;

	private Vector3 _forward;
	private Vector3 _right;

	/// <summary>
	/// Forward position relative to the camera 
	/// </summary>
	public Vector3 Forward
	{
		get
		{
			if (_forward == Vector3.zero)
			{
				_forward = _camera.transform.forward; // Set forward to equal the camera's forward vector
				_forward.y = 0; // make sure y is 0
				_forward = Vector3.Normalize(_forward);
				return _forward;
			}
			else
			{
				return _forward;
			}
		}
	}

	/// <summary>
	/// "Right" as in Orthoganol to "Forward"
	/// </summary>
	public Vector3 Right
	{
		get
		{
			if (_right == Vector3.zero)
			{
				return _right = Quaternion.Euler(new Vector3(0, 90, 0)) * Forward;
			}
			else
			{
				return _right;
			}
		}
	}


	void Start()
	{
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			int targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		Debug.Log(path.Length);
		Vector3 currentWaypoint = path[0];

		while (true)
		{
			if (transform.position.x == currentWaypoint.x && transform.position.z == currentWaypoint.z)
			{
				if (++targetIndex >= path.Length)
				{
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			Move(currentWaypoint - transform.position);

			//transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	void Move(Vector3 RelativePath)
	{
		RelativePath.y = 0;
		Vector3 move = moveSpeed * Time.deltaTime * RelativePath;
		Vector3 heading = Vector3.Normalize(move);
		transform.forward = heading; // Sets forward direction of our game object to whatever direction we're moving in
		transform.position += move;
	}

}
