using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Hero;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013AD RID: 5037
	public class BodyBlow : Behaviour, IFinish, IComboable
	{
		// Token: 0x06006353 RID: 25427 RVA: 0x00120E36 File Offset: 0x0011F036
		public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
		{
			comboSystem.Clear();
			yield return this.CCombat(controller);
			yield break;
		}

		// Token: 0x06006354 RID: 25428 RVA: 0x00120E53 File Offset: 0x0011F053
		public override IEnumerator CRun(AIController controller)
		{
			this._startAction.TryStart();
			while (this._startAction.running)
			{
				yield return null;
			}
			yield return this.CCombat(controller);
			yield break;
		}

		// Token: 0x06006355 RID: 25429 RVA: 0x00120E69 File Offset: 0x0011F069
		private IEnumerator CCombat(AIController controller)
		{
			Character character = controller.character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.CheckHit));
			this._readyAction.TryStart();
			while (this._readyAction.running)
			{
				yield return null;
			}
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			Character character2 = controller.character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(this.CheckHit));
			if (this._canUseSlashCombo)
			{
				yield return this._slashCombo.CRun(controller);
			}
			else
			{
				this._failAction.TryStart();
				while (this._failAction.running)
				{
					yield return null;
				}
			}
			this._canUseSlashCombo = false;
			yield return this._skipableIdle.CRun(controller);
			yield break;
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x00120E7F File Offset: 0x0011F07F
		private void CheckHit(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			this._canUseSlashCombo = true;
		}

		// Token: 0x04005013 RID: 20499
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _startAction;

		// Token: 0x04005014 RID: 20500
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _readyAction;

		// Token: 0x04005015 RID: 20501
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04005016 RID: 20502
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _failAction;

		// Token: 0x04005017 RID: 20503
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(SlashCombo))]
		private SlashCombo _slashCombo;

		// Token: 0x04005018 RID: 20504
		[UnityEditor.Subcomponent(typeof(SkipableIdle))]
		[SerializeField]
		private SkipableIdle _skipableIdle;

		// Token: 0x04005019 RID: 20505
		private bool _canUseSlashCombo;
	}
}
