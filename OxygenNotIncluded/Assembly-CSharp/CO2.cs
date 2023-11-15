using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006C2 RID: 1730
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
	// Token: 0x06002F1C RID: 12060 RVA: 0x000F8626 File Offset: 0x000F6826
	public void StartLoop()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play("exhale_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Play("exhale_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x000F8663 File Offset: 0x000F6863
	public void TriggerDestroy()
	{
		base.GetComponent<KBatchedAnimController>().Play("exhale_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001BE6 RID: 7142
	[Serialize]
	[NonSerialized]
	public Vector3 velocity = Vector3.zero;

	// Token: 0x04001BE7 RID: 7143
	[Serialize]
	[NonSerialized]
	public float mass;

	// Token: 0x04001BE8 RID: 7144
	[Serialize]
	[NonSerialized]
	public float temperature;

	// Token: 0x04001BE9 RID: 7145
	[Serialize]
	[NonSerialized]
	public float lifetimeRemaining;
}
