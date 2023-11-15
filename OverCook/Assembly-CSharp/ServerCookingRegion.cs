using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class ServerCookingRegion : ServerSynchroniserBase
{
	// Token: 0x060014B4 RID: 5300 RVA: 0x00070C52 File Offset: 0x0006F052
	public override EntityType GetEntityType()
	{
		return EntityType.CookingRegion;
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x00070C58 File Offset: 0x0006F058
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_CookingRegion = (CookingRegion)synchronisedObject;
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		this.m_gridIndex = this.m_gridManager.GetGridLocationFromPos(base.transform.position);
		this.m_recorder = base.gameObject.RequireComponent<TriggerRecorder>();
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x00070CB8 File Offset: 0x0006F0B8
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_CookingRegion == null || !this.m_CookingRegion.enabled)
		{
			return;
		}
		List<Collider> recentCollisions = this.m_recorder.GetRecentCollisions();
		for (int i = 0; i < recentCollisions.Count; i++)
		{
			Collider collider = recentCollisions[i];
			ICookable cookable = collider.gameObject.RequestInterface<ICookable>();
			if (cookable != null && cookable.GetRequiredStationType() == this.m_CookingRegion.m_StationType && !this.m_cookedThisFrame.Contains(cookable))
			{
				GridManager gridManager = GameUtils.GetGridManager(collider.transform);
				Vector3 position = collider.transform.position;
				if (!(gridManager != this.m_gridManager) && !(gridManager.GetGridLocationFromPos(position) != this.m_gridIndex))
				{
					IOrderDefinition orderDefinition = collider.gameObject.RequestInterface<IOrderDefinition>();
					if (orderDefinition.GetOrderComposition().Simpilfy() != AssembledDefinitionNode.NullNode)
					{
						cookable.Cook(TimeManager.GetDeltaTime(base.gameObject.layer));
						this.m_cookedThisFrame.Add(cookable);
					}
				}
			}
		}
		this.m_cookedThisFrame.Clear();
	}

	// Token: 0x04000FEB RID: 4075
	private CookingRegion m_CookingRegion;

	// Token: 0x04000FEC RID: 4076
	private TriggerRecorder m_recorder;

	// Token: 0x04000FED RID: 4077
	private GridManager m_gridManager;

	// Token: 0x04000FEE RID: 4078
	private GridIndex m_gridIndex;

	// Token: 0x04000FEF RID: 4079
	private List<ICookable> m_cookedThisFrame = new List<ICookable>();
}
