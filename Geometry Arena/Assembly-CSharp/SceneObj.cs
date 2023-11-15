using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class SceneObj : MonoBehaviour
{
	// Token: 0x170000FE RID: 254
	// (get) Token: 0x0600090D RID: 2317 RVA: 0x00034F8C File Offset: 0x0003318C
	public float SceneSize
	{
		get
		{
			return this.currentSize;
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00034F94 File Offset: 0x00033194
	private void Awake()
	{
		SceneObj.inst = this;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00034F9C File Offset: 0x0003319C
	private void Start()
	{
		float worldSize = GameParameters.Inst.WorldSize;
		this.transSizer.localScale = new Vector2(worldSize, worldSize);
		this.UpdateMaterial();
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00034FD1 File Offset: 0x000331D1
	private void Update()
	{
		this.SmoothColor();
		this.UpdateSceneSize();
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00034FDF File Offset: 0x000331DF
	private void UpdateMaterial()
	{
		this.bloom.InitMat(this.spr, ResourceLibrary.Inst.GlowIntensity_Wall, false);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00035000 File Offset: 0x00033200
	private void SmoothColor()
	{
		Color color = this.spr.color;
		float current = color.Hue();
		float current2 = color.Saturation();
		float current3 = color.Value();
		Color levelColor = Battle.inst.levelColor;
		float target = levelColor.Hue();
		float target2 = levelColor.Saturation();
		float target3 = levelColor.Value();
		float h = Mathf.SmoothDamp(current, target, ref this.colorSmooth_Hue_Ref, this.colorSmooth_Hue_SmoothTime);
		float s = Mathf.SmoothDamp(current2, target2, ref this.colorSmooth_Sat_Ref, this.colorSmooth_Sat_SmoothTime);
		float v = Mathf.SmoothDamp(current3, target3, ref this.colorSmooth_Val_Ref, this.colorSmooth_Val_SmoothTime);
		Color color2 = Color.HSVToRGB(h, s, v);
		this.spr.color = color2;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x000350A3 File Offset: 0x000332A3
	public void ApplyColorSoon()
	{
		this.spr.color = Battle.inst.levelColor;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x000350BC File Offset: 0x000332BC
	private void UpdateSceneSize()
	{
		if (!TempData.inst.diffiOptFlag[25])
		{
			return;
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			this.targetSize = 1f;
		}
		else
		{
			EnumModeType modeType = TempData.inst.modeType;
			if (modeType > EnumModeType.ENDLESS)
			{
				if (modeType == EnumModeType.WANDER)
				{
					BattleManager battleManager = BattleManager.inst;
					float num = 1f - battleManager.waveTimeLeft / battleManager.waveTimeTotal;
					this.targetSize = (this.minSize - 1f) * num + 1f;
				}
			}
			else
			{
				this.targetSize -= Time.deltaTime * this.shrinkSpeed;
				this.targetSize = Mathf.Max(this.minSize, this.targetSize);
			}
		}
		if (!MyTool.ifSimiliar(this.currentSize, this.targetSize))
		{
			this.currentSize = Mathf.SmoothDamp(this.currentSize, this.targetSize, ref this.sizeSmooth_RefSpd, this.sizeSmooth_Time);
			float num2 = GameParameters.Inst.WorldSize * this.currentSize;
			this.transSizer.localScale = new Vector2(num2, num2);
		}
	}

	// Token: 0x0400077F RID: 1919
	public static SceneObj inst;

	// Token: 0x04000780 RID: 1920
	[SerializeField]
	private SpriteRenderer spr;

	// Token: 0x04000781 RID: 1921
	[Header("ColorSmooth")]
	[SerializeField]
	private float colorSmooth_Hue_Ref;

	// Token: 0x04000782 RID: 1922
	[SerializeField]
	[Range(0f, 1f)]
	private float colorSmooth_Hue_SmoothTime = 0.1f;

	// Token: 0x04000783 RID: 1923
	[SerializeField]
	private float colorSmooth_Sat_Ref;

	// Token: 0x04000784 RID: 1924
	[SerializeField]
	[Range(0f, 1f)]
	private float colorSmooth_Sat_SmoothTime = 0.1f;

	// Token: 0x04000785 RID: 1925
	[SerializeField]
	private float colorSmooth_Val_Ref;

	// Token: 0x04000786 RID: 1926
	[SerializeField]
	[Range(0f, 1f)]
	private float colorSmooth_Val_SmoothTime = 0.1f;

	// Token: 0x04000787 RID: 1927
	[Header("SceneSize")]
	[SerializeField]
	private Transform trans;

	// Token: 0x04000788 RID: 1928
	[SerializeField]
	private Transform transSizer;

	// Token: 0x04000789 RID: 1929
	[SerializeField]
	private Transform directorXYSizer;

	// Token: 0x0400078A RID: 1930
	[SerializeField]
	private float targetSize = 1f;

	// Token: 0x0400078B RID: 1931
	[SerializeField]
	public float currentSize = 1f;

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	private float shrinkSpeed = 0.02f;

	// Token: 0x0400078D RID: 1933
	[SerializeField]
	private float minSize = 0.4f;

	// Token: 0x0400078E RID: 1934
	[SerializeField]
	private float sizeSmooth_Time = 0.1f;

	// Token: 0x0400078F RID: 1935
	[SerializeField]
	private float sizeSmooth_RefSpd;

	// Token: 0x04000790 RID: 1936
	[SerializeField]
	private BloomMaterialControl bloom;
}
