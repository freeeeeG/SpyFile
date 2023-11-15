using System;
using System.Collections;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009F9 RID: 2553
	public class RandomTriggerAbilityAttacher : AbilityAttacher
	{
		// Token: 0x06003646 RID: 13894 RVA: 0x000A0DE4 File Offset: 0x0009EFE4
		private void Awake()
		{
			this._trigger.onTriggered += this.OnTriggered;
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000A0DFD File Offset: 0x0009EFFD
		private void OnTriggered()
		{
			base.owner.ability.Add(this._abilityComponents.components.Random<AbilityComponent>().ability);
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000A0E25 File Offset: 0x0009F025
		public override void OnIntialize()
		{
			this._abilityComponents.Initialize();
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000A0E32 File Offset: 0x0009F032
		public override void StartAttach()
		{
			this._trigger.Attach(base.owner);
			this._cUpdateReference = this.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000A0E58 File Offset: 0x0009F058
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			this._trigger.Detach();
			this._cUpdateReference.Stop();
			foreach (AbilityComponent abilityComponent in this._abilityComponents.components)
			{
				base.owner.ability.Remove(abilityComponent.ability);
			}
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000A0EBF File Offset: 0x0009F0BF
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				this._trigger.UpdateTime(Chronometer.global.deltaTime);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B8B RID: 11147
		[SerializeField]
		[TriggerComponent.SubcomponentAttribute]
		private TriggerComponent _trigger;

		// Token: 0x04002B8C RID: 11148
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x04002B8D RID: 11149
		private CoroutineReference _cUpdateReference;
	}
}
