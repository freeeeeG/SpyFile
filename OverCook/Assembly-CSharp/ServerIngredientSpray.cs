using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060E RID: 1550
public class ServerIngredientSpray : ServerSprayingUtensil
{
	// Token: 0x06001D5A RID: 7514 RVA: 0x0008FE94 File Offset: 0x0008E294
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_Spray = (IngredientSpray)synchronisedObject;
		this.m_Adapter = new ServerIngredientSpray.CarrierAdapter(this);
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x0008FEB8 File Offset: 0x0008E2B8
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		float deltaTime = Time.deltaTime;
		for (int i = this.m_SprayHistory.Count - 1; i >= 0; i--)
		{
			ServerIngredientSpray.SprayedInfo value = this.m_SprayHistory[i];
			value.m_CooldownTimer -= deltaTime;
			this.m_SprayHistory[i] = value;
			if (value.m_CooldownTimer <= 0f)
			{
				this.m_SprayHistory.RemoveAt(i);
			}
		}
		if (base.IsSpraying())
		{
			List<ServerIngredientContainer> allIngredientContainers = ServerIngredientContainer.GetAllIngredientContainers();
			for (int j = 0; j < allIngredientContainers.Count; j++)
			{
				GameObject gameObject = allIngredientContainers[j].gameObject;
				IHandlePlacement controllingPlacementHandler_Server = PlayerControlsHelper.GetControllingPlacementHandler_Server(gameObject);
				if (controllingPlacementHandler_Server != null && !this.PlacementHandlerInHistory(controllingPlacementHandler_Server))
				{
					if (base.IsInSpray(allIngredientContainers[j].transform) && controllingPlacementHandler_Server.CanHandlePlacement(this.m_Adapter, Vector2.up, this.m_PlacementContext))
					{
						controllingPlacementHandler_Server.HandlePlacement(this.m_Adapter, Vector2.up, this.m_PlacementContext);
						this.m_SprayHistory.Add(new ServerIngredientSpray.SprayedInfo(controllingPlacementHandler_Server));
					}
				}
			}
		}
		else
		{
			this.m_SprayHistory.Clear();
		}
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x00090004 File Offset: 0x0008E404
	private bool PlacementHandlerInHistory(IHandlePlacement iPlacement)
	{
		for (int i = 0; i < this.m_SprayHistory.Count; i++)
		{
			if (this.m_SprayHistory[i].m_IPlacement == iPlacement)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040016C0 RID: 5824
	public IngredientSpray m_Spray;

	// Token: 0x040016C1 RID: 5825
	private ServerIngredientSpray.CarrierAdapter m_Adapter;

	// Token: 0x040016C2 RID: 5826
	private PlacementContext m_PlacementContext = new PlacementContext(PlacementContext.Source.Player);

	// Token: 0x040016C3 RID: 5827
	private List<ServerIngredientSpray.SprayedInfo> m_SprayHistory = new List<ServerIngredientSpray.SprayedInfo>(16);

	// Token: 0x0200060F RID: 1551
	private struct SprayedInfo
	{
		// Token: 0x06001D5D RID: 7517 RVA: 0x0009004A File Offset: 0x0008E44A
		public SprayedInfo(IHandlePlacement _iPlacement)
		{
			this.m_IPlacement = _iPlacement;
			this.m_CooldownTimer = 0.25f;
		}

		// Token: 0x040016C4 RID: 5828
		public const float kCOOLDOWN_TIME = 0.25f;

		// Token: 0x040016C5 RID: 5829
		public IHandlePlacement m_IPlacement;

		// Token: 0x040016C6 RID: 5830
		public float m_CooldownTimer;
	}

	// Token: 0x02000610 RID: 1552
	public class CarrierAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x06001D5E RID: 7518 RVA: 0x0009005E File Offset: 0x0008E45E
		public CarrierAdapter(ServerIngredientSpray _spray)
		{
			this.m_Spray = _spray;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0009006D File Offset: 0x0008E46D
		public GameObject InspectCarriedItem()
		{
			return this.m_Spray.m_Spray.m_OrderPrefab;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x0009007F File Offset: 0x0008E47F
		public GameObject AccessGameObject()
		{
			return this.m_Spray.Carrier.gameObject;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x00090091 File Offset: 0x0008E491
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00090093 File Offset: 0x0008E493
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x00090095 File Offset: 0x0008E495
		public void CarryItem(GameObject _object)
		{
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x00090097 File Offset: 0x0008E497
		public GameObject TakeItem()
		{
			return null;
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0009009A File Offset: 0x0008E49A
		public void DestroyCarriedItem()
		{
		}

		// Token: 0x040016C7 RID: 5831
		private ServerIngredientSpray m_Spray;
	}
}
