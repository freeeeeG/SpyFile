using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009F2 RID: 2546
	public class HealthAttacher : AbilityAttacher
	{
		// Token: 0x06003624 RID: 13860 RVA: 0x000A092E File Offset: 0x0009EB2E
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000A093B File Offset: 0x0009EB3B
		public override void StartAttach()
		{
			base.owner.health.onChanged += this.Check;
			this.Check();
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000A0960 File Offset: 0x0009EB60
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.health.onChanged -= this.Check;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000A09B4 File Offset: 0x0009EBB4
		private void Check()
		{
			if ((this._type == HealthAttacher.Type.GreaterThanOrEqual && base.owner.health.percent >= (double)this._healthPercent * 0.01) || (this._type == HealthAttacher.Type.LessThan && base.owner.health.percent < (double)this._healthPercent * 0.01))
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000A0A25 File Offset: 0x0009EC25
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000A0A53 File Offset: 0x0009EC53
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B74 RID: 11124
		[SerializeField]
		private HealthAttacher.Type _type;

		// Token: 0x04002B75 RID: 11125
		[Range(0f, 100f)]
		[SerializeField]
		private int _healthPercent;

		// Token: 0x04002B76 RID: 11126
		[SerializeField]
		private float _checkInterval = 0.1f;

		// Token: 0x04002B77 RID: 11127
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B78 RID: 11128
		private bool _attached;

		// Token: 0x020009F3 RID: 2547
		private enum Type
		{
			// Token: 0x04002B7A RID: 11130
			GreaterThanOrEqual,
			// Token: 0x04002B7B RID: 11131
			LessThan
		}
	}
}
