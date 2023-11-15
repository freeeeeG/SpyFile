using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E5 RID: 2533
	public sealed class DuringRunningActionAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060035DF RID: 13791 RVA: 0x0009FE88 File Offset: 0x0009E088
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x0009FE98 File Offset: 0x0009E098
		public override void StartAttach()
		{
			Characters.Actions.Action[] actions = this._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].onStart += this.AttachAbility;
			}
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0009FECE File Offset: 0x0009E0CE
		private void AttachAbility()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
			base.StopCoroutine("CCheckRunningAction");
			base.StartCoroutine("CCheckRunningAction");
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0009FF03 File Offset: 0x0009E103
		private IEnumerator CCheckRunningAction()
		{
			for (;;)
			{
				bool flag = false;
				Characters.Actions.Action[] actions = this._actions;
				for (int i = 0; i < actions.Length; i++)
				{
					if (actions[i].running)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				yield return null;
			}
			this.RemoveAbility();
			yield break;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0009FF12 File Offset: 0x0009E112
		private void RemoveAbility()
		{
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0009FF30 File Offset: 0x0009E130
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			Characters.Actions.Action[] actions = this._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].onStart -= this.AttachAbility;
			}
			this.RemoveAbility();
		}

		// Token: 0x04002B48 RID: 11080
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B49 RID: 11081
		[SerializeField]
		private Characters.Actions.Action[] _actions;
	}
}
