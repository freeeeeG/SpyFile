using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CE9 RID: 3305
	[Serializable]
	public sealed class OmenManaCycle : Ability
	{
		// Token: 0x060042D9 RID: 17113 RVA: 0x000C2AFC File Offset: 0x000C0CFC
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OmenManaCycle.Instance(owner, this);
		}

		// Token: 0x04003323 RID: 13091
		[SerializeField]
		private EffectInfo _effectOnA = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04003324 RID: 13092
		[SerializeField]
		private SoundInfo _effectASoundInfo;

		// Token: 0x04003325 RID: 13093
		[SerializeField]
		private Stat.Values _statOnA;

		// Token: 0x04003326 RID: 13094
		[SerializeField]
		private EffectInfo _effectOnB = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04003327 RID: 13095
		[SerializeField]
		private SoundInfo _effectBSoundInfo;

		// Token: 0x04003328 RID: 13096
		[SerializeField]
		private Stat.Values _statOnB;

		// Token: 0x02000CEA RID: 3306
		public sealed class Instance : AbilityInstance<OmenManaCycle>
		{
			// Token: 0x060042DB RID: 17115 RVA: 0x000C2B31 File Offset: 0x000C0D31
			public Instance(Character owner, OmenManaCycle ability) : base(owner, ability)
			{
			}

			// Token: 0x060042DC RID: 17116 RVA: 0x000C2B3C File Offset: 0x000C0D3C
			protected override void OnAttach()
			{
				this._isA = true;
				this.owner.stat.AttachValues(this.ability._statOnA);
				this._effectInstance = ((this.ability._effectOnA == null) ? null : this.ability._effectOnA.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._effectASoundInfo, this.owner.transform.position);
			}

			// Token: 0x060042DD RID: 17117 RVA: 0x000C2BD8 File Offset: 0x000C0DD8
			public override void Refresh()
			{
				if (this._isA)
				{
					if (this._effectInstance != null)
					{
						this._effectInstance.Stop();
					}
					this._effectInstance = ((this.ability._effectOnB == null) ? null : this.ability._effectOnB.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
					PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._effectBSoundInfo, this.owner.transform.position);
					this.owner.stat.DetachValues(this.ability._statOnA);
					this.owner.stat.AttachValues(this.ability._statOnB);
					this._isA = false;
					return;
				}
				if (this._effectInstance != null)
				{
					this._effectInstance.Stop();
				}
				this._effectInstance = ((this.ability._effectOnA == null) ? null : this.ability._effectOnA.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._effectASoundInfo, this.owner.transform.position);
				this.owner.stat.DetachValues(this.ability._statOnB);
				this.owner.stat.AttachValues(this.ability._statOnA);
				this._isA = true;
			}

			// Token: 0x060042DE RID: 17118 RVA: 0x000C2D78 File Offset: 0x000C0F78
			protected override void OnDetach()
			{
				if (this._effectInstance != null)
				{
					this._effectInstance.Stop();
					this._effectInstance = null;
				}
				this.owner.stat.DetachValues(this.ability._statOnA);
				this.owner.stat.DetachValues(this.ability._statOnB);
			}

			// Token: 0x04003329 RID: 13097
			private bool _isA;

			// Token: 0x0400332A RID: 13098
			private EffectPoolInstance _effectInstance;
		}
	}
}
