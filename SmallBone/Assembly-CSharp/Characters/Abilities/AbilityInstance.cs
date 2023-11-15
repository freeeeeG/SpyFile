using System;
using FX;
using FX.SpriteEffects;
using Singletons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000992 RID: 2450
	public abstract class AbilityInstance : IAbilityInstance
	{
		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x0009AF09 File Offset: 0x00099109
		// (set) Token: 0x06003485 RID: 13445 RVA: 0x0009AF11 File Offset: 0x00099111
		public float remainTime { get; set; }

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x0009AF1A File Offset: 0x0009911A
		// (set) Token: 0x06003487 RID: 13447 RVA: 0x0009AF22 File Offset: 0x00099122
		public bool attached { get; private set; }

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06003488 RID: 13448 RVA: 0x0009AF2B File Offset: 0x0009912B
		public virtual Sprite icon
		{
			get
			{
				return this.ability.defaultIcon;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x0009AF38 File Offset: 0x00099138
		public virtual float iconFillAmount
		{
			get
			{
				if (this.ability.duration != float.PositiveInfinity)
				{
					return 1f - this.remainTime / this.ability.duration;
				}
				return 0f;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x0009AF6A File Offset: 0x0009916A
		// (set) Token: 0x0600348B RID: 13451 RVA: 0x0009AF77 File Offset: 0x00099177
		public bool iconFillInversed
		{
			get
			{
				return this.ability.iconFillInversed;
			}
			set
			{
				this.ability.iconFillInversed = value;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x0009AF85 File Offset: 0x00099185
		// (set) Token: 0x0600348D RID: 13453 RVA: 0x0009AF92 File Offset: 0x00099192
		public bool iconFillFlipped
		{
			get
			{
				return this.ability.iconFillFlipped;
			}
			set
			{
				this.ability.iconFillFlipped = value;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x00018EC5 File Offset: 0x000170C5
		public virtual int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x0009AFA0 File Offset: 0x000991A0
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06003490 RID: 13456 RVA: 0x0009AFB2 File Offset: 0x000991B2
		Character IAbilityInstance.owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x0009AFBA File Offset: 0x000991BA
		IAbility IAbilityInstance.ability
		{
			get
			{
				return this.ability;
			}
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x0009AFC2 File Offset: 0x000991C2
		public AbilityInstance(Character owner, Ability ability)
		{
			this.owner = owner;
			this.ability = ability;
			this.remainTime = ability.duration;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x0009AFE4 File Offset: 0x000991E4
		public virtual void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x0009AFF4 File Offset: 0x000991F4
		public virtual void Refresh()
		{
			this.remainTime = this.ability.duration;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x0009B008 File Offset: 0x00099208
		public void Attach()
		{
			this.attached = true;
			this._loopEffect = ((this.ability.loopEffect == null) ? null : this.ability.loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
			if (this.owner.spriteEffectStack != null && this.ability.spriteEffect != null && this.ability.spriteEffect.enabled)
			{
				this._spriteEffect = this.ability.spriteEffect.CreateInstance();
				this.owner.spriteEffectStack.Add(this._spriteEffect);
			}
			EffectInfo effectOnAttach = this.ability.effectOnAttach;
			if (effectOnAttach != null)
			{
				effectOnAttach.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			}
			if (this.ability.soundOnAttach != null && this.ability.soundOnAttach.audioClip != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability.soundOnAttach, this.owner.transform.position);
			}
			this.OnAttach();
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x0009B144 File Offset: 0x00099344
		public void Detach()
		{
			this.attached = false;
			if (this.owner == null)
			{
				return;
			}
			if (this._loopEffect != null)
			{
				this._loopEffect.Stop();
				this._loopEffect = null;
			}
			if (this.owner.spriteEffectStack != null && this.ability.spriteEffect != null && this.ability.spriteEffect.enabled)
			{
				this.owner.spriteEffectStack.Remove(this._spriteEffect);
			}
			EffectInfo effectOnDetach = this.ability.effectOnDetach;
			if (effectOnDetach != null)
			{
				effectOnDetach.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			}
			if (this.ability.soundOnAttach != null && this.ability.soundOnDetach.audioClip != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability.soundOnDetach, this.owner.transform.position);
			}
			this.OnDetach();
		}

		// Token: 0x06003497 RID: 13463
		protected abstract void OnAttach();

		// Token: 0x06003498 RID: 13464
		protected abstract void OnDetach();

		// Token: 0x04002A6B RID: 10859
		private EffectPoolInstance _loopEffect;

		// Token: 0x04002A6C RID: 10860
		private GenericSpriteEffect _spriteEffect;

		// Token: 0x04002A6D RID: 10861
		public readonly Character owner;

		// Token: 0x04002A6E RID: 10862
		public readonly Ability ability;
	}
}
