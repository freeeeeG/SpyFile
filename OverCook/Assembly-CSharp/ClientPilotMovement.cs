using System;
using System.Diagnostics;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
public class ClientPilotMovement : ClientSynchroniserBase
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06001B77 RID: 7031 RVA: 0x0007EDD8 File Offset: 0x0007D1D8
	// (remove) Token: 0x06001B78 RID: 7032 RVA: 0x0007EE10 File Offset: 0x0007D210
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<bool> OnPilotStatusChanged = delegate(bool _)
	{
	};

	// Token: 0x06001B79 RID: 7033 RVA: 0x0007EE46 File Offset: 0x0007D246
	private bool IsClientOnly()
	{
		return !ConnectionStatus.IsHost() && ConnectionStatus.IsInSession();
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x0007EE60 File Offset: 0x0007D260
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_pilotMovement = (PilotMovement)synchronisedObject;
		this.m_gridManager = GameUtils.RequireManager<GridManager>();
		this.m_collider = base.gameObject.RequestComponentRecursive<Collider>();
		if (this.IsClientOnly() && this.m_collider != null)
		{
			this.m_extents = new Vector3(this.m_collider.bounds.extents.x - 0.15f, this.m_collider.bounds.extents.y, this.m_collider.bounds.extents.z - 0.15f);
			GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(this.m_collider.bounds.center - this.m_extents);
			GridIndex gridLocationFromPos2 = this.m_gridManager.GetGridLocationFromPos(this.m_collider.bounds.center + this.m_extents);
			if (this.m_gridManager.TryOccupyGridRegion(gridLocationFromPos, gridLocationFromPos2, base.gameObject))
			{
				this.m_min = gridLocationFromPos;
				this.m_max = gridLocationFromPos2;
			}
		}
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x0007EFA8 File Offset: 0x0007D3A8
	public virtual void AssignAvatar(GameObject _avatar)
	{
		if (this.m_avatar)
		{
			this.SetIgnoreCollision(this.m_avatar, false);
		}
		if (_avatar)
		{
			this.SetIgnoreCollision(_avatar, true);
		}
		this.m_avatar = _avatar;
		this.OnPilotStatusChanged(_avatar != null);
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x0007F000 File Offset: 0x0007D400
	private void SetIgnoreCollision(GameObject _avatar, bool _shouldIgnore)
	{
		Collider collider = _avatar.RequireComponent<Collider>();
		foreach (Collider collider2 in base.gameObject.RequestComponentsRecursive<Collider>())
		{
			if (!collider2.transform.IsChildOf(_avatar.transform))
			{
				Physics.IgnoreCollision(collider, collider2, _shouldIgnore);
			}
		}
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0007F05C File Offset: 0x0007D45C
	public override void UpdateSynchronising()
	{
		if (this.IsClientOnly() && this.m_pilotMovement != null)
		{
			this.m_gridManager.DeoccupyGridRegion(this.m_min, this.m_max);
			GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(this.m_collider.bounds.center - this.m_extents);
			GridIndex gridLocationFromPos2 = this.m_gridManager.GetGridLocationFromPos(this.m_collider.bounds.center + this.m_extents);
			if (this.m_gridManager.TryOccupyGridRegion(gridLocationFromPos, gridLocationFromPos2, base.gameObject))
			{
				this.m_min = gridLocationFromPos;
				this.m_max = gridLocationFromPos2;
			}
			else if (!this.m_gridManager.TryOccupyGridRegion(this.m_min, this.m_max, base.gameObject))
			{
			}
		}
	}

	// Token: 0x04001592 RID: 5522
	private PilotMovement m_pilotMovement;

	// Token: 0x04001593 RID: 5523
	private GridManager m_gridManager;

	// Token: 0x04001594 RID: 5524
	private Collider m_collider;

	// Token: 0x04001595 RID: 5525
	private GridIndex m_min;

	// Token: 0x04001596 RID: 5526
	private GridIndex m_max;

	// Token: 0x04001597 RID: 5527
	private Vector3 m_extents;

	// Token: 0x04001598 RID: 5528
	private const float k_castDistance = 0.3f;

	// Token: 0x04001599 RID: 5529
	private GameObject m_avatar;
}
