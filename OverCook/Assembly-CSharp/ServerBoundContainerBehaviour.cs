using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class ServerBoundContainerBehaviour : ServerSynchroniserBase
{
	// Token: 0x060004A5 RID: 1189 RVA: 0x00027B8C File Offset: 0x00025F8C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_boundContainer = (BoundContainer)synchronisedObject;
		this.m_utensils = UnityEngine.Object.FindObjectsOfType<UtensilRespawnBehaviour>();
		this.m_xPositionMin = this.m_boundContainer.transform.position.x - this.m_boundContainer.m_boundSize.x / 2f;
		this.m_xPositionMax = this.m_boundContainer.transform.position.x + this.m_boundContainer.m_boundSize.x / 2f;
		this.m_yPositionMin = this.m_boundContainer.transform.position.y - this.m_boundContainer.m_boundSize.y / 2f;
		this.m_yPositionMax = this.m_boundContainer.transform.position.y + this.m_boundContainer.m_boundSize.y / 2f;
		this.m_zPostionMin = this.m_boundContainer.transform.position.z - this.m_boundContainer.m_boundSize.z / 2f;
		this.m_zPostionMax = this.m_boundContainer.transform.position.z + this.m_boundContainer.m_boundSize.z / 2f;
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00027CF8 File Offset: 0x000260F8
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_utensils != null)
		{
			for (int i = 0; i < this.m_utensils.Length; i++)
			{
				GameObject gameObject = (!(this.m_utensils[i] != null)) ? null : this.m_utensils[i].gameObject;
				if (gameObject != null && !this.IsInBound(gameObject.transform.position) && gameObject.activeInHierarchy)
				{
					ServerPlayerRespawnManager.KillOrRespawn(gameObject, null);
				}
			}
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00027D8C File Offset: 0x0002618C
	private bool IsInBound(Vector3 position)
	{
		return position.x > this.m_xPositionMin && position.x < this.m_xPositionMax && position.y > this.m_yPositionMin && position.y < this.m_yPositionMax && position.z > this.m_zPostionMin && position.z < this.m_zPostionMax;
	}

	// Token: 0x0400040F RID: 1039
	private BoundContainer m_boundContainer;

	// Token: 0x04000410 RID: 1040
	private UtensilRespawnBehaviour[] m_utensils;

	// Token: 0x04000411 RID: 1041
	private float m_xPositionMin;

	// Token: 0x04000412 RID: 1042
	private float m_xPositionMax;

	// Token: 0x04000413 RID: 1043
	private float m_yPositionMin;

	// Token: 0x04000414 RID: 1044
	private float m_yPositionMax;

	// Token: 0x04000415 RID: 1045
	private float m_zPostionMin;

	// Token: 0x04000416 RID: 1046
	private float m_zPostionMax;
}
