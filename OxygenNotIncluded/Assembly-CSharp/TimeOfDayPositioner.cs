using System;
using UnityEngine;

// Token: 0x02000C7A RID: 3194
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDayPositioner")]
public class TimeOfDayPositioner : KMonoBehaviour
{
	// Token: 0x060065E3 RID: 26083 RVA: 0x0026080C File Offset: 0x0025EA0C
	private void Update()
	{
		float f = GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width;
		(base.transform as RectTransform).anchoredPosition = this.targetRect.anchoredPosition + new Vector2(Mathf.Round(f), 0f);
	}

	// Token: 0x04004626 RID: 17958
	[SerializeField]
	private RectTransform targetRect;
}
