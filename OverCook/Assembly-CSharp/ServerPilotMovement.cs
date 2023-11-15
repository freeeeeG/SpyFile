using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
public class ServerPilotMovement : ServerSynchroniserBase, IAssignControls
{
	// Token: 0x06001B70 RID: 7024 RVA: 0x0007E614 File Offset: 0x0007CA14
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_pilotMovement = (PilotMovement)synchronisedObject;
		this.m_gridManager = GameUtils.GetGridManager(base.transform.parent);
		this.m_collider = base.gameObject.RequestComponentRecursive<Collider>();
		if (this.m_collider != null)
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

	// Token: 0x06001B71 RID: 7025 RVA: 0x0007E759 File Offset: 0x0007CB59
	public void AssignPlayer(PlayerControls.ControlSchemeData _controlScheme)
	{
		this.m_controlScheme = _controlScheme;
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x0007E764 File Offset: 0x0007CB64
	private Vector3 GetNearestGridPosition()
	{
		GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		return this.m_gridManager.GetPosFromGridLocation(gridLocationFromPos);
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x0007E794 File Offset: 0x0007CB94
	public override void UpdateSynchronising()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		if (this.m_pilotMovement)
		{
			this.m_pilotMovement.RigidbodyMotion.SetKinematic(false);
			if (this.m_controlScheme != null && this.Update_Movement(deltaTime))
			{
				this.m_gridTarget = null;
				return;
			}
			if (this.m_gridTarget == null)
			{
				this.m_gridTarget = new Vector3?(this.GetNearestGridPosition());
			}
			float snapHalfLife = this.m_pilotMovement.SnapHalfLife;
			float d = Mathf.Log(2f) / snapHalfLife;
			Vector3 velocity = (this.m_gridTarget.Value - base.transform.position) * d;
			this.m_pilotMovement.RigidbodyMotion.SetVelocity(velocity);
			if (velocity.sqrMagnitude > 0.3f)
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
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0007E938 File Offset: 0x0007CD38
	private bool GridBoxCast(ref GridIndex min, ref GridIndex max, Vector3 centre, Vector3 extents, Vector3 direction, float distance, GridManager gridManager)
	{
		Vector3 a = centre + direction * distance;
		GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(a - extents);
		GridIndex gridLocationFromPos2 = this.m_gridManager.GetGridLocationFromPos(a + extents);
		if (this.m_gridManager.TryOccupyGridRegion(gridLocationFromPos, gridLocationFromPos2, base.gameObject))
		{
			min = gridLocationFromPos;
			max = gridLocationFromPos2;
			return true;
		}
		return false;
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x0007E9A8 File Offset: 0x0007CDA8
	private bool Update_Movement(float _deltaTime)
	{
		float value = this.m_controlScheme.m_moveX.GetValue();
		float z = -this.m_controlScheme.m_moveY.GetValue();
		Vector3 vector = new Vector3(value, 0f, z).SafeNormalised(Vector3.zero);
		if (vector.sqrMagnitude > 0.040000003f)
		{
			if (this.m_collider != null)
			{
				Vector3 zero = Vector3.zero;
				this.m_gridManager.DeoccupyGridRegion(this.m_min, this.m_max);
				if (this.GridBoxCast(ref this.m_min, ref this.m_max, this.m_collider.bounds.center, this.m_extents, vector, 0.3f, this.m_gridManager))
				{
					this.m_pilotMovement.RigidbodyMotion.SetVelocity(this.m_pilotMovement.MoveSpeed * vector);
					return true;
				}
				if (Mathf.Abs(vector.x) > 0.3f && this.GridBoxCast(ref this.m_min, ref this.m_max, this.m_collider.bounds.center, this.m_extents, new Vector3(Mathf.Sign(vector.x), 0f, 0f), 0.3f, this.m_gridManager))
				{
					this.m_pilotMovement.RigidbodyMotion.SetVelocity(new Vector3(this.m_pilotMovement.MoveSpeed * Mathf.Sign(vector.x), 0f, 0f));
					return true;
				}
				if (Mathf.Abs(vector.z) > 0.3f && this.GridBoxCast(ref this.m_min, ref this.m_max, this.m_collider.bounds.center, this.m_extents, new Vector3(0f, 0f, Mathf.Sign(vector.z)), 0.3f, this.m_gridManager))
				{
					this.m_pilotMovement.RigidbodyMotion.SetVelocity(new Vector3(0f, 0f, this.m_pilotMovement.MoveSpeed * Mathf.Sign(vector.z)));
					return true;
				}
				if (!this.m_gridManager.TryOccupyGridRegion(this.m_min, this.m_max, base.gameObject))
				{
				}
				this.m_pilotMovement.RigidbodyMotion.SetVelocity(Vector3.zero);
			}
			else
			{
				this.m_pilotMovement.RigidbodyMotion.SetVelocity(this.m_pilotMovement.MoveSpeed * vector);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04001588 RID: 5512
	protected PlayerControls.ControlSchemeData m_controlScheme;

	// Token: 0x04001589 RID: 5513
	private PilotMovement m_pilotMovement;

	// Token: 0x0400158A RID: 5514
	private GridManager m_gridManager;

	// Token: 0x0400158B RID: 5515
	private Vector3? m_gridTarget;

	// Token: 0x0400158C RID: 5516
	private Collider m_collider;

	// Token: 0x0400158D RID: 5517
	private GridIndex m_min;

	// Token: 0x0400158E RID: 5518
	private GridIndex m_max;

	// Token: 0x0400158F RID: 5519
	private Vector3 m_extents;

	// Token: 0x04001590 RID: 5520
	private const float k_castDistance = 0.3f;
}
