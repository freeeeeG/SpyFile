using System;
using InControl;

// Token: 0x020001FA RID: 506
public class BindingListener : PlayerActionSet
{
	// Token: 0x06000880 RID: 2176 RVA: 0x00033E0C File Offset: 0x0003220C
	public BindingListener()
	{
		this.m_Action = base.CreatePlayerAction("Action");
		base.ListenOptions.IncludeControllers = false;
		base.ListenOptions.IncludeNonStandardControls = false;
		base.ListenOptions.MaxAllowedBindings = 1U;
		base.ListenOptions.OnBindingFound = new Func<PlayerAction, BindingSource, bool>(this.OnBindingFound);
		base.ListenOptions.OnBindingRejected = new Action<PlayerAction, BindingSource, BindingSourceRejectionType>(this.OnBindingRejected);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00033E82 File Offset: 0x00032282
	public void StartListening(VoidGeneric<Key> OnBindingReceived)
	{
		this.m_BindingReceivedCallback = OnBindingReceived;
		this.m_Action.ListenForBinding();
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00033E96 File Offset: 0x00032296
	public void StopListening()
	{
		this.m_Action.StopListeningForBinding();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00033EA4 File Offset: 0x000322A4
	public bool OnBindingFound(PlayerAction action, BindingSource binding)
	{
		if (binding is KeyBindingSource)
		{
			KeyBindingSource keyBindingSource = binding as KeyBindingSource;
			Key param = keyBindingSource.Control.Get(0);
			if (this.m_BindingReceivedCallback != null)
			{
				this.m_BindingReceivedCallback(param);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00033EED File Offset: 0x000322ED
	public void OnBindingRejected(PlayerAction action, BindingSource binding, BindingSourceRejectionType reason)
	{
		if (reason == BindingSourceRejectionType.DuplicateBindingOnAction)
		{
			this.StopListening();
		}
	}

	// Token: 0x04000719 RID: 1817
	private PlayerAction m_Action;

	// Token: 0x0400071A RID: 1818
	private VoidGeneric<Key> m_BindingReceivedCallback;
}
