using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C30 RID: 3120
	[Serializable]
	public sealed class OverrideFinalStatValues : Ability
	{
		// Token: 0x06004014 RID: 16404 RVA: 0x000BA0E5 File Offset: 0x000B82E5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OverrideFinalStatValues.Instance(owner, this);
		}

		// Token: 0x0400314C RID: 12620
		[SerializeField]
		[Header("최종 고정 스텟값, Category는 사용되지 않음")]
		private Stat.Values _statValues;

		// Token: 0x0400314D RID: 12621
		[SerializeField]
		private int _priority;

		// Token: 0x02000C31 RID: 3121
		public sealed class Instance : AbilityInstance<OverrideFinalStatValues>
		{
			// Token: 0x06004016 RID: 16406 RVA: 0x000BA0EE File Offset: 0x000B82EE
			public Instance(Character owner, OverrideFinalStatValues ability) : base(owner, ability)
			{
			}

			// Token: 0x06004017 RID: 16407 RVA: 0x000BA0F8 File Offset: 0x000B82F8
			protected override void OnAttach()
			{
				this.owner.stat.onUpdated.Add(this.ability._priority, new Stat.OnUpdatedDelegate(this.HandleOnStatUpdated));
			}

			// Token: 0x06004018 RID: 16408 RVA: 0x000BA128 File Offset: 0x000B8328
			private double[] HandleOnStatUpdated(double[,] values)
			{
				double[] array = new double[Stat.Kind.values.Count];
				Stat.Value[] values2 = this.ability._statValues.values;
				for (int i = 0; i < Stat.Kind.values.Count; i++)
				{
					array[i] = values[Stat.Category.Final.index, i];
				}
				for (int j = 0; j < values2.Length; j++)
				{
					array[values2[j].kindIndex] = values2[j].value;
				}
				return array;
			}

			// Token: 0x06004019 RID: 16409 RVA: 0x000BA1A0 File Offset: 0x000B83A0
			protected override void OnDetach()
			{
				this.owner.stat.onUpdated.Remove(new Stat.OnUpdatedDelegate(this.HandleOnStatUpdated));
			}
		}
	}
}
