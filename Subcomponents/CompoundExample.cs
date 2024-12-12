using Bipolar.Subcomponents;
using UnityEngine;

[System.Serializable]
public class ExampleSubcomponentBase : SubBehavior
{
	[SerializeField]
	private string name;
}

[AddComponentMenu("Subcomponent Category/Compound Example")]
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

[AddComponentMenu("Category/Subcomponent A", 10)]
public class ExampleSubcomponentA : ExampleSubcomponentBase
{
	public float power;
}

public class ExampleSubcomponentB : ExampleSubcomponentBase
{
	public int health;
}

[AddComponentMenu("Category/SubCateee/Subcomponent C")]
public class ExampleSubcomponentC : ExampleSubcomponentBase
{
	public float power;
}

[AddComponentMenu("Category/Subcomponent D", 0)]
public class ExampleSubcomponentD : ExampleSubcomponentBase
{
	public int health;
}
