using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000ABC RID: 2748
	[Serializable]
	public class StackableShield : Ability
	{
		// Token: 0x14000095 RID: 149
		// (add) Token: 0x0600388D RID: 14477 RVA: 0x000A6CD4 File Offset: 0x000A4ED4
		// (remove) Token: 0x0600388E RID: 14478 RVA: 0x000A6D0C File Offset: 0x000A4F0C
		public event Action<Shield.Instance> onBroke;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x0600388F RID: 14479 RVA: 0x000A6D44 File Offset: 0x000A4F44
		// (remove) Token: 0x06003890 RID: 14480 RVA: 0x000A6D7C File Offset: 0x000A4F7C
		public event Action<Shield.Instance> onDetach;

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003891 RID: 14481 RVA: 0x000A6DB1 File Offset: 0x000A4FB1
		// (set) Token: 0x06003892 RID: 14482 RVA: 0x000A6DE5 File Offset: 0x000A4FE5
		public float amount
		{
			get
			{
				if (this._instance == null)
				{
					return 0f;
				}
				if (this._instance._shieldInstance == null)
				{
					return 0f;
				}
				return (float)this._instance._shieldInstance.amount;
			}
			set
			{
				if (this._instance == null)
				{
					return;
				}
				if (this._instance._shieldInstance == null)
				{
					return;
				}
				this._instance.AddShield(value);
			}
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x000A6E0A File Offset: 0x000A500A
		public void Load(Character owner, int stack)
		{
			owner.ability.Add(this);
			this.amount = (float)stack;
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x00089C49 File Offset: 0x00087E49
		public StackableShield()
		{
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x000A6E21 File Offset: 0x000A5021
		public StackableShield(float amount)
		{
			this._amount = amount;
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x000A6E30 File Offset: 0x000A5030
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._instance = new StackableShield.Instance(owner, this);
			return this._instance;
		}

		// Token: 0x04002D0A RID: 11530
		[SerializeField]
		private int _maxAmount;

		// Token: 0x04002D0B RID: 11531
		[SerializeField]
		private float _amount;

		// Token: 0x04002D0C RID: 11532
		private StackableShield.Instance _instance;

		// Token: 0x02000ABD RID: 2749
		public class Instance : AbilityInstance<StackableShield>
		{
			// Token: 0x06003897 RID: 14487 RVA: 0x000A6E45 File Offset: 0x000A5045
			public Instance(Character owner, StackableShield ability) : base(owner, ability)
			{
			}

			// Token: 0x06003898 RID: 14488 RVA: 0x000A6E4F File Offset: 0x000A504F
			public override void Refresh()
			{
				base.Refresh();
				this.AddShield(this.ability._amount);
			}

			// Token: 0x06003899 RID: 14489 RVA: 0x000A6E68 File Offset: 0x000A5068
			public void AddShield(float amount)
			{
				if (this._shieldInstance != null)
				{
					this._shieldInstance.amount = (double)Mathf.Min((float)this.ability._maxAmount, (float)this._shieldInstance.amount + amount);
					return;
				}
				this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._amount, new Action(this.OnShieldBroke));
			}

			// Token: 0x0600389A RID: 14490 RVA: 0x000A6EE1 File Offset: 0x000A50E1
			private void OnShieldBroke()
			{
				Action<Shield.Instance> onBroke = this.ability.onBroke;
				if (onBroke != null)
				{
					onBroke(this._shieldInstance);
				}
				this.owner.ability.Remove(this);
				this._shieldInstance = null;
			}

			// Token: 0x0600389B RID: 14491 RVA: 0x000A6F18 File Offset: 0x000A5118
			protected override void OnAttach()
			{
				this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._amount, new Action(this.OnShieldBroke));
			}

			// Token: 0x0600389C RID: 14492 RVA: 0x000A6F54 File Offset: 0x000A5154
			protected override void OnDetach()
			{
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

			// Token: 0x04002D0D RID: 11533
			internal Shield.Instance _shieldInstance;
		}
	}
}
