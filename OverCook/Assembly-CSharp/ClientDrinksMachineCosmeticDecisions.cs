using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class ClientDrinksMachineCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060011AD RID: 4525 RVA: 0x00065048 File Offset: 0x00063448
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_drinksMachineCosmeticDecisions = (DrinksMachineCosmeticDecisions)synchronisedObject;
		this.m_animator = base.gameObject.RequestComponentRecursive<Animator>();
		this.m_pickupItemSwitcher = base.gameObject.RequireComponent<PickupItemSwitcher>();
		this.m_spawner = base.gameObject.RequireComponent<ClientPickupItemSpawner>();
		this.OnItemSwitched();
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x0006509C File Offset: 0x0006349C
	public void OnItemSwitched()
	{
		GameObject itemPrefab = this.m_spawner.GetItemPrefab();
		for (int i = 0; i < this.m_pickupItemSwitcher.m_itemPrefabs.Length; i++)
		{
			if (this.m_pickupItemSwitcher.m_itemPrefabs[i] == itemPrefab)
			{
				this.m_animator.SetTrigger(ClientDrinksMachineCosmeticDecisions.m_iDrinks[i]);
			}
			else
			{
				this.m_animator.ResetTrigger(ClientDrinksMachineCosmeticDecisions.m_iDrinks[i]);
			}
		}
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x00065114 File Offset: 0x00063514
	public void OnPickupItem()
	{
		for (int i = 0; i < this.m_drinksMachineCosmeticDecisions.m_particleEffects.Length; i++)
		{
			this.m_drinksMachineCosmeticDecisions.m_particleEffects[i].SetActive(false);
		}
		GameObject itemPrefab = this.m_spawner.GetItemPrefab();
		for (int j = 0; j < this.m_pickupItemSwitcher.m_itemPrefabs.Length; j++)
		{
			if (this.m_pickupItemSwitcher.m_itemPrefabs[j] == itemPrefab)
			{
				for (int k = 0; k < this.m_drinksMachineCosmeticDecisions.m_particleEffects.Length; k++)
				{
					if (k == j)
					{
						this.m_drinksMachineCosmeticDecisions.m_particleEffects[k].SetActive(true);
						GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_08_Drinks_Machine_Dispense, base.gameObject.layer);
					}
					else
					{
						this.m_drinksMachineCosmeticDecisions.m_particleEffects[k].SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x04000DC1 RID: 3521
	private DrinksMachineCosmeticDecisions m_drinksMachineCosmeticDecisions;

	// Token: 0x04000DC2 RID: 3522
	private static readonly int[] m_iDrinks = new int[]
	{
		Animator.StringToHash("Drink3"),
		Animator.StringToHash("Drink2"),
		Animator.StringToHash("Drink1")
	};

	// Token: 0x04000DC3 RID: 3523
	private Animator m_animator;

	// Token: 0x04000DC4 RID: 3524
	private Transform m_meshTransform;

	// Token: 0x04000DC5 RID: 3525
	private PickupItemSwitcher m_pickupItemSwitcher;

	// Token: 0x04000DC6 RID: 3526
	private ClientPickupItemSpawner m_spawner;
}
