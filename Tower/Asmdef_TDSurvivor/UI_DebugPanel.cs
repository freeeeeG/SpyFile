using System;
using TMPro;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class UI_DebugPanel : MonoBehaviour
{
	// Token: 0x0600091F RID: 2335 RVA: 0x00022A2C File Offset: 0x00020C2C
	private void Update()
	{
		this.UpdateFPS();
		this.UpdateResolution();
		this.UpdateTimeElapsed();
		this.UpdateVSync();
		this.UpdateTimeScale();
		this.UpdateStageInfo();
		if (Input.GetKeyDown(KeyCode.F2))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00022A6C File Offset: 0x00020C6C
	private void UpdateFPS()
	{
		this.framesCount++;
		this.deltaTime += Time.unscaledDeltaTime;
		if (this.deltaTime >= 1f)
		{
			this.fps = (float)this.framesCount / this.deltaTime;
			this.framesCount = 0;
			this.deltaTime -= 1f;
		}
		this.fpsText.text = "FPS: " + this.fps.ToString("0.00");
		if (this.fps < 30f)
		{
			this.fpsText.color = Color.red;
			return;
		}
		if (this.fps < 60f)
		{
			this.fpsText.color = Color.yellow;
			return;
		}
		this.fpsText.color = Color.green;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00022B44 File Offset: 0x00020D44
	private void UpdateResolution()
	{
		this.resolutionText.text = "解析度: " + Screen.width.ToString() + "x" + Screen.height.ToString();
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00022B88 File Offset: 0x00020D88
	private void UpdateTimeElapsed()
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		this.timeElapsedText.text = "經過時間: " + this.FormatTime(timeSinceLevelLoad);
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00022BB8 File Offset: 0x00020DB8
	private void UpdateVSync()
	{
		bool flag = QualitySettings.vSyncCount != 0;
		this.vSyncText.text = "VSync: " + (flag ? "On" : "Off");
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00022BF4 File Offset: 0x00020DF4
	private string FormatTime(float time)
	{
		int num = Mathf.FloorToInt(time / 60f);
		int num2 = Mathf.FloorToInt(time % 60f);
		int num3 = Mathf.FloorToInt((time - (float)Mathf.FloorToInt(time)) * 1000f);
		return string.Format("{0:00}:{1:00}:{2:000}", num, num2, num3);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00022C4C File Offset: 0x00020E4C
	private void UpdateTimeScale()
	{
		this.timeScaleText.text = string.Format("遊戲速度: {0}", Time.timeScale);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00022C70 File Offset: 0x00020E70
	private void UpdateStageInfo()
	{
		string str = GameDataManager.instance.IntermediateData.isCorrupted ? "[腐化]" : "[普通]]";
		this.stageInfoText.text = "目前關卡: " + Singleton<StageDataReader>.Instance.CurrentUsingData.name + " " + str;
	}

	// Token: 0x0400073E RID: 1854
	[SerializeField]
	private TMP_Text fpsText;

	// Token: 0x0400073F RID: 1855
	[SerializeField]
	private TMP_Text resolutionText;

	// Token: 0x04000740 RID: 1856
	[SerializeField]
	private TMP_Text timeElapsedText;

	// Token: 0x04000741 RID: 1857
	[SerializeField]
	private TMP_Text vSyncText;

	// Token: 0x04000742 RID: 1858
	[SerializeField]
	private TMP_Text timeScaleText;

	// Token: 0x04000743 RID: 1859
	[SerializeField]
	private TMP_Text stageInfoText;

	// Token: 0x04000744 RID: 1860
	private float deltaTime;

	// Token: 0x04000745 RID: 1861
	private int framesCount;

	// Token: 0x04000746 RID: 1862
	private float fps;
}
