using System;
using FX;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E8D RID: 3725
	public sealed class RangeHeal : CharacterOperation
	{
		// Token: 0x060049BA RID: 18874 RVA: 0x000D7530 File Offset: 0x000D5730
		public override void Initialize()
		{
			this._overlapper = new NonAllocOverlapper(this._maxCount);
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x000D7544 File Offset: 0x000D5744
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(owner.gameObject));
			ReadonlyBoundedList<Collider2D> results = this._overlapper.OverlapCollider(this._range).results;
			for (int i = 0; i < results.Count; i++)
			{
				Target component = results[i].GetComponent<Target>();
				Character character;
				if (component == null)
				{
					character = results[i].GetComponent<Character>();
				}
				else
				{
					character = component.character;
				}
				if (!(character == null) && (!this._exceptSelf || !(character == owner)))
				{
					this._targetEffect.Spawn(character.transform.position, character, 0f, 1f);
					character.health.Heal(this.GetAmount(character), true);
				}
			}
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x000D761C File Offset: 0x000D581C
		private double GetAmount(Character target)
		{
			RangeHeal.Type type = this._type;
			if (type == RangeHeal.Type.Percent)
			{
				return (double)this._amount.value * target.health.maximumHealth * 0.01;
			}
			if (type != RangeHeal.Type.Constnat)
			{
				return 0.0;
			}
			return (double)this._amount.value;
		}

		// Token: 0x040038EE RID: 14574
		[SerializeField]
		private EffectInfo _targetEffect;

		// Token: 0x040038EF RID: 14575
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x040038F0 RID: 14576
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040038F1 RID: 14577
		[SerializeField]
		private int _maxCount;

		// Token: 0x040038F2 RID: 14578
		[SerializeField]
		private bool _exceptSelf;

		// Token: 0x040038F3 RID: 14579
		[SerializeField]
		private RangeHeal.Type _type;

		// Token: 0x040038F4 RID: 14580
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x040038F5 RID: 14581
		private NonAllocOverlapper _overlapper;

		// Token: 0x02000E8E RID: 3726
		private enum Type
		{
			// Token: 0x040038F7 RID: 14583
			Percent,
			// Token: 0x040038F8 RID: 14584
			Constnat
		}
	}
}
