using System;
using Data;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009DD RID: 2525
	public sealed class CurrencyAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060035A7 RID: 13735 RVA: 0x0009F56D File Offset: 0x0009D76D
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x0009F57A File Offset: 0x0009D77A
		public override void StartAttach()
		{
			this._startAttachCheck = true;
			this.Check();
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x0009F58C File Offset: 0x0009D78C
		private void Update()
		{
			if (!this._startAttachCheck)
			{
				return;
			}
			this._elapsed += Chronometer.global.deltaTime;
			if (this._elapsed < this._checkInterval)
			{
				return;
			}
			this._elapsed -= this._checkInterval;
			this.Check();
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x0009F5E1 File Offset: 0x0009D7E1
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			this._attached = false;
			this._startAttachCheck = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x0009F61C File Offset: 0x0009D81C
		private void Check()
		{
			switch (this._comparer)
			{
			case CurrencyAbilityAttacher.Comparer.Equal:
				if (this._count == GameData.Currency.currencies[this._type].balance)
				{
					this.Attach();
					return;
				}
				this.Detach();
				return;
			case CurrencyAbilityAttacher.Comparer.Greater:
				if (this._count < GameData.Currency.currencies[this._type].balance)
				{
					this.Attach();
					return;
				}
				this.Detach();
				return;
			case CurrencyAbilityAttacher.Comparer.GreaterThanOrEqual:
				if (this._count <= GameData.Currency.currencies[this._type].balance)
				{
					this.Attach();
					return;
				}
				this.Detach();
				return;
			case CurrencyAbilityAttacher.Comparer.LessThan:
				if (this._count > GameData.Currency.currencies[this._type].balance)
				{
					this.Attach();
					return;
				}
				this.Detach();
				return;
			case CurrencyAbilityAttacher.Comparer.LessThanOrEqual:
				if (this._count >= GameData.Currency.currencies[this._type].balance)
				{
					this.Attach();
					return;
				}
				this.Detach();
				return;
			default:
				return;
			}
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x0009F721 File Offset: 0x0009D921
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x0009F74F File Offset: 0x0009D94F
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B2B RID: 11051
		[SerializeField]
		private CurrencyAbilityAttacher.Comparer _comparer;

		// Token: 0x04002B2C RID: 11052
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04002B2D RID: 11053
		[SerializeField]
		private int _count;

		// Token: 0x04002B2E RID: 11054
		[SerializeField]
		private float _checkInterval = 1f;

		// Token: 0x04002B2F RID: 11055
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B30 RID: 11056
		private bool _startAttachCheck;

		// Token: 0x04002B31 RID: 11057
		private bool _attached;

		// Token: 0x04002B32 RID: 11058
		private float _elapsed;

		// Token: 0x020009DE RID: 2526
		private enum Comparer
		{
			// Token: 0x04002B34 RID: 11060
			Equal,
			// Token: 0x04002B35 RID: 11061
			Greater,
			// Token: 0x04002B36 RID: 11062
			GreaterThanOrEqual,
			// Token: 0x04002B37 RID: 11063
			LessThan,
			// Token: 0x04002B38 RID: 11064
			LessThanOrEqual
		}
	}
}
