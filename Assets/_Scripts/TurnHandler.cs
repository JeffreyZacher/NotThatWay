using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;

namespace Handler
{
	namespace TurnedBasedHandler
	{
		public class NewBehaviourScript : MonoBehaviour
		{
			private List<BaseActor> actors;


			IEnumerator ProcessTurn()
			{
				foreach(var actor in actors)
				{
					var foo = actor.target;
					actor.RequestPath();

					yield return new WaitForSeconds(5f);
				}
				yield return null;
			}
		}
	}
}