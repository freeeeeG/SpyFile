using System;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
[AddComponentMenu("KMonoBehaviour/scripts/BubbleSpawner")]
public class BubbleSpawner : KMonoBehaviour
{
	// Token: 0x06002442 RID: 9282 RVA: 0x000C5EDF File Offset: 0x000C40DF
	protected override void OnSpawn()
	{
		this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
		base.OnSpawn();
		base.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x000C5F20 File Offset: 0x000C4120
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Mass >= this.emitMass)
		{
			gameObject.GetComponent<PrimaryElement>().Mass -= this.emitMass;
			BubbleManager.instance.SpawnBubble(base.transform.GetPosition(), this.initialVelocity, component.ElementID, this.emitMass, component.Temperature);
		}
	}

	// Token: 0x040014CD RID: 5325
	public SimHashes element;

	// Token: 0x040014CE RID: 5326
	public float emitMass;

	// Token: 0x040014CF RID: 5327
	public float emitVariance;

	// Token: 0x040014D0 RID: 5328
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x040014D1 RID: 5329
	public Vector2 initialVelocity;

	// Token: 0x040014D2 RID: 5330
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040014D3 RID: 5331
	private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>(delegate(BubbleSpawner component, object data)
	{
		component.OnStorageChanged(data);
	});
}
