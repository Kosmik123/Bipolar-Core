using UnityEngine;

namespace Bipolar
{
    [System.Serializable]
    public struct RandomFloat
    {
        public float min;
        public float max;
        public readonly float Value => Random.Range(min, max);

        public RandomFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public static implicit operator float(RandomFloat self) => self.Value;

        public static implicit operator RandomFloat(float value) => new RandomFloat() { min = value, max = value }; 
        public static implicit operator RandomFloat((float min, float max) value) => new RandomFloat() { min = value.min, max = value.max}; 
    }

    [System.Serializable]
    public struct RandomInt
    {
        public int min;
        public int max;
        public readonly int Value => Random.Range(min, max);

        public RandomInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public static implicit operator int(RandomInt self) => self.Value;

        public static implicit operator RandomInt(int value) => new RandomInt() { min = value, max = value };
        public static implicit operator RandomInt((int min, int max) value) => new RandomInt() { min = value.min, max = value.max };
    }
}
