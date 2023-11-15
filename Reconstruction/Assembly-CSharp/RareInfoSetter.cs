using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class RareInfoSetter : MonoBehaviour
{
	// Token: 0x06001040 RID: 4160 RVA: 0x0002BDDE File Offset: 0x00029FDE
	private void Start()
	{
		this.rareInfoBtn.SetContent(GameMultiLang.GetTraduction("RARE"));
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x0002BDF8 File Offset: 0x00029FF8
	public void SetRare(int quality)
	{
		if (quality > 0)
		{
			base.gameObject.SetActive(true);
			GameObject[] array = this.rareSlots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			for (int j = 0; j < quality; j++)
			{
				this.rareSlots[j].SetActive(true);
			}
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000887 RID: 2183
	[SerializeField]
	private GameObject[] rareSlots;

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	private InfoBtn rareInfoBtn;
}
