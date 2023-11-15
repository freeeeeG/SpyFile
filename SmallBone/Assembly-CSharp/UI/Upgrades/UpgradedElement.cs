using System;
using System.Collections;
using Characters.Gear.Upgrades;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Upgrades
{
	// Token: 0x020003F5 RID: 1013
	public sealed class UpgradedElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x000393B5 File Offset: 0x000375B5
		public Selectable selectable
		{
			get
			{
				return this._button;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x000393BD File Offset: 0x000375BD
		public UpgradeResource.Reference reference
		{
			get
			{
				return this._reference;
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000393C8 File Offset: 0x000375C8
		public void OnSelect(BaseEventData eventData)
		{
			if (this._reference == null)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.moveSoundInfo, base.gameObject.transform.position);
			this._panel.UpdateCurrentOption(this);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00039415 File Offset: 0x00037615
		private void OnEnable()
		{
			this._failEffect.SetActive(false);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00039424 File Offset: 0x00037624
		private void OnDisable()
		{
			Animator[] effects = this._effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00039454 File Offset: 0x00037654
		public void Set(Panel panel, UpgradeResource.Reference reference, bool effect = false)
		{
			this._panel = panel;
			this._reference = reference;
			if (this._reference == null)
			{
				this.SetEmpty();
				return;
			}
			this.SetElement();
			if (effect)
			{
				this._ceffectReference.Stop();
				this._ceffectReference = this.StartCoroutineWithReference(this.CActiveGetEffect());
			}
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x000394A4 File Offset: 0x000376A4
		private IEnumerator CActiveGetEffect()
		{
			foreach (Animator animator in this._effects)
			{
				animator.gameObject.SetActive(false);
				animator.enabled = true;
				animator.Play(0, 0, 0f);
				animator.enabled = false;
				animator.gameObject.SetActive(true);
			}
			Animator[] effects;
			float deltaTime;
			for (float remainTime = 1.04f; remainTime > 0f; remainTime -= deltaTime)
			{
				yield return null;
				deltaTime = Chronometer.global.deltaTime;
				effects = this._effects;
				for (int i = 0; i < effects.Length; i++)
				{
					effects[i].Update(deltaTime);
				}
			}
			effects = this._effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000394B3 File Offset: 0x000376B3
		private void SetEmpty()
		{
			this._riskFrame.enabled = false;
			this._icon.enabled = false;
			this._level.Set(0, 0, false, false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000394E8 File Offset: 0x000376E8
		private void SetElement()
		{
			this._riskFrame.enabled = (this._reference.type == UpgradeObject.Type.Cursed);
			this._icon.enabled = true;
			this._icon.sprite = this._reference.icon;
			int maxLevel = this._reference.maxLevel;
			int currentLevel = this._reference.GetCurrentLevel();
			this._level.Set(currentLevel, maxLevel, this._reference.type == UpgradeObject.Type.Cursed, false);
			UnityAction call = new UnityAction(this.TryUpgrade);
			this._button.onClick.RemoveAllListeners();
			this._button.onClick.AddListener(call);
			this._button.navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic
			};
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x000395BC File Offset: 0x000377BC
		private void TryUpgrade()
		{
			if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(this._reference))
			{
				if (!Singleton<UpgradeShop>.Instance.TryLevelUp(this._reference))
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.failSoundInfo, base.gameObject.transform.position);
					base.StartCoroutine(this.CEmitFailEffect());
					return;
				}
				this._panel.UpdateUpgradedList();
			}
			else
			{
				if (!Singleton<UpgradeShop>.Instance.TryUpgrade(this._reference))
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.failSoundInfo, base.gameObject.transform.position);
					base.StartCoroutine(this.CEmitFailEffect());
					return;
				}
				this._panel.AppendToUpgradedList();
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.upgradeSoundInfo, base.gameObject.transform.position);
			this._panel.UpdateCurrentOption(this);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x000396D9 File Offset: 0x000378D9
		public void PlayFailEffect()
		{
			base.StartCoroutine(this.CEmitFailEffect());
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x000396E8 File Offset: 0x000378E8
		private IEnumerator CEmitFailEffect()
		{
			this._failEffect.SetActive(true);
			yield return Chronometer.global.WaitForSeconds(0.3f);
			this._failEffect.SetActive(false);
			yield break;
		}

		// Token: 0x04001008 RID: 4104
		[SerializeField]
		private Button _button;

		// Token: 0x04001009 RID: 4105
		[SerializeField]
		private Image _riskFrame;

		// Token: 0x0400100A RID: 4106
		[SerializeField]
		private Image _icon;

		// Token: 0x0400100B RID: 4107
		[SerializeField]
		private Level _level;

		// Token: 0x0400100C RID: 4108
		[SerializeField]
		private GameObject _failEffect;

		// Token: 0x0400100D RID: 4109
		[SerializeField]
		[Header("이펙트")]
		private Animator[] _effects;

		// Token: 0x0400100E RID: 4110
		private UpgradeResource.Reference _reference;

		// Token: 0x0400100F RID: 4111
		private Panel _panel;

		// Token: 0x04001010 RID: 4112
		private CoroutineReference _ceffectReference;

		// Token: 0x04001011 RID: 4113
		private const float _effectLength = 1.04f;
	}
}
