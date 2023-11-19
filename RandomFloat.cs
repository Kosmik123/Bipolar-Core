using UnityEngine;

namespace Bipolar
{
    [System.Serializable]
    public struct RandomFloat
    {
        public float min;
        public float max;

        public static implicit operator float(RandomFloat self) => Random.Range(self.min, self.max);
        public static implicit operator RandomFloat(float value) => new RandomFloat() { min = value, max = value }; 
        public static implicit operator RandomFloat((float min, float max) value) => new RandomFloat() { min = value.min, max = value.max}; 
    }
}
