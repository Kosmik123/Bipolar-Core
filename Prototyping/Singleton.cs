using Codice.Client.Common;
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
                    instance = new GameObject(typeof(TSelf).Name).AddComponent<TSelf>();
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
                instance = (TSelf)this;
            else if (instance != this)
                Destroy(this);
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }

    public abstract class ScriptableSingleton<TSelf> : ScriptableObject
        where TSelf : ScriptableSingleton<TSelf>
    {
        private static TSelf instance;
        public static TSelf Instance
        {
            get
            {
                if (instance == null)
                {
                    var all = Resources.FindObjectsOfTypeAll<TSelf>();
                    if (all != null && all.Length > 0)
                    {
                        instance = all[0];
                    }
                    else
                    {
                        instance = CreateInstance<TSelf>();
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (instance == null)
                    instance = (TSelf)this;
                else if (instance != this)
                    Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
