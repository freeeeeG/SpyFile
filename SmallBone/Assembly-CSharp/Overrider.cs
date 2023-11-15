using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class Overrider : MonoBehaviour
{
	// Token: 0x04000068 RID: 104
	[SerializeField]
	private MonoBehaviour _target;

	// Token: 0x04000069 RID: 105
	[SerializeField]
	[HideInInspector]
	private bool[] _overrides;

	// Token: 0x0400006A RID: 106
	[SerializeField]
	private object[] _objects = new object[]
	{
		1,
		3,
		5
	};
}
