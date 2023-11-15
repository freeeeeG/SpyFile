using System;
using FX.SpriteEffects;
using UnityEngine;

namespace FX
{
	// Token: 0x0200025C RID: 604
	public class SpriteEffectStack : MonoBehaviour, ISpriteEffectStack
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00020979 File Offset: 0x0001EB79
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x00020981 File Offset: 0x0001EB81
		public SpriteRenderer mainRenderer
		{
			get
			{
				return this._spriteRenderer;
			}
			set
			{
				this._spriteRenderer = value;
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0002098A File Offset: 0x0001EB8A
		protected virtual void Awake()
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			this.props = new MaterialPropertyBlock();
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000209B4 File Offset: 0x0001EBB4
		protected virtual void LateUpdate()
		{
			for (int i = this._effects.Count - 1; i >= 0; i--)
			{
				if (!this._effects[i].Update(this._chronometer.DeltaTime()))
				{
					this._effects.RemoveAt(i);
				}
			}
			SpriteEffect.@default.Apply(this._spriteRenderer);
			for (int j = 0; j < this._effects.Count; j++)
			{
				this._effects[j].Apply(this._spriteRenderer);
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00020A40 File Offset: 0x0001EC40
		public void Add(SpriteEffect effect)
		{
			this._effects.Add(effect.priority, effect);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00020A54 File Offset: 0x0001EC54
		public bool Contains(SpriteEffect effect)
		{
			return this._effects.Contains(effect);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00020A62 File Offset: 0x0001EC62
		public bool Remove(SpriteEffect effect)
		{
			return this._effects.Remove(effect);
		}

		// Token: 0x040009E0 RID: 2528
		private readonly PriorityList<SpriteEffect> _effects = new PriorityList<SpriteEffect>();

		// Token: 0x040009E1 RID: 2529
		protected Chronometer _chronometer;

		// Token: 0x040009E2 RID: 2530
		[SerializeField]
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x040009E3 RID: 2531
		private MaterialPropertyBlock props;
	}
}
