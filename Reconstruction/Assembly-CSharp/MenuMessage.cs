using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200026F RID: 623
public class MenuMessage : IUserInterface
{
	// Token: 0x06000F7D RID: 3965 RVA: 0x0002969F File Offset: 0x0002789F
	public void SetText(string text)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.MessageCor(text));
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x000296B5 File Offset: 0x000278B5
	private IEnumerator MessageCor(string content)
	{
		this.Show();
		this.messageTxt.text = content;
		yield return new WaitForSeconds(3f);
		this.Hide();
		yield break;
	}

	// Token: 0x040007E8 RID: 2024
	[SerializeField]
	private Text messageTxt;
}
