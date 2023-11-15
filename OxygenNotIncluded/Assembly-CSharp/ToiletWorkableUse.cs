using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006AA RID: 1706
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableUse")]
public class ToiletWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06002E35 RID: 11829 RVA: 0x000F42E2 File Offset: 0x000F24E2
	private ToiletWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x000F42F2 File Offset: 0x000F24F2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x000F4328 File Offset: 0x000F2528
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x000F43BA File Offset: 0x000F25BA
	protected override void OnStopWork(Worker worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x000F43EB File Offset: 0x000F25EB
	protected override void OnAbortWork(Worker worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnAbortWork(worker);
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x000F441C File Offset: 0x000F261C
	protected override void OnCompleteWork(Worker worker)
	{
		Db.Get().Amounts.Bladder.Lookup(worker).SetValue(0f);
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
			AmountInstance amountInstance = Db.Get().Amounts.RadiationBalance.Lookup(worker);
			RadiationMonitor.Instance smi = worker.GetSMI<RadiationMonitor.Instance>();
			float num = Math.Min(amountInstance.value, 100f * smi.difficultySettingMod);
			if (num >= 1f)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, worker.transform, Vector3.up * 2f, 1.5f, false, false);
			}
			amountInstance.ApplyDelta(-num);
		}
		this.timesUsed++;
		base.Trigger(-350347868, worker);
		base.OnCompleteWork(worker);
	}

	// Token: 0x04001B3D RID: 6973
	[Serialize]
	public int timesUsed;
}
