using UnityEngine;

namespace Bipolar.Prototyping
{
	public interface ISingletonProvider<TSingleton> 
		where TSingleton : ISingleton<TSingleton> 
	{
		public TSingleton Get();
	}

	public interface ISingleton<TSelf> 
		where TSelf : ISingleton<TSelf>
	{ }

	public abstract class SceneSingleton<TSelf, TProvider> : MonoBehaviour, ISingleton<TSelf>
        where TSelf : SceneSingleton<TSelf, TProvider>
        where TProvider : ISingletonProvider<TSelf>, new()
    {
		private static readonly TProvider instanceProvider = new TProvider();

		private static TSelf instance;
        public static TSelf Instance
        {
            get
            {
                if (instance == null)
                    instance = instanceProvider.Get();
                return instance;
            }
        }

		protected virtual void Awake()
		{
			if (Instance == null)
			    instance = (TSelf)this;
			else if (Instance != this)
				Destroy(this);
		}

		protected virtual void OnDestroy()
		{
            if (instance == this)
    			instance = null;
		}
	}

	public sealed class NoneInstanceProvider<TSingleton> : ISingletonProvider<TSingleton>
		where TSingleton : ISingleton<TSingleton>
	{
		public TSingleton Get() => default;
	}

	public abstract class SceneSingleton<TSelf> : SceneSingleton<TSelf, NoneInstanceProvider<TSelf>>
        where TSelf : SceneSingleton<TSelf>
    { }

	public sealed class CreatingComponentInstanceProvider<TSingleton> : ISingletonProvider<TSingleton>
		where TSingleton : SceneSingleton<TSingleton, CreatingComponentInstanceProvider<TSingleton>>
	{
		public TSingleton Get() => new GameObject(typeof(TSingleton).Name).AddComponent<TSingleton>();
	}

	public abstract class SelfCreatingSingleton<TSelf> : SceneSingleton<TSelf, CreatingComponentInstanceProvider<TSelf>>
        where TSelf : SelfCreatingSingleton<TSelf>
    { }
}
