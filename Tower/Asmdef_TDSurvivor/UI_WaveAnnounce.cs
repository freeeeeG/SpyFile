using System;
using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class UI_WaveAnnounce : AUISituational
{
	// Token: 0x06000AB1 RID: 2737 RVA: 0x000283CB File Offset: 0x000265CB
	private void OnEnable()
	{
		EventMgr.Register<int, bool>(eGameEvents.OnWaveIndexChanged, new Action<int, bool>(this.OnWaveIndexChanged));
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x000283E5 File Offset: 0x000265E5
	private void OnDisable()
	{
		EventMgr.Remove<int, bool>(eGameEvents.OnWaveIndexChanged, new Action<int, bool>(this.OnWaveIndexChanged));
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00028400 File Offset: 0x00026600
	private void OnWaveIndexChanged(int index, bool isFinalWave)
	{
		this.text_EnemyIncoming.ShowText(LocalizationManager.Instance.GetString("UI", "WAVEANNOUNCE_ENEMY_INCOMING", Array.Empty<object>()));
		if (isFinalWave)
		{
			this.text_WaveCount.text = LocalizationManager.Instance.GetString("UI", "WAVEANNOUNCE_FINAL_WAVE", Array.Empty<object>());
		}
		else
		{
			this.text_WaveCount.text = LocalizationManager.Instance.GetString("UI", "WAVEANNOUNCE_WAVE_COUNT", new object[]
			{
				index + 1
			});
		}
		base.StartCoroutine(this.CR_Proc());
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00028497 File Offset: 0x00026697
	private IEnumerator CR_Proc()
	{
		base.Toggle(true);
		SoundManager.PlaySound("UI", "WaveAnnounce_Show", -1f, -1f, -1f);
		yield return new WaitForSeconds(this.waitTime);
		this.text_EnemyIncoming.StartDisappearingText();
		yield return new WaitForSeconds(0.5f);
		base.Toggle(false);
		yield break;
	}

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private float waitTime;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private TMP_Text text_WaveCount;

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private TypewriterByCharacter text_EnemyIncoming;
}
