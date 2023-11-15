using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000284 RID: 644
public class MessageUI : IUserInterface
{
	// Token: 0x06000FF6 RID: 4086 RVA: 0x0002ABCF File Offset: 0x00028DCF
	public void SetText(string text)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.MessageCor(text));
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0002ABE5 File Offset: 0x00028DE5
	private IEnumerator MessageCor(string content)
	{
		this.Show();
		this.messageTxt.text = content;
		yield return new WaitForSeconds(3f);
		this.Hide();
		yield break;
	}

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	private Text messageTxt;
}
