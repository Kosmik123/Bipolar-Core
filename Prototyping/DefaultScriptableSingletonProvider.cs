using UnityEngine;

namespace Bipolar.Prototyping
{
	public sealed class DefaultScriptableSingletonProvider<TSingleton> : ISingletonProvider<TSingleton>
        where TSingleton : Object, ISingleton<TSingleton>
	{
		public TSingleton Get()
		{
			if (TryLoadFirst(out var instance) != false)
				return instance;
			
            var all = Resources.LoadAll<TSingleton>(string.Empty);
			if (all != null && all.Length > 0)
				return all[0];

            return null;
		}

		private static bool TryLoadFirst(out TSingleton found)
		{
			found = Resources.Load<TSingleton>(typeof(TSingleton).Name);
			return found;
		}
	}
}
