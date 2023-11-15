using System;

// Token: 0x020006A3 RID: 1699
public class TeleporterWorkableUse : Workable
{
	// Token: 0x06002DEA RID: 11754 RVA: 0x000F2ED6 File Offset: 0x000F10D6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x000F2EDE File Offset: 0x000F10DE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x000F2EF8 File Offset: 0x000F10F8
	protected override void OnStartWork(Worker worker)
	{
		Teleporter component = base.GetComponent<Teleporter>();
		Teleporter teleporter = component.FindTeleportTarget();
		component.SetTeleportTarget(teleporter);
		TeleportalPad.StatesInstance smi = teleporter.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.targetTeleporter.Trigger(smi);
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x000F2F30 File Offset: 0x000F1130
	protected override void OnStopWork(Worker worker)
	{
		TeleportalPad.StatesInstance smi = this.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
	}
}
