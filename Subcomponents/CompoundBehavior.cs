using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Subcomponents 
{
    public interface ISubcomponent
    {

    }

    public interface ICompoundBehavior
    {

    }

    public class CompoundBehavior<T> : MonoBehaviour, ICompoundBehavior
        where T : ISubcomponent
    {
        [SerializeReference]
        //[HideInInspector]
        internal List<T> subcomponents = new List<T>();

        public void AddSubcomponent(T component)
        {
            subcomponents.Add(component);
        }

        public T AddSubcomponent<TComponent>()
            where TComponent : T, new()
        {
            var component = new TComponent();
            AddSubcomponent(component);
            return component;
        }

        public void RemoveSubcomponent(T component)
        {
            subcomponents.Remove(component);
        }

        public bool TryGetSubcomponent<TComponent>(out TComponent component)
            where TComponent : T
        {
            component = default;
            for (int i = 0; i < subcomponents.Count; i++)
            {
                if (subcomponents[i] is TComponent correctType)
                {
                    component = correctType;
                    return true;
                }
            }
            return false;
        }
    }
}
