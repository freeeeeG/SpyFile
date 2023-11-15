using System;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class LogicalPadValue<InputProvider> : LogicalPadValueBase<ControlPadInput.Value> where InputProvider : Singleton<InputProvider>, IGamepadInputProvider, new()
{
	// Token: 0x060008E2 RID: 2274 RVA: 0x000351CB File Offset: 0x000335CB
	public LogicalPadValue(ControlPadInput.PadNum _pad, ControlPadInput.Value _value, ILogicalButton _nveButton, ILogicalButton _pveButton) : base(_pad, _value)
	{
		this.m_inputProvider = Singleton<InputProvider>.Get();
		this.m_negativeButton = _nveButton;
		this.m_positiveButton = _pveButton;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00035205 File Offset: 0x00033605
	public LogicalPadValue(ControlPadInput.PadNum _pad, ControlPadInput.Value _value) : this(_pad, _value, new LogicalKeycodeButton(), new LogicalKeycodeButton())
	{
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00035219 File Offset: 0x00033619
	protected virtual bool CanProcessInput()
	{
		return Application.isFocused;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00035220 File Offset: 0x00033620
	public override float GetValue()
	{
		if (!this.CanProcessInput())
		{
			return 0f;
		}
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

	// Token: 0x040007C5 RID: 1989
	private InputProvider m_inputProvider;

	// Token: 0x040007C6 RID: 1990
	protected ILogicalButton m_positiveButton = new LogicalKeycodeButton();

	// Token: 0x040007C7 RID: 1991
	protected ILogicalButton m_negativeButton = new LogicalKeycodeButton();
}
