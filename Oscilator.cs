using UnityEngine;

namespace Bipolar
{
    public class Oscilator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 amplitude;
        private Vector3 Amplitude { get => amplitude; set => amplitude = value; }

        [SerializeField]
        private Vector3 offset;
        private Vector3 Offset { get => offset; set => offset = value; }

        [SerializeField]
        private Vector3 frequency;
        private Vector3 Frequency
        {
            get => frequency;
            set => frequency = value;
        }

        [SerializeField]
        private Vector3 phase;
        private Vector3 Phase { get => phase; set => phase = value; }

        private void Update()
        {
            float time = Time.time;
            transform.position = CalucalatePosition(time);
        }

        private Vector3 CalucalatePosition(float time) => CalculatePosition(Amplitude, Offset, Frequency, Phase, time);

        public static Vector3 CalculatePosition(Vector3 Amplitude, Vector3 Offset, Vector3 Frequency, Vector3 Phase, float time)
        {
            var position = new Vector3(
                Mathf.Sin(Frequency.x * Mathf.PI * (time + Phase.x)),
                Mathf.Sin(Frequency.y * Mathf.PI * (time + Phase.y)),
                Mathf.Sin(Frequency.z * Mathf.PI * (time + Phase.z)));
            position.Scale(Amplitude);
            position += Offset;
            return position;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Oscilator))]
    public class OscillatorEditor : UnityEditor.Editor
    {
        private const float preferedTimeDelta = 0.05f;

        public void OnSceneGUI()
        {
            var frequencies = serializedObject.FindProperty("frequency").vector3Value;

            float frequency = Gcd3(frequencies.x, frequencies.y, frequencies.z);
            float period = 2f / frequency;
            int resolution = Mathf.FloorToInt(period / preferedTimeDelta);
            float dt = period / resolution;

            var amplitude = serializedObject.FindProperty("amplitude").vector3Value;
            var offset = serializedObject.FindProperty("offset").vector3Value;
            var phase = serializedObject.FindProperty("phase").vector3Value;
            
            UnityEditor.Handles.color = Color.yellow;
            var previousPosition = Oscilator.CalculatePosition(amplitude, offset, frequencies, phase, 0);
            for (int i = 1; i <= resolution; i++)
            {
                var position = Oscilator.CalculatePosition(amplitude, offset, frequencies, phase, i * dt);
                UnityEditor.Handles.DrawLine(previousPosition, position);
                previousPosition = position;
            }
        }

        private static float Gcd(float a, float b, float maxError)
        {
            if (a < b)
                return Gcd(b, a, maxError);

            if (Mathf.Abs(b) < maxError)
                return a;

            return Gcd(b, a - Mathf.Floor(a / b) * b, maxError);
        }

        private static float Gcd3(float a, float b, float c, float maxError = 0.001f)
        {
            return Gcd(c, Gcd(b, a, maxError), maxError);
        }
    }
#endif
}
