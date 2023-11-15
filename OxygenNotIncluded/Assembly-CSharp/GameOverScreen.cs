using System;

// Token: 0x02000B06 RID: 2822
public class GameOverScreen : KModalScreen
{
	// Token: 0x06005711 RID: 22289 RVA: 0x001FD1AE File Offset: 0x001FB3AE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x06005712 RID: 22290 RVA: 0x001FD1BC File Offset: 0x001FB3BC
	private void Init()
	{
		if (this.QuitButton)
		{
			this.QuitButton.onClick += delegate()
			{
				this.Quit();
			};
		}
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x06005713 RID: 22291 RVA: 0x001FD211 File Offset: 0x001FB411
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x06005714 RID: 22292 RVA: 0x001FD218 File Offset: 0x001FB418
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x04003AAF RID: 15023
	public KButton DismissButton;

	// Token: 0x04003AB0 RID: 15024
	public KButton QuitButton;
}
