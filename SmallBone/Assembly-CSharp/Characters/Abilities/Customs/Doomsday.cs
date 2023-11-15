using System;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D44 RID: 3396
	[Serializable]
	public class Doomsday : Ability
	{
		// Token: 0x06004473 RID: 17523 RVA: 0x000C6E1D File Offset: 0x000C501D
		public override void Initialize()
		{
			base.Initialize();
			this._onAttach.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x000C6E3B File Offset: 0x000C503B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Doomsday.Instance(owner, this);
		}

		// Token: 0x04003432 RID: 13362
		[SerializeField]
		private Item _item;

		// Token: 0x04003433 RID: 13363
		[NonSerialized]
		public DoomsdayComponent component;

		// Token: 0x04003434 RID: 13364
		[SerializeField]
		private double _maxBaseDamage = 9999.0;

		// Token: 0x04003435 RID: 13365
		[SerializeField]
		[Information("입힌 물리피해의 전환비율, 폭발 시 입히는 피해도 물리피해라서 스탯 효과를 받으므로 주의.", InformationAttribute.InformationType.Info, false)]
		private double _damageConversionRatio = 0.20000000298023224;

		// Token: 0x04003436 RID: 13366
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onAttach;

		// Token: 0x04003437 RID: 13367
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000D45 RID: 3397
		public class Instance : AbilityInstance<Doomsday>
		{
			// Token: 0x17000E34 RID: 3636
			// (get) Token: 0x06004476 RID: 17526 RVA: 0x000C6E6A File Offset: 0x000C506A
			public override int iconStacks
			{
				get
				{
					return (int)(this._stackedDamage * this.ability._damageConversionRatio);
				}
			}

			// Token: 0x06004477 RID: 17527 RVA: 0x000C6E7F File Offset: 0x000C507F
			public Instance(Character owner, Doomsday ability) : base(owner, ability)
			{
			}

			// Token: 0x06004478 RID: 17528 RVA: 0x000C6E8C File Offset: 0x000C508C
			protected override void OnAttach()
			{
				this.owner.StartCoroutine(this.ability._onAttach.CRun(this.owner));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x06004479 RID: 17529 RVA: 0x000C6EE4 File Offset: 0x000C50E4
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				if (this.ability._item != null && this.ability._item.state != Gear.State.Dropped)
				{
					this.Explode();
				}
			}

			// Token: 0x0600447A RID: 17530 RVA: 0x000C6F44 File Offset: 0x000C5144
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (gaveDamage.attribute != Damage.Attribute.Physical)
				{
					return;
				}
				this._stackedDamage += damageDealt;
				if (this._stackedDamage > this.ability._maxBaseDamage)
				{
					this._stackedDamage = this.ability._maxBaseDamage;
				}
			}

			// Token: 0x0600447B RID: 17531 RVA: 0x000C6F9C File Offset: 0x000C519C
			private void Explode()
			{
				this.ability.component.amount = (float)(this._stackedDamage * this.ability._damageConversionRatio);
				this.ability._operations.Run(this.owner);
			}

			// Token: 0x04003438 RID: 13368
			private double _stackedDamage;
		}
	}
}
