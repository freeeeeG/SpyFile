using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000A39 RID: 2617
internal class Windowed : Option<Windowed.States>, OptionsData.IInit, OptionsData.IEnabled
{
	// Token: 0x17000394 RID: 916
	// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000F35D0 File Offset: 0x000F19D0
	public override string Label
	{
		get
		{
			return "OptionType.Windowed";
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x060033C2 RID: 13250 RVA: 0x000F35D7 File Offset: 0x000F19D7
	public override OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.Graphics;
		}
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x000F35DA File Offset: 0x000F19DA
	public bool IsEnabled()
	{
		return !SteamUtils.IsSteamInBigPictureMode();
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x000F35E4 File Offset: 0x000F19E4
	public void Init()
	{
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x000F35E6 File Offset: 0x000F19E6
	protected override Windowed.States GetState()
	{
		if (this.m_fullscreen)
		{
			return Windowed.States.Off;
		}
		return Windowed.States.On;
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x000F35F6 File Offset: 0x000F19F6
	protected override void SetState(Windowed.States _state)
	{
		this.m_fullscreen = (_state != Windowed.States.On);
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x000F360C File Offset: 0x000F1A0C
	public override void Commit()
	{
		if (Screen.fullScreen != this.m_fullscreen)
		{
			Screen.fullScreen = this.m_fullscreen;
		}
	}

	// Token: 0x0400299D RID: 10653
	private bool m_fullscreen = true;

	// Token: 0x02000A3A RID: 2618
	public enum States
	{
		// Token: 0x0400299F RID: 10655
		Off,
		// Token: 0x040029A0 RID: 10656
		On
	}
}
