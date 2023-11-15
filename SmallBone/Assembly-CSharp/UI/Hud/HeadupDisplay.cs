using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Characters.Player;
using Hardmode;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
	// Token: 0x02000461 RID: 1121
	public class HeadupDisplay : MonoBehaviour
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x00042EDD File Offset: 0x000410DD
		public AbilityIconDisplay abilityIconDisplay
		{
			get
			{
				return this._abilityIconDisplay;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x00042EE5 File Offset: 0x000410E5
		public BossHealthbarController bossHealthBar
		{
			get
			{
				return this._bossHealthBar;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x00042EED File Offset: 0x000410ED
		public DarkEnemyHealthbarController darkEnemyHealthBar
		{
			get
			{
				return this._darkEnemyHealthBar;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00042EF5 File Offset: 0x000410F5
		public DavyJonesHud davyJonesHud
		{
			get
			{
				return this._davyJonesHud;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x00042EFD File Offset: 0x000410FD
		// (set) Token: 0x0600154E RID: 5454 RVA: 0x00042F0A File Offset: 0x0004110A
		public bool visible
		{
			get
			{
				return this._container.activeSelf;
			}
			set
			{
				this._container.SetActive(value);
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x00042F18 File Offset: 0x00041118
		// (set) Token: 0x06001550 RID: 5456 RVA: 0x00042F25 File Offset: 0x00041125
		public bool minimapVisible
		{
			get
			{
				return this._rightBottomWithMinimap.activeSelf;
			}
			set
			{
				this._rightBottomWithMinimap.SetActive(value);
				this._rightBottomWithoutMinimap.SetActive(!value);
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00042F44 File Offset: 0x00041144
		public void Initialize(Character player)
		{
			this._character = player;
			this._weaponInventory = player.GetComponent<WeaponInventory>();
			this._itemInventory = player.GetComponent<ItemInventory>();
			this._quintessenceInventory = player.GetComponent<QuintessenceInventory>();
			this._abilityIconDisplay.Initialize(player);
			this._healthBar.Initialize(player);
			this._healthValue.Initialize(player.health, player.health.shield);
			this._weaponInventory.onSwap += this.UpdateGauge;
			this._weaponInventory.onChanged += this.OnWeaponChange;
			this._weaponInventory.onSwapReady += this.SpawnSwapReadyEffect;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00042FF5 File Offset: 0x000411F5
		private void SpawnSwapReadyEffect()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this._cPlaySwapReadyEffectReference.Stop();
			this._cPlaySwapReadyEffectReference = this.StartCoroutineWithReference(this.CPlaySwapReadyEffect());
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0004301D File Offset: 0x0004121D
		private IEnumerator CPlaySwapReadyEffect()
		{
			this._swapReadyEffect.gameObject.SetActive(true);
			this._swapReadyEffect.Play(0, 0, 0f);
			yield return Chronometer.global.WaitForSeconds(this._swapReadyEffect.GetCurrentAnimatorStateInfo(0).length);
			this._swapReadyEffect.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0004302C File Offset: 0x0004122C
		private void OnWeaponChange(Weapon old, Weapon @new)
		{
			this.UpdateGauge();
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00043034 File Offset: 0x00041234
		private void UpdateGauge()
		{
			this._gaugeBar.gauge = this._weaponInventory.polymorphOrCurrent.gauge;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00043051 File Offset: 0x00041251
		private void SetActive(GameObject gameObject, bool value)
		{
			if (gameObject.activeSelf == value)
			{
				return;
			}
			gameObject.SetActive(value);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00043064 File Offset: 0x00041264
		private void OnDisable()
		{
			this._swapReadyEffect.gameObject.SetActive(false);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00043078 File Offset: 0x00041278
		private void Update()
		{
			if (this._character == null)
			{
				return;
			}
			this._currentWeapon.sprite = this._weaponInventory.polymorphOrCurrent.mainIcon;
			Weapon next = this._weaponInventory.next;
			if (next == null)
			{
				this.SetActive(this._nextWeapon.transform.parent.gameObject, false);
				this._changeWeaponCooldown.fillAmount = 0f;
				for (int i = 0; i < this._subskills.Length; i++)
				{
					this.SetActive(this._subskills[i].gameObject, false);
				}
			}
			else
			{
				this.SetActive(this._nextWeapon.transform.parent.gameObject, true);
				this.SetActive(this._nextWeaponSilenceMask.gameObject, this._character.silence.value);
				this._nextWeapon.sprite = next.subIcon;
				this._nextWeapon.preserveAspect = true;
				this._changeWeaponCooldown.fillAmount = this._weaponInventory.reaminCooldownPercent;
				List<SkillInfo> currentSkills = this._weaponInventory.next.currentSkills;
				for (int j = 0; j < this._subskills.Length; j++)
				{
					ActionIcon actionIcon = this._subskills[j];
					if (j >= currentSkills.Count)
					{
						this.SetActive(actionIcon.gameObject, false);
					}
					else
					{
						SkillInfo skillInfo = currentSkills[j];
						this.SetActive(actionIcon.gameObject, true);
						actionIcon.icon.sprite = skillInfo.cachedIcon;
						actionIcon.icon.preserveAspect = true;
						actionIcon.action = skillInfo.action;
						actionIcon.cooldown = skillInfo.action.cooldown;
					}
				}
			}
			List<SkillInfo> currentSkills2 = this._weaponInventory.polymorphOrCurrent.currentSkills;
			for (int k = 0; k < this._skills.Length; k++)
			{
				ActionIcon actionIcon2 = this._skills[k];
				if (k >= currentSkills2.Count)
				{
					this.SetActive(actionIcon2.gameObject, false);
				}
				else
				{
					SkillInfo skillInfo2 = currentSkills2[k];
					this.SetActive(actionIcon2.gameObject, true);
					actionIcon2.icon.sprite = skillInfo2.cachedIcon;
					actionIcon2.icon.preserveAspect = true;
					actionIcon2.action = skillInfo2.action;
					actionIcon2.cooldown = skillInfo2.action.cooldown;
				}
			}
			if (this._quintessenceInventory.items[0] == null)
			{
				this.SetActive(this._quintessence.gameObject, false);
			}
			else
			{
				this.SetActive(this._quintessence.gameObject, true);
				HeadupDisplay.<Update>g__SetQuintessenceInfo|45_0(this._quintessence, this._quintessenceInventory.items[0]);
			}
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				for (int l = 0; l < this._heartQuartzDisplays.Length; l++)
				{
					this._heartQuartzDisplays[l].gameObject.SetActive(true);
				}
				return;
			}
			for (int m = 0; m < this._heartQuartzDisplays.Length; m++)
			{
				this._heartQuartzDisplays[m].gameObject.SetActive(false);
			}
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000433A2 File Offset: 0x000415A2
		[CompilerGenerated]
		internal static void <Update>g__SetQuintessenceInfo|45_0(EssenceIcon essenceIcon, Quintessence quintessence)
		{
			essenceIcon.icon.sprite = quintessence.hudIcon;
			essenceIcon.icon.preserveAspect = true;
			essenceIcon.cooldown = quintessence.cooldown;
			essenceIcon.constraints = quintessence.constraints;
		}

		// Token: 0x0400129B RID: 4763
		private Character _character;

		// Token: 0x0400129C RID: 4764
		private WeaponInventory _weaponInventory;

		// Token: 0x0400129D RID: 4765
		private ItemInventory _itemInventory;

		// Token: 0x0400129E RID: 4766
		private QuintessenceInventory _quintessenceInventory;

		// Token: 0x0400129F RID: 4767
		[SerializeField]
		private GameObject _container;

		// Token: 0x040012A0 RID: 4768
		[SerializeField]
		private GameObject _rightBottomWithMinimap;

		// Token: 0x040012A1 RID: 4769
		[SerializeField]
		private GameObject _rightBottomWithoutMinimap;

		// Token: 0x040012A2 RID: 4770
		[SerializeField]
		private AbilityIconDisplay _abilityIconDisplay;

		// Token: 0x040012A3 RID: 4771
		[SerializeField]
		private DavyJonesHud _davyJonesHud;

		// Token: 0x040012A4 RID: 4772
		[SerializeField]
		private CharacterHealthBar _healthBar;

		// Token: 0x040012A5 RID: 4773
		[SerializeField]
		private HealthValue _healthValue;

		// Token: 0x040012A6 RID: 4774
		[SerializeField]
		private GaugeBar _gaugeBar;

		// Token: 0x040012A7 RID: 4775
		[SerializeField]
		private BossHealthbarController _bossHealthBar;

		// Token: 0x040012A8 RID: 4776
		[SerializeField]
		private DarkEnemyHealthbarController _darkEnemyHealthBar;

		// Token: 0x040012A9 RID: 4777
		[SerializeField]
		private Image _currentWeapon;

		// Token: 0x040012AA RID: 4778
		[SerializeField]
		private Image _nextWeapon;

		// Token: 0x040012AB RID: 4779
		[SerializeField]
		private Image _nextWeaponSilenceMask;

		// Token: 0x040012AC RID: 4780
		[SerializeField]
		private Image _changeWeaponCooldown;

		// Token: 0x040012AD RID: 4781
		[SerializeField]
		private ActionIcon[] _skills;

		// Token: 0x040012AE RID: 4782
		[SerializeField]
		private ActionIcon[] _subskills;

		// Token: 0x040012AF RID: 4783
		[SerializeField]
		private EssenceIcon _quintessence;

		// Token: 0x040012B0 RID: 4784
		[SerializeField]
		private Animator _swapReadyEffect;

		// Token: 0x040012B1 RID: 4785
		[SerializeField]
		private CurrencyDisplay[] _heartQuartzDisplays;

		// Token: 0x040012B2 RID: 4786
		private CoroutineReference _cPlaySwapReadyEffectReference;
	}
}
