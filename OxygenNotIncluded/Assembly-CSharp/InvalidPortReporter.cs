using System;

// Token: 0x02000728 RID: 1832
public class InvalidPortReporter : KMonoBehaviour
{
	// Token: 0x06003267 RID: 12903 RVA: 0x0010B95D File Offset: 0x00109B5D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnTagsChanged(null);
		base.Subscribe<InvalidPortReporter>(-1582839653, InvalidPortReporter.OnTagsChangedDelegate);
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x0010B97D File Offset: 0x00109B7D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x0010B988 File Offset: 0x00109B88
	private void OnTagsChanged(object data)
	{
		bool flag = base.gameObject.HasTag(GameTags.HasInvalidPorts);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(InvalidPortReporter.portsNotOverlapping, !flag);
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidPortOverlap, flag, base.gameObject);
		}
	}

	// Token: 0x04001E3F RID: 7743
	public static readonly Operational.Flag portsNotOverlapping = new Operational.Flag("ports_not_overlapping", Operational.Flag.Type.Functional);

	// Token: 0x04001E40 RID: 7744
	private static readonly EventSystem.IntraObjectHandler<InvalidPortReporter> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<InvalidPortReporter>(delegate(InvalidPortReporter component, object data)
	{
		component.OnTagsChanged(data);
	});
}
