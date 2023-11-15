using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020002A2 RID: 674
public class WaveInfoHolder : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600107E RID: 4222 RVA: 0x0002D620 File Offset: 0x0002B820
	public void SetWaveInfo(List<EnemySequence> sequences)
	{
		this.currentAtts.Clear();
		Image[] array = this.enemyIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < sequences.Count; j++)
		{
			this.enemyIcons[j].gameObject.SetActive(true);
			EnemyAttribute enemyAttribute = Singleton<StaticData>.Instance.EnemyFactory.Get(sequences[j].EnemyType);
			this.currentAtts.Add(enemyAttribute);
			this.enemyIcons[j].sprite = enemyAttribute.Icon;
		}
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x0002D6BA File Offset: 0x0002B8BA
	public void OnPointerEnter(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.ShowEnemyTips(this.currentAtts, StaticData.MidUpPos);
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0002D6D1 File Offset: 0x0002B8D1
	public void OnPointerExit(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private Image[] enemyIcons;

	// Token: 0x040008D2 RID: 2258
	private List<EnemyAttribute> currentAtts = new List<EnemyAttribute>();
}
