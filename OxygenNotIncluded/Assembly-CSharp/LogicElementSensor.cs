using System;
using KSerialization;

// Token: 0x0200062E RID: 1582
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicElementSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x06002869 RID: 10345 RVA: 0x000DAA4A File Offset: 0x000D8C4A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Filterable>().onFilterChanged += this.OnElementSelected;
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000DAA6C File Offset: 0x000D8C6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		base.Subscribe<LogicElementSensor>(-592767678, LogicElementSensor.OnOperationalChangedDelegate);
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000DAABC File Offset: 0x000D8CBC
	public void Sim200ms(float dt)
	{
		int i = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			this.samples[this.sampleIdx] = (Grid.ElementIdx[i] == this.desiredElementIdx);
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		bool flag = true;
		bool[] array = this.samples;
		for (int j = 0; j < array.Length; j++)
		{
			flag = (array[j] && flag);
		}
		if (base.IsSwitchedOn != flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000DAB3B File Offset: 0x000D8D3B
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000DAB4C File Offset: 0x000D8D4C
	private void UpdateLogicCircuit()
	{
		bool flag = this.switchedOn && base.GetComponent<Operational>().IsOperational;
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x000DAB88 File Offset: 0x000D8D88
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000DAC10 File Offset: 0x000D8E10
	private void OnElementSelected(Tag element_tag)
	{
		if (!element_tag.IsValid)
		{
			return;
		}
		Element element = ElementLoader.GetElement(element_tag);
		bool on = true;
		if (element != null)
		{
			this.desiredElementIdx = ElementLoader.GetElementIndex(element.id);
			on = (element.id == SimHashes.Void || element.id == SimHashes.Vacuum);
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x000DAC7F File Offset: 0x000D8E7F
	private void OnOperationalChanged(object data)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000DAC90 File Offset: 0x000D8E90
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001779 RID: 6009
	private bool wasOn;

	// Token: 0x0400177A RID: 6010
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x0400177B RID: 6011
	private const int WINDOW_SIZE = 8;

	// Token: 0x0400177C RID: 6012
	private bool[] samples = new bool[8];

	// Token: 0x0400177D RID: 6013
	private int sampleIdx;

	// Token: 0x0400177E RID: 6014
	private ushort desiredElementIdx = ushort.MaxValue;

	// Token: 0x0400177F RID: 6015
	private static readonly EventSystem.IntraObjectHandler<LogicElementSensor> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicElementSensor>(delegate(LogicElementSensor component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
