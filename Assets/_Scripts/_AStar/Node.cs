﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
	public class Node : IHeapItem<Node>
	{

		public bool walkable;
		public Vector3 worldPosition;
		public int gridX;
		public int gridY;

		public int gCost;
		public int hCost;
		public Node parent;
		int heapIndex;

		public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
		{
			walkable = _walkable;
			worldPosition = _worldPosition;
			gridX = _gridX;
			gridY = _gridY;
		}

		public int fCost
		{
			get { return gCost + hCost; }
		}

		public int HeapIndex
		{
			get
			{
				return heapIndex;
			}

			set
			{
				heapIndex = value;
			}
		}

		public int CompareTo(Node other)
		{
			int compare = this.fCost.CompareTo(other.fCost);
			if (compare == 0)
			{
				compare = this.hCost.CompareTo(other.hCost);
			}
			return -compare;
		}
	}
}