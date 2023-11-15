using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000168 RID: 360
	public class DamageBuff : Buff
	{
		// Token: 0x0600091F RID: 2335 RVA: 0x00025DFC File Offset: 0x00023FFC
		public override void OnAttach()
		{
			if (this.modAllDamageType)
			{
				this.AddObserver(new Action<object, object>(this.OnTweakDamage), Health.TweakDamageEvent);
				return;
			}
			this.AddObserver(new Action<object, object>(this.OnTweakDamage), string.Format("Health.Tweak{0}Damage", this.damageType.ToString()));
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00025E58 File Offset: 0x00024058
		public override void OnUnattach()
		{
			if (this.modAllDamageType)
			{
				this.RemoveObserver(new Action<object, object>(this.OnTweakDamage), Health.TweakDamageEvent);
				return;
			}
			this.RemoveObserver(new Action<object, object>(this.OnTweakDamage), string.Format("Health.Tweak{0}Damage", this.damageType.ToString()));
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00025EB4 File Offset: 0x000240B4
		private void OnTweakDamage(object sender, object args)
		{
			GameObject gameObject = (sender as Health).gameObject;
			if (this.conditional == null || this.conditional.ConditionMet(gameObject))
			{
				(args as List<ValueModifier>).Add(this.modifier.GetMod());
			}
		}

		// Token: 0x040006A9 RID: 1705
		[SerializeField]
		private bool modAllDamageType;

		// Token: 0x040006AA RID: 1706
		[SerializeField]
		private DamageType damageType;

		// Token: 0x040006AB RID: 1707
		[SerializeReference]
		private IDamageModifier modifier;

		// Token: 0x040006AC RID: 1708
		[SerializeReference]
		private IBuffConditional conditional;
	}
}
