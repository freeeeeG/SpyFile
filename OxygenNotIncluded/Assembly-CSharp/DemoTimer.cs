using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AEA RID: 2794
public class DemoTimer : MonoBehaviour
{
	// Token: 0x0600561A RID: 22042 RVA: 0x001F5765 File Offset: 0x001F3965
	public static void DestroyInstance()
	{
		DemoTimer.Instance = null;
	}

	// Token: 0x0600561B RID: 22043 RVA: 0x001F5770 File Offset: 0x001F3970
	private void Start()
	{
		DemoTimer.Instance = this;
		if (GenericGameSettings.instance != null)
		{
			if (GenericGameSettings.instance.demoMode)
			{
				this.duration = (float)GenericGameSettings.instance.demoTime;
				this.labelText.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
				this.clockImage.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.duration = (float)GenericGameSettings.instance.demoTime;
		this.fadeOutScreen = Util.KInstantiateUI(this.Prefab_FadeOutScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
		Image component = this.fadeOutScreen.GetComponent<Image>();
		component.raycastTarget = false;
		this.fadeOutColor = component.color;
		this.fadeOutColor.a = 0f;
		this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x001F5870 File Offset: 0x001F3A70
	private void Update()
	{
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.CountdownActive = !this.CountdownActive;
			this.UpdateLabel();
		}
		if (this.demoOver || !this.CountdownActive)
		{
			return;
		}
		if (this.beginTime == -1f)
		{
			this.beginTime = Time.unscaledTime;
		}
		this.elapsed = Mathf.Clamp(0f, Time.unscaledTime - this.beginTime, this.duration);
		if (this.elapsed + 5f >= this.duration)
		{
			float f = (this.duration - this.elapsed) / 5f;
			this.fadeOutColor.a = Mathf.Min(1f, 1f - Mathf.Sqrt(f));
			this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
		}
		if (this.elapsed >= this.duration)
		{
			this.EndDemo();
		}
		this.UpdateLabel();
	}

	// Token: 0x0600561D RID: 22045 RVA: 0x001F5978 File Offset: 0x001F3B78
	private void UpdateLabel()
	{
		int num = Mathf.RoundToInt(this.duration - this.elapsed);
		int num2 = Mathf.FloorToInt((float)(num / 60));
		int num3 = num % 60;
		this.labelText.text = string.Concat(new string[]
		{
			UI.DEMOOVERSCREEN.TIMEREMAINING,
			" ",
			num2.ToString("00"),
			":",
			num3.ToString("00")
		});
		if (!this.CountdownActive)
		{
			this.labelText.text = UI.DEMOOVERSCREEN.TIMERINACTIVE;
		}
	}

	// Token: 0x0600561E RID: 22046 RVA: 0x001F5A14 File Offset: 0x001F3C14
	public void EndDemo()
	{
		if (this.demoOver)
		{
			return;
		}
		this.demoOver = true;
		Util.KInstantiateUI(this.Prefab_DemoOverScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false).GetComponent<DemoOverScreen>().Show(true);
	}

	// Token: 0x040039D6 RID: 14806
	public static DemoTimer Instance;

	// Token: 0x040039D7 RID: 14807
	public LocText labelText;

	// Token: 0x040039D8 RID: 14808
	public Image clockImage;

	// Token: 0x040039D9 RID: 14809
	public GameObject Prefab_DemoOverScreen;

	// Token: 0x040039DA RID: 14810
	public GameObject Prefab_FadeOutScreen;

	// Token: 0x040039DB RID: 14811
	private float duration;

	// Token: 0x040039DC RID: 14812
	private float elapsed;

	// Token: 0x040039DD RID: 14813
	private bool demoOver;

	// Token: 0x040039DE RID: 14814
	private float beginTime = -1f;

	// Token: 0x040039DF RID: 14815
	public bool CountdownActive;

	// Token: 0x040039E0 RID: 14816
	private GameObject fadeOutScreen;

	// Token: 0x040039E1 RID: 14817
	private Color fadeOutColor;
}
