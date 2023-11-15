using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A0E RID: 2574
[SerializationConfig(MemberSerialization.OptOut)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGround")]
public class UnstableGround : KMonoBehaviour
{
	// Token: 0x0400323B RID: 12859
	public SimHashes element;

	// Token: 0x0400323C RID: 12860
	public float mass;

	// Token: 0x0400323D RID: 12861
	public float temperature;

	// Token: 0x0400323E RID: 12862
	public byte diseaseIdx;

	// Token: 0x0400323F RID: 12863
	public int diseaseCount;
}
