using System;
using UnityEngine;

// Token: 0x02000A18 RID: 2584
[Serializable]
public class PlayerPhysicsSurfaceProperties : ScriptableObject
{
	// Token: 0x04002922 RID: 10530
	public float SpeedMultiplier = 1f;

	// Token: 0x04002923 RID: 10531
	public float Slippiness;

	// Token: 0x04002924 RID: 10532
	public float Slidiness;

	// Token: 0x04002925 RID: 10533
	public bool UseOverrideGravityNormal;

	// Token: 0x04002926 RID: 10534
	public Vector3 OverrideGravityNormal;

	// Token: 0x04002927 RID: 10535
	public float GravityMultiplier = 1f;
}
