using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013D8 RID: 5080
	public class ActionAttack : Attack
	{
		// Token: 0x06006419 RID: 25625 RVA: 0x001227D5 File Offset: 0x001209D5
		private void Start()
		{
			this._childs = new List<Behaviour>
			{
				this.idle
			};
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x001227EE File Offset: 0x001209EE
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this.gaveDamage = false;
			if (this.attack.TryStart())
			{
				while (this.attack.running)
				{
					yield return null;
				}
				base.result = Behaviour.Result.Success;
				if (MMMaths.Chance(this.chanceOfDelayAffterAttack))
				{
					yield return this.idle.CRun(controller);
				}
			}
			else
			{
				base.result = Behaviour.Result.Fail;
			}
			yield break;
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x00122804 File Offset: 0x00120A04
		public bool CanUse()
		{
			return this.attack.canUse;
		}

		// Token: 0x040050B9 RID: 20665
		[SerializeField]
		protected Characters.Actions.Action attack;

		// Token: 0x040050BA RID: 20666
		[Range(0f, 1f)]
		[SerializeField]
		protected float chanceOfDelayAffterAttack;

		// Token: 0x040050BB RID: 20667
		[UnityEditor.Subcomponent(typeof(Idle))]
		[SerializeField]
		protected Idle idle;
	}
}
