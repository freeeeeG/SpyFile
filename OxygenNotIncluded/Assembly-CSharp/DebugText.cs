using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000746 RID: 1862
[AddComponentMenu("KMonoBehaviour/scripts/DebugText")]
public class DebugText : KMonoBehaviour
{
	// Token: 0x0600336B RID: 13163 RVA: 0x00112324 File Offset: 0x00110524
	public static void DestroyInstance()
	{
		DebugText.Instance = null;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x0011232C File Offset: 0x0011052C
	protected override void OnPrefabInit()
	{
		DebugText.Instance = this;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x00112334 File Offset: 0x00110534
	public void Draw(string text, Vector3 pos, Color color)
	{
		DebugText.Entry item = new DebugText.Entry
		{
			text = text,
			pos = pos,
			color = color
		};
		this.entries.Add(item);
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x00112370 File Offset: 0x00110570
	private void LateUpdate()
	{
		foreach (Text text in this.texts)
		{
			UnityEngine.Object.Destroy(text.gameObject);
		}
		this.texts.Clear();
		foreach (DebugText.Entry entry in this.entries)
		{
			GameObject gameObject = new GameObject();
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(entry.pos);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = Assets.DebugFont;
			text2.text = entry.text;
			text2.color = entry.color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;
			this.texts.Add(text2);
		}
		this.entries.Clear();
	}

	// Token: 0x04001EE4 RID: 7908
	public static DebugText Instance;

	// Token: 0x04001EE5 RID: 7909
	private List<DebugText.Entry> entries = new List<DebugText.Entry>();

	// Token: 0x04001EE6 RID: 7910
	private List<Text> texts = new List<Text>();

	// Token: 0x020014E3 RID: 5347
	private struct Entry
	{
		// Token: 0x040066BD RID: 26301
		public string text;

		// Token: 0x040066BE RID: 26302
		public Vector3 pos;

		// Token: 0x040066BF RID: 26303
		public Color color;
	}
}
