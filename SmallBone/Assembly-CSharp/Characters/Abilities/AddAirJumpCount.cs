using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009A0 RID: 2464
	[Serializable]
	public class AddAirJumpCount : Ability
	{
		// Token: 0x060034E8 RID: 13544 RVA: 0x0009CA8A File Offset: 0x0009AC8A
		public AddAirJumpCount()
		{
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x0009CA99 File Offset: 0x0009AC99
		public AddAirJumpCount(int count)
		{
			this._count = count;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x0009CAAF File Offset: 0x0009ACAF
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AddAirJumpCount.Instance(owner, this);
		}

		// Token: 0x04002A93 RID: 10899
		[SerializeField]
		private int _count = 1;

		// Token: 0x020009A1 RID: 2465
		public class Instance : AbilityInstance<AddAirJumpCount>
		{
			// Token: 0x060034EB RID: 13547 RVA: 0x0009CAB8 File Offset: 0x0009ACB8
			public Instance(Character owner, AddAirJumpCount ability) : base(owner, ability)
			{
			}

			// Token: 0x060034EC RID: 13548 RVA: 0x0009CAC2 File Offset: 0x0009ACC2
			protected override void OnAttach()
			{
				this.owner.movement.airJumpCount.Add(this, this.ability._count);
			}

			// Token: 0x060034ED RID: 13549 RVA: 0x0009CAE5 File Offset: 0x0009ACE5
			protected override void OnDetach()
			{
				this.owner.movement.airJumpCount.Remove(this);
			}
		}
	}
}
