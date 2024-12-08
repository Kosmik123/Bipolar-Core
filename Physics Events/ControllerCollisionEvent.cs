using System;
using UnityEngine;

namespace Bipolar.PhysicsEvents
{
	public class ControllerCollisionEvent : CollisionEvent<ControllerColliderHit>
	{
		protected override GameObject GetGameObject(ControllerColliderHit data) => data.gameObject;

		private void OnControllerColliderHit(ControllerColliderHit hit) => TryInvokeEvent(hit);
	}
}
