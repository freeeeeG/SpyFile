using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using Data;
using FX;
using GameResources;
using Level;
using Scenes;
using Services;
using Singletons;
using UI.Hud;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BD0 RID: 3024
	public sealed class DarkEnemy : MonoBehaviour
	{
		// Token: 0x06003E45 RID: 15941 RVA: 0x000B4FE8 File Offset: 0x000B31E8
		private void OnEnable()
		{
			ColorUtility.TryParseHtmlString("#68009F", out this._minimapColor);
			this._character.stat.AttachValues(this._baseStatValues);
			this._character.stat.AttachValues(this._bonusStatValues);
			this._introOperations.Initialize();
			this._character.cinematic.Attach(this);
			this._characterDieEffect.particleInfo = null;
			this._characterDieEffect.effect = null;
			this._character.health.onDiedTryCatch += this.HandleOnDied;
			GameObject attach = this._character.attach;
			if (attach == null)
			{
				return;
			}
			LineText componentInChildren = attach.GetComponentInChildren<LineText>();
			if (componentInChildren == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(componentInChildren.transform.parent.gameObject);
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x000B50C0 File Offset: 0x000B32C0
		private void HandleOnDied()
		{
			Singleton<Service>.Instance.levelManager.DropCurrency(GameData.Currency.Type.HeartQuartz, 1, 1, MMMaths.RandomPointWithinBounds(this._character.collider.bounds));
			if (DarkEnemySelector.instance.dieEffects != null)
			{
				foreach (EffectInfo effectInfo in DarkEnemySelector.instance.dieEffects)
				{
					if (effectInfo != null)
					{
						effectInfo.Spawn(this._character.transform.position, 0f, 1f);
					}
				}
			}
			if (DarkEnemySelector.instance.dieSound != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(DarkEnemySelector.instance.dieSound, this._character.transform.position);
			}
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x000B5178 File Offset: 0x000B3378
		private void OnDestroy()
		{
			this._character.stat.DetachValues(this._baseStatValues);
			this._character.stat.DetachValues(this._bonusStatValues);
			this._character.health.onDiedTryCatch -= this.HandleOnDied;
			if (Service.quitting)
			{
				return;
			}
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Close();
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.RemoveTarget(this._character);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x000B520D File Offset: 0x000B340D
		public void Initialize(DarkAbilityAttacher attacher)
		{
			base.enabled = true;
			this._attacher = attacher;
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x000B521D File Offset: 0x000B341D
		private IEnumerator CRunIntroVisualEffect()
		{
			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
			SpriteRenderer renderer = this._character.spriteEffectStack.mainRenderer;
			int id = Shader.PropertyToID("_DissolveLevels");
			float elapsed = 0f;
			float duration = 1f;
			if (DarkEnemySelector.instance.introSound != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(DarkEnemySelector.instance.introSound, this._character.transform.position);
			}
			while (elapsed < duration)
			{
				renderer.GetPropertyBlock(propertyBlock);
				propertyBlock.SetFloat(id, 1f - elapsed / duration);
				renderer.SetPropertyBlock(propertyBlock);
				yield return null;
				elapsed += this._character.chronometer.master.deltaTime;
			}
			renderer.GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat(id, 0f);
			renderer.SetPropertyBlock(propertyBlock);
			yield break;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x000B522C File Offset: 0x000B342C
		public void RunIntro()
		{
			this._character.cinematic.Detach(this);
			this._character.spriteEffectStack.mainRenderer.material = MaterialResource.darkEnemy;
			if (DarkEnemySelector.instance.introEffects != null)
			{
				foreach (EffectInfo effectInfo in DarkEnemySelector.instance.introEffects)
				{
					if (effectInfo != null)
					{
						effectInfo.Spawn(this._character.transform.position, 0f, 1f);
					}
				}
			}
			base.StartCoroutine(this.CRunIntroVisualEffect());
			this._attacher.StartAttach();
			if (this._character.attach != null)
			{
				this.ChangeMinimapAgent();
				CharacterHealthBarAttacher component = this._character.GetComponent<CharacterHealthBarAttacher>();
				if (component != null)
				{
					component.SetHealthBar(DarkEnemySelector.instance.healthbar);
					DarkAbilityGaugeBar componentInChildren = component.GetComponentInChildren<DarkAbilityGaugeBar>();
					if (componentInChildren != null)
					{
						componentInChildren.Initialize(this._attacher);
					}
				}
			}
			this.OpenHUDHealthBar();
			base.StartCoroutine(this._introOperations.CRun(this._character));
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x000B5348 File Offset: 0x000B3548
		private void ChangeMinimapAgent()
		{
			string value = "Minimap Agent";
			int num = -1;
			for (int i = 0; i < this._character.attach.transform.childCount; i++)
			{
				if (this._character.attach.transform.GetChild(i).name.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				this._character.attach.transform.GetChild(num).GetComponent<SpriteRenderer>().color = this._minimapColor;
			}
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x000B53D0 File Offset: 0x000B35D0
		private void OpenHUDHealthBar()
		{
			if (this._attacher == null)
			{
				Debug.LogError("Attacher의 위치가 잘못되었습니다.");
				return;
			}
			HeadupDisplay hud = Scene<GameBase>.instance.uiManager.headupDisplay;
			hud.darkEnemyHealthBar.Open(this._character, this._attacher.displayName);
			hud.darkEnemyHealthBar.AddTarget(this._character, this._attacher.displayName);
			this._character.health.onDiedTryCatch += delegate()
			{
				IDictionary<Character, string> attached = hud.darkEnemyHealthBar.attached;
				hud.darkEnemyHealthBar.RemoveTarget(this._character);
				if (hud.darkEnemyHealthBar.attached.Count == 0)
				{
					hud.darkEnemyHealthBar.Close();
					return;
				}
				hud.darkEnemyHealthBar.Set(attached.Random<KeyValuePair<Character, string>>().Key);
			};
		}

		// Token: 0x04003017 RID: 12311
		[SerializeField]
		private Stat.Values _bonusStatValues;

		// Token: 0x04003018 RID: 12312
		[SerializeField]
		private Character _character;

		// Token: 0x04003019 RID: 12313
		[SerializeField]
		private CharacterDieEffect _characterDieEffect;

		// Token: 0x0400301A RID: 12314
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _introOperations;

		// Token: 0x0400301B RID: 12315
		private Stat.Values _baseStatValues = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, -1.0)
		});

		// Token: 0x0400301C RID: 12316
		private DarkAbilityAttacher _attacher;

		// Token: 0x0400301D RID: 12317
		private const string minimapColorString = "#68009F";

		// Token: 0x0400301E RID: 12318
		private Color _minimapColor;
	}
}
