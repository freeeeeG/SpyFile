using System;
using UnityEngine;

// Token: 0x02000577 RID: 1399
[AddComponentMenu("Scripts/Game/Environment/AutoWorkstation")]
public class AutoWorkstation : MonoBehaviour
{
	// Token: 0x040014E9 RID: 5353
	[SerializeField]
	[Range(1f, 16f)]
	public int m_choppingPower = 8;

	// Token: 0x040014EA RID: 5354
	[SerializeField]
	public string m_workTrigger = string.Empty;

	// Token: 0x040014EB RID: 5355
	[SerializeField]
	public string m_workFinishedTrigger = string.Empty;

	// Token: 0x040014EC RID: 5356
	[SerializeField]
	public GameObject m_workFinishedTarget;
}
