using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class ServerExtinguishCollider : MonoBehaviour
{
	// Token: 0x06001505 RID: 5381 RVA: 0x00072920 File Offset: 0x00070D20
	public void ObjectAdded(GameObject _gameObject)
	{
		ServerFlammable serverFlammable = _gameObject.RequestComponent<ServerFlammable>();
		if (serverFlammable)
		{
			serverFlammable.Extinguish();
			serverFlammable.SetCanCatchFire(false);
		}
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x0007294C File Offset: 0x00070D4C
	public void ObjectRemoved(GameObject _gameObject)
	{
		ServerFlammable serverFlammable = _gameObject.RequestComponent<ServerFlammable>();
		if (serverFlammable)
		{
			serverFlammable.SetCanCatchFire(true);
		}
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x00072972 File Offset: 0x00070D72
	private void OnCollisionEnter(Collision _other)
	{
		this.ObjectAdded(_other.gameObject);
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x00072980 File Offset: 0x00070D80
	private void OnTriggerEnter(Collider _other)
	{
		this.ObjectAdded(_other.gameObject);
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x0007298E File Offset: 0x00070D8E
	private void OnTriggerExit(Collider _other)
	{
		this.ObjectRemoved(_other.gameObject);
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x0007299C File Offset: 0x00070D9C
	private void OnCollisionExit(Collision _other)
	{
		this.ObjectRemoved(_other.gameObject);
	}
}
