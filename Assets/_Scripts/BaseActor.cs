using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;

namespace Actor
{
	public abstract class BaseActor : MonoBehaviour
	{
		public Transform target;
		
		/// <summary>
		/// The normal Time to wait between moves
		/// </summary>
		[SerializeField]
		protected float baseTimeBetwenMoves = 0.5f;

		protected virtual float TimeBetweenMoves
		{
			get { return baseTimeBetwenMoves; }
		}

		/// <summary>
		/// The Base move distance of any actor
		/// </summary>
		[SerializeField]
		protected int baseMoveDistance;

		/// <summary>
		/// the path the actor will take to the destination
		/// </summary>
		protected Vector3[] path;

		/// <summary>
		/// Will request a path from the actor to the destination
		/// </summary>
		public void RequestPath()
		{
			PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		}

		/// <summary>
		/// The call back to be run when a path is found
		/// </summary>
		/// <param name="newPath"></param>
		/// <param name="pathSuccessful"></param>
		public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				path = newPath;
				StopCoroutine("FollowPath");
				StartCoroutine("FollowPath");
			}
		}

		/// <summary>
		/// The corouting that makes the actor follow the path
		/// </summary>
		/// <returns></returns>
		IEnumerator FollowPath()
		{
			Debug.Log(path.Length);

			for (int i = 0; i < path.Length; i++)
			{
				transform.position = path[i] + new Vector3(0, .5f, 0);

				yield return new WaitForSeconds(TimeBetweenMoves);
			}
			yield break;
		}
	}
}