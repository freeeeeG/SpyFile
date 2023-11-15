using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class ClientCondimentDispenserCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001169 RID: 4457 RVA: 0x00063A30 File Offset: 0x00061E30
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_condimentDispenserCosmeticDecisions = (CondimentDispenserCosmeticDecisions)synchronisedObject;
		this.m_animator = base.gameObject.RequestComponentRecursive<Animator>();
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
		this.m_placementItemSwitcher = base.gameObject.RequireComponent<PlacementItemSwitcher>();
		this.OnItemSwitched();
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00063A84 File Offset: 0x00061E84
	public void OnPickupItem()
	{
		for (int i = 0; i < this.m_condimentDispenserCosmeticDecisions.m_particleEffects.Length; i++)
		{
			this.m_condimentDispenserCosmeticDecisions.m_particleEffects[i].SetActive(false);
		}
		AssembledDefinitionNode orderComposition = this.m_orderDefinition.GetOrderComposition();
		for (int j = 0; j < this.m_placementItemSwitcher.m_ingredients.Length; j++)
		{
			if (AssembledDefinitionNode.MatchingAlreadySimple(orderComposition, new IngredientAssembledNode(this.m_placementItemSwitcher.m_ingredients[j])))
			{
				for (int k = 0; k < this.m_condimentDispenserCosmeticDecisions.m_particleEffects.Length; k++)
				{
					if (k == j)
					{
						this.m_condimentDispenserCosmeticDecisions.m_particleEffects[k].SetActive(true);
						GameUtils.TriggerAudio(this.m_condimentDispenserCosmeticDecisions.m_audioTag, base.gameObject.layer);
					}
					else
					{
						this.m_condimentDispenserCosmeticDecisions.m_particleEffects[k].SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x00063B78 File Offset: 0x00061F78
	public void OnItemSwitched()
	{
		AssembledDefinitionNode orderComposition = this.m_orderDefinition.GetOrderComposition();
		for (int i = 0; i < this.m_placementItemSwitcher.m_ingredients.Length; i++)
		{
			if (AssembledDefinitionNode.MatchingAlreadySimple(orderComposition, new IngredientAssembledNode(this.m_placementItemSwitcher.m_ingredients[i])))
			{
				this.m_animator.SetTrigger(ClientCondimentDispenserCosmeticDecisions.m_iCondiments[i]);
			}
			else
			{
				this.m_animator.ResetTrigger(ClientCondimentDispenserCosmeticDecisions.m_iCondiments[i]);
			}
		}
	}

	// Token: 0x04000D85 RID: 3461
	private CondimentDispenserCosmeticDecisions m_condimentDispenserCosmeticDecisions;

	// Token: 0x04000D86 RID: 3462
	private static readonly int[] m_iCondiments = new int[]
	{
		Animator.StringToHash("Condiment1"),
		Animator.StringToHash("Condiment2")
	};

	// Token: 0x04000D87 RID: 3463
	private Animator m_animator;

	// Token: 0x04000D88 RID: 3464
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04000D89 RID: 3465
	private PlacementItemSwitcher m_placementItemSwitcher;
}
