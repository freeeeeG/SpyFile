using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E2 RID: 2530
	public class DuringChargingAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060035C8 RID: 13768 RVA: 0x0009FB5A File Offset: 0x0009DD5A
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x0009FB68 File Offset: 0x0009DD68
		public override void StartAttach()
		{
			Character owner = base.owner;
			owner.onStartCharging = (Action<Characters.Actions.Action>)Delegate.Combine(owner.onStartCharging, new Action<Characters.Actions.Action>(this.OnStartCharging));
			Character owner2 = base.owner;
			owner2.onStopCharging = (Action<Characters.Actions.Action>)Delegate.Combine(owner2.onStopCharging, new Action<Characters.Actions.Action>(this.OnEndCharging));
			Character owner3 = base.owner;
			owner3.onCancelCharging = (Action<Characters.Actions.Action>)Delegate.Combine(owner3.onCancelCharging, new Action<Characters.Actions.Action>(this.OnEndCharging));
			base.StartCoroutine(this.CCooldown());
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x0009FBF8 File Offset: 0x0009DDF8
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			Character owner = base.owner;
			owner.onStartCharging = (Action<Characters.Actions.Action>)Delegate.Remove(owner.onStartCharging, new Action<Characters.Actions.Action>(this.OnStartCharging));
			Character owner2 = base.owner;
			owner2.onStopCharging = (Action<Characters.Actions.Action>)Delegate.Remove(owner2.onStopCharging, new Action<Characters.Actions.Action>(this.OnEndCharging));
			Character owner3 = base.owner;
			owner3.onCancelCharging = (Action<Characters.Actions.Action>)Delegate.Remove(owner3.onCancelCharging, new Action<Characters.Actions.Action>(this.OnEndCharging));
			base.StopAllCoroutines();
			this.Detach();
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x0009FC95 File Offset: 0x0009DE95
		private IEnumerator CCooldown()
		{
			for (;;)
			{
				this._remainCooldown -= base.owner.chronometer.master.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x0009FCA4 File Offset: 0x0009DEA4
		private void Attach()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0009FCC2 File Offset: 0x0009DEC2
		private void Detach()
		{
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0009FCE0 File Offset: 0x0009DEE0
		private void OnStartCharging(Characters.Actions.Action action)
		{
			if (this._remainCooldown > 0f)
			{
				return;
			}
			this._remainCooldown = this._cooldown;
			this.Attach();
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0009FD02 File Offset: 0x0009DF02
		private void OnEndCharging(Characters.Actions.Action action)
		{
			this.Detach();
		}

		// Token: 0x04002B41 RID: 11073
		[SerializeField]
		private float _cooldown;

		// Token: 0x04002B42 RID: 11074
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B43 RID: 11075
		private float _remainCooldown;
	}
}
