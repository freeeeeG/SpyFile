using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000438 RID: 1080
[AddComponentMenu("Scripts/Game/Environment/AttachStation")]
[RequireComponent(typeof(Collider))]
public class AttachStation : MonoBehaviour, IParentable
{
	// Token: 0x060013D8 RID: 5080 RVA: 0x0006D65D File Offset: 0x0006BA5D
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_attachPoint;
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x0006D665 File Offset: 0x0006BA65
	public void SetClientSidePredictionEnabled(bool bEnabled)
	{
		this.m_bClientSidePrediction = bEnabled;
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x0006D66E File Offset: 0x0006BA6E
	public bool HasClientSidePrediction()
	{
		return this.m_bClientSidePrediction;
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x0006D678 File Offset: 0x0006BA78
	public PlacementType CalculatePlacementType<T>(GameObject _item, PlacementContext _context, ICarrier _iCarrier, Vector2 _directionXZ, IBaseHandlePlacement placementHandler, List<Generic<bool, GameObject, PlacementContext>> _placementQuery, ICarrier _holder) where T : class, IBaseHandlePlacement
	{
		if (_item == null)
		{
			if (this.CanAttachToSelf(_item, _iCarrier.InspectCarriedItem(), _context, _placementQuery))
			{
				return PlacementType.OntoEmpty;
			}
			if (this.CanAttachContents(_item, _iCarrier.InspectCarriedItem(), _context, _placementQuery))
			{
				return PlacementType.ContentsOntoEmpty;
			}
			return PlacementType.NotValid;
		}
		else if (placementHandler != null)
		{
			if (!placementHandler.CanHandlePlacement(_iCarrier, _directionXZ, _context))
			{
				return PlacementType.NotValid;
			}
			if (this.CanPlaceUnder<T>(_item, _iCarrier, _directionXZ, _holder, _context, _placementQuery))
			{
				return PlacementType.OntoAndUnderOccupant;
			}
			return PlacementType.OntoOccupant;
		}
		else
		{
			if (this.CanPlaceUnder<T>(_item, _iCarrier, _directionXZ, _holder, _context, _placementQuery))
			{
				return PlacementType.UnderOccupant;
			}
			return PlacementType.NotValid;
		}
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x0006D70C File Offset: 0x0006BB0C
	public bool CanPlaceUnder<T>(GameObject _item, ICarrier _iCarrier, Vector2 _directionXZ, ICarrier _holder, PlacementContext _context, List<Generic<bool, GameObject, PlacementContext>> _placementQuery) where T : class, IBaseHandlePlacement
	{
		GameObject gameObject = _iCarrier.InspectCarriedItem();
		T t = gameObject.RequestInterface<T>();
		return t != null && t.CanHandlePlacement(_holder, _directionXZ, _context) && this.CouldAttachToSelfIfEmpty(gameObject, _context, _placementQuery) && _item.RequestInterface<IPlaceUnder>() != null;
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x0006D767 File Offset: 0x0006BB67
	public bool CanAttachToSelf(GameObject _item, GameObject _carriedItem, PlacementContext _context, List<Generic<bool, GameObject, PlacementContext>> _placementQuery)
	{
		return _item == null && this.CouldAttachToSelfIfEmpty(_carriedItem, _context, _placementQuery);
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x0006D782 File Offset: 0x0006BB82
	public bool CouldAttachToSelfIfEmpty(GameObject _item, PlacementContext _context, List<Generic<bool, GameObject, PlacementContext>> _placementQuery)
	{
		return _item.RequestInterface<IAttachment>() != null && !_placementQuery.CallForResult(false, _item, _context);
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x0006D7A0 File Offset: 0x0006BBA0
	public bool CanAttachContents(GameObject _item, GameObject _itemContainer, PlacementContext _context, List<Generic<bool, GameObject, PlacementContext>> _placementQuery)
	{
		if (_item == null)
		{
			IIngredientContents component = _itemContainer.GetComponent<IIngredientContents>();
			if (component != null && component.GetContentsCount() == 1)
			{
				AssembledDefinitionNode assembledDefinitionNode = component.GetContents()[0];
				if (assembledDefinitionNode.m_freeObject != null)
				{
					return this.CouldAttachToSelfIfEmpty(assembledDefinitionNode.m_freeObject, _context, _placementQuery);
				}
			}
		}
		return false;
	}

	// Token: 0x04000F64 RID: 3940
	[SerializeField]
	public Transform m_attachPoint;

	// Token: 0x04000F65 RID: 3941
	[SerializeField]
	public int m_pickupPriority;

	// Token: 0x04000F66 RID: 3942
	[SerializeField]
	public int m_placementPriority;

	// Token: 0x04000F67 RID: 3943
	[SerializeField]
	public int m_catchingPriority;

	// Token: 0x04000F68 RID: 3944
	[SerializeField]
	public bool m_canCatch = true;

	// Token: 0x04000F69 RID: 3945
	private bool m_bClientSidePrediction;
}
