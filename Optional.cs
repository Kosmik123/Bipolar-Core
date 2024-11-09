using UnityEngine;

namespace Bipolar
{
    [System.Serializable]
	public class Optional<T>
	{
		[SerializeField]
		private bool hasValue;
        public bool HasValue
        {
            get => hasValue;
            set => hasValue = value;
        }

        [SerializeField]
		private T value;
		public T Value
		{
			get => value;
			set => this.value = value;
		}

		public Optional(T value)
		{
			this.hasValue = true;
			this.value = value;
		}

		public static implicit operator T(Optional<T> optional) => optional.value;
		public static implicit operator Optional<T>(T value) => new Optional<T>(value);	

		public T GetValue(T defaultValue) => hasValue ? value : defaultValue;
	}
}
