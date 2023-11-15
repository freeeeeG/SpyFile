using System;
using System.Linq;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Enemies
{
	// Token: 0x02000B95 RID: 2965
	[Serializable]
	public class CurseOfLight : Ability
	{
		// Token: 0x06003D68 RID: 15720 RVA: 0x000B2977 File Offset: 0x000B0B77
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CurseOfLight.Instance(owner, this);
		}

		// Token: 0x02000B96 RID: 2966
		public class Instance : AbilityInstance<CurseOfLight>
		{
			// Token: 0x17000D23 RID: 3363
			// (get) Token: 0x06003D6A RID: 15722 RVA: 0x000B0F17 File Offset: 0x000AF117
			private string floatingText
			{
				get
				{
					return Localization.GetLocalizedString("floating/curseoflight");
				}
			}

			// Token: 0x17000D24 RID: 3364
			// (get) Token: 0x06003D6B RID: 15723 RVA: 0x000B2980 File Offset: 0x000B0B80
			public override int iconStacks
			{
				get
				{
					return this._stacks;
				}
			}

			// Token: 0x17000D25 RID: 3365
			// (get) Token: 0x06003D6C RID: 15724 RVA: 0x000B0EA4 File Offset: 0x000AF0A4
			public override Sprite icon
			{
				get
				{
					return SavableAbilityResource.instance.curseIcon;
				}
			}

			// Token: 0x06003D6D RID: 15725 RVA: 0x000B2988 File Offset: 0x000B0B88
			public Instance(Character owner, CurseOfLight ability) : base(owner, ability)
			{
				this._effectInfo = new EffectInfo(SavableAbilityResource.instance.curseAttachEffect)
				{
					attachInfo = new EffectInfo.AttachInfo(true, false, 1, EffectInfo.AttachInfo.Pivot.Bottom),
					trackChildren = false,
					sortingLayerId = SortingLayer.layers.Last<SortingLayer>().id
				};
				this._soundInfo = new SoundInfo(SavableAbilityResource.instance.curseAttachSound);
			}

			// Token: 0x06003D6E RID: 15726 RVA: 0x000B29F5 File Offset: 0x000B0BF5
			protected override void OnAttach()
			{
				this._stat = CurseOfLight.Instance._statPerStack.Clone();
				this._stacks = 1;
				this.SpawnEffects();
			}

			// Token: 0x06003D6F RID: 15727 RVA: 0x000B2A14 File Offset: 0x000B0C14
			protected override void OnDetach()
			{
				Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(0f);
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06003D70 RID: 15728 RVA: 0x000B2A45 File Offset: 0x000B0C45
			public override void Refresh()
			{
				base.Refresh();
				this._stacks++;
				if (this._stacks == 3)
				{
					this.AttachStatBonus();
				}
				else if (this._stacks % 3 == 0)
				{
					this.UpdateStack();
				}
				this.SpawnEffects();
			}

			// Token: 0x06003D71 RID: 15729 RVA: 0x000B2A82 File Offset: 0x000B0C82
			private void AttachStatBonus()
			{
				Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack((float)this._stacks);
				this.owner.stat.AttachValues(this._stat);
				this.SpawnBuffText();
			}

			// Token: 0x06003D72 RID: 15730 RVA: 0x000B2ABC File Offset: 0x000B0CBC
			private void UpdateStack()
			{
				Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack((float)this._stacks);
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this._stat.values[i].value + (double)CurseOfLight.Instance.increasement;
				}
				this.owner.stat.SetNeedUpdate();
				this.SpawnBuffText();
			}

			// Token: 0x06003D73 RID: 15731 RVA: 0x000B2B40 File Offset: 0x000B0D40
			private void SpawnEffects()
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._soundInfo, this.owner.transform.position);
				this._effectInfo.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			}

			// Token: 0x06003D74 RID: 15732 RVA: 0x000B2B9C File Offset: 0x000B0D9C
			private void SpawnBuffText()
			{
				float num = (float)(this._stacks / 3) * CurseOfLight.Instance.increasement * 100f;
				string text = string.Format(this.floatingText, num);
				Vector3 center = this.owner.collider.bounds.center;
				Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(text, center, "#F2F2F2");
			}

			// Token: 0x04002F86 RID: 12166
			private static float increasement = 0.1f;

			// Token: 0x04002F87 RID: 12167
			private static readonly Stat.Values _statPerStack = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, (double)(1f + CurseOfLight.Instance.increasement))
			});

			// Token: 0x04002F88 RID: 12168
			private readonly EffectInfo _effectInfo;

			// Token: 0x04002F89 RID: 12169
			private readonly SoundInfo _soundInfo;

			// Token: 0x04002F8A RID: 12170
			private const int _phase = 3;

			// Token: 0x04002F8B RID: 12171
			private int _stacks;

			// Token: 0x04002F8C RID: 12172
			private Stat.Values _stat;
		}
	}
}
