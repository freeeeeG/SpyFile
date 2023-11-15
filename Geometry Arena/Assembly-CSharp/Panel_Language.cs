using System;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class Panel_Language : MonoBehaviour
{
	// Token: 0x0600066C RID: 1644 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00008879 File Offset: 0x00006A79
	public void Open()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00024CCB File Offset: 0x00022ECB
	public void Button_Close()
	{
		this.Close();
		MainCanvas.inst.OpenPanel_WordsFromDeveloper();
	}
}
