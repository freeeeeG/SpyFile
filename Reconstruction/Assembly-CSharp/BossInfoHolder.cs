using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020002A1 RID: 673
public class BossInfoHolder : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600107A RID: 4218 RVA: 0x0002D5B0 File Offset: 0x0002B7B0
	public void SetBossInfo(EnemyType bossType, int nextBossWave)
	{
		EnemyAttribute enemyAttribute = Singleton<StaticData>.Instance.EnemyFactory.Get(bossType);
		this.BossIcon.sprite = enemyAttribute.Icon;
		this.nextBossType = bossType;
		this.nextBossWave = nextBossWave;
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0002D5ED File Offset: 0x0002B7ED
	public void OnPointerEnter(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.ShowBossTips(this.nextBossType, this.nextBossWave, StaticData.MidTipsPos);
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0002D60A File Offset: 0x0002B80A
	public void OnPointerExit(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.HideBossTips();
	}

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private Image BossIcon;

	// Token: 0x040008CF RID: 2255
	private EnemyType nextBossType;

	// Token: 0x040008D0 RID: 2256
	private int nextBossWave;
}
