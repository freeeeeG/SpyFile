using System;

// Token: 0x02000212 RID: 530
public class LogicalGenericPadButton<InputProvider, ButtonID, ValueID> : LogicalPadButtonBase<ButtonID> where InputProvider : Singleton<InputProvider>, IGamepadInputProvider<ButtonID, ValueID>, new()
{
	// Token: 0x060008D8 RID: 2264 RVA: 0x0003504F File Offset: 0x0003344F
	public LogicalGenericPadButton(ControlPadInput.PadNum _pad, ButtonID _button) : base(_pad, _button)
	{
		this.m_inputProvider = Singleton<InputProvider>.Get();
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00035064 File Offset: 0x00033464
	public override bool IsDown()
	{
		bool flag = this.m_inputProvider.IsDown(this.m_pad, this.m_button);
		base.Update(flag);
		return flag;
	}

	// Token: 0x040007BE RID: 1982
	protected InputProvider m_inputProvider;
}
