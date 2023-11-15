using System;
using System.Collections.Generic;
using Characters;
using FX;
using FX.SpriteEffects;
using UnityEngine;

namespace Level.Altars
{
	// Token: 0x02000607 RID: 1543
	public class Altar : MonoBehaviour
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06001EEF RID: 7919 RVA: 0x0005DCE8 File Offset: 0x0005BEE8
		// (remove) Token: 0x06001EF0 RID: 7920 RVA: 0x0005DD20 File Offset: 0x0005BF20
		public event Action onDestroyed;

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x0005DD55 File Offset: 0x0005BF55
		public Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x0005DD5D File Offset: 0x0005BF5D
		// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x0005DD65 File Offset: 0x0005BF65
		public List<Character> characters { get; private set; } = new List<Character>();

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0005DD6E File Offset: 0x0005BF6E
		private void Awake()
		{
			this._prop = base.GetComponentInParent<Prop>();
			this._prop.onDestroy += this.Destroy;
			this._shaderEffect.Initialize();
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0005DD9E File Offset: 0x0005BF9E
		private void Destroy()
		{
			this._collider.enabled = false;
			Action action = this.onDestroyed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0005DDBC File Offset: 0x0005BFBC
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Altar.<>c__DisplayClass20_0 CS$<>8__locals1 = new Altar.<>c__DisplayClass20_0();
			Target component = collision.GetComponent<Target>();
			if (component == null || component.character == null)
			{
				return;
			}
			Character character = component.character;
			if (character.type == Character.Type.Trap || character.type == Character.Type.Dummy)
			{
				return;
			}
			CS$<>8__locals1.spawnedEffect = this._effect.Spawn(component.transform.position, component.character, 0f, 1f);
			CS$<>8__locals1.spawnedEffect.GetComponent<SpriteRenderer>();
			this._shaderEffect.Attach(component.character);
			component.character.stat.AttachValues(this._stat, new Stat.ValuesWithEvent.OnDetachDelegate(CS$<>8__locals1.<OnTriggerEnter2D>g__DespawnEffect|0));
			this.characters.Add(component.character);
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0005DE84 File Offset: 0x0005C084
		private void OnTriggerExit2D(Collider2D collision)
		{
			Target component = collision.GetComponent<Target>();
			if (!(component == null) && !(component.character == null))
			{
				CharacterHealth health = component.character.health;
				if (health == null || !health.dead)
				{
					component.character.stat.DetachValues(this._stat);
					this._shaderEffect.Detach(component.character);
					this.characters.Remove(component.character);
					return;
				}
			}
		}

		// Token: 0x04001A1E RID: 6686
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04001A1F RID: 6687
		[SerializeField]
		[GetComponent]
		private Animator _animator;

		// Token: 0x04001A20 RID: 6688
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04001A21 RID: 6689
		[SerializeField]
		private Target _target;

		// Token: 0x04001A22 RID: 6690
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x04001A23 RID: 6691
		[SerializeField]
		private Altar.ShaderEffect _shaderEffect;

		// Token: 0x04001A24 RID: 6692
		[SerializeField]
		private int _offset = 1;

		// Token: 0x04001A25 RID: 6693
		private Prop _prop;

		// Token: 0x02000608 RID: 1544
		[Serializable]
		private class ShaderEffect
		{
			// Token: 0x06001EF9 RID: 7929 RVA: 0x0005DF1C File Offset: 0x0005C11C
			public void Initialize()
			{
				this._effect = new GenericSpriteEffect(this._priority, 2.1474836E+09f, 1f, this._colorOverlay, this._colorBlend, this._outline, this._grayScale);
			}

			// Token: 0x06001EFA RID: 7930 RVA: 0x0005DF51 File Offset: 0x0005C151
			public void Attach(Character target)
			{
				if (target.spriteEffectStack == null)
				{
					return;
				}
				target.spriteEffectStack.Add(this._effect);
			}

			// Token: 0x06001EFB RID: 7931 RVA: 0x0005DF6D File Offset: 0x0005C16D
			public void Detach(Character target)
			{
				if (this._effect == null || target.spriteEffectStack == null)
				{
					return;
				}
				target.spriteEffectStack.Remove(this._effect);
			}

			// Token: 0x04001A27 RID: 6695
			[SerializeField]
			private int _priority;

			// Token: 0x04001A28 RID: 6696
			[SerializeField]
			private bool _proportionalToTenacity;

			// Token: 0x04001A29 RID: 6697
			[SerializeField]
			private GenericSpriteEffect.ColorOverlay _colorOverlay;

			// Token: 0x04001A2A RID: 6698
			[SerializeField]
			private GenericSpriteEffect.ColorBlend _colorBlend;

			// Token: 0x04001A2B RID: 6699
			[SerializeField]
			private GenericSpriteEffect.Outline _outline;

			// Token: 0x04001A2C RID: 6700
			[SerializeField]
			private GenericSpriteEffect.GrayScale _grayScale;

			// Token: 0x04001A2D RID: 6701
			private GenericSpriteEffect _effect;
		}
	}
}
