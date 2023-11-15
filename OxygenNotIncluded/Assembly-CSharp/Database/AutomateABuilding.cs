using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D4E RID: 3406
	public class AutomateABuilding : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AD9 RID: 27353 RVA: 0x0029A314 File Offset: 0x00298514
		public override bool Success()
		{
			foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
			{
				LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
				if (logicCircuitNetwork.Receivers.Count > 0 && logicCircuitNetwork.Senders.Count > 0)
				{
					bool flag = false;
					foreach (ILogicEventReceiver logicEventReceiver in logicCircuitNetwork.Receivers)
					{
						if (!logicEventReceiver.IsNullOrDestroyed())
						{
							GameObject gameObject = Grid.Objects[logicEventReceiver.GetLogicCell(), 1];
							if (gameObject != null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag = true;
								break;
							}
						}
					}
					bool flag2 = false;
					foreach (ILogicEventSender logicEventSender in logicCircuitNetwork.Senders)
					{
						if (!logicEventSender.IsNullOrDestroyed())
						{
							GameObject gameObject2 = Grid.Objects[logicEventSender.GetLogicCell(), 1];
							if (gameObject2 != null && !gameObject2.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x0029A4B8 File Offset: 0x002986B8
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x0029A4BA File Offset: 0x002986BA
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.AUTOMATE_A_BUILDING;
		}
	}
}
