using UnityEngine;

namespace Bipolar
{
    [System.Serializable]
    public class Serialized<TInterface>
        where TInterface : class
    {
        [SerializeField]
        private Object serializedObject;

        private TInterface _value;
        public TInterface Value
        {
            get
            {
                _value ??= serializedObject as TInterface;
                return _value;
            }
            set
            {
                if (!(value is Object @object))
                    throw new System.InvalidCastException();

                serializedObject = @object;
                _value = value;
            }
        }

        public System.Type Type => typeof(TInterface);

        public static implicit operator TInterface(Serialized<TInterface> iface) => iface.Value;
    }
}
