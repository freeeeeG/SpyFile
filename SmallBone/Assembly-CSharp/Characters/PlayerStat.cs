using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200069B RID: 1691
	public class PlayerStat : MonoBehaviour, ICharacterStat
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x00065931 File Offset: 0x00063B31
		// (set) Token: 0x060021C4 RID: 8644 RVA: 0x00065939 File Offset: 0x00063B39
		public Stat stat { get; private set; }

		// Token: 0x060021C5 RID: 8645 RVA: 0x00002191 File Offset: 0x00000391
		public void Awake()
		{
		}

		// Token: 0x04001CC9 RID: 7369
		private readonly Stat _commonStat;

		// Token: 0x04001CCA RID: 7370
		public readonly Stat weaponStat;

		// Token: 0x04001CCB RID: 7371
		public readonly Stat quintessenceStat;
	}
}
