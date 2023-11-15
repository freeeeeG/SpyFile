using System;
using System.Collections;
using System.Linq;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B84 RID: 2948
	public sealed class Curse : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06003C08 RID: 15368 RVA: 0x000B0E93 File Offset: 0x000AF093
		// (set) Token: 0x06003C09 RID: 15369 RVA: 0x000B0E9B File Offset: 0x000AF09B
		public Character owner { get; private set; }

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003C0A RID: 15370 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003C0B RID: 15371 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003C0C RID: 15372 RVA: 0x000B0EA4 File Offset: 0x000AF0A4
		public Sprite icon
		{
			get
			{
				return SavableAbilityResource.instance.curseIcon;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06003C0D RID: 15373 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06003C0E RID: 15374 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06003C0F RID: 15375 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06003C10 RID: 15376 RVA: 0x000B0EBF File Offset: 0x000AF0BF
		public int iconStacks
		{
			get
			{
				return this._stack;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06003C11 RID: 15377 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06003C12 RID: 15378 RVA: 0x000B0573 File Offset: 0x000AE773
		public float duration
		{
			get
			{
				return 2.1474836E+09f;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06003C13 RID: 15379 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06003C14 RID: 15380 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x000B0EC7 File Offset: 0x000AF0C7
		// (set) Token: 0x06003C16 RID: 15382 RVA: 0x000B0ECF File Offset: 0x000AF0CF
		public int index { get; set; }

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06003C17 RID: 15383 RVA: 0x000B0ED8 File Offset: 0x000AF0D8
		// (set) Token: 0x06003C18 RID: 15384 RVA: 0x000B0EE0 File Offset: 0x000AF0E0
		public float remainTime { get; set; }

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x000B0EE9 File Offset: 0x000AF0E9
		// (set) Token: 0x06003C1A RID: 15386 RVA: 0x000B0EF2 File Offset: 0x000AF0F2
		public float stack
		{
			get
			{
				return (float)this._stack;
			}
			set
			{
				this._stack = (int)value;
				if (this._stack <= 0)
				{
					return;
				}
				CoroutineProxy.instance.StartCoroutine(this.CLoadStack());
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x000B0F17 File Offset: 0x000AF117
		private string floatingText
		{
			get
			{
				return Localization.GetLocalizedString("floating/curseoflight");
			}
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x000B0F23 File Offset: 0x000AF123
		public void Attach()
		{
			this._statClone = this._statPerStack.Clone();
			this.SpawnEffects();
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x000B0F3C File Offset: 0x000AF13C
		public void Detach()
		{
			this._stack = 1;
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(0f);
			this.owner.stat.DetachValues(this._statClone);
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x000B0F74 File Offset: 0x000AF174
		public void Refresh()
		{
			this._stack++;
			if (this._stack == 3)
			{
				this.AttachStatBonus();
			}
			else if (this._stack % 3 == 0)
			{
				this.UpdateStack();
			}
			this.SpawnEffects();
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x000B0FAC File Offset: 0x000AF1AC
		public IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			this._effectInfo = new EffectInfo(SavableAbilityResource.instance.curseAttachEffect)
			{
				attachInfo = new EffectInfo.AttachInfo(true, false, 1, EffectInfo.AttachInfo.Pivot.Bottom),
				trackChildren = false,
				sortingLayerId = SortingLayer.layers.Last<SortingLayer>().id
			};
			this._soundInfo = new SoundInfo(SavableAbilityResource.instance.curseAttachSound);
			return this;
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x000B1019 File Offset: 0x000AF219
		private void AttachStatBonus()
		{
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack((float)this._stack);
			this.owner.stat.AttachValues(this._statClone);
			this.SpawnBuffText();
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x000B1054 File Offset: 0x000AF254
		private void UpdateStack()
		{
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack((float)this._stack);
			for (int i = 0; i < this._statClone.values.Length; i++)
			{
				this._statClone.values[i].value = this._statClone.values[i].value + (double)Curse.valuePerStack;
			}
			this.owner.stat.SetNeedUpdate();
			this.SpawnBuffText();
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x000B10D5 File Offset: 0x000AF2D5
		private IEnumerator CLoadStack()
		{
			while (this.owner == null)
			{
				yield return null;
			}
			this.LoadStack();
			yield break;
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x000B10E4 File Offset: 0x000AF2E4
		private void LoadStack()
		{
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack((float)this._stack);
			this.owner.stat.AttachValues(this._statClone);
			for (int i = 0; i < this._statClone.values.Length; i++)
			{
				this._statClone.values[i].value = this._statClone.values[i].value + (double)(Curse.valuePerStack * (float)Mathf.FloorToInt((float)(this._stack / 3)));
			}
			this.owner.stat.SetNeedUpdate();
			this.SpawnBuffText();
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x000B118C File Offset: 0x000AF38C
		private void SpawnEffects()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._soundInfo, this.owner.transform.position);
			this._effectInfo.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x000B11E8 File Offset: 0x000AF3E8
		private void SpawnBuffText()
		{
			float num = (float)(this._stack / 3) * Curse.valuePerStack * 100f;
			string text = string.Format(this.floatingText, num);
			Vector3 center = this.owner.collider.bounds.center;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(text, center, "#F2F2F2");
		}

		// Token: 0x04002F2A RID: 12074
		private static float valuePerStack = 0.1f;

		// Token: 0x04002F2B RID: 12075
		private const int _stackUpStep = 3;

		// Token: 0x04002F2C RID: 12076
		private int _stack = 1;

		// Token: 0x04002F2D RID: 12077
		private Stat.Values _statPerStack = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, (double)(1f + Curse.valuePerStack))
		});

		// Token: 0x04002F2E RID: 12078
		private Stat.Values _statClone;

		// Token: 0x04002F2F RID: 12079
		private EffectInfo _effectInfo;

		// Token: 0x04002F30 RID: 12080
		private SoundInfo _soundInfo;
	}
}
