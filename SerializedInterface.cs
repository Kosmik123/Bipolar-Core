using UnityEngine;

namespace Bipolar
{
    [System.Serializable]
    public class SerializedInterface<TInterface>
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
                else
                {
                    serializedObject = @object;
                    _value = value;
                }
            }
        }

        internal System.Type Type => typeof(TInterface);

        public static implicit operator TInterface (SerializedInterface<TInterface> iface) => iface.Value;
    }
}
