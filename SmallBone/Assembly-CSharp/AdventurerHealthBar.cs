using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class AdventurerHealthBar : MonoBehaviour
{
	// Token: 0x060002D0 RID: 720 RVA: 0x0000B1F5 File Offset: 0x000093F5
	public void ShowDeadPortrait()
	{
		this._deadPortrait.SetActive(true);
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0000B203 File Offset: 0x00009403
	public void HideDeadPortrait()
	{
		this._deadPortrait.SetActive(false);
	}

	// Token: 0x04000257 RID: 599
	[SerializeField]
	private GameObject _deadPortrait;
}
