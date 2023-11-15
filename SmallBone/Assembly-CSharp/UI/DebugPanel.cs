using System;
using System.Text;
using Characters;
using Characters.Controllers;
using Services;
using Singletons;
using TMPro;
using UI.TestingTool;
using UnityEngine;

namespace UI
{
	// Token: 0x02000397 RID: 919
	public class DebugPanel : MonoBehaviour
	{
		// Token: 0x060010D4 RID: 4308 RVA: 0x00031AEA File Offset: 0x0002FCEA
		public void StartLog()
		{
			this._logger.StartLog();
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00031AF7 File Offset: 0x0002FCF7
		private void OnEnable()
		{
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
			this.DisplayPlayerStat();
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00031B1A File Offset: 0x0002FD1A
		private void OnDisable()
		{
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00031B34 File Offset: 0x0002FD34
		private void DisplayPlayerStat()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player == null)
			{
				this._status.text = string.Empty;
				return;
			}
			double final = player.stat.GetFinal(Stat.Kind.AttackDamage);
			stringBuilder.AppendFormat("Attack Damage : {0:0%}\r\n", final);
			stringBuilder.AppendFormat("Physical Attack Damage : {0:0%}\r\n", final * player.stat.GetFinal(Stat.Kind.PhysicalAttackDamage));
			stringBuilder.AppendFormat("Magic Attack Damage : {0:0%}\r\n", final * player.stat.GetFinal(Stat.Kind.MagicAttackDamage));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Critical Chance : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.CriticalChance));
			stringBuilder.AppendFormat("Critical Damage Multiplier : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.CriticalDamage));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Attack Speed : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.AttackSpeed));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Movement Speed : {0:0%}\r\n", player.stat.GetFinalPercent(Stat.Kind.MovementSpeed));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Health : {0}\r\n", player.stat.GetFinal(Stat.Kind.Health));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Taking Damage (smaller is better) : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.TakingDamage));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Cooldown Speed : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.CooldownSpeed));
			stringBuilder.AppendFormat("Skill Cooldown Speed : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.SkillCooldownSpeed));
			stringBuilder.AppendFormat("Dash Cooldown Speed : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.DashCooldownSpeed));
			stringBuilder.AppendFormat("Swap Cooldown Speed : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.SwapCooldownSpeed));
			stringBuilder.AppendFormat("Essence Cooldown Speed : {0:0%}\r\n", player.stat.GetFinal(Stat.Kind.EssenceCooldownSpeed));
			this._status.text = stringBuilder.ToString();
		}

		// Token: 0x04000DC9 RID: 3529
		[SerializeField]
		private TMP_Text _status;

		// Token: 0x04000DCA RID: 3530
		[SerializeField]
		private Log _logger;
	}
}
