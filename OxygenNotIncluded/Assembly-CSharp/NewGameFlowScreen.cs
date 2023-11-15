using System;

// Token: 0x02000B9C RID: 2972
public abstract class NewGameFlowScreen : KModalScreen
{
	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06005CB6 RID: 23734 RVA: 0x0021FB7C File Offset: 0x0021DD7C
	// (remove) Token: 0x06005CB7 RID: 23735 RVA: 0x0021FBB4 File Offset: 0x0021DDB4
	public event System.Action OnNavigateForward;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06005CB8 RID: 23736 RVA: 0x0021FBEC File Offset: 0x0021DDEC
	// (remove) Token: 0x06005CB9 RID: 23737 RVA: 0x0021FC24 File Offset: 0x0021DE24
	public event System.Action OnNavigateBackward;

	// Token: 0x06005CBA RID: 23738 RVA: 0x0021FC59 File Offset: 0x0021DE59
	protected void NavigateBackward()
	{
		this.OnNavigateBackward();
	}

	// Token: 0x06005CBB RID: 23739 RVA: 0x0021FC66 File Offset: 0x0021DE66
	protected void NavigateForward()
	{
		this.OnNavigateForward();
	}

	// Token: 0x06005CBC RID: 23740 RVA: 0x0021FC73 File Offset: 0x0021DE73
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.NavigateBackward();
		}
		base.OnKeyDown(e);
	}
}
