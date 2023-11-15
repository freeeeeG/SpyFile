using System;
using UnityEngine;

// Token: 0x02000489 RID: 1161
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Cancellable")]
public class Cancellable : KMonoBehaviour
{
	// Token: 0x060019EC RID: 6636 RVA: 0x00089697 File Offset: 0x00087897
	protected override void OnPrefabInit()
	{
		base.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000896AA File Offset: 0x000878AA
	protected virtual void OnCancel(object data)
	{
		this.DeleteObject();
	}

	// Token: 0x04000E6B RID: 3691
	private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>(delegate(Cancellable component, object data)
	{
		component.OnCancel(data);
	});
}
