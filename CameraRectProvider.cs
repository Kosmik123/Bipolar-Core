using UnityEngine;

namespace Bipolar
{
    public interface IRectProvider
    {
        Rect Rect { get; }
    }

    [RequireComponent(typeof(Camera))]
    public class CameraRectProvider : MonoBehaviour, IRectProvider
    {
        private Camera _camera;

        public Rect Rect
        {
            get
            {
                if (_camera == null)
                    _camera = GetComponent<Camera>();
                return new Rect(_camera.transform.position, 2 * _camera.orthographicSize * new Vector2(_camera.aspect, 1));
            }
        }
    }
}
