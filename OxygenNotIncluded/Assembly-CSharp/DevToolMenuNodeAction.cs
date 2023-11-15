using System;

// Token: 0x02000561 RID: 1377
public class DevToolMenuNodeAction : IMenuNode
{
	// Token: 0x0600218D RID: 8589 RVA: 0x000B59C0 File Offset: 0x000B3BC0
	public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000B59D6 File Offset: 0x000B3BD6
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x000B59DE File Offset: 0x000B3BDE
	public void Draw()
	{
		if (ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
		{
			this.onClickFn();
		}
	}

	// Token: 0x04001301 RID: 4865
	public string name;

	// Token: 0x04001302 RID: 4866
	public System.Action onClickFn;

	// Token: 0x04001303 RID: 4867
	public Func<bool> isEnabledFn;
}
