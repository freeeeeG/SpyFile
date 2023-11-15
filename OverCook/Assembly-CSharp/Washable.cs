using System;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class Washable : MonoBehaviour
{
	// Token: 0x06001DF2 RID: 7666 RVA: 0x000912A8 File Offset: 0x0008F6A8
	public int GetWashTimeMultiplier()
	{
		return 1;
	}

	// Token: 0x04001716 RID: 5910
	[SerializeField]
	public ProgressUIController m_progressUIPrefab;

	// Token: 0x04001717 RID: 5911
	[SerializeField]
	public int m_duration = 1;
}
