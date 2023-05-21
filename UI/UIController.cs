using UnityEngine;

namespace Bipolar.UI
{
    public abstract class UIController<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        protected T model;
    }
}
