using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000A5B RID: 2651
public class CreditsScreen : KModalScreen
{
	// Token: 0x0600503A RID: 20538 RVA: 0x001C6FF0 File Offset: 0x001C51F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (TextAsset csv in this.creditsFiles)
		{
			this.AddCredits(csv);
		}
		this.CloseButton.onClick += this.Close;
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x001C703A File Offset: 0x001C523A
	public void Close()
	{
		this.Deactivate();
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x001C7044 File Offset: 0x001C5244
	private void AddCredits(TextAsset csv)
	{
		string[,] array = CSVReader.SplitCsvGrid(csv.text, csv.name);
		List<string> list = new List<string>();
		for (int i = 1; i < array.GetLength(1); i++)
		{
			string text = string.Format("{0} {1}", array[0, i], array[1, i]);
			if (!(text == " "))
			{
				list.Add(text);
			}
		}
		list.Shuffle<string>();
		string text2 = array[0, 0];
		GameObject gameObject = Util.KInstantiateUI(this.teamHeaderPrefab, this.entryContainer.gameObject, true);
		gameObject.GetComponent<LocText>().text = text2;
		this.teamContainers.Add(text2, gameObject);
		foreach (string text3 in list)
		{
			Util.KInstantiateUI(this.entryPrefab, this.teamContainers[text2], true).GetComponent<LocText>().text = text3;
		}
	}

	// Token: 0x04003486 RID: 13446
	public GameObject entryPrefab;

	// Token: 0x04003487 RID: 13447
	public GameObject teamHeaderPrefab;

	// Token: 0x04003488 RID: 13448
	private Dictionary<string, GameObject> teamContainers = new Dictionary<string, GameObject>();

	// Token: 0x04003489 RID: 13449
	public Transform entryContainer;

	// Token: 0x0400348A RID: 13450
	public KButton CloseButton;

	// Token: 0x0400348B RID: 13451
	public TextAsset[] creditsFiles;
}
