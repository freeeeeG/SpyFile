using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ADC RID: 2780
public class ComicViewer : KScreen
{
	// Token: 0x060055A9 RID: 21929 RVA: 0x001F2C9C File Offset: 0x001F0E9C
	public void ShowComic(ComicData comic, bool isVictoryComic)
	{
		for (int i = 0; i < Mathf.Max(comic.images.Length, comic.stringKeys.Length); i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.panelPrefab, this.contentContainer, true);
			this.activePanels.Add(gameObject);
			gameObject.GetComponentInChildren<Image>().sprite = comic.images[i];
			gameObject.GetComponentInChildren<LocText>().SetText(comic.stringKeys[i]);
		}
		this.closeButton.ClearOnClick();
		if (isVictoryComic)
		{
			this.closeButton.onClick += delegate()
			{
				this.Stop();
				this.Show(false);
			};
			return;
		}
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
	}

	// Token: 0x060055AA RID: 21930 RVA: 0x001F2D4B File Offset: 0x001F0F4B
	public void Stop()
	{
		this.OnStop();
		this.Show(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400396A RID: 14698
	public GameObject panelPrefab;

	// Token: 0x0400396B RID: 14699
	public GameObject contentContainer;

	// Token: 0x0400396C RID: 14700
	public List<GameObject> activePanels = new List<GameObject>();

	// Token: 0x0400396D RID: 14701
	public KButton closeButton;

	// Token: 0x0400396E RID: 14702
	public System.Action OnStop;
}
