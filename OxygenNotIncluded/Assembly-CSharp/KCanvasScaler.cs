using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B2A RID: 2858
[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
	// Token: 0x06005812 RID: 22546 RVA: 0x00204804 File Offset: 0x00202A04
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
		{
			this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
		}
		else
		{
			this.SetUserScale(1f);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x0020486C File Offset: 0x00202A6C
	private void OnResize()
	{
		this.SetUserScale(this.userScale);
	}

	// Token: 0x06005814 RID: 22548 RVA: 0x0020487A File Offset: 0x00202A7A
	public void SetUserScale(float scale)
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
		this.userScale = scale;
		this.canvasScaler.scaleFactor = this.GetCanvasScale();
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x002048AE File Offset: 0x00202AAE
	public float GetUserScale()
	{
		return this.userScale;
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x002048B6 File Offset: 0x00202AB6
	public float GetCanvasScale()
	{
		return this.userScale * this.ScreenRelativeScale();
	}

	// Token: 0x06005817 RID: 22551 RVA: 0x002048C8 File Offset: 0x00202AC8
	private float ScreenRelativeScale()
	{
		float dpi = Screen.dpi;
		Camera x = Camera.main;
		if (x == null)
		{
			x = UnityEngine.Object.FindObjectOfType<Camera>();
		}
		x != null;
		if ((float)Screen.height <= this.scaleSteps[0].maxRes_y || (float)Screen.width / (float)Screen.height < 1.6777778f)
		{
			return this.scaleSteps[0].scale;
		}
		if ((float)Screen.height > this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
		{
			return this.scaleSteps[this.scaleSteps.Length - 1].scale;
		}
		for (int i = 0; i < this.scaleSteps.Length; i++)
		{
			if ((float)Screen.height > this.scaleSteps[i].maxRes_y && (float)Screen.height <= this.scaleSteps[i + 1].maxRes_y)
			{
				float t = ((float)Screen.height - this.scaleSteps[i].maxRes_y) / (this.scaleSteps[i + 1].maxRes_y - this.scaleSteps[i].maxRes_y);
				return Mathf.Lerp(this.scaleSteps[i].scale, this.scaleSteps[i + 1].scale, t);
			}
		}
		return 1f;
	}

	// Token: 0x04003B90 RID: 15248
	[MyCmpReq]
	private CanvasScaler canvasScaler;

	// Token: 0x04003B91 RID: 15249
	public static string UIScalePrefKey = "UIScalePref";

	// Token: 0x04003B92 RID: 15250
	private float userScale = 1f;

	// Token: 0x04003B93 RID: 15251
	[Range(0.75f, 2f)]
	private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[]
	{
		new KCanvasScaler.ScaleStep(720f, 0.86f),
		new KCanvasScaler.ScaleStep(1080f, 1f),
		new KCanvasScaler.ScaleStep(2160f, 1.33f)
	};

	// Token: 0x02001A36 RID: 6710
	[Serializable]
	public struct ScaleStep
	{
		// Token: 0x06009664 RID: 38500 RVA: 0x0033C766 File Offset: 0x0033A966
		public ScaleStep(float maxRes_y, float scale)
		{
			this.maxRes_y = maxRes_y;
			this.scale = scale;
		}

		// Token: 0x040078DD RID: 30941
		public float scale;

		// Token: 0x040078DE RID: 30942
		public float maxRes_y;
	}
}
