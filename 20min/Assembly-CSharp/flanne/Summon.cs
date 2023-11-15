using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200012E RID: 302
	public class Summon : MonoBehaviour
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00022841 File Offset: 0x00020A41
		public StatMod summonDamageMod
		{
			get
			{
				return this.stats[StatType.SummonDamage];
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002284F File Offset: 0x00020A4F
		public StatMod summonAtkSpdMod
		{
			get
			{
				return this.stats[StatType.SummonAttackSpeed];
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0002285D File Offset: 0x00020A5D
		private void Start()
		{
			this.player = PlayerController.Instance;
			this.stats = this.player.stats;
			if (this.dontParent)
			{
				base.transform.SetParent(null);
			}
			this.Init();
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00002F51 File Offset: 0x00001151
		protected virtual void Init()
		{
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00022895 File Offset: 0x00020A95
		protected int ApplyDamageMods(int damage)
		{
			return damage.NotifyModifiers(Summon.TweakSummonDamageNotification, this, this.SummonTypeID);
		}

		// Token: 0x040005F5 RID: 1525
		public static string TweakSummonDamageNotification = "Summon.TweakSummonDamageNotification";

		// Token: 0x040005F6 RID: 1526
		public static string SummonOnHitNotification = "Summon.SummonOnHitNotification";

		// Token: 0x040005F7 RID: 1527
		public string SummonTypeID;

		// Token: 0x040005F8 RID: 1528
		[SerializeField]
		private bool dontParent;

		// Token: 0x040005F9 RID: 1529
		protected PlayerController player;

		// Token: 0x040005FA RID: 1530
		private StatsHolder stats;
	}
}
