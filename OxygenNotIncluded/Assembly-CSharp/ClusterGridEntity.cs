using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020009C3 RID: 2499
public abstract class ClusterGridEntity : KMonoBehaviour
{
	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06004AC5 RID: 19141
	public abstract string Name { get; }

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06004AC6 RID: 19142
	public abstract EntityLayer Layer { get; }

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06004AC7 RID: 19143
	public abstract List<ClusterGridEntity.AnimConfig> AnimConfigs { get; }

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06004AC8 RID: 19144
	public abstract bool IsVisible { get; }

	// Token: 0x06004AC9 RID: 19145 RVA: 0x001A52DC File Offset: 0x001A34DC
	public virtual bool ShowName()
	{
		return false;
	}

	// Token: 0x06004ACA RID: 19146 RVA: 0x001A52DF File Offset: 0x001A34DF
	public virtual bool ShowProgressBar()
	{
		return false;
	}

	// Token: 0x06004ACB RID: 19147 RVA: 0x001A52E2 File Offset: 0x001A34E2
	public virtual float GetProgress()
	{
		return 0f;
	}

	// Token: 0x06004ACC RID: 19148 RVA: 0x001A52E9 File Offset: 0x001A34E9
	public virtual bool SpaceOutInSameHex()
	{
		return false;
	}

	// Token: 0x06004ACD RID: 19149 RVA: 0x001A52EC File Offset: 0x001A34EC
	public virtual bool KeepRotationWhenSpacingOutInHex()
	{
		return false;
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x001A52EF File Offset: 0x001A34EF
	public virtual bool ShowPath()
	{
		return true;
	}

	// Token: 0x06004ACF RID: 19151 RVA: 0x001A52F2 File Offset: 0x001A34F2
	public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06004AD0 RID: 19152
	public abstract ClusterRevealLevel IsVisibleInFOW { get; }

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06004AD1 RID: 19153 RVA: 0x001A52F4 File Offset: 0x001A34F4
	// (set) Token: 0x06004AD2 RID: 19154 RVA: 0x001A52FC File Offset: 0x001A34FC
	public AxialI Location
	{
		get
		{
			return this.m_location;
		}
		set
		{
			if (value != this.m_location)
			{
				AxialI location = this.m_location;
				this.m_location = value;
				if (base.gameObject.GetSMI<StateMachine.Instance>() == null)
				{
					this.positionDirty = true;
				}
				this.SendClusterLocationChangedEvent(location, this.m_location);
			}
		}
	}

	// Token: 0x06004AD3 RID: 19155 RVA: 0x001A5348 File Offset: 0x001A3548
	protected override void OnSpawn()
	{
		ClusterGrid.Instance.RegisterEntity(this);
		if (this.m_selectable != null)
		{
			this.m_selectable.SetName(this.Name);
		}
		if (!this.isWorldEntity)
		{
			this.m_transform.SetLocalPosition(new Vector3(-1f, 0f, 0f));
		}
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.Trigger(1980521255, null);
		}
	}

	// Token: 0x06004AD4 RID: 19156 RVA: 0x001A53C4 File Offset: 0x001A35C4
	protected override void OnCleanUp()
	{
		ClusterGrid.Instance.UnregisterEntity(this);
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x001A53D4 File Offset: 0x001A35D4
	public virtual Sprite GetUISprite()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
			if (animConfigs.Count > 0)
			{
				return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, "ui", false, "");
			}
		}
		else
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
				if (worldData == null)
				{
					return null;
				}
				return Assets.GetSprite(worldData.asteroidIcon);
			}
		}
		return null;
	}

	// Token: 0x06004AD6 RID: 19158 RVA: 0x001A5450 File Offset: 0x001A3650
	public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
	{
		ClusterLocationChangedEvent data = new ClusterLocationChangedEvent
		{
			entity = this,
			oldLocation = oldLocation,
			newLocation = newLocation
		};
		base.Trigger(-1298331547, data);
		Game.Instance.Trigger(-1298331547, data);
		if (this.m_selectable != null && this.m_selectable.IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x04003109 RID: 12553
	[Serialize]
	protected AxialI m_location;

	// Token: 0x0400310A RID: 12554
	public bool positionDirty;

	// Token: 0x0400310B RID: 12555
	[MyCmpGet]
	protected KSelectable m_selectable;

	// Token: 0x0400310C RID: 12556
	[MyCmpReq]
	private Transform m_transform;

	// Token: 0x0400310D RID: 12557
	public bool isWorldEntity;

	// Token: 0x02001852 RID: 6226
	public struct AnimConfig
	{
		// Token: 0x040071A7 RID: 29095
		public KAnimFile animFile;

		// Token: 0x040071A8 RID: 29096
		public string initialAnim;

		// Token: 0x040071A9 RID: 29097
		public KAnim.PlayMode playMode;

		// Token: 0x040071AA RID: 29098
		public string symbolSwapTarget;

		// Token: 0x040071AB RID: 29099
		public string symbolSwapSymbol;

		// Token: 0x040071AC RID: 29100
		public Vector3 animOffset;

		// Token: 0x040071AD RID: 29101
		public float animPlaySpeedModifier;
	}
}
