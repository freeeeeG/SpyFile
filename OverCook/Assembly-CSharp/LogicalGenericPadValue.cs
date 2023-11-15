using System;

// Token: 0x02000213 RID: 531
public class LogicalGenericPadValue<InputProvider, ButtonID, ValueID> : LogicalPadValueBase<ValueID> where InputProvider : Singleton<InputProvider>, IGamepadInputProvider<ButtonID, ValueID>, new()
{
	// Token: 0x060008DA RID: 2266 RVA: 0x000350C9 File Offset: 0x000334C9
	public LogicalGenericPadValue(ControlPadInput.PadNum _pad, ValueID _value, ILogicalButton _nveButton, ILogicalButton _pveButton) : base(_pad, _value)
	{
		this.m_inputProvider = Singleton<InputProvider>.Get();
		this.m_negativeButton = _nveButton;
		this.m_positiveButton = _pveButton;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00035103 File Offset: 0x00033503
	public LogicalGenericPadValue(ControlPadInput.PadNum _pad, ValueID _value) : this(_pad, _value, new LogicalKeycodeButton(), new LogicalKeycodeButton())
	{
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00035118 File Offset: 0x00033518
	public override float GetValue()
	{
		if (this.m_positiveButton.IsDown())
		{
			return 1f;
		}
		if (this.m_negativeButton.IsDown())
		{
			return -1f;
		}
		return this.m_inputProvider.GetValue(this.m_pad, this.m_value);
	}

	// Token: 0x040007BF RID: 1983
	private InputProvider m_inputProvider;

	// Token: 0x040007C0 RID: 1984
	protected ILogicalButton m_positiveButton = new LogicalKeycodeButton();

	// Token: 0x040007C1 RID: 1985
	protected ILogicalButton m_negativeButton = new LogicalKeycodeButton();
}
