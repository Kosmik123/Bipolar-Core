using UnityEngine;

namespace Bipolar.UI
{
	[CreateAssetMenu(menuName = CreateAssetPath.UI + "Choice Options")]
	public class ChoiceOptions : ScriptableObject
	{
		[SerializeField]
		private string[] options;

		[SerializeField]
		private bool isLooped;
		public bool IsLooped => isLooped;

		public int Count => options.Length;

		public string Get(int index) => options[index];
	}
}
