using UnityEngine;

namespace Bipolar.UI.Controls
{
    [CreateAssetMenu(menuName = "Bipolar/UI/Choice Options")]
    public class ChoiceOptions : ScriptableObject
    {
        [SerializeField]
        private string[] values;

        public int Count => values.Length;

        public string Get(int index) => values[index];
    }
}
