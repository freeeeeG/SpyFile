using System;
using UnityEngine;

// Token: 0x02000A3D RID: 2621
internal class Quality : Option<Quality.Levels>
{
	// Token: 0x17000398 RID: 920
	// (get) Token: 0x060033D0 RID: 13264 RVA: 0x000F3689 File Offset: 0x000F1A89
	public override string Label
	{
		get
		{
			return "OptionType.Quality";
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x060033D1 RID: 13265 RVA: 0x000F3690 File Offset: 0x000F1A90
	public override OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.Graphics;
		}
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x000F3693 File Offset: 0x000F1A93
	protected override Quality.Levels GetState()
	{
		return (Quality.Levels)this.m_level;
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x000F369B File Offset: 0x000F1A9B
	protected override void SetState(Quality.Levels _state)
	{
		this.m_level = (int)_state;
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x000F36A4 File Offset: 0x000F1AA4
	public override void Commit()
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		int option = 0;
		VSync vsync = (!(metaGameProgress != null)) ? null : ((VSync)metaGameProgress.AccessOptionsData.GetOption(OptionsData.OptionType.VSync));
		if (vsync != null)
		{
			option = vsync.GetOption();
		}
		if (QualitySettings.GetQualityLevel() != this.m_level)
		{
			QualitySettings.SetQualityLevel(this.m_level);
		}
		if (vsync != null)
		{
			vsync.SetOption(option);
		}
	}

	// Token: 0x040029A5 RID: 10661
	private int m_level;

	// Token: 0x02000A3E RID: 2622
	public enum Levels
	{
		// Token: 0x040029A7 RID: 10663
		SuperQuick,
		// Token: 0x040029A8 RID: 10664
		Quick,
		// Token: 0x040029A9 RID: 10665
		Standard,
		// Token: 0x040029AA RID: 10666
		Dreamy
	}
}
