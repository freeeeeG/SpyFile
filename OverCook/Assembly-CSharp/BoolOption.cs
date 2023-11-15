using System;

// Token: 0x02000A37 RID: 2615
internal abstract class BoolOption : Option<BoolOption.States>, OptionsData.IUnloadable
{
	// Token: 0x060033BC RID: 13244 RVA: 0x000F3592 File Offset: 0x000F1992
	public void Unload()
	{
		this.m_prevState = BoolOption.States.False;
		this.m_state = BoolOption.States.False;
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x000F35A2 File Offset: 0x000F19A2
	protected override BoolOption.States GetState()
	{
		return this.m_state;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x000F35AA File Offset: 0x000F19AA
	protected override void SetState(BoolOption.States _state)
	{
		this.m_prevState = this.m_state;
		this.m_state = _state;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x000F35BF File Offset: 0x000F19BF
	public override void Commit()
	{
	}

	// Token: 0x04002998 RID: 10648
	protected BoolOption.States m_prevState;

	// Token: 0x04002999 RID: 10649
	protected BoolOption.States m_state;

	// Token: 0x02000A38 RID: 2616
	public enum States
	{
		// Token: 0x0400299B RID: 10651
		False,
		// Token: 0x0400299C RID: 10652
		True
	}
}
