using System;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
[AddComponentMenu("KMonoBehaviour/scripts/ElementDropper")]
public class ElementDropper : KMonoBehaviour
{
	// Token: 0x0600264C RID: 9804 RVA: 0x000D016A File Offset: 0x000CE36A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x000D0183 File Offset: 0x000CE383
	private void OnStorageChanged(object data)
	{
		if (this.storage.GetMassAvailable(this.emitTag) >= this.emitMass)
		{
			this.storage.DropSome(this.emitTag, this.emitMass, false, false, this.emitOffset, true, true);
		}
	}

	// Token: 0x040015EA RID: 5610
	[SerializeField]
	public Tag emitTag;

	// Token: 0x040015EB RID: 5611
	[SerializeField]
	public float emitMass;

	// Token: 0x040015EC RID: 5612
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x040015ED RID: 5613
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040015EE RID: 5614
	private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>(delegate(ElementDropper component, object data)
	{
		component.OnStorageChanged(data);
	});
}
