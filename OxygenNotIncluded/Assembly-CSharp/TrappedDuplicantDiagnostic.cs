﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000767 RID: 1895
public class TrappedDuplicantDiagnostic : ColonyDiagnostic
{
	// Token: 0x0600345C RID: 13404 RVA: 0x0011908C File Offset: 0x0011728C
	public TrappedDuplicantDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_power";
		base.AddCriterion("CheckTrapped", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.CRITERIA.CHECKTRAPPED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTrapped)));
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x001190DC File Offset: 0x001172DC
	public ColonyDiagnostic.DiagnosticResult CheckTrapped()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		bool flag = false;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
		{
			if (flag)
			{
				break;
			}
			if (!ClusterManager.Instance.GetWorld(base.worldID).IsModuleInterior && this.CheckMinionBasicallyIdle(minionIdentity))
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				bool flag2 = true;
				foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
				{
					if (!(minionIdentity == minionIdentity2) && !this.CheckMinionBasicallyIdle(minionIdentity2) && component.CanReach(minionIdentity2.GetComponent<IApproachable>()))
					{
						flag2 = false;
						break;
					}
				}
				List<Telepad> worldItems = Components.Telepads.GetWorldItems(component.GetMyWorld().id, false);
				if (worldItems != null && worldItems.Count > 0)
				{
					flag2 = (flag2 && !component.CanReach(worldItems[0].GetComponent<IApproachable>()));
				}
				List<WarpReceiver> worldItems2 = Components.WarpReceivers.GetWorldItems(component.GetMyWorld().id, false);
				if (worldItems2 != null && worldItems2.Count > 0)
				{
					foreach (WarpReceiver warpReceiver in worldItems2)
					{
						flag2 = (flag2 && !component.CanReach(worldItems2[0].GetComponent<IApproachable>()));
					}
				}
				List<Sleepable> worldItems3 = Components.Sleepables.GetWorldItems(component.GetMyWorld().id, false);
				for (int i = 0; i < worldItems3.Count; i++)
				{
					Assignable component2 = worldItems3[i].GetComponent<Assignable>();
					if (component2 != null && component2.IsAssignedTo(minionIdentity))
					{
						flag2 = (flag2 && !component.CanReach(worldItems3[i].GetComponent<IApproachable>()));
					}
				}
				if (flag2)
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject);
				}
				flag = (flag || flag2);
			}
		}
		result.opinion = (flag ? ColonyDiagnostic.DiagnosticResult.Opinion.Bad : ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
		result.Message = (flag ? UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.STUCK : UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.NORMAL);
		return result;
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x001193AC File Offset: 0x001175AC
	private bool CheckMinionBasicallyIdle(MinionIdentity minion)
	{
		return minion.HasTag(GameTags.Idle) || minion.HasTag(GameTags.RecoveringBreath) || minion.HasTag(GameTags.MakingMess);
	}
}
