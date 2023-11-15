using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200023B RID: 571
public class ConfirmPanel : IUserInterface
{
	// Token: 0x06000EB4 RID: 3764 RVA: 0x00025FBF File Offset: 0x000241BF
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00025FD3 File Offset: 0x000241D3
	public void SetContent(string mainTxt, string confirmTxt, string cancelTxt, UnityAction callback)
	{
		this.MainTxt.text = mainTxt;
		this.ConfirmBtnTxt.text = confirmTxt;
		this.CancelBtnTxt.text = cancelTxt;
		this.ConfirmBtnTxt.GetComponent<Button>().onClick.AddListener(callback);
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00026010 File Offset: 0x00024210
	public override void Show()
	{
		base.Show();
		this.m_Anim.SetBool("Show", true);
	}

	// Token: 0x04000715 RID: 1813
	private Animator m_Anim;

	// Token: 0x04000716 RID: 1814
	[SerializeField]
	private Text MainTxt;

	// Token: 0x04000717 RID: 1815
	[SerializeField]
	private Text ConfirmBtnTxt;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private Text CancelBtnTxt;
}
