using System;
using System.Linq;
using FX;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B72 RID: 2930
	public sealed class StatusEffect
	{
		// Token: 0x06003B2E RID: 15150 RVA: 0x000AE7A9 File Offset: 0x000AC9A9
		public static StatusEffect.Stun GetDefaultStunEffectHandler(Character character)
		{
			return new StatusEffect.Stun(character);
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000AE7B1 File Offset: 0x000AC9B1
		public static StatusEffect.Freeze GetDefaultFreezeEffectHanlder(Character character)
		{
			return new StatusEffect.Freeze(character);
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public static StatusEffect.Burn GetDefaultBurnEffectHanlder(Character character)
		{
			return new StatusEffect.Burn(character);
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000AE7C1 File Offset: 0x000AC9C1
		public static StatusEffect.Poison GetDefaultPoisonEffectHanlder(Character character)
		{
			return new StatusEffect.Poison(character);
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000AE7C9 File Offset: 0x000AC9C9
		public static StatusEffect.Wound GetDefaultWoundEffectHanlder(Character character)
		{
			return new StatusEffect.Wound(character);
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000AE7D1 File Offset: 0x000AC9D1
		public static StatusEffect.Stun CopyFrom(StatusEffect.Stun stun, Character character)
		{
			return new StatusEffect.Stun(stun, character);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000AE7DA File Offset: 0x000AC9DA
		public static StatusEffect.GigantEnemyFreeze CopyFrom(StatusEffect.GigantEnemyFreeze freeze, Character character)
		{
			return new StatusEffect.GigantEnemyFreeze(freeze, character);
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000AE7E3 File Offset: 0x000AC9E3
		public static StatusEffect.Burn CopyFrom(StatusEffect.Burn freeze, Character character)
		{
			return new StatusEffect.Burn(freeze, character);
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000AE7EC File Offset: 0x000AC9EC
		public static StatusEffect.Poison CopyFrom(StatusEffect.Poison freeze, Character character)
		{
			return new StatusEffect.Poison(freeze, character);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000AE7F5 File Offset: 0x000AC9F5
		public static StatusEffect.Wound CopyFrom(StatusEffect.Wound freeze, Character character)
		{
			return new StatusEffect.Wound(freeze, character);
		}

		// Token: 0x02000B73 RID: 2931
		public abstract class EffectHandler
		{
			// Token: 0x06003B39 RID: 15161
			public abstract void Initialize(Character character);

			// Token: 0x06003B3A RID: 15162 RVA: 0x00002191 File Offset: 0x00000391
			public virtual void UpdateTime(float detaTime)
			{
			}

			// Token: 0x06003B3B RID: 15163
			public abstract void Dispose();

			// Token: 0x06003B3C RID: 15164
			public abstract void HandleOnAttach(Character attacker, Character target);

			// Token: 0x06003B3D RID: 15165
			public abstract void HandleOnDetach(Character attacker, Character target);

			// Token: 0x06003B3E RID: 15166
			public abstract void HandleOnRefresh(Character attacker, Character target);
		}

		// Token: 0x02000B74 RID: 2932
		[Serializable]
		public class Stun : StatusEffect.EffectHandler
		{
			// Token: 0x06003B40 RID: 15168 RVA: 0x000AE7FE File Offset: 0x000AC9FE
			public Stun(Character character)
			{
				this.Initialize(character);
			}

			// Token: 0x06003B41 RID: 15169 RVA: 0x000AE80D File Offset: 0x000ACA0D
			public Stun(StatusEffect.Stun stun, Character character)
			{
				this._range = stun._range;
				this.Initialize(character);
			}

			// Token: 0x06003B42 RID: 15170 RVA: 0x000AE828 File Offset: 0x000ACA28
			public override void Initialize(Character character)
			{
				this._character = character;
				if (this._range == null)
				{
					this._range = character.collider;
				}
				this._effect = new EffectInfo(CommonResource.instance.stunEffect)
				{
					attachInfo = new EffectInfo.AttachInfo(true, false, 1, EffectInfo.AttachInfo.Pivot.Top),
					loop = true,
					trackChildren = true
				};
			}

			// Token: 0x06003B43 RID: 15171 RVA: 0x000AE888 File Offset: 0x000ACA88
			public override void HandleOnAttach(Character attacker, Character target)
			{
				this._effect.DespawnChildren();
				this._effect.Spawn(target.transform.position, target, 0f, 1f);
				this.SpawnFloatingText();
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.stun.attachSound, target.transform.position);
			}

			// Token: 0x06003B44 RID: 15172 RVA: 0x000AE8ED File Offset: 0x000ACAED
			public override void HandleOnRefresh(Character attacker, Character target)
			{
				this.SpawnFloatingText();
			}

			// Token: 0x06003B45 RID: 15173 RVA: 0x000AE8F5 File Offset: 0x000ACAF5
			public override void HandleOnDetach(Character attacker, Character target)
			{
				this._effect.DespawnChildren();
			}

			// Token: 0x06003B46 RID: 15174 RVA: 0x00002191 File Offset: 0x00000391
			public override void Dispose()
			{
			}

			// Token: 0x06003B47 RID: 15175 RVA: 0x000AE904 File Offset: 0x000ACB04
			private void SpawnFloatingText()
			{
				Vector2 v = MMMaths.RandomPointWithinBounds(this._range.bounds);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(StatusEffect.Stun._floatingTextKey), v, StatusEffect.Stun._floatingTextColor);
			}

			// Token: 0x04002EC0 RID: 11968
			private static readonly string _floatingTextKey = "floating/status/stun";

			// Token: 0x04002EC1 RID: 11969
			private static readonly string _floatingTextColor = "#ffffff";

			// Token: 0x04002EC2 RID: 11970
			[SerializeField]
			private Collider2D _range;

			// Token: 0x04002EC3 RID: 11971
			private Character _character;

			// Token: 0x04002EC4 RID: 11972
			private EffectInfo _effect;
		}

		// Token: 0x02000B75 RID: 2933
		public abstract class FreezeHandler : StatusEffect.EffectHandler
		{
			// Token: 0x06003B49 RID: 15177
			public abstract void HandleOnTakeDamage(Character attacker, Character target);
		}

		// Token: 0x02000B76 RID: 2934
		[Serializable]
		public class Freeze : StatusEffect.FreezeHandler
		{
			// Token: 0x06003B4B RID: 15179 RVA: 0x000AE964 File Offset: 0x000ACB64
			static Freeze()
			{
				StatusEffect.Freeze._particles[Character.SizeForEffect.Small] = CommonResource.instance.freezeSmallParticle;
				StatusEffect.Freeze._particles[Character.SizeForEffect.Medium] = CommonResource.instance.freezeMediumParticle;
				StatusEffect.Freeze._particles[Character.SizeForEffect.Large] = CommonResource.instance.freezeLargeParticle;
				StatusEffect.Freeze._particles[Character.SizeForEffect.ExtraLarge] = CommonResource.instance.freezeLargeParticle;
			}

			// Token: 0x06003B4C RID: 15180 RVA: 0x000AEA0D File Offset: 0x000ACC0D
			public Freeze(Character character)
			{
				this.Initialize(character);
			}

			// Token: 0x06003B4D RID: 15181 RVA: 0x000AEA1C File Offset: 0x000ACC1C
			public override void Initialize(Character character)
			{
				this._character = character;
				if (character.collider != null)
				{
					RuntimeAnimatorController freezeAnimator = CommonResource.instance.GetFreezeAnimator(character.collider.size * 32f);
					this._effect = new EffectInfo(freezeAnimator)
					{
						attachInfo = new EffectInfo.AttachInfo(true, false, 1, EffectInfo.AttachInfo.Pivot.Center),
						loop = true,
						trackChildren = true,
						sortingLayerId = SortingLayer.layers.Last<SortingLayer>().id
					};
				}
			}

			// Token: 0x06003B4E RID: 15182 RVA: 0x000AEAA0 File Offset: 0x000ACCA0
			public override void UpdateTime(float deltaTime)
			{
				if (deltaTime <= 0f)
				{
					return;
				}
				if (this._character.status.freeze.remainTime < CharacterStatusSetting.instance.freeze.breakDuration && this._character.status.freeze.remainTime > 0f && this._frontEffect != null)
				{
					Vector3 localPosition = this._basePosition + UnityEngine.Random.insideUnitCircle * 0.1f;
					this._frontEffect.transform.localPosition = localPosition;
					this._backEffect.transform.localPosition = localPosition;
				}
			}

			// Token: 0x06003B4F RID: 15183 RVA: 0x00002191 File Offset: 0x00000391
			public override void Dispose()
			{
			}

			// Token: 0x06003B50 RID: 15184 RVA: 0x000AEB48 File Offset: 0x000ACD48
			public override void HandleOnAttach(Character attacker, Character target)
			{
				target.spriteEffectStack.Add(StatusEffect.Freeze._colorOverlay);
				this._effect.DespawnChildren();
				this._frontEffect = this._effect.Spawn(target.transform.position, target, 0f, 1f);
				this._frontEffect.renderer.sharedMaterial = MaterialResource.effect_linearDodge;
				this._frontEffect.renderer.sortingLayerID = SortingLayer.layers.Last<SortingLayer>().id;
				this._backEffect = this._effect.Spawn(target.transform.position, target, 0f, 1f);
				this._backEffect.renderer.sortingLayerID = SortingLayer.layers[0].id;
				this._basePosition = this._frontEffect.transform.localPosition;
				this.SpawnFloatingText();
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.attachSound, this._character.transform.position);
			}

			// Token: 0x06003B51 RID: 15185 RVA: 0x000AEC5B File Offset: 0x000ACE5B
			public override void HandleOnRefresh(Character attacker, Character target)
			{
				this.SpawnFloatingText();
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.attachSound, this._character.transform.position);
			}

			// Token: 0x06003B52 RID: 15186 RVA: 0x000AEC90 File Offset: 0x000ACE90
			public override void HandleOnDetach(Character attacker, Character target)
			{
				target.spriteEffectStack.Remove(StatusEffect.Freeze._colorOverlay);
				this._effect.DespawnChildren();
				StatusEffect.Freeze._particles[target.sizeForEffect].Emit(target.transform.position, target.collider.bounds, Vector2.zero, true);
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.detachSound, this._character.transform.position);
			}

			// Token: 0x06003B53 RID: 15187 RVA: 0x000AED1C File Offset: 0x000ACF1C
			private void SpawnFloatingText()
			{
				Vector2 v = MMMaths.RandomPointWithinBounds(this._character.collider.bounds);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(StatusEffect.Freeze._floatingTextKey), v, StatusEffect.Freeze._floatingTextColor);
			}

			// Token: 0x06003B54 RID: 15188 RVA: 0x000AED63 File Offset: 0x000ACF63
			public override void HandleOnTakeDamage(Character attacker, Character target)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.hitSound, this._character.transform.position);
			}

			// Token: 0x04002EC5 RID: 11973
			private static readonly ColorBlend _colorOverlay = new ColorBlend(100, new Color(0f, 0.14509805f, 0.73333335f, 1f), 0f);

			// Token: 0x04002EC6 RID: 11974
			private static readonly EnumArray<Character.SizeForEffect, ParticleEffectInfo> _particles = new EnumArray<Character.SizeForEffect, ParticleEffectInfo>();

			// Token: 0x04002EC7 RID: 11975
			private static readonly string _floatingTextKey = "floating/status/freeze";

			// Token: 0x04002EC8 RID: 11976
			private static readonly string _floatingTextColor = "#04FFE6";

			// Token: 0x04002EC9 RID: 11977
			private EffectInfo _effect;

			// Token: 0x04002ECA RID: 11978
			private EffectPoolInstance _frontEffect;

			// Token: 0x04002ECB RID: 11979
			private EffectPoolInstance _backEffect;

			// Token: 0x04002ECC RID: 11980
			private Vector3 _basePosition;

			// Token: 0x04002ECD RID: 11981
			private Character _character;
		}

		// Token: 0x02000B77 RID: 2935
		[Serializable]
		public class GigantEnemyFreeze : StatusEffect.FreezeHandler
		{
			// Token: 0x06003B55 RID: 15189 RVA: 0x000AED8F File Offset: 0x000ACF8F
			static GigantEnemyFreeze()
			{
				StatusEffect.GigantEnemyFreeze._particle = CommonResource.instance.freezeLargeParticle;
			}

			// Token: 0x06003B56 RID: 15190 RVA: 0x000AEDB4 File Offset: 0x000ACFB4
			public GigantEnemyFreeze(Character character)
			{
				this.Initialize(character);
			}

			// Token: 0x06003B57 RID: 15191 RVA: 0x000AEDE4 File Offset: 0x000ACFE4
			public GigantEnemyFreeze(StatusEffect.GigantEnemyFreeze copyFrom, Character character)
			{
				this._shakeIntensity = copyFrom._shakeIntensity;
				this._shakeDuration = copyFrom._shakeDuration;
				this._shakeStartTime = copyFrom._shakeStartTime;
				this._spriteEffect = copyFrom._spriteEffect;
				this._toggleEffects = copyFrom._toggleEffects;
				this.Initialize(character);
			}

			// Token: 0x06003B58 RID: 15192 RVA: 0x000AEE5C File Offset: 0x000AD05C
			public override void Initialize(Character character)
			{
				this._character = character;
				this._originPositions = new Vector2[this._toggleEffects.Length];
				for (int i = 0; i < this._toggleEffects.Length; i++)
				{
					this._originPositions[i] = this._toggleEffects[i].transform.localPosition;
				}
			}

			// Token: 0x06003B59 RID: 15193 RVA: 0x000AEEBC File Offset: 0x000AD0BC
			public override void UpdateTime(float detaTime)
			{
				if (this._character.health.dead)
				{
					return;
				}
				if (this._character.status.freeze.remainTime < this._shakeStartTime && this._character.status.freeze.remainTime > this._shakeStartTime - this._shakeDuration)
				{
					this.Shake();
				}
			}

			// Token: 0x06003B5A RID: 15194 RVA: 0x000AEF24 File Offset: 0x000AD124
			public override void Dispose()
			{
				Collider2D[] toggleEffects = this._toggleEffects;
				for (int i = 0; i < toggleEffects.Length; i++)
				{
					toggleEffects[i].gameObject.SetActive(false);
				}
			}

			// Token: 0x06003B5B RID: 15195 RVA: 0x000AEF54 File Offset: 0x000AD154
			public override void HandleOnAttach(Character attacker, Character target)
			{
				this.SpawnFloatingText();
				for (int i = 0; i < this._toggleEffects.Length; i++)
				{
					this._toggleEffects[i].transform.localPosition = this._originPositions[i];
					this._toggleEffects[i].gameObject.SetActive(true);
					this._spriteEffect.Spawn(this._toggleEffects[i].transform.position, 0f, 1f);
				}
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.attachSound, this._character.transform.position);
			}

			// Token: 0x06003B5C RID: 15196 RVA: 0x000AF002 File Offset: 0x000AD202
			public override void HandleOnRefresh(Character attacker, Character target)
			{
				this.SpawnFloatingText();
			}

			// Token: 0x06003B5D RID: 15197 RVA: 0x000AF00C File Offset: 0x000AD20C
			public override void HandleOnDetach(Character attacker, Character target)
			{
				foreach (Collider2D collider2D in this._toggleEffects)
				{
					collider2D.gameObject.SetActive(false);
					StatusEffect.GigantEnemyFreeze._particle.Emit(collider2D.transform.position, collider2D.bounds, Vector2.zero, true);
					this._spriteEffect.Spawn(collider2D.transform.position, 0f, 1f);
				}
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.detachSound, this._character.transform.position);
			}

			// Token: 0x06003B5E RID: 15198 RVA: 0x000AF0AF File Offset: 0x000AD2AF
			public override void HandleOnTakeDamage(Character attacker, Character target)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.hitSound, this._character.transform.position);
			}

			// Token: 0x06003B5F RID: 15199 RVA: 0x000AF0DC File Offset: 0x000AD2DC
			private void Shake()
			{
				for (int i = 0; i < this._toggleEffects.Length; i++)
				{
					Vector2 v = this._originPositions[i] + UnityEngine.Random.insideUnitCircle * this._shakeIntensity;
					this._toggleEffects[i].transform.localPosition = v;
				}
			}

			// Token: 0x06003B60 RID: 15200 RVA: 0x000AF138 File Offset: 0x000AD338
			private void SpawnFloatingText()
			{
				Vector2 v = MMMaths.RandomPointWithinBounds(this._character.collider.bounds);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(StatusEffect.GigantEnemyFreeze._floatingTextKey), v, StatusEffect.GigantEnemyFreeze._floatingTextColor);
			}

			// Token: 0x04002ECE RID: 11982
			private static readonly ParticleEffectInfo _particle;

			// Token: 0x04002ECF RID: 11983
			private static readonly string _floatingTextKey = "floating/status/freeze";

			// Token: 0x04002ED0 RID: 11984
			private static readonly string _floatingTextColor = "#04FFE6";

			// Token: 0x04002ED1 RID: 11985
			[Range(0f, 1f)]
			[SerializeField]
			private float _shakeIntensity = 0.05f;

			// Token: 0x04002ED2 RID: 11986
			[SerializeField]
			private float _shakeDuration = 0.2f;

			// Token: 0x04002ED3 RID: 11987
			[SerializeField]
			[Tooltip("빙결 끝나지 x초 전")]
			private float _shakeStartTime = 1.5f;

			// Token: 0x04002ED4 RID: 11988
			[SerializeField]
			private EffectInfo _spriteEffect;

			// Token: 0x04002ED5 RID: 11989
			[SerializeField]
			private Collider2D[] _toggleEffects;

			// Token: 0x04002ED6 RID: 11990
			private Vector2[] _originPositions;

			// Token: 0x04002ED7 RID: 11991
			private Character _character;
		}

		// Token: 0x02000B78 RID: 2936
		public abstract class BurnHandler : StatusEffect.EffectHandler
		{
			// Token: 0x06003B61 RID: 15201
			public abstract void HandleOnTookBurnDamage(Character attacker, Character target);

			// Token: 0x06003B62 RID: 15202
			public abstract void HandleOnTookEmberDamage(Character attacker, Character target);
		}

		// Token: 0x02000B79 RID: 2937
		[Serializable]
		public class Burn : StatusEffect.BurnHandler
		{
			// Token: 0x06003B64 RID: 15204 RVA: 0x000AF17F File Offset: 0x000AD37F
			static Burn()
			{
				StatusEffect.Burn._hitParticle = CommonResource.instance.hitParticle;
			}

			// Token: 0x06003B65 RID: 15205 RVA: 0x000AF1A4 File Offset: 0x000AD3A4
			public Burn(Character character)
			{
				this.Initialize(character);
			}

			// Token: 0x06003B66 RID: 15206 RVA: 0x000AF1C8 File Offset: 0x000AD3C8
			public Burn(StatusEffect.Burn copyFrom, Character character)
			{
				this._range = copyFrom._range;
				this._scale = copyFrom._scale;
				this.Initialize(character);
			}

			// Token: 0x06003B67 RID: 15207 RVA: 0x000AF204 File Offset: 0x000AD404
			public override void Initialize(Character character)
			{
				this._character = character;
				if (this._range == null)
				{
					this._range = character.collider;
				}
				this._burnEffect = new EffectInfo(CharacterStatusSetting.instance.burn.loopEffect)
				{
					angle = new CustomAngle(0f, 360f),
					scale = this._scale
				};
				this._emberEffect = new EffectInfo(CharacterStatusSetting.instance.burn.loopEffect)
				{
					angle = new CustomAngle(0f, 360f),
					scale = new CustomFloat(0.4f, 0.6f)
				};
				this._spriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.burn.duration, 1f, CharacterStatusSetting.instance.burn.colorOverlay, CharacterStatusSetting.instance.burn.colorBlend, CharacterStatusSetting.instance.burn.outline, CharacterStatusSetting.instance.burn.grayScale);
			}

			// Token: 0x06003B68 RID: 15208 RVA: 0x00002191 File Offset: 0x00000391
			public override void Dispose()
			{
			}

			// Token: 0x06003B69 RID: 15209 RVA: 0x000AF30D File Offset: 0x000AD50D
			public override void HandleOnAttach(Character attacker, Character target)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.burn.attachSound, target.transform.position);
				this.SpawnFloatingText();
			}

			// Token: 0x06003B6A RID: 15210 RVA: 0x000AF33A File Offset: 0x000AD53A
			public override void HandleOnRefresh(Character attacker, Character target)
			{
				this.SpawnFloatingText();
			}

			// Token: 0x06003B6B RID: 15211 RVA: 0x000AF342 File Offset: 0x000AD542
			public override void HandleOnDetach(Character attacker, Character target)
			{
				this._character.spriteEffectStack.Remove(this._spriteEffect);
			}

			// Token: 0x06003B6C RID: 15212 RVA: 0x000AF35C File Offset: 0x000AD55C
			public override void HandleOnTookBurnDamage(Character attacker, Character target)
			{
				this._character.spriteEffectStack.Remove(this._spriteEffect);
				this._spriteEffect.Reset();
				this._character.spriteEffectStack.Add(this._spriteEffect);
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.burn.attackSound, target.transform.position);
				for (int i = 0; i < 3; i++)
				{
					this._burnEffect.Spawn(MMMaths.RandomPointWithinBounds(this._range.bounds), 0f, 1f);
				}
				StatusEffect.Burn._hitParticle.Emit(this._range.transform.position, this._range.bounds, Vector2.zero, true);
			}

			// Token: 0x06003B6D RID: 15213 RVA: 0x000AF430 File Offset: 0x000AD630
			public override void HandleOnTookEmberDamage(Character attacker, Character target)
			{
				this._emberEffect.Spawn(MMMaths.RandomPointWithinBounds(target.collider.bounds), 0f, 1f);
				StatusEffect.Burn._hitParticle.Emit(target.transform.position, target.collider.bounds, Vector2.zero, true);
			}

			// Token: 0x06003B6E RID: 15214 RVA: 0x000AF494 File Offset: 0x000AD694
			public void SpawnFloatingText()
			{
				Vector2 v = MMMaths.RandomPointWithinBounds(this._range.bounds);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(StatusEffect.Burn._floatingTextKey), v, StatusEffect.Burn._floatingTextColor);
			}

			// Token: 0x04002ED8 RID: 11992
			private static readonly ParticleEffectInfo _hitParticle;

			// Token: 0x04002ED9 RID: 11993
			private static readonly string _floatingTextKey = "floating/status/burn";

			// Token: 0x04002EDA RID: 11994
			private static readonly string _floatingTextColor = "#DD4900";

			// Token: 0x04002EDB RID: 11995
			[SerializeField]
			private Collider2D _range;

			// Token: 0x04002EDC RID: 11996
			[SerializeField]
			private CustomFloat _scale = new CustomFloat(0.3f, 0.5f);

			// Token: 0x04002EDD RID: 11997
			private EffectInfo _burnEffect;

			// Token: 0x04002EDE RID: 11998
			private EffectInfo _emberEffect;

			// Token: 0x04002EDF RID: 11999
			private GenericSpriteEffect _spriteEffect;

			// Token: 0x04002EE0 RID: 12000
			private Character _character;
		}

		// Token: 0x02000B7A RID: 2938
		public abstract class PoisonHandler : StatusEffect.EffectHandler
		{
			// Token: 0x06003B6F RID: 15215
			public abstract void HandleOnTookPoisonTickDamage(Character attacker, Character target);
		}

		// Token: 0x02000B7B RID: 2939
		[Serializable]
		public class Poison : StatusEffect.PoisonHandler
		{
			// Token: 0x06003B71 RID: 15217 RVA: 0x000AF4D6 File Offset: 0x000AD6D6
			public Poison(Character character)
			{
				this.Initialize(character);
			}

			// Token: 0x06003B72 RID: 15218 RVA: 0x000AF4FA File Offset: 0x000AD6FA
			public Poison(StatusEffect.Poison copyFrom, Character character)
			{
				this._scale = copyFrom._scale;
				this._range = copyFrom._range;
				this.Initialize(character);
			}

			// Token: 0x06003B73 RID: 15219 RVA: 0x000AF538 File Offset: 0x000AD738
			public override void Initialize(Character character)
			{
				this._character = character;
				if (this._range == null)
				{
					this._range = character.collider;
				}
				this._effects = new EffectInfo[]
				{
					new EffectInfo(CharacterStatusSetting.instance.poison.loopEffectA)
					{
						scale = this._scale
					},
					new EffectInfo(CharacterStatusSetting.instance.poison.loopEffectB)
					{
						scale = this._scale
					},
					new EffectInfo(CharacterStatusSetting.instance.poison.loopEffectC)
					{
						scale = this._scale
					}
				};
				this._spriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.poison.colorOverlay.duration, 1f, CharacterStatusSetting.instance.poison.colorOverlay, CharacterStatusSetting.instance.poison.colorBlend, CharacterStatusSetting.instance.poison.outline, CharacterStatusSetting.instance.poison.grayScale);
			}

			// Token: 0x06003B74 RID: 15220 RVA: 0x00002191 File Offset: 0x00000391
			public override void Dispose()
			{
			}

			// Token: 0x06003B75 RID: 15221 RVA: 0x000AF63C File Offset: 0x000AD83C
			public override void HandleOnTookPoisonTickDamage(Character attacker, Character target)
			{
				EffectPoolInstance effectPoolInstance = this._effects.Random<EffectInfo>().Spawn(MMMaths.RandomPointWithinBounds(this._range.bounds), 0f, 1f);
				if (MMMaths.RandomBool())
				{
					effectPoolInstance.transform.localScale = new Vector3(-1f, 1f, 1f) * this._scale.value;
				}
				this._character.spriteEffectStack.Remove(this._spriteEffect);
				this._spriteEffect.Reset();
				this._character.spriteEffectStack.Add(this._spriteEffect);
			}

			// Token: 0x06003B76 RID: 15222 RVA: 0x000AF6E7 File Offset: 0x000AD8E7
			public override void HandleOnAttach(Character attacker, Character target)
			{
				this.SpawnFloatingText();
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.poison.attachSound, target.transform.position);
			}

			// Token: 0x06003B77 RID: 15223 RVA: 0x000AF714 File Offset: 0x000AD914
			public override void HandleOnRefresh(Character attacker, Character target)
			{
				this.SpawnFloatingText();
			}

			// Token: 0x06003B78 RID: 15224 RVA: 0x00002191 File Offset: 0x00000391
			public override void HandleOnDetach(Character attacker, Character target)
			{
			}

			// Token: 0x06003B79 RID: 15225 RVA: 0x000AF71C File Offset: 0x000AD91C
			public void SpawnFloatingText()
			{
				Vector2 v = MMMaths.RandomPointWithinBounds(this._range.bounds);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(StatusEffect.Poison._floatingTextKey), v, StatusEffect.Poison._floatingTextColor);
				this._character.spriteEffectStack.Remove(this._spriteEffect);
			}

			// Token: 0x04002EE1 RID: 12001
			private static readonly string _floatingTextKey = "floating/status/poision";

			// Token: 0x04002EE2 RID: 12002
			private static readonly string _floatingTextColor = "#A229FF";

			// Token: 0x04002EE3 RID: 12003
			[SerializeField]
			private CustomFloat _scale = new CustomFloat(0.15f, 0.2f);

			// Token: 0x04002EE4 RID: 12004
			[SerializeField]
			private Collider2D _range;

			// Token: 0x04002EE5 RID: 12005
			private EffectInfo[] _effects;

			// Token: 0x04002EE6 RID: 12006
			private GenericSpriteEffect _spriteEffect;

			// Token: 0x04002EE7 RID: 12007
			private Character _character;
		}

		// Token: 0x02000B7C RID: 2940
		[Serializable]
		public class Wound : StatusEffect.EffectHandler
		{
			// Token: 0x06003B7B RID: 15227 RVA: 0x000AF78B File Offset: 0x000AD98B
			static Wound()
			{
				StatusEffect.Wound._hitParticle = CommonResource.instance.hitParticle;
			}

			// Token: 0x06003B7C RID: 15228 RVA: 0x000AF7BC File Offset: 0x000AD9BC
			public Wound(Character character)
			{
				this.Initialize(character);
			}

			// Token: 0x06003B7D RID: 15229 RVA: 0x000AF818 File Offset: 0x000ADA18
			public Wound(StatusEffect.Wound copyFrom, Character character)
			{
				this._range = copyFrom._range;
				this._loopEffectScale = copyFrom._loopEffectScale;
				this._loopEffectImpactScale = copyFrom._loopEffectImpactScale;
				this._impactEffectScale = copyFrom._impactEffectScale;
				this.Initialize(character);
			}

			// Token: 0x06003B7E RID: 15230 RVA: 0x000AF8A4 File Offset: 0x000ADAA4
			public override void Initialize(Character character)
			{
				this._character = character;
				if (this._range == null)
				{
					this._range = character.collider;
				}
				this._loopEffectInterval = new CustomFloat(0.5f, 1.5f);
				this._loopEffectImpactInterval = new CustomFloat(2f, 3f);
				this._loopEffectAngle = new CustomAngle(-60f, 60f);
				this._loopEffects = new EffectInfo[]
				{
					new EffectInfo(CharacterStatusSetting.instance.bleed.loopEffectA)
					{
						scale = this._loopEffectScale,
						angle = this._loopEffectAngle
					},
					new EffectInfo(CharacterStatusSetting.instance.bleed.loopEffectB)
					{
						scale = this._loopEffectScale,
						angle = this._loopEffectAngle
					}
				};
				this._loopEffectImpact = new EffectInfo(CharacterStatusSetting.instance.bleed.dripEffect)
				{
					scale = this._loopEffectImpactScale
				};
				this._impactEffect = new EffectInfo(CharacterStatusSetting.instance.bleed.impactEffect)
				{
					scale = this._impactEffectScale
				};
				this._woundSpriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.bleed.colorBlend.duration, 1f, CharacterStatusSetting.instance.bleed.colorOverlay, CharacterStatusSetting.instance.bleed.colorBlend, CharacterStatusSetting.instance.bleed.outline, CharacterStatusSetting.instance.bleed.grayScale);
				this._bleedSpriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.bleed.colorBlendBleed.duration, 1f, CharacterStatusSetting.instance.bleed.colorOverlayBleed, CharacterStatusSetting.instance.bleed.colorBlendBleed, CharacterStatusSetting.instance.bleed.outlineBleed, CharacterStatusSetting.instance.bleed.grayScaleBleed);
			}

			// Token: 0x06003B7F RID: 15231 RVA: 0x000AFA8C File Offset: 0x000ADC8C
			public override void UpdateTime(float deltaTime)
			{
				this._loopEffectRemainTime -= deltaTime;
				this._loopEffectImpactRemainTime -= deltaTime;
				if (this._loopEffectRemainTime < 0f)
				{
					EffectInfo effectInfo = this._loopEffects.Random<EffectInfo>();
					effectInfo.flipX = MMMaths.RandomBool();
					effectInfo.Spawn(MMMaths.RandomPointWithinBounds(this._range.bounds), 0f, 1f);
					this._loopEffectRemainTime += this._loopEffectInterval.value;
					PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.bleed.loopImpactSound, this._character.transform.position);
				}
				if (this._loopEffectImpactRemainTime < 0f)
				{
					this._loopEffectImpact.flipX = MMMaths.RandomBool();
					this._loopEffectImpact.Spawn(MMMaths.RandomPointWithinBounds(this._range.bounds), 0f, 1f);
					this._loopEffectImpactRemainTime += this._loopEffectImpactInterval.value;
				}
			}

			// Token: 0x06003B80 RID: 15232 RVA: 0x000AFBA0 File Offset: 0x000ADDA0
			public override void HandleOnAttach(Character attacker, Character target)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.bleed.attachSound, target.transform.position);
				this.SpawnFloatingText(Localization.GetLocalizedString(StatusEffect.Wound._floatingWoundTextKey));
				this._character.spriteEffectStack.Remove(this._woundSpriteEffect);
				this._woundSpriteEffect.Reset();
				this._character.spriteEffectStack.Add(this._woundSpriteEffect);
			}

			// Token: 0x06003B81 RID: 15233 RVA: 0x000AFC1C File Offset: 0x000ADE1C
			public override void HandleOnDetach(Character attacker, Character target)
			{
				this._character.spriteEffectStack.Remove(this._woundSpriteEffect);
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.bleed.impactSound, target.transform.position);
				StatusEffect.Wound._hitParticle.Emit(this._range.transform.position, this._range.bounds, Vector2.zero, true);
				bool flipX = attacker.transform.position.x > target.transform.position.x;
				this._impactEffect.flipX = flipX;
				this._impactEffect.Spawn(MMMaths.RandomPointWithinBounds(this._range.bounds), (float)UnityEngine.Random.Range(-15, -45), 1f);
				this.SpawnFloatingText(Localization.GetLocalizedString(StatusEffect.Wound._floatingBleedTextKey));
				this._character.spriteEffectStack.Remove(this._bleedSpriteEffect);
				this._bleedSpriteEffect.Reset();
				this._character.spriteEffectStack.Add(this._bleedSpriteEffect);
			}

			// Token: 0x06003B82 RID: 15234 RVA: 0x00002191 File Offset: 0x00000391
			public override void Dispose()
			{
			}

			// Token: 0x06003B83 RID: 15235 RVA: 0x000AFD3C File Offset: 0x000ADF3C
			private void SpawnFloatingText(string text)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(text, MMMaths.RandomPointWithinBounds(this._range.bounds), StatusEffect.Wound._floatingBleedTextColor);
			}

			// Token: 0x06003B84 RID: 15236 RVA: 0x00002191 File Offset: 0x00000391
			public override void HandleOnRefresh(Character attacker, Character target)
			{
			}

			// Token: 0x04002EE8 RID: 12008
			private static readonly ParticleEffectInfo _hitParticle;

			// Token: 0x04002EE9 RID: 12009
			private static readonly string _floatingWoundTextKey = "floating/status/wound";

			// Token: 0x04002EEA RID: 12010
			private static readonly string _floatingBleedTextKey = "floating/status/bleed";

			// Token: 0x04002EEB RID: 12011
			private static readonly string _floatingBleedTextColor = "#d62d00";

			// Token: 0x04002EEC RID: 12012
			[SerializeField]
			private Collider2D _range;

			// Token: 0x04002EED RID: 12013
			[SerializeField]
			private CustomFloat _loopEffectScale = new CustomFloat(0.2f, 0.4f);

			// Token: 0x04002EEE RID: 12014
			[SerializeField]
			private CustomFloat _loopEffectImpactScale = new CustomFloat(0.3f, 0.5f);

			// Token: 0x04002EEF RID: 12015
			[SerializeField]
			private CustomFloat _impactEffectScale = new CustomFloat(0.4f, 0.6f);

			// Token: 0x04002EF0 RID: 12016
			private EffectInfo[] _loopEffects;

			// Token: 0x04002EF1 RID: 12017
			private EffectInfo _loopEffectImpact;

			// Token: 0x04002EF2 RID: 12018
			private EffectInfo _impactEffect;

			// Token: 0x04002EF3 RID: 12019
			private CustomAngle _loopEffectAngle;

			// Token: 0x04002EF4 RID: 12020
			private CustomFloat _loopEffectInterval;

			// Token: 0x04002EF5 RID: 12021
			private CustomFloat _loopEffectImpactInterval;

			// Token: 0x04002EF6 RID: 12022
			private float _loopEffectRemainTime;

			// Token: 0x04002EF7 RID: 12023
			private float _loopEffectImpactRemainTime;

			// Token: 0x04002EF8 RID: 12024
			private Character _character;

			// Token: 0x04002EF9 RID: 12025
			private GenericSpriteEffect _woundSpriteEffect;

			// Token: 0x04002EFA RID: 12026
			private GenericSpriteEffect _bleedSpriteEffect;
		}
	}
}
