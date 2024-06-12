using UnityEngine;

namespace Bipolar
{
    public interface IRectProvider
    {
        Rect Rect { get; }
    }

    [RequireComponent(typeof(Camera))]
    public sealed class CameraRectProvider : MonoBehaviour, IRectProvider
    {
        private Camera _camera;

        public static Rect GetRect(Camera camera) => 
            new Rect(camera.transform.position, 2 * camera.orthographicSize * new Vector2(camera.aspect, 1));

        public Rect Rect
        {
            get
            {
                if (_camera == null)
                    _camera = GetComponent<Camera>();
                return GetRect(_camera);
            }
        }
    }

    public static class CameraRectExtension
    {
        public static Rect GetRect(this Camera camera) => CameraRectProvider.GetRect(camera);
    }
}


