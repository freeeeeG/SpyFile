using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class ClientDirtyPlateStack : ClientPlateStackBase, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x060014F4 RID: 5364 RVA: 0x00072618 File Offset: 0x00070A18
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		DirtyPlateStack dirtyPlateStack = (DirtyPlateStack)synchronisedObject;
		if (dirtyPlateStack.m_washedPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, dirtyPlateStack.m_washedPrefab);
		}
		if (dirtyPlateStack.m_cleanPlatePrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, dirtyPlateStack.m_cleanPlatePrefab);
		}
		this.m_highlight = base.gameObject.RequireComponent<ClientAnticipateInteractionHighlight>();
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x00072688 File Offset: 0x00070A88
	protected override void PlateSpawned(GameObject _object)
	{
		this.m_stack.AddToStack(_object);
		base.StartCoroutine(this.DelayUpdateHighlight());
		base.NotifyPlateAdded(_object);
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x000726AC File Offset: 0x00070AAC
	protected override void PlateRemoved()
	{
		GameObject plate = this.m_stack.RemoveFromStack();
		base.StartCoroutine(this.DelayUpdateHighlight());
		base.NotifyPlateRemoved(plate);
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000726DC File Offset: 0x00070ADC
	private IEnumerator DelayUpdateHighlight()
	{
		yield return null;
		this.m_highlight.RebuildHighlightMaterials();
		yield break;
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000726F8 File Offset: 0x00070AF8
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject obj = _carrier.InspectCarriedItem();
		DirtyPlateStack dirtyPlateStack = obj.RequestComponent<DirtyPlateStack>();
		if (dirtyPlateStack != null)
		{
			DirtyPlateStack dirtyPlateStack2 = (DirtyPlateStack)this.m_plateStack;
			return dirtyPlateStack.m_plateType == dirtyPlateStack2.m_plateType;
		}
		return false;
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0007273E File Offset: 0x00070B3E
	public int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x04001024 RID: 4132
	private ClientAnticipateInteractionHighlight m_highlight;
}
