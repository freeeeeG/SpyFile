using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class ClientAttachedOrderCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060010E8 RID: 4328 RVA: 0x00061094 File Offset: 0x0005F494
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_attachedOrderCosmeticDecisions = (AttachedOrderCosmeticDecisions)synchronisedObject;
		this.m_iOrderDefinition = base.gameObject.RequireInterface<IClientOrderDefinition>();
		this.m_iOrderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
		this.OnOrderCompositionChanged(this.m_iOrderDefinition.GetOrderComposition());
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x000610E8 File Offset: 0x0005F4E8
	private void OnOrderCompositionChanged(AssembledDefinitionNode _orderComposition)
	{
		if (this.m_container != null)
		{
			UnityEngine.Object.Destroy(this.m_container);
			this.m_container = null;
		}
		if (!this.IsEmpty(_orderComposition))
		{
			GameObject orderPlatingPrefab = GameUtils.GetOrderPlatingPrefab(_orderComposition, this.m_attachedOrderCosmeticDecisions.m_platingStep);
			this.m_container = UnityEngine.Object.Instantiate<GameObject>((!(orderPlatingPrefab != null)) ? this.m_attachedOrderCosmeticDecisions.m_noMatchingRecipePrefab : orderPlatingPrefab);
			this.m_container.transform.SetParent(this.m_attachedOrderCosmeticDecisions.m_attachPoint);
			this.m_container.transform.localPosition = Vector3.zero;
			this.m_container.transform.localRotation = Quaternion.identity;
			RendererSceneInfo rendererSceneInfo = this.m_container.RequestComponent<RendererSceneInfo>();
			if (rendererSceneInfo == null)
			{
				rendererSceneInfo = this.m_container.AddComponent<RendererSceneInfo>();
				rendererSceneInfo.m_rendererClass = RendererSceneSettings.RendererClass.MealCosmetic;
			}
			IAssignOrderDefinition assignOrderDefinition = this.m_container.RequestInterface<IAssignOrderDefinition>();
			if (assignOrderDefinition != null)
			{
				assignOrderDefinition.SetOrderComposition(_orderComposition);
			}
			this.OnContentsCreated(this.m_container);
		}
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x000611F4 File Offset: 0x0005F5F4
	private bool IsEmpty(AssembledDefinitionNode _orderComposition)
	{
		CompositeAssembledNode compositeAssembledNode = _orderComposition as CompositeAssembledNode;
		return compositeAssembledNode != null && compositeAssembledNode.m_composition.Length == 0;
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0006121B File Offset: 0x0005F61B
	protected virtual void OnContentsCreated(GameObject _contents)
	{
	}

	// Token: 0x04000D0D RID: 3341
	private AttachedOrderCosmeticDecisions m_attachedOrderCosmeticDecisions;

	// Token: 0x04000D0E RID: 3342
	private GameObject m_container;

	// Token: 0x04000D0F RID: 3343
	private IClientOrderDefinition m_iOrderDefinition;
}
