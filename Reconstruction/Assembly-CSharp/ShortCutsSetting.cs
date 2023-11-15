using System;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class ShortCutsSetting : MonoBehaviour
{
	// Token: 0x0600100B RID: 4107 RVA: 0x0002AFE2 File Offset: 0x000291E2
	public void Initialize()
	{
		this.SetAllContents();
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0002AFEC File Offset: 0x000291EC
	private void SetAllContents()
	{
		KeyBingdingSetter[] setters = this.m_Setters;
		for (int i = 0; i < setters.Length; i++)
		{
			setters[i].SetContent();
		}
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0002B016 File Offset: 0x00029216
	public void ResetAllKey()
	{
		Singleton<InputManager>.Instance.ResetAllKeys();
		this.SetAllContents();
	}

	// Token: 0x04000853 RID: 2131
	[SerializeField]
	private KeyBingdingSetter[] m_Setters;
}
