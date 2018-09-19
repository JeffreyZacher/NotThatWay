using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;

namespace Actor
{
	public class Unit : BaseActor
	{
		/// <summary>
		/// Addition to the time between movements
		/// </summary>
		[SerializeField]
		private float timeBetweenMovesModifier = 0;

		protected override float TimeBetweenMoves
		{
			get { return baseTimeBetwenMoves + timeBetweenMovesModifier; }
		}

		private void Start()
		{
			this.RequestPath();
		}
	}
}