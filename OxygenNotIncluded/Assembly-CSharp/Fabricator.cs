using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005FB RID: 1531
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Fabricator")]
public class Fabricator : KMonoBehaviour
{
	// Token: 0x0600265F RID: 9823 RVA: 0x000D0943 File Offset: 0x000CEB43
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}
}
