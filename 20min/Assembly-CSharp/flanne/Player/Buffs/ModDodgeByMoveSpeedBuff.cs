using System;
using System.Collections.Generic;

namespace flanne.Player.Buffs
{
	// Token: 0x02000174 RID: 372
	public class ModDodgeByMoveSpeedBuff : Buff
	{
		// Token: 0x0600093B RID: 2363 RVA: 0x000261F8 File Offset: 0x000243F8
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnTweakDodge), DodgeRoller.TweakDodgeNotification);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00026211 File Offset: 0x00024411
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakDodge), DodgeRoller.TweakDodgeNotification);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002622C File Offset: 0x0002442C
		private void OnTweakDodge(object sender, object args)
		{
			float toMultiply = PlayerController.Instance.stats[StatType.MoveSpeed].Modify(1f);
			(args as List<ValueModifier>).Add(new MultValueModifier(0, toMultiply));
		}
	}
}
