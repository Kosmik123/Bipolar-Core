using Bipolar.Subcomponents;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ExampleSubcomponentBase : SubBehavior
{
	[SerializeField]
	private string name;
}

public class CompoundExample : CompoundBehavior<ExampleSubcomponentBase>
{
	[SerializeField]
	public int number;

	[ContextMenu("Set")]
	private void Set()
	{
		subcomponents.Add(new ExampleSubcomponentA());
	}
}

public class ExampleSubcomponentA : ExampleSubcomponentBase
{
	public float power;
}

public class ExampleSubcomponentB : ExampleSubcomponentBase
{
	public int health;
}
