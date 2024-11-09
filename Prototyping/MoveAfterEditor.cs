using UnityEngine;

namespace Bipolar.Prototyping
{
#if UNITY_EDITOR
	public class MoveAfterEditor : MonoBehaviour
	{
		private void Update()
		{
			var view = UnityEditor.SceneView.lastActiveSceneView;
			if (view != null)
			{
				transform.position = view.pivot;
			}
		}
	}
#endif
}
