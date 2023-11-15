using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class ClientTrayCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060012BF RID: 4799 RVA: 0x00069188 File Offset: 0x00067588
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_trayCosmeticDecisions = (TrayCosmeticDecisions)synchronisedObject;
		this.m_iOrderDefinition = base.gameObject.RequireInterface<IClientOrderDefinition>();
		this.m_iOrderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
		this.m_tray = base.gameObject.RequireComponent<Tray>();
		this.m_clientTray = base.gameObject.RequireComponent<ClientTray>();
		this.m_containers = new GameObject[this.m_trayCosmeticDecisions.m_attachPoints.Length];
		this.m_ingredientContainer = base.gameObject.RequireComponent<ClientIngredientContainer>();
		this.OnOrderCompositionChanged(this.m_iOrderDefinition.GetOrderComposition());
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x00069228 File Offset: 0x00067628
	protected void OnOrderCompositionChanged(AssembledDefinitionNode _orderComposition)
	{
		ClientPlate.IngredientContainerAdapter ingredientContainerAdapter = new ClientPlate.IngredientContainerAdapter(this.m_ingredientContainer);
		for (int i = 0; i < this.m_trayCosmeticDecisions.m_attachPoints.Length; i++)
		{
			if (this.m_containers[i] != null)
			{
				UnityEngine.Object.Destroy(this.m_containers[i]);
				this.m_containers[i] = null;
			}
			IIngredientContents ingredientContents = this.m_clientTray.GetIngredientContents(i, false);
			if (ingredientContents != null)
			{
				AssembledDefinitionNode orderComposition = this.m_tray.GetOrderComposition(ingredientContents.GetContents());
				if (!this.IsEmpty(orderComposition))
				{
					GameObject orderPlatingPrefab = GameUtils.GetOrderPlatingPrefab(orderComposition, this.m_trayCosmeticDecisions.m_platingStep);
					this.m_containers[i] = UnityEngine.Object.Instantiate<GameObject>((!(orderPlatingPrefab != null)) ? this.m_trayCosmeticDecisions.m_noMatchingRecipePrefab : orderPlatingPrefab);
					this.m_containers[i].transform.SetParent(this.m_trayCosmeticDecisions.m_attachPoints[i]);
					this.m_containers[i].transform.localPosition = Vector3.zero;
					this.m_containers[i].transform.localRotation = Quaternion.identity;
					RendererSceneInfo rendererSceneInfo = this.m_containers[i].RequestComponent<RendererSceneInfo>();
					if (rendererSceneInfo == null)
					{
						rendererSceneInfo = this.m_containers[i].AddComponent<RendererSceneInfo>();
						rendererSceneInfo.m_rendererClass = RendererSceneSettings.RendererClass.MealCosmetic;
					}
					IAssignOrderDefinition assignOrderDefinition = this.m_containers[i].RequestInterface<IAssignOrderDefinition>();
					if (assignOrderDefinition != null)
					{
						assignOrderDefinition.SetOrderComposition(orderComposition);
					}
				}
			}
		}
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x00069398 File Offset: 0x00067798
	private bool IsEmpty(AssembledDefinitionNode _orderComposition)
	{
		CompositeAssembledNode compositeAssembledNode = _orderComposition as CompositeAssembledNode;
		return compositeAssembledNode != null && compositeAssembledNode.m_composition.Length == 0;
	}

	// Token: 0x04000EBC RID: 3772
	protected TrayCosmeticDecisions m_trayCosmeticDecisions;

	// Token: 0x04000EBD RID: 3773
	private GameObject[] m_containers;

	// Token: 0x04000EBE RID: 3774
	private IClientOrderDefinition m_iOrderDefinition;

	// Token: 0x04000EBF RID: 3775
	private Tray m_tray;

	// Token: 0x04000EC0 RID: 3776
	private ClientTray m_clientTray;

	// Token: 0x04000EC1 RID: 3777
	private ClientIngredientContainer m_ingredientContainer;
}
