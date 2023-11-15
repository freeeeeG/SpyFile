using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class ClusterFogOfWarManager : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>
{
	// Token: 0x06002FDD RID: 12253 RVA: 0x000FCA7C File Offset: 0x000FAC7C
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.Initialize();
		}).EventHandler(GameHashes.DiscoveredWorldsChanged, (ClusterFogOfWarManager.Instance smi) => Game.Instance, delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.UpdateRevealedCellsFromDiscoveredWorlds();
		});
	}

	// Token: 0x04001C50 RID: 7248
	public const int AUTOMATIC_PEEK_RADIUS = 2;

	// Token: 0x0200141C RID: 5148
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200141D RID: 5149
	public new class Instance : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.GameInstance
	{
		// Token: 0x06008388 RID: 33672 RVA: 0x002FDA81 File Offset: 0x002FBC81
		public Instance(IStateMachineTarget master, ClusterFogOfWarManager.Def def) : base(master, def)
		{
		}

		// Token: 0x06008389 RID: 33673 RVA: 0x002FDA96 File Offset: 0x002FBC96
		public void Initialize()
		{
			this.UpdateRevealedCellsFromDiscoveredWorlds();
			this.EnsureRevealedTilesHavePeek();
		}

		// Token: 0x0600838A RID: 33674 RVA: 0x002FDAA4 File Offset: 0x002FBCA4
		public ClusterRevealLevel GetCellRevealLevel(AxialI location)
		{
			if (this.GetRevealCompleteFraction(location) >= 1f)
			{
				return ClusterRevealLevel.Visible;
			}
			if (this.GetRevealCompleteFraction(location) > 0f)
			{
				return ClusterRevealLevel.Peeked;
			}
			return ClusterRevealLevel.Hidden;
		}

		// Token: 0x0600838B RID: 33675 RVA: 0x002FDAC7 File Offset: 0x002FBCC7
		public void DEBUG_REVEAL_ENTIRE_MAP()
		{
			this.RevealLocation(AxialI.ZERO, 100);
		}

		// Token: 0x0600838C RID: 33676 RVA: 0x002FDAD6 File Offset: 0x002FBCD6
		public bool IsLocationRevealed(AxialI location)
		{
			return this.GetRevealCompleteFraction(location) >= 1f;
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x002FDAEC File Offset: 0x002FBCEC
		private void EnsureRevealedTilesHavePeek()
		{
			foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
			{
				if (this.IsLocationRevealed(keyValuePair.Key))
				{
					this.PeekLocation(keyValuePair.Key, 2);
				}
			}
		}

		// Token: 0x0600838E RID: 33678 RVA: 0x002FDB5C File Offset: 0x002FBD5C
		public void PeekLocation(AxialI location, int radius)
		{
			foreach (AxialI key in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				if (this.m_revealPointsByCell.ContainsKey(key))
				{
					this.m_revealPointsByCell[key] = Mathf.Max(this.m_revealPointsByCell[key], 0.01f);
				}
				else
				{
					this.m_revealPointsByCell[key] = 0.01f;
				}
			}
		}

		// Token: 0x0600838F RID: 33679 RVA: 0x002FDBEC File Offset: 0x002FBDEC
		public void RevealLocation(AxialI location, int radius = 0)
		{
			if (ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(location, EntityLayer.Asteroid).Count > 0 || ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null)
			{
				radius = Mathf.Max(radius, 1);
			}
			bool flag = false;
			foreach (AxialI cell in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				flag |= this.RevealCellIfValid(cell);
			}
			if (flag)
			{
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x06008390 RID: 33680 RVA: 0x002FDC90 File Offset: 0x002FBE90
		public void EarnRevealPointsForLocation(AxialI location, float points)
		{
			global::Debug.Assert(ClusterGrid.Instance.IsValidCell(location), string.Format("EarnRevealPointsForLocation called with invalid location: {0}", location));
			if (this.IsLocationRevealed(location))
			{
				return;
			}
			if (this.m_revealPointsByCell.ContainsKey(location))
			{
				Dictionary<AxialI, float> revealPointsByCell = this.m_revealPointsByCell;
				revealPointsByCell[location] += points;
			}
			else
			{
				this.m_revealPointsByCell[location] = points;
				Game.Instance.Trigger(-1554423969, location);
			}
			if (this.IsLocationRevealed(location))
			{
				this.RevealLocation(location, 0);
				this.PeekLocation(location, 2);
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x06008391 RID: 33681 RVA: 0x002FDD40 File Offset: 0x002FBF40
		public float GetRevealCompleteFraction(AxialI location)
		{
			if (!ClusterGrid.Instance.IsValidCell(location))
			{
				global::Debug.LogError(string.Format("GetRevealCompleteFraction called with invalid location: {0}, {1}", location.r, location.q));
			}
			if (DebugHandler.RevealFogOfWar)
			{
				return 1f;
			}
			float num;
			if (this.m_revealPointsByCell.TryGetValue(location, out num))
			{
				return Mathf.Min(num / ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL, 1f);
			}
			return 0f;
		}

		// Token: 0x06008392 RID: 33682 RVA: 0x002FDDB3 File Offset: 0x002FBFB3
		private bool RevealCellIfValid(AxialI cell)
		{
			if (!ClusterGrid.Instance.IsValidCell(cell))
			{
				return false;
			}
			if (this.IsLocationRevealed(cell))
			{
				return false;
			}
			this.m_revealPointsByCell[cell] = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL;
			this.PeekLocation(cell, 2);
			return true;
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x002FDDEC File Offset: 0x002FBFEC
		public bool GetUnrevealedLocationWithinRadius(AxialI center, int radius, out AxialI result)
		{
			for (int i = 0; i <= radius; i++)
			{
				foreach (AxialI axialI in AxialUtil.GetRing(center, i))
				{
					if (ClusterGrid.Instance.IsValidCell(axialI) && !this.IsLocationRevealed(axialI))
					{
						result = axialI;
						return true;
					}
				}
			}
			result = AxialI.ZERO;
			return false;
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x002FDE74 File Offset: 0x002FC074
		public void UpdateRevealedCellsFromDiscoveredWorlds()
		{
			int radius = DlcManager.IsExpansion1Active() ? 0 : 2;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered && !DebugHandler.RevealFogOfWar)
				{
					this.RevealLocation(worldContainer.GetComponent<ClusterGridEntity>().Location, radius);
				}
			}
		}

		// Token: 0x04006462 RID: 25698
		[Serialize]
		private Dictionary<AxialI, float> m_revealPointsByCell = new Dictionary<AxialI, float>();
	}
}
