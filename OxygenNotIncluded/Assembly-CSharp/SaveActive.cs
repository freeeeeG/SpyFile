using System;

// Token: 0x02000BDE RID: 3038
public class SaveActive : KScreen
{
	// Token: 0x0600601B RID: 24603 RVA: 0x00238A3D File Offset: 0x00236C3D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x00238A73 File Offset: 0x00236C73
	private void DoCallBack(HashedString name)
	{
		this.controller.onAnimComplete -= this.DoCallBack;
		this.readyForSaveCallback();
		this.readyForSaveCallback = null;
	}

	// Token: 0x0600601D RID: 24605 RVA: 0x00238A9E File Offset: 0x00236C9E
	private void ActiveateSaveIndicator(Game.CansaveCB cb)
	{
		this.readyForSaveCallback = cb;
		this.controller.onAnimComplete += this.DoCallBack;
		this.controller.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600601E RID: 24606 RVA: 0x00238ADE File Offset: 0x00236CDE
	private void SetActiveSaveIndicator()
	{
		this.controller.Play("working_loop", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x00238B00 File Offset: 0x00236D00
	private void DeactivateSaveIndicator()
	{
		this.controller.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006020 RID: 24608 RVA: 0x00238B22 File Offset: 0x00236D22
	public override void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x0400416B RID: 16747
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x0400416C RID: 16748
	private Game.CansaveCB readyForSaveCallback;
}
