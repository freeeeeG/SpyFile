using System;
using UnityEngine;

// Token: 0x0200051D RID: 1309
public class ServerPlayerRespawnManager : MonoBehaviour
{
	// Token: 0x06001864 RID: 6244 RVA: 0x0007BF94 File Offset: 0x0007A394
	public void StartRespawning(IRespawnBehaviour _iRespawnBehaviour, ServerRespawnCollider _collider)
	{
		base.StartCoroutine(_iRespawnBehaviour.RespawnCoroutine(_collider));
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x0007BFA4 File Offset: 0x0007A3A4
	private void Awake()
	{
		ServerPlayerRespawnManager.ms_Instance = this;
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0007BFAC File Offset: 0x0007A3AC
	private void OnDestroy()
	{
		ServerPlayerRespawnManager.ms_Instance = null;
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x0007BFB4 File Offset: 0x0007A3B4
	public static void KillOrRespawn(GameObject _gameObject, ServerRespawnCollider _collider)
	{
		IRespawnBehaviour respawnBehaviour = _gameObject.RequestInterfaceRecursive<IRespawnBehaviour>();
		if (respawnBehaviour != null)
		{
			if (ServerPlayerRespawnManager.ms_Instance != null)
			{
				ServerPlayerRespawnManager.ms_Instance.StartRespawning(respawnBehaviour, _collider);
			}
		}
		else
		{
			NetworkUtils.DestroyObject(_gameObject);
		}
	}

	// Token: 0x040013A1 RID: 5025
	private static ServerPlayerRespawnManager ms_Instance;
}
