using System;

// Token: 0x02000AE9 RID: 2793
public class DemoOverScreen : KModalScreen
{
	// Token: 0x06005615 RID: 22037 RVA: 0x001F570C File Offset: 0x001F390C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x001F5735 File Offset: 0x001F3935
	private void Init()
	{
		this.QuitButton.onClick += delegate()
		{
			this.Quit();
		};
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x001F574E File Offset: 0x001F394E
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x040039D5 RID: 14805
	public KButton QuitButton;
}
