// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class WeaponCategory
{
	private string _id_k__BackingField;

	private float _reach_k__BackingField;

	private bool _twoHanded_k__BackingField;

	private bool _dualWield_k__BackingField;

	private List<HitVector> _hitVectors_k__BackingField;

	private List<Weapon> _weapons_k__BackingField;

	public int index;

	public int weaponCount;

	public string id
	{
		get;
		set;
	}

	public float reach
	{
		get;
		set;
	}

	public bool twoHanded
	{
		get;
		set;
	}

	public bool dualWield
	{
		get;
		set;
	}

	public List<HitVector> hitVectors
	{
		get;
		set;
	}

	public List<Weapon> weapons
	{
		get;
		set;
	}
}
