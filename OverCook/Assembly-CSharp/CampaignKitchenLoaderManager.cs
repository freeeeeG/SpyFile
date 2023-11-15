using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004FD RID: 1277
[ExecutionDependency(typeof(PlayerInputLookup))]
public class CampaignKitchenLoaderManager : KitchenLoaderManager
{
	// Token: 0x060017CC RID: 6092 RVA: 0x00079874 File Offset: 0x00077C74
	protected override void Awake()
	{
		base.Awake();
		int count = ClientUserSystem.m_Users.Count;
		if (count == 1)
		{
		}
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x0007989C File Offset: 0x00077C9C
	public override void AssignChefEntities(FastList<User> users)
	{
		if (users.Count == 1)
		{
			uint entityID = 0U;
			uint entity2ID = 0U;
			for (int i = 0; i < PlayerIDProvider.s_AllProviders.Count; i++)
			{
				PlayerIDProvider playerIDProvider = PlayerIDProvider.s_AllProviders._items[i];
				if (playerIDProvider != null)
				{
					if (playerIDProvider.GetID() == PlayerInputLookup.Player.One)
					{
						EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(playerIDProvider.gameObject);
						if (entry != null)
						{
							entityID = entry.m_Header.m_uEntityID;
						}
					}
					else if (playerIDProvider.GetID() == PlayerInputLookup.Player.Two)
					{
						EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(playerIDProvider.gameObject);
						if (entry2 != null)
						{
							entity2ID = entry2.m_Header.m_uEntityID;
						}
					}
				}
			}
			users._items[0].EntityID = entityID;
			users._items[0].Entity2ID = entity2ID;
		}
		else
		{
			int num = 0;
			PlayerIDProvider[] array = PlayerIDProvider.s_AllProviders.ToArray();
			Array.Sort<PlayerIDProvider>(array, (PlayerIDProvider x, PlayerIDProvider y) => x.GetID() - y.GetID());
			int num2 = 0;
			while (num2 < users.Count && num < array.Length)
			{
				PlayerIDProvider playerIDProvider2 = array[num];
				EntitySerialisationEntry entry3 = EntitySerialisationRegistry.GetEntry(playerIDProvider2.gameObject);
				users._items[num2].EntityID = entry3.m_Header.m_uEntityID;
				num2++;
				num++;
			}
		}
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x000799FC File Offset: 0x00077DFC
	private GameObject DuplicateWithPrefab(GameObject _instance, GameObject _replacementPrefab)
	{
		GameObject gameObject = _replacementPrefab.Instantiate(_instance.transform.position, _instance.transform.rotation);
		this.CloneHierarchyData(gameObject.transform, _instance.transform);
		return gameObject;
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x00079A3C File Offset: 0x00077E3C
	private void CloneHierarchyData(Transform _receiver, Transform _sample)
	{
		if (_sample.parent != null)
		{
			_receiver.SetParent(_sample.parent);
		}
		_receiver.localPosition = _sample.localPosition;
		_receiver.localRotation = _sample.localRotation;
		_receiver.localScale = _sample.localScale;
		_receiver.name = _sample.name;
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x00079A98 File Offset: 0x00077E98
	private GameObject ReplaceWithPrefab(GameObject _instance, GameObject _replacementPrefab)
	{
		GameObject result = this.DuplicateWithPrefab(_instance, _replacementPrefab);
		UnityEngine.Object.DestroyImmediate(_instance);
		return result;
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x00079AB8 File Offset: 0x00077EB8
	private void OnDrawGizmosSelected()
	{
		if (this.m_singeplayerTargetBounds.HasValue)
		{
			Bounds value = this.m_singeplayerTargetBounds.Value;
			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			Gizmos.DrawCube(value.center, value.size);
		}
	}

	// Token: 0x04001181 RID: 4481
	[SerializeField]
	private GameObject m_singleplayerCameraPrefab;

	// Token: 0x04001182 RID: 4482
	[SerializeField]
	private OptionalBounds m_singeplayerTargetBounds;
}
