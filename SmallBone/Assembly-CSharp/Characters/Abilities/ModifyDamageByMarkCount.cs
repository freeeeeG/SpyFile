using System;
using Characters.Marks;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A68 RID: 2664
	[Serializable]
	public class ModifyDamageByMarkCount : Ability
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x000A40B5 File Offset: 0x000A22B5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByMarkCount.Instance(owner, this);
		}

		// Token: 0x04002C46 RID: 11334
		[SerializeField]
		private MarkInfo _mark;

		// Token: 0x04002C47 RID: 11335
		[SerializeField]
		private string _key;

		// Token: 0x04002C48 RID: 11336
		[Tooltip("표식이 없을 때인 0개부터 시작")]
		[SerializeField]
		private float[] _damagePercents;

		// Token: 0x02000A69 RID: 2665
		public class Instance : AbilityInstance<ModifyDamageByMarkCount>
		{
			// Token: 0x0600379C RID: 14236 RVA: 0x000A40BE File Offset: 0x000A22BE
			public Instance(Character owner, ModifyDamageByMarkCount ability) : base(owner, ability)
			{
			}

			// Token: 0x0600379D RID: 14237 RVA: 0x00002191 File Offset: 0x00000391
			public override void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x0600379E RID: 14238 RVA: 0x000A40C8 File Offset: 0x000A22C8
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x0600379F RID: 14239 RVA: 0x000A40EB File Offset: 0x000A22EB
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x060037A0 RID: 14240 RVA: 0x000A410C File Offset: 0x000A230C
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (target.character == null || !damage.key.Equals(this.ability._key))
				{
					return false;
				}
				int num = math.min((int)target.character.mark.TakeAllStack(this.ability._mark), this.ability._mark.maxStack);
				damage.percentMultiplier *= (double)this.ability._damagePercents[num];
				return false;
			}
		}
	}
}
