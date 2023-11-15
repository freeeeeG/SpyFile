using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CC8 RID: 3272
	[Serializable]
	public sealed class GraceOfLeonia : Ability
	{
		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x0600424E RID: 16974 RVA: 0x000C1124 File Offset: 0x000BF324
		// (remove) Token: 0x0600424F RID: 16975 RVA: 0x000C115C File Offset: 0x000BF35C
		public event Action<Shield.Instance> onBroke;

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06004250 RID: 16976 RVA: 0x000C1194 File Offset: 0x000BF394
		// (remove) Token: 0x06004251 RID: 16977 RVA: 0x000C11CC File Offset: 0x000BF3CC
		public event Action<Shield.Instance> onDetach;

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x000C1201 File Offset: 0x000BF401
		// (set) Token: 0x06004253 RID: 16979 RVA: 0x000C1209 File Offset: 0x000BF409
		public float amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				this._amount = value;
			}
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x00089C49 File Offset: 0x00087E49
		public GraceOfLeonia()
		{
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x000C1212 File Offset: 0x000BF412
		public GraceOfLeonia(float amount)
		{
			this._amount = amount;
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x000C1221 File Offset: 0x000BF421
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x000C1234 File Offset: 0x000BF434
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GraceOfLeonia.Instance(owner, this);
		}

		// Token: 0x040032C7 RID: 12999
		[SerializeField]
		private float _operationInterval;

		// Token: 0x040032C8 RID: 13000
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x040032C9 RID: 13001
		[SerializeField]
		private Stat.Values _stats;

		// Token: 0x040032CA RID: 13002
		[SerializeField]
		private float _amount;

		// Token: 0x040032CB RID: 13003
		[SerializeField]
		private GraceOfLeonia.HealthType _healthType;

		// Token: 0x02000CC9 RID: 3273
		public sealed class Instance : AbilityInstance<GraceOfLeonia>
		{
			// Token: 0x06004258 RID: 16984 RVA: 0x000C123D File Offset: 0x000BF43D
			public Instance(Character owner, GraceOfLeonia ability) : base(owner, ability)
			{
			}

			// Token: 0x06004259 RID: 16985 RVA: 0x000C1248 File Offset: 0x000BF448
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._elapsed -= deltaTime;
				if (this._elapsed > 0f)
				{
					return;
				}
				this._elapsed = this.ability._operationInterval;
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
			}

			// Token: 0x0600425A RID: 16986 RVA: 0x000C12AB File Offset: 0x000BF4AB
			public override void Refresh()
			{
				base.Refresh();
				if (this._shieldInstance != null)
				{
					this._shieldInstance.amount = (double)this.ability._amount;
				}
			}

			// Token: 0x0600425B RID: 16987 RVA: 0x000C12D2 File Offset: 0x000BF4D2
			private void OnShieldBroke()
			{
				Action<Shield.Instance> onBroke = this.ability.onBroke;
				if (onBroke != null)
				{
					onBroke(this._shieldInstance);
				}
				this.owner.ability.Remove(this);
			}

			// Token: 0x0600425C RID: 16988 RVA: 0x000C1304 File Offset: 0x000BF504
			protected override void OnAttach()
			{
				this.owner.stat.AttachOrUpdateValues(this.ability._stats);
				if (this.ability._healthType == GraceOfLeonia.HealthType.Constant)
				{
					this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._amount, new Action(this.OnShieldBroke));
					return;
				}
				this._shieldInstance = this.owner.health.shield.Add(this.ability, (float)(this.owner.health.maximumHealth * (double)this.ability._amount * 0.009999999776482582), new Action(this.OnShieldBroke));
			}

			// Token: 0x0600425D RID: 16989 RVA: 0x000C13C8 File Offset: 0x000BF5C8
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stats);
				Action<Shield.Instance> onDetach = this.ability.onDetach;
				if (onDetach != null)
				{
					onDetach(this._shieldInstance);
				}
				if (this.owner.health.shield.Remove(this.ability))
				{
					this._shieldInstance = null;
				}
			}

			// Token: 0x040032CC RID: 13004
			private Shield.Instance _shieldInstance;

			// Token: 0x040032CD RID: 13005
			private float _elapsed;
		}

		// Token: 0x02000CCA RID: 3274
		public enum HealthType
		{
			// Token: 0x040032CF RID: 13007
			Constant,
			// Token: 0x040032D0 RID: 13008
			Percent
		}
	}
}
