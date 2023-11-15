using System;
using System.Collections.Generic;

// Token: 0x0200012D RID: 301
public class EnemyBuffStategy : IBuffStategy
{
	// Token: 0x17000312 RID: 786
	// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0001489E File Offset: 0x00012A9E
	// (set) Token: 0x060007D2 RID: 2002 RVA: 0x000148A6 File Offset: 0x00012AA6
	public List<EnemyBuff> TileBuffs { get; set; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000148AF File Offset: 0x00012AAF
	// (set) Token: 0x060007D4 RID: 2004 RVA: 0x000148B7 File Offset: 0x00012AB7
	public List<EnemyBuff> TimeBuffs { get; set; }

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x060007D5 RID: 2005 RVA: 0x000148C0 File Offset: 0x00012AC0
	// (set) Token: 0x060007D6 RID: 2006 RVA: 0x000148C8 File Offset: 0x00012AC8
	public float SlowRate { get; set; }

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x060007D7 RID: 2007 RVA: 0x000148D1 File Offset: 0x00012AD1
	// (set) Token: 0x060007D8 RID: 2008 RVA: 0x000148D9 File Offset: 0x00012AD9
	public float SlowResist { get; set; }

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000148E2 File Offset: 0x00012AE2
	// (set) Token: 0x060007DA RID: 2010 RVA: 0x000148EA File Offset: 0x00012AEA
	public float SpeedIntensify { get; set; }

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x060007DB RID: 2011 RVA: 0x000148F3 File Offset: 0x00012AF3
	// (set) Token: 0x060007DC RID: 2012 RVA: 0x000148FB File Offset: 0x00012AFB
	public float DamageIntensify { get; set; }

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x060007DD RID: 2013 RVA: 0x00014904 File Offset: 0x00012B04
	// (set) Token: 0x060007DE RID: 2014 RVA: 0x0001490C File Offset: 0x00012B0C
	public float TrapIntensify { get; set; }

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x00014915 File Offset: 0x00012B15
	// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0001491D File Offset: 0x00012B1D
	public float CurrentFrost { get; set; }

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00014926 File Offset: 0x00012B26
	// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0001492E File Offset: 0x00012B2E
	public float MaxFrost { get; set; }

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00014937 File Offset: 0x00012B37
	// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0001493F File Offset: 0x00012B3F
	public float StunTime { get; set; }

	// Token: 0x060007E5 RID: 2021 RVA: 0x00014948 File Offset: 0x00012B48
	public void ApplyBuff()
	{
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0001494A File Offset: 0x00012B4A
	public void ClearBuffs()
	{
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0001494C File Offset: 0x00012B4C
	public void OnUpdate()
	{
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0001494E File Offset: 0x00012B4E
	public void TileTick()
	{
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00014950 File Offset: 0x00012B50
	public void TimeTick()
	{
	}
}
