using System;

// Token: 0x0200012C RID: 300
public interface IDamageStrategy
{
	// Token: 0x1700030D RID: 781
	// (get) Token: 0x060007C8 RID: 1992
	string ExplosionSound { get; }

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x060007C9 RID: 1993
	float MaxHealth { get; }

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x060007CA RID: 1994
	float CurrentHealth { get; }

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x060007CB RID: 1995
	bool IsDie { get; }

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x060007CC RID: 1996
	bool IsEnemy { get; }

	// Token: 0x060007CD RID: 1997
	void ApplyDamage();

	// Token: 0x060007CE RID: 1998
	void OnDie();

	// Token: 0x060007CF RID: 1999
	void OnUpdate();

	// Token: 0x060007D0 RID: 2000
	void Reset();
}
