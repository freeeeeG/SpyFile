using System;
using UnityEngine;

// Token: 0x02000A3B RID: 2619
internal class VSync : Option<VSync.States>, OptionsData.IEnabled
{
	// Token: 0x17000396 RID: 918
	// (get) Token: 0x060033C9 RID: 13257 RVA: 0x000F3631 File Offset: 0x000F1A31
	public override string Label
	{
		get
		{
			return "OptionType.VSync";
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x060033CA RID: 13258 RVA: 0x000F3638 File Offset: 0x000F1A38
	public override OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.Graphics;
		}
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x000F363B File Offset: 0x000F1A3B
	public bool IsEnabled()
	{
		return true;
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x000F363E File Offset: 0x000F1A3E
	protected override VSync.States GetState()
	{
		if (this.m_vsync == 0)
		{
			return VSync.States.Off;
		}
		return VSync.States.On;
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x000F364E File Offset: 0x000F1A4E
	protected override void SetState(VSync.States _state)
	{
		this.m_vsync = ((_state != VSync.States.On) ? 0 : 1);
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x000F3664 File Offset: 0x000F1A64
	public override void Commit()
	{
		if (QualitySettings.vSyncCount != this.m_vsync)
		{
			QualitySettings.vSyncCount = this.m_vsync;
		}
	}

	// Token: 0x040029A1 RID: 10657
	private int m_vsync;

	// Token: 0x02000A3C RID: 2620
	public enum States
	{
		// Token: 0x040029A3 RID: 10659
		Off,
		// Token: 0x040029A4 RID: 10660
		On
	}
}
