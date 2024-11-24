using Bipolar.Subcomponents;
using UnityEngine;

    [System.Serializable]
    public class ExampleSubcomponent : ISubcomponent
    {

    }

    public class CompoundExample : CompoundBehavior<ExampleSubcomponent>
    {
        [SerializeField]
        public int number;
    }
