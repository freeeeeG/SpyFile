using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AA7 RID: 2727
public class AsteroidClock : MonoBehaviour
{
	// Token: 0x0600533C RID: 21308 RVA: 0x001DD4B4 File Offset: 0x001DB6B4
	private void Awake()
	{
		this.UpdateOverlay();
	}

	// Token: 0x0600533D RID: 21309 RVA: 0x001DD4BC File Offset: 0x001DB6BC
	private void Start()
	{
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x001DD4BE File Offset: 0x001DB6BE
	private void Update()
	{
		if (GameClock.Instance != null)
		{
			this.rotationTransform.rotation = Quaternion.Euler(0f, 0f, 360f * -GameClock.Instance.GetCurrentCycleAsPercentage());
		}
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001DD4F8 File Offset: 0x001DB6F8
	private void UpdateOverlay()
	{
		float fillAmount = 0.125f;
		this.NightOverlay.fillAmount = fillAmount;
	}

	// Token: 0x0400376E RID: 14190
	public Transform rotationTransform;

	// Token: 0x0400376F RID: 14191
	public Image NightOverlay;
}
