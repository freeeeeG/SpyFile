using System;

// Token: 0x02000214 RID: 532
public class LogicalPadButton<InputProvider> : LogicalPadButtonBase<ControlPadInput.Button> where InputProvider : Singleton<InputProvider>, IGamepadInputProvider, new()
{
	// Token: 0x060008DD RID: 2269 RVA: 0x0003516E File Offset: 0x0003356E
	public LogicalPadButton(ControlPadInput.PadNum _pad, ControlPadInput.Button _button) : base(_pad, _button)
	{
		this.m_inputProvider = Singleton<InputProvider>.Get();
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00035184 File Offset: 0x00033584
	public override bool IsDown()
	{
		if (!this.CanProcessInput())
		{
			base.Update(false);
			return false;
		}
		bool flag = this.m_inputProvider.IsDown(this.m_pad, this.m_button);
		base.Update(flag);
		return flag;
	}

	// Token: 0x040007C2 RID: 1986
	protected InputProvider m_inputProvider;
}
