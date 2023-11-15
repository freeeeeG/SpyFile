using System;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class LuckProgress : MonoBehaviour
{
	// Token: 0x06000F5D RID: 3933 RVA: 0x00028F00 File Offset: 0x00027100
	public void SetProgress(int value)
	{
		for (int i = 0; i < this.luckSlot.Length; i++)
		{
			if (i < value)
			{
				this.luckSlot[i].SetActive(true);
			}
			else
			{
				this.luckSlot[i].SetActive(false);
			}
		}
	}

	// Token: 0x040007C4 RID: 1988
	[SerializeField]
	private GameObject[] luckSlot;
}
