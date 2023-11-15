using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200018B RID: 395
	public class OnCurseTrigger : Trigger
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x00026BD5 File Offset: 0x00024DD5
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnInflictCurse), CurseSystem.InflictCurseEvent);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00026BEE File Offset: 0x00024DEE
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnInflictCurse), CurseSystem.InflictCurseEvent);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00026C08 File Offset: 0x00024E08
		private void OnInflictCurse(object sender, object args)
		{
			GameObject target = args as GameObject;
			if (Random.Range(0f, 1f) < this.chanceToTrigger)
			{
				base.RaiseTrigger(target);
			}
		}

		// Token: 0x040006E9 RID: 1769
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToTrigger = 1f;
	}
}
