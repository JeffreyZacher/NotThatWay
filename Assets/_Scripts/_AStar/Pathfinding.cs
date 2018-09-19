using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AStar
{
	public class Pathfinding : MonoBehaviour
	{

		PathRequestManager requestManager;
		Grid1 grid;

		void Awake()
		{
			requestManager = GetComponent<PathRequestManager>();
			grid = GetComponent<Grid1>();
		}

		public void StartFindPath(Vector3 startPos, Vector3 targetPos)
		{
			StartCoroutine(FindPath(startPos, targetPos));
		}

		IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
		{
			Vector3[] waypoints = new Vector3[0];
			bool pathSuccess = false;

			Node startNode = grid.NodeFromWorldPoint(startPos);
			Node targetNode = grid.NodeFromWorldPoint(targetPos);

			if (startNode.walkable && targetNode.walkable)
			{
				Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
				HashSet<Node> closedSet = new HashSet<Node>();
				openSet.Add(startNode);

				while (openSet.Count > 0)
				{
					Node currentNode = openSet.RemoveFirst();

					closedSet.Add(currentNode);

					if (currentNode == targetNode)
					{
						pathSuccess = true;
						break;
					}

					foreach (Node neighbour in grid.Getneighbors(currentNode))
					{
						if (!neighbour.walkable || closedSet.Contains(neighbour))
						{
							continue;
						}

						int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbour);
						if (newMovementCostToNeighbor < neighbour.gCost || !openSet.Contains(neighbour))
						{
							neighbour.gCost = newMovementCostToNeighbor;
							neighbour.hCost = GetDistance(neighbour, targetNode);
							neighbour.parent = currentNode;

							if (!openSet.Contains(neighbour))
							{
								openSet.Add(neighbour);
							}
							else
							{
								openSet.UpdateItem(neighbour);
							}
						}
					}
				}
			}
			yield return null;
			if (pathSuccess)
			{
				Debug.Log("Path successful");
				waypoints = RetracePath(startNode, targetNode);
			}
			requestManager.FinishedProcessingPath(waypoints, pathSuccess);
		}

		Vector3[] RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}

			Vector3[] waypoint = SimplyPath(path, true);
			Array.Reverse(waypoint);
			return waypoint;
		}

		/// <summary>
		/// Will turn given path into a simple list of waypoints to follow
		/// </summary>
		/// <param name="path">a list of nodes to traverse</param>
		/// <returns></returns>
		Vector3[] SimplyPath(List<Node> path, bool fullpath = false)
		{
			List<Vector3> waypoints = new List<Vector3>();
			Vector2 directionOld = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
				if (directionNew != directionOld && fullpath)
				{
					waypoints.Add(path[i].worldPosition);
					directionOld = directionNew;
				}
				else if (fullpath)
				{
					waypoints.Add(path[i].worldPosition);
				}
			}

			return waypoints.ToArray();
		}


		int GetDistance(Node nodeA, Node nodeB)
		{
			int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
			int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}
	}
}