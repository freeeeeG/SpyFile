using System;
using System.Collections.Generic;

// Token: 0x0200012B RID: 299
public interface IBuffStategy
{
	// Token: 0x17000303 RID: 771
	// (get) Token: 0x060007AF RID: 1967
	// (set) Token: 0x060007B0 RID: 1968
	List<EnemyBuff> TileBuffs { get; set; }

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x060007B1 RID: 1969
	// (set) Token: 0x060007B2 RID: 1970
	List<EnemyBuff> TimeBuffs { get; set; }

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x060007B3 RID: 1971
	// (set) Token: 0x060007B4 RID: 1972
	float SlowRate { get; set; }

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x060007B5 RID: 1973
	// (set) Token: 0x060007B6 RID: 1974
	float SlowResist { get; set; }

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x060007B7 RID: 1975
	// (set) Token: 0x060007B8 RID: 1976
	float SpeedIntensify { get; set; }

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x060007B9 RID: 1977
	// (set) Token: 0x060007BA RID: 1978
	float DamageIntensify { get; set; }

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x060007BB RID: 1979
	// (set) Token: 0x060007BC RID: 1980
	float TrapIntensify { get; set; }

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x060007BD RID: 1981
	// (set) Token: 0x060007BE RID: 1982
	float CurrentFrost { get; set; }

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x060007BF RID: 1983
	// (set) Token: 0x060007C0 RID: 1984
	float MaxFrost { get; set; }

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x060007C1 RID: 1985
	// (set) Token: 0x060007C2 RID: 1986
	float StunTime { get; set; }

	// Token: 0x060007C3 RID: 1987
	void ApplyBuff();

	// Token: 0x060007C4 RID: 1988
	void OnUpdate();

	// Token: 0x060007C5 RID: 1989
	void TimeTick();

	// Token: 0x060007C6 RID: 1990
	void TileTick();

	// Token: 0x060007C7 RID: 1991
	void ClearBuffs();
}
