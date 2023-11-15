using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000048 RID: 72
public class FPSDetector : MonoBehaviour
{
	// Token: 0x060002DD RID: 733 RVA: 0x00011E8C File Offset: 0x0001008C
	private void Awake()
	{
		FPSDetector.inst = this;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00011E94 File Offset: 0x00010094
	private void Start()
	{
		this.update_LastTime = Time.realtimeSinceStartup;
		this.update_Frames = 0f;
		this.curFixedTimeSet = 0.02f;
		Time.fixedDeltaTime = 0.02f;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00011EC4 File Offset: 0x000100C4
	private void Update()
	{
		this.update_Frames += 1f;
		if (Time.realtimeSinceStartup > this.update_LastTime + this.update_DetectDelta)
		{
			this.fps = this.update_Frames / (Time.realtimeSinceStartup - this.update_LastTime);
			this.update_Frames = 0f;
			this.update_LastTime = Time.realtimeSinceStartup;
			this.AdjustFixedTimestep(this.fps);
			this.text_FPS.text = string.Format("FPS {0}  Physics {1} ", this.fps.RoundToInt(), (1f / Time.fixedDeltaTime).RoundToInt());
		}
		this.UpdateFixedDeltaTime();
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00011F74 File Offset: 0x00010174
	private void AdjustFixedTimestep(float fps)
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		if (this.test_on)
		{
			this.curFixedTimeSet = this.test_FixedTime;
			this.UpdateFixedDeltaTime();
			return;
		}
		if (!Setting.Inst.Option_DynamicFPSadjust)
		{
			this.curFixedTimeSet = 0.02f;
			this.UpdateFixedDeltaTime();
			return;
		}
		float num = 1f / fixedDeltaTime;
		num = Mathf.Lerp(num, fps, 0.7f);
		this.curFixedTimeSet = 1f / num;
		this.curFixedTimeSet = Mathf.Clamp(this.curFixedTimeSet, 0.02f, 0.1f);
		this.UpdateFixedDeltaTime();
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00012004 File Offset: 0x00010204
	private void UpdateFixedDeltaTime()
	{
		if (TimeManager.inst.ifPause)
		{
			return;
		}
		float num = Time.fixedDeltaTime = this.curFixedTimeSet * Time.timeScale;
		Time.maximumDeltaTime = num * this.adjust_TimeStepMulti;
		Time.maximumParticleDeltaTime = num * 1.5f;
		if (!Setting.Inst.Option_DynamicFPSadjust)
		{
			Time.maximumDeltaTime = 0.2f;
		}
	}

	// Token: 0x0400029E RID: 670
	public static FPSDetector inst;

	// Token: 0x0400029F RID: 671
	[SerializeField]
	private float update_DetectDelta = 0.2f;

	// Token: 0x040002A0 RID: 672
	private float update_Frames;

	// Token: 0x040002A1 RID: 673
	private float update_LastTime;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private Text text_FPS;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private float[] adjust_FPSmins = new float[]
	{
		5f,
		10f,
		20f
	};

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private float[] adjust_FPSmuls = new float[]
	{
		3f,
		2f,
		1.5f
	};

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private float[] adjust_FPSmaxs = new float[]
	{
		30f,
		40f,
		50f,
		60f
	};

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private float[] adjust_FPSdivs = new float[]
	{
		1.5f,
		2f,
		3f,
		5f
	};

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	private float adjust_TimeStepMulti = 5f;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	public float curFixedTimeSet = 0.02f;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	private bool test_on;

	// Token: 0x040002AA RID: 682
	[SerializeField]
	[Range(0.02f, 5f)]
	private float test_FixedTime = 0.5f;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	public float fps = 60f;
}
