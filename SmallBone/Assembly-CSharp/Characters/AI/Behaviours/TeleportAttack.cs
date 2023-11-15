using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012DA RID: 4826
	public sealed class TeleportAttack : Behaviour
	{
		// Token: 0x06005F76 RID: 24438 RVA: 0x00117AE7 File Offset: 0x00115CE7
		private void Awake()
		{
			this._behaviours = new Behaviour[]
			{
				this._teleportBehind,
				this._attack,
				this._escapeTeleport
			};
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x00117B10 File Offset: 0x00115D10
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			int num;
			for (int i = 0; i < this._behaviours.Length; i = num + 1)
			{
				yield return this._behaviours[i].CRun(controller);
				if (base.result != Behaviour.Result.Doing)
				{
					yield break;
				}
				num = i;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x00117B26 File Offset: 0x00115D26
		public bool CanUse()
		{
			return this._attack.CanUse();
		}

		// Token: 0x04004CB7 RID: 19639
		[UnityEditor.Subcomponent(typeof(TeleportBehind))]
		[SerializeField]
		private TeleportBehind _teleportBehind;

		// Token: 0x04004CB8 RID: 19640
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004CB9 RID: 19641
		[UnityEditor.Subcomponent(typeof(EscapeTeleport))]
		[SerializeField]
		private EscapeTeleport _escapeTeleport;

		// Token: 0x04004CBA RID: 19642
		private Behaviour[] _behaviours;
	}
}
