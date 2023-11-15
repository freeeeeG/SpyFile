using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000171 RID: 369
	public class SummonDamageBuff : Buff
	{
		// Token: 0x06000932 RID: 2354 RVA: 0x0002608C File Offset: 0x0002428C
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnTweakDamage), Summon.TweakSummonDamageNotification);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000260A5 File Offset: 0x000242A5
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakDamage), Summon.TweakSummonDamageNotification);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x000260C0 File Offset: 0x000242C0
		private void OnTweakDamage(object sender, object args)
		{
			Info<List<ValueModifier>, string> info = args as Info<List<ValueModifier>, string>;
			if (info.arg1 != this.summonTypeID)
			{
				return;
			}
			info.arg0.Add(this.modifier.GetMod());
		}

		// Token: 0x040006B6 RID: 1718
		[SerializeField]
		private string summonTypeID;

		// Token: 0x040006B7 RID: 1719
		[SerializeReference]
		private IDamageModifier modifier;
	}
}
