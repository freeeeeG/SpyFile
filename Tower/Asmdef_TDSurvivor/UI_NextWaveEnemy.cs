using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000173 RID: 371
public class UI_NextWaveEnemy : AUISituational
{
	// Token: 0x060009CF RID: 2511 RVA: 0x0002503E File Offset: 0x0002323E
	private void Awake()
	{
		this.layoutRectTransform = this.layoutGroup.transform.GetComponent<RectTransform>();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00025056 File Offset: 0x00023256
	private void Start()
	{
		this.text_Title.text = LocalizationManager.Instance.GetString("UI", "NEXT_WAVE_ENEMY", Array.Empty<object>());
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0002507C File Offset: 0x0002327C
	private void OnEnable()
	{
		EventMgr.Register<WaveInfoData>(eGameEvents.UI_UpdateNextWaveMonster, new Action<WaveInfoData>(this.OnUpdateNextWaveMonster));
		EventMgr.Register<bool>(eGameEvents.UI_ToggleNextWaveMonsterUI, new Action<bool>(this.OnToggleNextWaveMonsterUI));
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x000250AE File Offset: 0x000232AE
	private void OnDisable()
	{
		EventMgr.Remove<WaveInfoData>(eGameEvents.UI_UpdateNextWaveMonster, new Action<WaveInfoData>(this.OnUpdateNextWaveMonster));
		EventMgr.Remove<bool>(eGameEvents.UI_ToggleNextWaveMonsterUI, new Action<bool>(this.OnToggleNextWaveMonsterUI));
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x000250E0 File Offset: 0x000232E0
	private void OnToggleNextWaveMonsterUI(bool isOn)
	{
		base.Toggle(isOn);
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000250EC File Offset: 0x000232EC
	private void OnUpdateNextWaveMonster(WaveInfoData waveInfoData)
	{
		List<WaveInfoMonsterData> data = waveInfoData.GetData();
		string text = "";
		for (int i = 0; i < data.Count; i++)
		{
			WaveInfoMonsterData waveInfoMonsterData = data[i];
			text += string.Format("{0} x {1}", LocalizationManager.Instance.GetString("MonsterName", waveInfoMonsterData.type.ToString(), Array.Empty<object>()), waveInfoMonsterData.count);
			if (i != data.Count - 1)
			{
				text += "\n";
			}
		}
		this.text_Content.text = text;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutRectTransform);
	}

	// Token: 0x040007A6 RID: 1958
	[SerializeField]
	private TMP_Text text_Content;

	// Token: 0x040007A7 RID: 1959
	[SerializeField]
	private TMP_Text text_Title;

	// Token: 0x040007A8 RID: 1960
	[SerializeField]
	private VerticalLayoutGroup layoutGroup;

	// Token: 0x040007A9 RID: 1961
	private RectTransform layoutRectTransform;
}
