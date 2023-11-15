using System;

// Token: 0x020007D7 RID: 2007
public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
	// Token: 0x0600389F RID: 14495 RVA: 0x0013AC78 File Offset: 0x00138E78
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suffocationMonitor = new SuffocationMonitor.Instance(oxygen_breather);
		this.suffocationMonitor.StartSM();
		this.safeCellMonitor = new SafeCellMonitor.Instance(oxygen_breather);
		this.safeCellMonitor.StartSM();
		this.oxygenBreather = oxygen_breather;
		this.nav = this.oxygenBreather.GetComponent<Navigator>();
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x0013ACCB File Offset: 0x00138ECB
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suffocationMonitor.StopSM("Removed gas provider");
		this.safeCellMonitor.StopSM("Removed gas provider");
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x0013ACED File Offset: 0x00138EED
	public bool ShouldEmitCO2()
	{
		return this.nav.CurrentNavType != NavType.Tube;
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x0013AD00 File Offset: 0x00138F00
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x0013AD03 File Offset: 0x00138F03
	public bool IsLowOxygen()
	{
		return this.oxygenBreather.IsLowOxygenAtMouthCell();
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x0013AD10 File Offset: 0x00138F10
	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (this.nav.CurrentNavType != NavType.Tube)
		{
			SimHashes getBreathableElement = oxygen_breather.GetBreathableElement;
			if (getBreathableElement == SimHashes.Vacuum)
			{
				return false;
			}
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback), this, "GasBreatherFromWorldProvider");
			SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed, 3, handle.index);
		}
		return true;
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x0013AD74 File Offset: 0x00138F74
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((GasBreatherFromWorldProvider)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x0013AD84 File Offset: 0x00138F84
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (this.oxygenBreather == null || this.oxygenBreather.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			return;
		}
		if (ElementLoader.elements[(int)mass_cb_info.elemIdx].id == SimHashes.ContaminatedOxygen)
		{
			this.oxygenBreather.Trigger(-935848905, mass_cb_info);
		}
		Game.Instance.accumulators.Accumulate(this.oxygenBreather.O2Accumulator, mass_cb_info.mass);
		float value = -mass_cb_info.mass;
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, this.oxygenBreather.GetProperName(), null);
		this.oxygenBreather.Consume(mass_cb_info);
	}

	// Token: 0x0400258A RID: 9610
	private SuffocationMonitor.Instance suffocationMonitor;

	// Token: 0x0400258B RID: 9611
	private SafeCellMonitor.Instance safeCellMonitor;

	// Token: 0x0400258C RID: 9612
	private OxygenBreather oxygenBreather;

	// Token: 0x0400258D RID: 9613
	private Navigator nav;
}
