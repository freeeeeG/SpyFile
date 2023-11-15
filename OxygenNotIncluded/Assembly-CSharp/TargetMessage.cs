using System;
using KSerialization;

// Token: 0x02000B89 RID: 2953
public abstract class TargetMessage : Message
{
	// Token: 0x06005BB2 RID: 23474 RVA: 0x0021999A File Offset: 0x00217B9A
	protected TargetMessage()
	{
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x002199A2 File Offset: 0x00217BA2
	public TargetMessage(KPrefabID prefab_id)
	{
		this.target = new MessageTarget(prefab_id);
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x002199B6 File Offset: 0x00217BB6
	public MessageTarget GetTarget()
	{
		return this.target;
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x002199BE File Offset: 0x00217BBE
	public override void OnCleanUp()
	{
		this.target.OnCleanUp();
	}

	// Token: 0x04003DC3 RID: 15811
	[Serialize]
	private MessageTarget target;
}
