using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Behaviours;
using Data;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x02001178 RID: 4472
	public class DarkRush : MonoBehaviour
	{
		// Token: 0x06005793 RID: 22419 RVA: 0x001044CF File Offset: 0x001026CF
		public IEnumerator CRun(DarkAideAI darkAideAI)
		{
			darkAideAI.character.status.unstoppable.Attach(this);
			yield return this._teleportBehind.CRun(darkAideAI);
			this._fristAttack.TryStart();
			while (this._fristAttack.running)
			{
				yield return null;
			}
			yield return this._teleportBehind.CRun(darkAideAI);
			this._secondAttack.TryStart();
			while (this._secondAttack.running)
			{
				yield return null;
			}
			yield return this.CFinishAttack();
			darkAideAI.character.status.unstoppable.Detach(this);
			while (this._standing.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x001044E5 File Offset: 0x001026E5
		private IEnumerator CFinishAttack()
		{
			this._finishAttack.TryStart();
			while (this._finishAttack.running)
			{
				yield return null;
			}
			DarkRushEffect[] componentsInChildren = this._parentPool.currentEffectParent.GetComponentsInChildren<DarkRushEffect>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].HideSign();
			}
			this._standing.TryStart();
			componentsInChildren = this._parentPool.currentEffectParent.GetComponentsInChildren<DarkRushEffect>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ShowImpact();
			}
			float seconds = GameData.HardmodeProgress.hardmode ? this._attackLength_hard : this._attackLength_normal;
			yield return Chronometer.global.WaitForSeconds(seconds);
			componentsInChildren = this._parentPool.currentEffectParent.GetComponentsInChildren<DarkRushEffect>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].HideImpact();
			}
			yield break;
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x001044F4 File Offset: 0x001026F4
		public bool CanUse()
		{
			return this._finishAttack.canUse;
		}

		// Token: 0x0400467E RID: 18046
		[SerializeField]
		private Characters.Actions.Action _standing;

		// Token: 0x0400467F RID: 18047
		[Subcomponent(typeof(TeleportBehind))]
		[SerializeField]
		[Header("MeleeAttack")]
		[Space]
		private TeleportBehind _teleportBehind;

		// Token: 0x04004680 RID: 18048
		[SerializeField]
		private Characters.Actions.Action _fristAttack;

		// Token: 0x04004681 RID: 18049
		[SerializeField]
		private Characters.Actions.Action _secondAttack;

		// Token: 0x04004682 RID: 18050
		[Header("Last Attack")]
		[SerializeField]
		private Characters.Actions.Action _finishAttack;

		// Token: 0x04004683 RID: 18051
		[Space]
		[SerializeField]
		private ParentPool _parentPool;

		// Token: 0x04004684 RID: 18052
		[SerializeField]
		private float _attackLength_normal = 1f;

		// Token: 0x04004685 RID: 18053
		[SerializeField]
		private float _attackLength_hard = 1f;
	}
}
