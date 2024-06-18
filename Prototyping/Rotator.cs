using UnityEngine;

namespace Bipolar.Prototyping
{
	public class Rotator : MonoBehaviour
	{
		[field: SerializeField]
		public Vector3 RotationSpeed { get; set; }

		private void Update()
		{
			float dt = Time.deltaTime;
			transform.Rotate(dt * RotationSpeed);
		}

		private void OnDrawGizmosSelected()
		{
			using (new GizmosSpace(transform))
			{
				Gizmos.color = new Color(1, 0.75f, 0, 1);
				var rotationAxis = RotationSpeed.normalized;
				Gizmos.DrawLine(rotationAxis, -rotationAxis);
			}
		}
	}

	public struct GizmosSpace : System.IDisposable
	{
		private Matrix4x4 previousMatrix;

		public GizmosSpace(Matrix4x4 matrix)
		{
			previousMatrix = Gizmos.matrix;
			Gizmos.matrix = matrix;
		}

		public GizmosSpace(Transform transform) : this(transform.localToWorldMatrix) { }

		public void Dispose()
		{
			Gizmos.matrix = previousMatrix;
		}
	}
}
