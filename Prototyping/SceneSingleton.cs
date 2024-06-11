using UnityEngine;

namespace Bipolar.Prototyping
{
    public class SceneSingleton<TSelf> : MonoBehaviour 
        where TSelf : SceneSingleton<TSelf>
    {
        public static TSelf Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null) 
            {
                Instance = (TSelf)this;
            }
            else if (Instance != this)
            {
                Destroy(this);
            }
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}
