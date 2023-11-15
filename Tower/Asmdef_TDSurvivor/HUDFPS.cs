using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E4 RID: 228
[AddComponentMenu("Utilities/HUDFPS")]
public class HUDFPS : MonoBehaviour
{
	// Token: 0x060005A6 RID: 1446 RVA: 0x00016737 File Offset: 0x00014937
	private void Start()
	{
		base.StartCoroutine(this.FPS());
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00016746 File Offset: 0x00014946
	private void Update()
	{
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0001676E File Offset: 0x0001496E
	private IEnumerator FPS()
	{
		for (;;)
		{
			float num = this.accum / (float)this.frames;
			this.sFPS = num.ToString("f" + Mathf.Clamp(this.nbDecimal, 0, 10).ToString());
			this.color = ((num >= 30f) ? Color.green : ((num > 10f) ? Color.red : Color.yellow));
			this.accum = 0f;
			this.frames = 0;
			yield return new WaitForSeconds(this.frequency);
		}
		yield break;
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00016780 File Offset: 0x00014980
	private void OnGUI()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.label);
			this.style.normal.textColor = Color.white;
			this.style.fontSize = (int)(48f * ((float)Screen.resolutions[0].width / 1920f));
			this.style.alignment = TextAnchor.MiddleCenter;
		}
		GUI.color = (this.updateColor ? this.color : Color.white);
		this.startRect = GUI.Window(0, this.startRect, new GUI.WindowFunction(this.DoMyWindow), "");
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00016834 File Offset: 0x00014A34
	private void DoMyWindow(int windowID)
	{
		GUI.Label(new Rect(0f, 0f, this.startRect.width, this.startRect.height), this.sFPS + " FPS", this.style);
		if (this.allowDrag)
		{
			GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		}
	}

	// Token: 0x04000515 RID: 1301
	public Rect startRect = new Rect(10f, 10f, 250f, 160f);

	// Token: 0x04000516 RID: 1302
	public bool updateColor = true;

	// Token: 0x04000517 RID: 1303
	public bool allowDrag = true;

	// Token: 0x04000518 RID: 1304
	public float frequency = 0.5f;

	// Token: 0x04000519 RID: 1305
	public int nbDecimal = 1;

	// Token: 0x0400051A RID: 1306
	private float accum;

	// Token: 0x0400051B RID: 1307
	private int frames;

	// Token: 0x0400051C RID: 1308
	private Color color = Color.white;

	// Token: 0x0400051D RID: 1309
	private string sFPS = "";

	// Token: 0x0400051E RID: 1310
	private GUIStyle style;
}
