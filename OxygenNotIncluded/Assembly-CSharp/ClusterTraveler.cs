using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000995 RID: 2453
public class ClusterTraveler : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06004871 RID: 18545 RVA: 0x001986CC File Offset: 0x001968CC
	public List<AxialI> CurrentPath
	{
		get
		{
			if (this.m_cachedPath == null || this.m_destinationSelector.GetDestination() != this.m_cachedPathDestination)
			{
				this.m_cachedPathDestination = this.m_destinationSelector.GetDestination();
				this.m_cachedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector);
			}
			return this.m_cachedPath;
		}
	}

	// Token: 0x06004872 RID: 18546 RVA: 0x00198737 File Offset: 0x00196937
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterTravelers.Add(this);
	}

	// Token: 0x06004873 RID: 18547 RVA: 0x0019874A File Offset: 0x0019694A
	protected override void OnCleanUp()
	{
		Components.ClusterTravelers.Remove(this);
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		base.OnCleanUp();
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x00198778 File Offset: 0x00196978
	private void ForceRevealLocation(AxialI location)
	{
		if (!ClusterGrid.Instance.IsCellVisible(location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 0);
		}
	}

	// Token: 0x06004875 RID: 18549 RVA: 0x00198798 File Offset: 0x00196998
	protected override void OnSpawn()
	{
		base.Subscribe<ClusterTraveler>(543433792, ClusterTraveler.ClusterDestinationChangedHandler);
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		this.UpdateAnimationTags();
		this.MarkPathDirty();
		this.RevalidatePath(false);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(this.m_clusterGridEntity.Location);
		}
	}

	// Token: 0x06004876 RID: 18550 RVA: 0x001987FE File Offset: 0x001969FE
	private void MarkPathDirty()
	{
		this.m_isPathDirty = true;
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x00198807 File Offset: 0x00196A07
	private void OnClusterFogOfWarRevealed(object data)
	{
		this.MarkPathDirty();
	}

	// Token: 0x06004878 RID: 18552 RVA: 0x0019880F File Offset: 0x00196A0F
	private void OnClusterDestinationChanged(object data)
	{
		if (this.m_destinationSelector.IsAtDestination())
		{
			this.m_movePotential = 0f;
			if (this.CurrentPath != null)
			{
				this.CurrentPath.Clear();
			}
		}
		this.MarkPathDirty();
	}

	// Token: 0x06004879 RID: 18553 RVA: 0x00198842 File Offset: 0x00196A42
	public int GetDestinationWorldID()
	{
		return this.m_destinationSelector.GetDestinationWorld();
	}

	// Token: 0x0600487A RID: 18554 RVA: 0x0019884F File Offset: 0x00196A4F
	public float TravelETA()
	{
		if (!this.IsTraveling() || this.getSpeedCB == null)
		{
			return 0f;
		}
		return this.RemainingTravelDistance() / this.getSpeedCB();
	}

	// Token: 0x0600487B RID: 18555 RVA: 0x0019887C File Offset: 0x00196A7C
	public float RemainingTravelDistance()
	{
		int num = this.RemainingTravelNodes();
		if (this.GetDestinationWorldID() >= 0)
		{
			num--;
			num = Mathf.Max(num, 0);
		}
		return (float)num * 600f - this.m_movePotential;
	}

	// Token: 0x0600487C RID: 18556 RVA: 0x001988B4 File Offset: 0x00196AB4
	public int RemainingTravelNodes()
	{
		int count = this.CurrentPath.Count;
		return Mathf.Max(0, count);
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x001988D4 File Offset: 0x00196AD4
	public float GetMoveProgress()
	{
		return this.m_movePotential / 600f;
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x001988E2 File Offset: 0x00196AE2
	public bool IsTraveling()
	{
		return !this.m_destinationSelector.IsAtDestination();
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x001988F4 File Offset: 0x00196AF4
	public void Sim200ms(float dt)
	{
		if (!this.IsTraveling())
		{
			return;
		}
		bool flag = this.CurrentPath != null && this.CurrentPath.Count > 0;
		bool flag2 = this.m_destinationSelector.HasAsteroidDestination();
		bool arg = flag2 && flag && this.CurrentPath.Count == 1;
		if (this.getCanTravelCB != null && !this.getCanTravelCB(arg))
		{
			return;
		}
		AxialI location = this.m_clusterGridEntity.Location;
		if (flag)
		{
			if (flag2)
			{
				bool requireLaunchPadOnAsteroidDestination = this.m_destinationSelector.requireLaunchPadOnAsteroidDestination;
			}
			if (!flag2 || this.CurrentPath.Count > 1 || !this.quickTravelToAsteroidIfInOrbit)
			{
				float num = dt * this.getSpeedCB();
				this.m_movePotential += num;
				if (this.m_movePotential >= 600f)
				{
					this.m_movePotential = 600f;
					if (this.AdvancePathOneStep())
					{
						global::Debug.Assert(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_clusterGridEntity.Location, EntityLayer.Asteroid) == null || (flag2 && this.CurrentPath.Count == 0), string.Format("Somehow this clustercraft pathed through an asteroid at {0}", this.m_clusterGridEntity.Location));
						this.m_movePotential -= 600f;
						if (this.onTravelCB != null)
						{
							this.onTravelCB();
						}
					}
				}
			}
			else
			{
				this.AdvancePathOneStep();
			}
		}
		this.RevalidatePath(true);
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x00198A6C File Offset: 0x00196C6C
	public bool AdvancePathOneStep()
	{
		if (this.validateTravelCB != null && !this.validateTravelCB(this.CurrentPath[0]))
		{
			return false;
		}
		AxialI location = this.CurrentPath[0];
		this.CurrentPath.RemoveAt(0);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(location);
		}
		this.m_clusterGridEntity.Location = location;
		this.UpdateAnimationTags();
		return true;
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x00198AD8 File Offset: 0x00196CD8
	private void UpdateAnimationTags()
	{
		if (this.CurrentPath == null)
		{
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		if (!(ClusterGrid.Instance.GetAsteroidAtCell(this.m_clusterGridEntity.Location) != null))
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityMoving);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			return;
		}
		if (this.CurrentPath.Count == 0 || this.m_clusterGridEntity.Location == this.CurrentPath[this.CurrentPath.Count - 1])
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLaunching);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x00198C08 File Offset: 0x00196E08
	public void RevalidatePath(bool react_to_change = true)
	{
		string reason;
		List<AxialI> cachedPath;
		if (this.HasCurrentPathChanged(out reason, out cachedPath))
		{
			if (this.stopAndNotifyWhenPathChanges && react_to_change)
			{
				this.m_destinationSelector.SetDestination(this.m_destinationSelector.GetMyWorldLocation());
				string message = MISC.NOTIFICATIONS.BADROCKETPATH.TOOLTIP;
				Notification notification = new Notification(MISC.NOTIFICATIONS.BADROCKETPATH.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => message + notificationList.ReduceMessages(false) + "\n\n" + reason, null, true, 0f, null, null, null, true, false, false);
				base.GetComponent<Notifier>().Add(notification, "");
				return;
			}
			this.m_cachedPath = cachedPath;
		}
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x00198CA0 File Offset: 0x00196EA0
	private bool HasCurrentPathChanged(out string reason, out List<AxialI> updatedPath)
	{
		if (!this.m_isPathDirty)
		{
			reason = null;
			updatedPath = null;
			return false;
		}
		this.m_isPathDirty = false;
		updatedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector, out reason, this.m_destinationSelector.dodgesHiddenAsteroids);
		if (updatedPath == null)
		{
			return true;
		}
		if (updatedPath.Count != this.m_cachedPath.Count)
		{
			return true;
		}
		for (int i = 0; i < this.m_cachedPath.Count; i++)
		{
			if (this.m_cachedPath[i] != updatedPath[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x00198D43 File Offset: 0x00196F43
	[ContextMenu("Fill Move Potential")]
	public void FillMovePotential()
	{
		this.m_movePotential = 600f;
	}

	// Token: 0x04002FF5 RID: 12277
	[MyCmpReq]
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x04002FF6 RID: 12278
	[MyCmpReq]
	private ClusterGridEntity m_clusterGridEntity;

	// Token: 0x04002FF7 RID: 12279
	[Serialize]
	private float m_movePotential;

	// Token: 0x04002FF8 RID: 12280
	public Func<float> getSpeedCB;

	// Token: 0x04002FF9 RID: 12281
	public Func<bool, bool> getCanTravelCB;

	// Token: 0x04002FFA RID: 12282
	public Func<AxialI, bool> validateTravelCB;

	// Token: 0x04002FFB RID: 12283
	public System.Action onTravelCB;

	// Token: 0x04002FFC RID: 12284
	private AxialI m_cachedPathDestination;

	// Token: 0x04002FFD RID: 12285
	private List<AxialI> m_cachedPath;

	// Token: 0x04002FFE RID: 12286
	private bool m_isPathDirty;

	// Token: 0x04002FFF RID: 12287
	public bool revealsFogOfWarAsItTravels = true;

	// Token: 0x04003000 RID: 12288
	public bool quickTravelToAsteroidIfInOrbit = true;

	// Token: 0x04003001 RID: 12289
	public bool stopAndNotifyWhenPathChanges;

	// Token: 0x04003002 RID: 12290
	private static EventSystem.IntraObjectHandler<ClusterTraveler> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<ClusterTraveler>(delegate(ClusterTraveler cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});
}
