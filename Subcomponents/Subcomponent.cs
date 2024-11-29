using UnityEngine;

namespace Bipolar.Subcomponents
{
	public interface ISubcomponent
	{
		
	}

	public interface ISubBehavior : ISubcomponent
	{
		bool IsEnabled { get; set; }
	}

	public interface IUpdatable : ISubcomponent
	{
		void Update();
	}

	public interface IEnableCallbackReceiver : ISubcomponent
	{
		void OnEnable();
	}

	public interface IDisableCallbackReceiver : ISubcomponent
	{
		void OnDisable();
	}

	public static class SubcomponentsExtensions
	{
		public static bool IsActiveAndEnabled(this ISubcomponent subcomponent) => default;	
	}
}
