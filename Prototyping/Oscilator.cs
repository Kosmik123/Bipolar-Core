using UnityEngine;

namespace Bipolar.Prototyping
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

        private float time;

        private void Update()
        {
            time += Time.deltaTime;
            transform.localPosition = CalculatePosition(time);
        }

        private Vector3 CalculatePosition(float time) => CalculatePosition(Amplitude, Offset, Frequency, Phase, time);

        public static Vector3 CalculatePosition(Vector3 amplitude, Vector3 offset, Vector3 frequency, Vector3 phase, float time)
        {
            var position = new Vector3(
                Mathf.Sin(frequency.x * Mathf.PI * time + phase.x),
                Mathf.Sin(frequency.y * Mathf.PI * time + phase.y),
                Mathf.Sin(frequency.z * Mathf.PI * time + phase.z));

            position.Scale(amplitude);
            position += offset;
            return position;
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            var matrix = Gizmos.matrix;
            if (transform.parent)
				Gizmos.matrix = transform.parent.localToWorldMatrix;

            Gizmos.color = Color.yellow;
            float time = Application.isPlaying ? Time.time : (float)UnityEditor.EditorApplication.timeSinceStartup;
			Gizmos.DrawSphere(CalculatePosition(time), 0.1f);
            Gizmos.matrix = matrix;
#endif
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Oscilator))]
    public class OscillatorEditor : UnityEditor.Editor
    {
        private const float preferedTimeDelta = 0.05f;

        public void OnSceneGUI()
        {
            var transform = ((Component)target).transform;
            var frequencies = serializedObject.FindProperty("frequency").vector3Value;

            float frequency = Gcd3(frequencies.x, frequencies.y, frequencies.z);
            float period = 2f / frequency;
            int resolution = Mathf.FloorToInt(period / preferedTimeDelta);
            float dt = period / resolution;

            var amplitude = serializedObject.FindProperty("amplitude").vector3Value;
            var offset = serializedObject.FindProperty("offset").vector3Value;
            var phase = serializedObject.FindProperty("phase").vector3Value;

            UnityEditor.Handles.color = Color.yellow;
            var matrix = UnityEditor.Handles.matrix;
			if (transform.parent)
				UnityEditor.Handles.matrix = transform.parent.localToWorldMatrix;
			var previousPosition = Oscilator.CalculatePosition(amplitude, offset, frequencies, phase, 0);
            for (int i = 1; i <= resolution; i++)
            {
                var localPosition = Oscilator.CalculatePosition(amplitude, offset, frequencies, phase, i * dt);
                UnityEditor.Handles.DrawLine(previousPosition, localPosition);
                previousPosition = localPosition;
            }
            UnityEditor.Handles.matrix = matrix;
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
