using UnityEditor;
using UnityEngine;

namespace Bipolar
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField]
        private bool visible;
        public bool Visible { get => visible; set => visible = value; }

        [SerializeField]
        private CursorLockMode cursorLockMode;
        public CursorLockMode CursorLockMode { get => cursorLockMode; set => cursorLockMode = value; }

        private void Update()
        {
            Cursor.visible = visible;
            Cursor.lockState = cursorLockMode;
        }
    }
}
