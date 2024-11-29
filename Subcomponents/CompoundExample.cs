﻿using Bipolar.Subcomponents;
using UnityEngine;

[System.Serializable]
public class ExampleSubcomponent : ISubBehavior
{
	public int index;
	public string name;

	public bool IsEnabled { get; set; }
}

public class CompoundExample : CompoundBehavior<ExampleSubcomponent>
{
	[SerializeField]
	public int number;
}
