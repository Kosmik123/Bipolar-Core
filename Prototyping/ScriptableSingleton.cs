using UnityEngine;

namespace Bipolar.Prototyping
{
	public abstract class ScriptableSingleton<TSelf, TProvider> : ScriptableObject, ISingleton<TSelf>
		where TSelf : ScriptableSingleton<TSelf, TProvider>
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
			if (Application.isPlaying)
			{
				if (instance == null)
					instance = (TSelf)this;
				else if (instance != this)
					Destroy(this);
			}
		}

		protected virtual void OnDestroy()
		{
			if (instance == this)
				instance = null;
		}

		protected virtual void OnValidate()
		{
			if (name.Contains(GetType().Name) == false)
				name = GetType().Name;

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
			UnityEditor.EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;

			static void OnPlaymodeStateChanged(UnityEditor.PlayModeStateChange state)
			{
				instance = null;
			}
#endif
		}
	}

	public abstract class ScriptableSingleton<TSelf> : ScriptableSingleton<TSelf, DefaultScriptableSingletonProvider<TSelf>>
        where TSelf : ScriptableSingleton<TSelf>
    { }
}
