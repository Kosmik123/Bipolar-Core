using UnityEngine;

namespace Bipolar.Subcomponents
{
	public interface ISubcomponent
	{ }

	public abstract class SubBehavior : ISubcomponent, ISubBehavior 
	{
		[field: SerializeField, HideInInspector]
		public virtual bool IsEnabled { get; set; }
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

	internal interface ISubBehavior
	{
		bool IsEnabled { get; set; }
	}
}
