using System;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
public class CapsuleCollisionHelper : MonoBehaviour
{
	// Token: 0x06002C9B RID: 11419 RVA: 0x000D2954 File Offset: 0x000D0D54
	private void Awake()
	{
		this.m_CapsuleCollider = base.gameObject.RequireComponent<CapsuleCollider>();
		this.m_fCapsuleRadius = this.m_CapsuleCollider.radius * 0.99f;
		this.m_fCapsuleDiameter = this.m_fCapsuleRadius * 2f;
		float num = this.m_CapsuleCollider.height * 0.5f - this.m_fCapsuleRadius;
		float num2 = this.m_CapsuleCollider.center.y - base.transform.position.y;
		this.m_CapsuleTopOffset.Set(0f, num2 + num, 0f);
		this.m_CapsuleBottomOffset.Set(0f, num2 - num, 0f);
		this.m_RayCollisionMask = (-1 ^ 1 << LayerMask.NameToLayer("KillPlane") ^ 1 << LayerMask.NameToLayer("Ground") ^ 1 << LayerMask.NameToLayer("SlopedGround") ^ 1 << LayerMask.NameToLayer("Ignore Raycast") ^ 1 << LayerMask.NameToLayer("PlayerTriggerZone") ^ 1 << LayerMask.NameToLayer("Attachments") ^ 1 << LayerMask.NameToLayer("HeldAttachments") ^ 1 << LayerMask.NameToLayer("PushedObjectBounds"));
		this.m_CapsuleCheckCollisionMask = (-1 ^ 1 << LayerMask.NameToLayer("KillPlane") ^ 1 << LayerMask.NameToLayer("Ground") ^ 1 << LayerMask.NameToLayer("SlopedGround") ^ 1 << LayerMask.NameToLayer("Ignore Raycast") ^ 1 << LayerMask.NameToLayer("PlayerTriggerZone") ^ 1 << LayerMask.NameToLayer("Players") ^ 1 << LayerMask.NameToLayer("Attachments") ^ 1 << LayerMask.NameToLayer("HeldAttachments") ^ 1 << LayerMask.NameToLayer("PushedObjectBounds"));
		this.m_CapsuleKillPlaneMask = 1 << LayerMask.NameToLayer("KillPlane");
		Transform transform = base.transform.Find("Capsule");
		if (transform != null)
		{
			this.m_bShowSweepTests = true;
			MeshFilter component = transform.GetComponent<MeshFilter>();
			this.m_DebugMesh = component.mesh;
			Renderer component2 = transform.GetComponent<Renderer>();
			this.m_DebugMaterial = component2.material;
			Color color = this.m_DebugMaterial.color;
			color.a = 0.5f;
			this.m_DebugMaterial.color = color;
			this.m_DebugSortLayer = component2.sortingLayerID;
		}
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x000D2BC4 File Offset: 0x000D0FC4
	public void RenderCapsule(Vector3 position)
	{
		Vector3 vector = new Vector3(this.m_fCapsuleDiameter, this.m_fCapsuleDiameter, this.m_fCapsuleDiameter);
		Matrix4x4 matrix = Matrix4x4.Translate(position) * Matrix4x4.Scale(vector);
		Graphics.DrawMesh(this.m_DebugMesh, matrix, this.m_DebugMaterial, this.m_DebugSortLayer);
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x000D2C14 File Offset: 0x000D1014
	public void UpdateCollisionMask(bool bChefVsChef)
	{
		int num = 1 << LayerMask.NameToLayer("Players");
		if (bChefVsChef)
		{
			this.m_RayCollisionMask |= num;
		}
		else
		{
			this.m_RayCollisionMask &= ~num;
		}
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x000D2C59 File Offset: 0x000D1059
	public bool CheckCapsule(Vector3 startPosition)
	{
		return Physics.CheckCapsule(this.m_CapsuleTopOffset + startPosition, this.m_CapsuleBottomOffset + startPosition, this.m_fCapsuleRadius, this.m_CapsuleCheckCollisionMask, QueryTriggerInteraction.Ignore);
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000D2C85 File Offset: 0x000D1085
	public bool CheckCapsuleKillPlane(Vector3 startPosition)
	{
		return Physics.CheckCapsule(this.m_CapsuleTopOffset + startPosition, this.m_CapsuleBottomOffset + startPosition, this.m_fCapsuleRadius, this.m_CapsuleKillPlaneMask, QueryTriggerInteraction.Collide);
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x000D2CB4 File Offset: 0x000D10B4
	public void CastPositionForward(ref Vector3 startPosition, Vector3 direction, bool bChefVsChef)
	{
		float magnitude = direction.magnitude;
		Vector3 normalized = direction.normalized;
		float b = 0.015f;
		if ((double)magnitude <= 0.001)
		{
			return;
		}
		if (Physics.CapsuleCast(this.m_CapsuleTopOffset + startPosition, this.m_CapsuleBottomOffset + startPosition, this.m_fCapsuleRadius, normalized, out this.m_RaycastHit, magnitude, this.m_RayCollisionMask, QueryTriggerInteraction.Ignore))
		{
			Vector3 vector = startPosition;
			float num = magnitude;
			int num2 = 0;
			int num3 = 30;
			for (;;)
			{
				vector += normalized * (this.m_RaycastHit.distance - 0.01f);
				num -= this.m_RaycastHit.distance - 0.01f;
				Vector3 vector2 = new Vector3(-this.m_RaycastHit.normal.z, 0f, this.m_RaycastHit.normal.x);
				Vector3 normalized2 = vector2.normalized;
				float num4 = Vector3.Dot(normalized, normalized2);
				if (num4 == 0f)
				{
					break;
				}
				float num5 = Mathf.Min(num * Mathf.Abs(num4), b);
				Vector3 b2 = num5 * Mathf.Sign(num4) * normalized2;
				vector += b2;
				num = Mathf.Max(0f, num - num5 / Mathf.Max(Mathf.Abs(num4), 1E-06f));
				num2++;
				if (num2 >= num3 || num <= 0f || !Physics.CapsuleCast(this.m_CapsuleTopOffset + vector, this.m_CapsuleBottomOffset + vector, this.m_fCapsuleRadius, normalized, out this.m_RaycastHit, num, this.m_RayCollisionMask, QueryTriggerInteraction.Ignore))
				{
					goto IL_1A6;
				}
			}
			return;
			IL_1A6:
			Vector3 vector3 = vector + normalized * Mathf.Min(0f, num);
			vector3.y = startPosition.y;
			startPosition = vector3;
			return;
		}
		if (magnitude > 0f)
		{
			startPosition += normalized * magnitude;
		}
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x000D2EBC File Offset: 0x000D12BC
	public bool CheckPathToPoint(Vector3 start, Vector3 end)
	{
		Vector3 vector = end - start;
		float magnitude = vector.magnitude;
		vector.Normalize();
		if (Physics.CapsuleCast(start + this.m_CapsuleTopOffset, start + this.m_CapsuleBottomOffset, this.m_fCapsuleRadius, vector, out this.m_RaycastHit, magnitude, this.m_RayCollisionMask, QueryTriggerInteraction.Ignore))
		{
			float num = Vector3.Dot(vector, this.m_RaycastHit.normal);
			num = Mathf.Acos(num) * 57.29578f;
			if (num > 95f)
			{
				Vector3 vector2 = vector * -1f;
				if (Physics.CapsuleCast(end + this.m_CapsuleTopOffset, end + this.m_CapsuleBottomOffset, this.m_fCapsuleRadius, vector2, out this.m_RaycastHit, magnitude, this.m_RayCollisionMask, QueryTriggerInteraction.Ignore))
				{
					float num2 = Vector3.Dot(vector2, this.m_RaycastHit.normal);
					num2 = Mathf.Acos(num2) * 57.29578f;
					if (num2 > 95f)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x040023E3 RID: 9187
	private CapsuleCollider m_CapsuleCollider;

	// Token: 0x040023E4 RID: 9188
	private RaycastHit m_RaycastHit = default(RaycastHit);

	// Token: 0x040023E5 RID: 9189
	private float m_fCapsuleRadius = 1f;

	// Token: 0x040023E6 RID: 9190
	private float m_fCapsuleDiameter = 2f;

	// Token: 0x040023E7 RID: 9191
	private Vector3 m_CapsuleTopOffset = Vector3.zero;

	// Token: 0x040023E8 RID: 9192
	private Vector3 m_CapsuleBottomOffset = Vector3.zero;

	// Token: 0x040023E9 RID: 9193
	private int m_RayCollisionMask;

	// Token: 0x040023EA RID: 9194
	private int m_CapsuleCheckCollisionMask;

	// Token: 0x040023EB RID: 9195
	private int m_CapsuleKillPlaneMask;

	// Token: 0x040023EC RID: 9196
	private bool m_bShowSweepTests;

	// Token: 0x040023ED RID: 9197
	private Material m_DebugMaterial;

	// Token: 0x040023EE RID: 9198
	private Mesh m_DebugMesh;

	// Token: 0x040023EF RID: 9199
	private int m_DebugSortLayer;

	// Token: 0x040023F0 RID: 9200
	private Vector3 m_DebugMeshCentre = Vector3.zero;
}
