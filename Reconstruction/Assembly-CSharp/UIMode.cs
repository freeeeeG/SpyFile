using System;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class UIMode : IUserInterface
{
	// Token: 0x06000FE0 RID: 4064 RVA: 0x0002A878 File Offset: 0x00028A78
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
		this.m_UIStandardMode.Initialize();
		this.m_UIEndlessMode.Initialize();
		this.m_UIChallengeMode.Initialize();
		this.m_UIStandardMode.gameObject.SetActive(true);
		this.m_UIEndlessMode.gameObject.SetActive(false);
		this.m_UIChallengeMode.gameObject.SetActive(false);
		this.Hide();
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x0002A8F1 File Offset: 0x00028AF1
	public override void Show()
	{
		base.Show();
		this.m_Anim.SetBool("OpenLevel", true);
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x0002A90A File Offset: 0x00028B0A
	public override void ClosePanel()
	{
		this.m_Anim.SetBool("OpenLevel", false);
		Singleton<MenuManager>.Instance.ShowMenu();
	}

	// Token: 0x04000837 RID: 2103
	private Animator m_Anim;

	// Token: 0x04000838 RID: 2104
	[SerializeField]
	private UIStandardMode m_UIStandardMode;

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	private UIEndlessMode m_UIEndlessMode;

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private UIChallengeMode m_UIChallengeMode;
}
