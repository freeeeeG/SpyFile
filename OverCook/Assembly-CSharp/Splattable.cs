using System;
using UnityEngine;

// Token: 0x02000816 RID: 2070
public class Splattable : MonoBehaviour
{
	// Token: 0x060027A4 RID: 10148 RVA: 0x000BA4DB File Offset: 0x000B88DB
	private void Awake()
	{
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04001F27 RID: 7975
	[SerializeField]
	public GameObject[] m_splatPrefab;

	// Token: 0x04001F28 RID: 7976
	[SerializeField]
	public bool m_alignToGrid = true;

	// Token: 0x04001F29 RID: 7977
	public int m_prefabIndex;
}
