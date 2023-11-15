using System;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009FB RID: 2555
	public sealed class ShieldAttacher : AbilityAttacher
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x000A0F40 File Offset: 0x0009F140
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000A0F4D File Offset: 0x0009F14D
		public override void StartAttach()
		{
			this._checkReference = this.StartCoroutineWithReference(this.CCheck());
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000A0F61 File Offset: 0x0009F161
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			this._checkReference.Stop();
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000A0F99 File Offset: 0x0009F199
		private IEnumerator CCheck()
		{
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds(this._checkInterval);
				this.Check();
			}
			yield break;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000A0FA8 File Offset: 0x0009F1A8
		private void Check()
		{
			if (!base.owner.health.shield.hasAny)
			{
				this.Detach();
				return;
			}
			if ((this._type == ShieldAttacher.Type.GreaterThanOrEqual && base.owner.health.shield.amount >= (double)this._shieldAmount) || (this._type == ShieldAttacher.Type.LessThan && base.owner.health.shield.amount < (double)this._shieldAmount))
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000A102D File Offset: 0x0009F22D
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000A105B File Offset: 0x0009F25B
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B91 RID: 11153
		[SerializeField]
		private ShieldAttacher.Type _type;

		// Token: 0x04002B92 RID: 11154
		[SerializeField]
		[Range(0f, 100f)]
		private int _shieldAmount;

		// Token: 0x04002B93 RID: 11155
		[SerializeField]
		private float _checkInterval = 0.1f;

		// Token: 0x04002B94 RID: 11156
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B95 RID: 11157
		private bool _attached;

		// Token: 0x04002B96 RID: 11158
		private CoroutineReference _checkReference;

		// Token: 0x020009FC RID: 2556
		private enum Type
		{
			// Token: 0x04002B98 RID: 11160
			GreaterThanOrEqual,
			// Token: 0x04002B99 RID: 11161
			LessThan
		}
	}
}
