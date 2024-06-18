using UnityEngine;

namespace Bipolar.Prototyping
{
    public abstract class SceneSingleton<TSelf> : MonoBehaviour 
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

    public abstract class SelfCreatingSingleton<TSelf> : MonoBehaviour
		where TSelf : SelfCreatingSingleton<TSelf>
    {
        private static TSelf instance;
        public static TSelf Instance
        {
            get
            {
                if (instance == null)
                    new GameObject(typeof(TSelf).Name).AddComponent<TSelf>();
                return instance;
            }
            set
            {
                instance = value;
            }
        }

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
