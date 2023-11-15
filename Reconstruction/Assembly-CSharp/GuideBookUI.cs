using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000246 RID: 582
public class GuideBookUI : IUserInterface
{
	// Token: 0x06000EF0 RID: 3824 RVA: 0x00027AA3 File Offset: 0x00025CA3
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
		this.ShowPage(0);
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x00027ABE File Offset: 0x00025CBE
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("isOpen", true);
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x00027AD7 File Offset: 0x00025CD7
	public void ShowPage(int index)
	{
		this.tabs[index].isOn = true;
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x00027AE7 File Offset: 0x00025CE7
	public override void Hide()
	{
		this.anim.SetBool("isOpen", false);
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.GuideBookContinue);
	}

	// Token: 0x04000760 RID: 1888
	[SerializeField]
	private Toggle[] tabs;

	// Token: 0x04000761 RID: 1889
	private Animator anim;
}
