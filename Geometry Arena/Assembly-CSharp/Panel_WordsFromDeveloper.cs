using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class Panel_WordsFromDeveloper : MonoBehaviour
{
	// Token: 0x06000682 RID: 1666 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Awake()
	{
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x000252BC File Offset: 0x000234BC
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			this.Close();
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x000252CC File Offset: 0x000234CC
	public void Open()
	{
		if (Panel_WordsFromDeveloper.ifShown)
		{
			this.Close();
			MainCanvas.inst.panel_MainMenu.gameObject.SetActive(true);
			base.gameObject.SetActive(false);
			return;
		}
		MainCanvas.inst.panel_MainMenu.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
		Panel_WordsFromDeveloper.ifShown = true;
		this.UpdateLanguage();
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00025338 File Offset: 0x00023538
	public void UpdateLanguage()
	{
		if (Setting.Inst == null)
		{
			return;
		}
		for (int i = 0; i < this.objLanguages.Length; i++)
		{
			this.objLanguages[i].SetActive(i == (int)Setting.Inst.language);
		}
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002537A File Offset: 0x0002357A
	public void Close()
	{
		base.gameObject.SetActive(false);
		MainCanvas.inst.panel_MainMenu.gameObject.SetActive(true);
	}

	// Token: 0x04000569 RID: 1385
	public static bool ifShown;

	// Token: 0x0400056A RID: 1386
	public GameObject[] objLanguages;
}
