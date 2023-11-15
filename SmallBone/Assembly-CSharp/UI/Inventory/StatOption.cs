using System;
using Characters;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
	// Token: 0x02000447 RID: 1095
	public sealed class StatOption : MonoBehaviour
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x00040A05 File Offset: 0x0003EC05
		public string titleText
		{
			get
			{
				return Localization.GetLocalizedString("stat/title");
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x00040A11 File Offset: 0x0003EC11
		public string healthText
		{
			get
			{
				return Localization.GetLocalizedString("stat/health/name");
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x00040A1D File Offset: 0x0003EC1D
		public string takingDamageText
		{
			get
			{
				return Localization.GetLocalizedString("stat/takingDamage/name");
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x00040A29 File Offset: 0x0003EC29
		public string physicalAttackDamageText
		{
			get
			{
				return Localization.GetLocalizedString("stat/physicalAttackDamage/name");
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x00040A35 File Offset: 0x0003EC35
		public string magicalAttackDamageText
		{
			get
			{
				return Localization.GetLocalizedString("stat/magicalAttackDamage/name");
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x00040A41 File Offset: 0x0003EC41
		public string attackSpeedText
		{
			get
			{
				return Localization.GetLocalizedString("stat/attackSpeed/name");
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x00040A4D File Offset: 0x0003EC4D
		public string movementSpeedText
		{
			get
			{
				return Localization.GetLocalizedString("stat/movementSpeed/name");
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060014D1 RID: 5329 RVA: 0x00040A59 File Offset: 0x0003EC59
		public string chargingSpeedText
		{
			get
			{
				return Localization.GetLocalizedString("stat/chargingSpeed/name");
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060014D2 RID: 5330 RVA: 0x00040A65 File Offset: 0x0003EC65
		public string skillCooldownSpeedText
		{
			get
			{
				return Localization.GetLocalizedString("stat/skillCooldownSpeed/name");
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060014D3 RID: 5331 RVA: 0x00040A71 File Offset: 0x0003EC71
		public string swapCooldownSpeedText
		{
			get
			{
				return Localization.GetLocalizedString("stat/swapCooldownSpeed/name");
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x00040A7D File Offset: 0x0003EC7D
		public string essenceCooldownSpeedText
		{
			get
			{
				return Localization.GetLocalizedString("stat/essenceCooldownSpeed/name");
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x00040A89 File Offset: 0x0003EC89
		public string criticalChanceText
		{
			get
			{
				return Localization.GetLocalizedString("stat/criticalChance/name");
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x00040A95 File Offset: 0x0003EC95
		public string criticalDamageText
		{
			get
			{
				return Localization.GetLocalizedString("stat/criticalDamage/name");
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00040AA4 File Offset: 0x0003ECA4
		private void OnEnable()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player == null)
			{
				return;
			}
			this._titleLabel.text = this.titleText;
			this._healthLabel.text = (this.healthText ?? "");
			this._takeDamageLabel.text = (this.takingDamageText ?? "");
			this._physicalDamageLabel.text = (this.physicalAttackDamageText ?? "");
			this._magicalDamageLabel.text = (this.magicalAttackDamageText ?? "");
			this._attackSpeedLabel.text = (this.attackSpeedText ?? "");
			this._movementSpeedLabel.text = (this.movementSpeedText ?? "");
			this._chargingSpeedLabel.text = (this.chargingSpeedText ?? "");
			this._skillCooldownLabel.text = (this.skillCooldownSpeedText ?? "");
			this._swapCooldownLabel.text = (this.swapCooldownSpeedText ?? "");
			this._essenceCooldownLabel.text = (this.essenceCooldownSpeedText ?? "");
			this._criticalChanceLabel.text = (this.criticalChanceText ?? "");
			this._criticalDamageLabel.text = (this.criticalDamageText ?? "");
			Stat stat = player.stat;
			double final = stat.GetFinal(Stat.Kind.Health);
			double final2 = stat.GetFinal(Stat.Kind.TakingDamage);
			this._health.Set(string.Format("{0}", final), true, "");
			this._takingDamage.Set(string.Format("x{0:0.00}", final2), false, "");
			double final3 = player.stat.GetFinal(Stat.Kind.PhysicalAttackDamage);
			double final4 = player.stat.GetFinal(Stat.Kind.MagicAttackDamage);
			this._physicalDamage.Set(string.Format("{0:0}", final3 * 100.0), false, "%");
			this._magicalDamage.Set(string.Format("{0:0}", final4 * 100.0), false, "%");
			double final5 = stat.GetFinal(Stat.Kind.BasicAttackSpeed);
			double num = stat.GetFinal(Stat.Kind.MovementSpeed) / 5.0;
			double final6 = stat.GetFinal(Stat.Kind.ChargingSpeed);
			this._attackSpeed.Set(string.Format("{0:0}", final5 * 100.0), false, "%");
			this._movementSpeed.Set(string.Format("{0:0}", num * 100.0), false, "%");
			this._chargingSpeed.Set(string.Format("{0:0}", final6 * 100.0), false, "%");
			double final7 = stat.GetFinal(Stat.Kind.SkillCooldownSpeed);
			double final8 = stat.GetFinal(Stat.Kind.SwapCooldownSpeed);
			double final9 = stat.GetFinal(Stat.Kind.EssenceCooldownSpeed);
			this._skillCooldown.Set(string.Format("{0:0}", final7 * 100.0), false, "%");
			this._swapCooldown.Set(string.Format("{0:0}", final8 * 100.0), false, "%");
			this._essenceCooldown.Set(string.Format("{0:0}", final9 * 100.0), false, "%");
			double num2 = stat.GetFinal(Stat.Kind.CriticalChance) - 1.0;
			double final10 = stat.GetFinal(Stat.Kind.CriticalDamage);
			this._criticalChance.Set(string.Format("{0:0}", num2 * 100.0), num2 >= 0.0, "%");
			this._criticalDamage.Set(string.Format("x{0:0.00}", final10), false, "");
		}

		// Token: 0x040011E9 RID: 4585
		[Header("Title")]
		[SerializeField]
		private TMP_Text _titleLabel;

		// Token: 0x040011EA RID: 4586
		[Header("Labels")]
		[SerializeField]
		private TMP_Text _healthLabel;

		// Token: 0x040011EB RID: 4587
		[SerializeField]
		private TMP_Text _takeDamageLabel;

		// Token: 0x040011EC RID: 4588
		[SerializeField]
		private TMP_Text _physicalDamageLabel;

		// Token: 0x040011ED RID: 4589
		[SerializeField]
		private TMP_Text _magicalDamageLabel;

		// Token: 0x040011EE RID: 4590
		[SerializeField]
		private TMP_Text _attackSpeedLabel;

		// Token: 0x040011EF RID: 4591
		[SerializeField]
		private TMP_Text _movementSpeedLabel;

		// Token: 0x040011F0 RID: 4592
		[SerializeField]
		private TMP_Text _chargingSpeedLabel;

		// Token: 0x040011F1 RID: 4593
		[SerializeField]
		private TMP_Text _skillCooldownLabel;

		// Token: 0x040011F2 RID: 4594
		[SerializeField]
		private TMP_Text _swapCooldownLabel;

		// Token: 0x040011F3 RID: 4595
		[SerializeField]
		private TMP_Text _essenceCooldownLabel;

		// Token: 0x040011F4 RID: 4596
		[SerializeField]
		private TMP_Text _criticalChanceLabel;

		// Token: 0x040011F5 RID: 4597
		[SerializeField]
		private TMP_Text _criticalDamageLabel;

		// Token: 0x040011F6 RID: 4598
		[Header("Values")]
		[SerializeField]
		private StatValue _health;

		// Token: 0x040011F7 RID: 4599
		[SerializeField]
		private StatValue _takingDamage;

		// Token: 0x040011F8 RID: 4600
		[SerializeField]
		private StatValue _physicalDamage;

		// Token: 0x040011F9 RID: 4601
		[SerializeField]
		private StatValue _magicalDamage;

		// Token: 0x040011FA RID: 4602
		[SerializeField]
		private StatValue _attackSpeed;

		// Token: 0x040011FB RID: 4603
		[SerializeField]
		private StatValue _movementSpeed;

		// Token: 0x040011FC RID: 4604
		[SerializeField]
		private StatValue _chargingSpeed;

		// Token: 0x040011FD RID: 4605
		[SerializeField]
		private StatValue _skillCooldown;

		// Token: 0x040011FE RID: 4606
		[SerializeField]
		private StatValue _swapCooldown;

		// Token: 0x040011FF RID: 4607
		[SerializeField]
		private StatValue _essenceCooldown;

		// Token: 0x04001200 RID: 4608
		[SerializeField]
		private StatValue _criticalChance;

		// Token: 0x04001201 RID: 4609
		[SerializeField]
		private StatValue _criticalDamage;
	}
}
