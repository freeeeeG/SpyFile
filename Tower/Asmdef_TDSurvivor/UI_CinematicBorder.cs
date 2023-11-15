using System;

// Token: 0x02000157 RID: 343
public class UI_CinematicBorder : APopupWindow
{
	// Token: 0x060008FA RID: 2298 RVA: 0x000220B7 File Offset: 0x000202B7
	protected override void ShowWindowProc()
	{
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000220CA File Offset: 0x000202CA
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x000220DD File Offset: 0x000202DD
	private new void Start()
	{
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x000220DF File Offset: 0x000202DF
	private void Update()
	{
	}
}
