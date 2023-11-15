using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class ClientItemCrateCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060011F8 RID: 4600 RVA: 0x000660D0 File Offset: 0x000644D0
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_itemCrateCosmeticDecisions = (ItemCrateCosmeticDecisions)synchronisedObject;
		ClientPickupItemSpawner component = base.gameObject.GetComponent<ClientPickupItemSpawner>();
		GameObject itemPrefab = component.GetItemPrefab();
		WorkableItem workableItem = itemPrefab.RequestComponent<WorkableItem>();
		ISpawnableItem spawnableItem;
		if (workableItem != null)
		{
			GameObject nextPrefab = workableItem.GetNextPrefab();
			spawnableItem = nextPrefab.RequireInterface<ISpawnableItem>();
		}
		else
		{
			spawnableItem = itemPrefab.RequireInterface<ISpawnableItem>();
		}
		SubTexture2D subTexture = spawnableItem.GetSubTexture();
		Transform transform = base.transform.FindChildRecursive(this.m_itemCrateCosmeticDecisions.m_crateLidMeshName);
		Renderer component2 = transform.GetComponent<SkinnedMeshRenderer>();
		if (component2 == null)
		{
			component2 = transform.GetComponent<MeshRenderer>();
		}
		Material material = component2.materials[this.m_itemCrateCosmeticDecisions.m_materialNumber];
		material.mainTexture = subTexture.m_atlasTexture;
		float num = (float)subTexture.m_atlasTexture.width;
		float num2 = (float)subTexture.m_atlasTexture.height;
		float x = subTexture.m_subRect.x / num;
		float y = 1f / this.m_itemCrateCosmeticDecisions.m_uvScale.y - subTexture.m_subRect.y / num2;
		material.mainTextureOffset = new Vector2(x, y);
		float x2 = this.m_itemCrateCosmeticDecisions.m_uvScale.x * subTexture.m_subRect.width / num;
		float num3 = this.m_itemCrateCosmeticDecisions.m_uvScale.y * subTexture.m_subRect.height / num2;
		material.mainTextureScale = new Vector2(x2, -num3);
		this.m_animator = base.gameObject.RequestComponentRecursive<Animator>();
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x0006625A File Offset: 0x0006465A
	public void OnPickupItem()
	{
		this.m_animator.SetTrigger(ClientItemCrateCosmeticDecisions.m_iOpen);
	}

	// Token: 0x04000E01 RID: 3585
	private ItemCrateCosmeticDecisions m_itemCrateCosmeticDecisions;

	// Token: 0x04000E02 RID: 3586
	private static readonly int m_iOpen = Animator.StringToHash("Open");

	// Token: 0x04000E03 RID: 3587
	private Animator m_animator;
}
