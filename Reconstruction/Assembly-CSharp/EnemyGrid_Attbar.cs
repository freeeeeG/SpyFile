using System;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class EnemyGrid_Attbar : MonoBehaviour
{
	// Token: 0x06001020 RID: 4128 RVA: 0x0002B334 File Offset: 0x00029534
	public void SetAtt(int att)
	{
		for (int i = 0; i < this.energys.Length; i++)
		{
			this.energys[i].SetActive(i < att);
		}
	}

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private GameObject[] energys;
}
