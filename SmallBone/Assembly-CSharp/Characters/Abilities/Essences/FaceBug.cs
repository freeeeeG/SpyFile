using System;
using FX;
using UnityEngine;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BE2 RID: 3042
	[Serializable]
	public sealed class FaceBug : Ability
	{
		// Token: 0x06003E8C RID: 16012 RVA: 0x000B5BE7 File Offset: 0x000B3DE7
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new FaceBug.Instance(owner, this);
		}

		// Token: 0x04003041 RID: 12353
		[SerializeField]
		private EffectInfo _attachEffectInfo;

		// Token: 0x04003042 RID: 12354
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x02000BE3 RID: 3043
		public class Instance : AbilityInstance<FaceBug>
		{
			// Token: 0x06003E8E RID: 16014 RVA: 0x000B5BF0 File Offset: 0x000B3DF0
			public Instance(Character owner, FaceBug ability) : base(owner, ability)
			{
			}

			// Token: 0x06003E8F RID: 16015 RVA: 0x000B5BFC File Offset: 0x000B3DFC
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this.ability._stat);
				this._spawned = this.ability._attachEffectInfo.Spawn(this.owner.collider.bounds.center, this.owner, 0f, 1f);
			}

			// Token: 0x06003E90 RID: 16016 RVA: 0x000B5C62 File Offset: 0x000B3E62
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
				if (this._spawned != null)
				{
					this._spawned.Stop();
					this._spawned = null;
				}
			}

			// Token: 0x06003E91 RID: 16017 RVA: 0x000B5CA0 File Offset: 0x000B3EA0
			public override void Refresh()
			{
				base.Refresh();
				if (this._spawned != null)
				{
					this._spawned.Stop();
					this._spawned = null;
				}
				this._spawned = this.ability._attachEffectInfo.Spawn(this.owner.collider.bounds.center, this.owner, 0f, 1f);
			}

			// Token: 0x06003E92 RID: 16018 RVA: 0x000B5D14 File Offset: 0x000B3F14
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				BoxCollider2D collider = this.owner.collider;
				if (this._before != collider.bounds)
				{
					this._spawned.transform.position = collider.bounds.center;
					this._before = collider.bounds;
				}
			}

			// Token: 0x04003043 RID: 12355
			private Bounds _before;

			// Token: 0x04003044 RID: 12356
			private EffectPoolInstance _spawned;
		}
	}
}
