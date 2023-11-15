using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C57 RID: 3159
public class TimeRangeSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06006429 RID: 25641 RVA: 0x002501B0 File Offset: 0x0024E3B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderStart.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON;
		this.labelHeaderDuration.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION;
	}

	// Token: 0x0600642A RID: 25642 RVA: 0x002501E2 File Offset: 0x0024E3E2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimeOfDaySensor>() != null;
	}

	// Token: 0x0600642B RID: 25643 RVA: 0x002501F0 File Offset: 0x0024E3F0
	public override void SetTarget(GameObject target)
	{
		this.imageActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.imageInactiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimeOfDaySensor>();
		this.duration.onValueChanged.RemoveAllListeners();
		this.startTime.onValueChanged.RemoveAllListeners();
		this.startTime.value = this.targetTimedSwitch.startTime;
		this.duration.value = this.targetTimedSwitch.duration;
		this.ChangeSetting();
		this.startTime.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.duration.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x0600642C RID: 25644 RVA: 0x002502D8 File Offset: 0x0024E4D8
	private void ChangeSetting()
	{
		this.targetTimedSwitch.startTime = this.startTime.value;
		this.targetTimedSwitch.duration = this.duration.value;
		this.imageActiveZone.rectTransform.rotation = Quaternion.identity;
		this.imageActiveZone.rectTransform.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value));
		this.imageActiveZone.fillAmount = this.duration.value;
		this.labelValueStart.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None);
		this.labelValueDuration.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None);
		this.endIndicator.rotation = Quaternion.identity;
		this.endIndicator.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value + this.duration.value));
		this.startTime.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None)));
		this.duration.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None)));
	}

	// Token: 0x0600642D RID: 25645 RVA: 0x0025044F File Offset: 0x0024E64F
	public void Render200ms(float dt)
	{
		this.currentTimeMarker.rotation = Quaternion.identity;
		this.currentTimeMarker.Rotate(0f, 0f, this.NormalizedValueToDegrees(GameClock.Instance.GetCurrentCycleAsPercentage()));
	}

	// Token: 0x0600642E RID: 25646 RVA: 0x00250486 File Offset: 0x0024E686
	private float NormalizedValueToDegrees(float value)
	{
		return 360f * value;
	}

	// Token: 0x0600642F RID: 25647 RVA: 0x0025048F File Offset: 0x0024E68F
	private float SecondsToDegrees(float seconds)
	{
		return 360f * (seconds / 600f);
	}

	// Token: 0x06006430 RID: 25648 RVA: 0x0025049E File Offset: 0x0024E69E
	private float DegreesToNormalizedValue(float degrees)
	{
		return degrees / 360f;
	}

	// Token: 0x04004457 RID: 17495
	public Image imageInactiveZone;

	// Token: 0x04004458 RID: 17496
	public Image imageActiveZone;

	// Token: 0x04004459 RID: 17497
	private LogicTimeOfDaySensor targetTimedSwitch;

	// Token: 0x0400445A RID: 17498
	public KSlider startTime;

	// Token: 0x0400445B RID: 17499
	public KSlider duration;

	// Token: 0x0400445C RID: 17500
	public RectTransform endIndicator;

	// Token: 0x0400445D RID: 17501
	public LocText labelHeaderStart;

	// Token: 0x0400445E RID: 17502
	public LocText labelHeaderDuration;

	// Token: 0x0400445F RID: 17503
	public LocText labelValueStart;

	// Token: 0x04004460 RID: 17504
	public LocText labelValueDuration;

	// Token: 0x04004461 RID: 17505
	public RectTransform currentTimeMarker;
}
