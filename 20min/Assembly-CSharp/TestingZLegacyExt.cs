using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class TestingZLegacyExt : MonoBehaviour
{
	// Token: 0x06000083 RID: 131 RVA: 0x00002F51 File Offset: 0x00001151
	private void Awake()
	{
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000075D8 File Offset: 0x000057D8
	private void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo").transform;
		LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
		this.origin = this.ltLogo.position;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00006B71 File Offset: 0x00004D71
	private void pauseNow()
	{
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00007618 File Offset: 0x00005818
	private void OnGUI()
	{
		string text = this.useEstimatedTime ? "useEstimatedTime" : ("timeScale:" + Time.timeScale);
		GUI.Label(new Rect(0.03f * (float)Screen.width, 0.03f * (float)Screen.height, 0.5f * (float)Screen.width, 0.3f * (float)Screen.height), text);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00006BF4 File Offset: 0x00004DF4
	private void endlessCallback()
	{
		Debug.Log("endless");
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00007684 File Offset: 0x00005884
	private void cycleThroughExamples()
	{
		if (this.exampleIter == 0)
		{
			int num = (int)(this.timingType + 1);
			if (num > 4)
			{
				num = 0;
			}
			this.timingType = (TestingZLegacyExt.TimingType)num;
			this.useEstimatedTime = (this.timingType == TestingZLegacyExt.TimingType.IgnoreTimeScale);
			Time.timeScale = (this.useEstimatedTime ? 0f : 1f);
			if (this.timingType == TestingZLegacyExt.TimingType.HalfTimeScale)
			{
				Time.timeScale = 0.5f;
			}
			if (this.timingType == TestingZLegacyExt.TimingType.VariableTimeScale)
			{
				this.descrTimeScaleChangeId = base.gameObject.LeanValue(0.01f, 10f, 3f).setOnUpdate(delegate(float val)
				{
					Time.timeScale = val;
				}).setEase(LeanTweenType.easeInQuad).setUseEstimatedTime(true).setRepeat(-1).id;
			}
			else
			{
				Debug.Log("cancel variable time");
				LeanTween.cancel(this.descrTimeScaleChangeId);
			}
		}
		base.gameObject.BroadcastMessage(this.exampleFunctions[this.exampleIter]);
		float delayTime = 1.1f;
		base.gameObject.LeanDelayedCall(delayTime, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
		this.exampleIter = ((this.exampleIter + 1 >= this.exampleFunctions.Length) ? 0 : (this.exampleIter + 1));
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000077D0 File Offset: 0x000059D0
	public void updateValue3Example()
	{
		Debug.Log("updateValue3Example Time:" + Time.time);
		base.gameObject.LeanValue(new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0f, 270f, 0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00002F51 File Offset: 0x00001151
	public void updateValue3ExampleUpdate(Vector3 val)
	{
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00007869 File Offset: 0x00005A69
	public void updateValue3ExampleCallback(Vector3 val)
	{
		this.ltLogo.transform.eulerAngles = val;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0000787C File Offset: 0x00005A7C
	public void loopTestClamp()
	{
		Debug.Log("loopTestClamp Time:" + Time.time);
		Transform transform = GameObject.Find("Cube1").transform;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.LeanScaleZ(4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000078F8 File Offset: 0x00005AF8
	public void loopTestPingPong()
	{
		Debug.Log("loopTestPingPong Time:" + Time.time);
		Transform transform = GameObject.Find("Cube2").transform;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.LeanScaleY(4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00007970 File Offset: 0x00005B70
	public void colorExample()
	{
		GameObject.Find("LCharacter").LeanColor(new Color(1f, 0f, 0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600008F RID: 143 RVA: 0x000079C8 File Offset: 0x00005BC8
	public void moveOnACurveExample()
	{
		Debug.Log("moveOnACurveExample Time:" + Time.time);
		Vector3[] to = new Vector3[]
		{
			this.origin,
			this.pt1.position,
			this.pt2.position,
			this.pt3.position,
			this.pt3.position,
			this.pt4.position,
			this.pt5.position,
			this.origin
		};
		this.ltLogo.LeanMove(to, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00007AA4 File Offset: 0x00005CA4
	public void customTweenExample()
	{
		Debug.Log(string.Concat(new object[]
		{
			"customTweenExample starting pos:",
			this.ltLogo.position,
			" origin:",
			this.origin
		}));
		this.ltLogo.LeanMoveX(-10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
		this.ltLogo.LeanMoveX(0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00007B54 File Offset: 0x00005D54
	public void moveExample()
	{
		Debug.Log("moveExample");
		this.ltLogo.LeanMove(new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		this.ltLogo.LeanMove(this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00007BC8 File Offset: 0x00005DC8
	public void rotateExample()
	{
		Debug.Log("rotateExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("yo", 5.0);
		this.ltLogo.LeanRotate(new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam(hashtable).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00002F51 File Offset: 0x00001151
	public void rotateOnUpdate(float val)
	{
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00007C5C File Offset: 0x00005E5C
	public void rotateFinished(object hash)
	{
		Hashtable hashtable = hash as Hashtable;
		Debug.Log("rotateFinished hash:" + hashtable["yo"]);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00007C8C File Offset: 0x00005E8C
	public void scaleExample()
	{
		Debug.Log("scaleExample");
		Vector3 localScale = this.ltLogo.localScale;
		this.ltLogo.LeanScale(new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00007CFC File Offset: 0x00005EFC
	public void updateValueExample()
	{
		Debug.Log("updateValueExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("message", "hi");
		base.gameObject.LeanValue(new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(hashtable).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00007D74 File Offset: 0x00005F74
	public void updateValueExampleCallback(float val, object hash)
	{
		Vector3 eulerAngles = this.ltLogo.eulerAngles;
		eulerAngles.y = val;
		this.ltLogo.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00007DA6 File Offset: 0x00005FA6
	public void delayedCallExample()
	{
		Debug.Log("delayedCallExample");
		LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007DD4 File Offset: 0x00005FD4
	public void delayedCallExampleCallback()
	{
		Debug.Log("Delayed function was called");
		Vector3 localScale = this.ltLogo.localScale;
		this.ltLogo.LeanScale(new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00007E44 File Offset: 0x00006044
	public void alphaExample()
	{
		Debug.Log("alphaExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		gameObject.LeanAlpha(0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		gameObject.LeanAlpha(1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00007EA8 File Offset: 0x000060A8
	public void moveLocalExample()
	{
		Debug.Log("moveLocalExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		Vector3 localPosition = gameObject.transform.localPosition;
		gameObject.LeanMoveLocal(new Vector3(0f, 2f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		gameObject.LeanMoveLocal(localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00007F21 File Offset: 0x00006121
	public void rotateAroundExample()
	{
		Debug.Log("rotateAroundExample");
		GameObject.Find("LCharacter").LeanRotateAround(Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00007F57 File Offset: 0x00006157
	public void loopPause()
	{
		GameObject.Find("Cube1").LeanPause();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00007F68 File Offset: 0x00006168
	public void loopResume()
	{
		GameObject.Find("Cube1").LeanResume();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00007F79 File Offset: 0x00006179
	public void punchTest()
	{
		this.ltLogo.LeanMoveX(7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x04000092 RID: 146
	public AnimationCurve customAnimationCurve;

	// Token: 0x04000093 RID: 147
	public Transform pt1;

	// Token: 0x04000094 RID: 148
	public Transform pt2;

	// Token: 0x04000095 RID: 149
	public Transform pt3;

	// Token: 0x04000096 RID: 150
	public Transform pt4;

	// Token: 0x04000097 RID: 151
	public Transform pt5;

	// Token: 0x04000098 RID: 152
	private int exampleIter;

	// Token: 0x04000099 RID: 153
	private string[] exampleFunctions = new string[]
	{
		"updateValue3Example",
		"loopTestClamp",
		"loopTestPingPong",
		"moveOnACurveExample",
		"customTweenExample",
		"moveExample",
		"rotateExample",
		"scaleExample",
		"updateValueExample",
		"delayedCallExample",
		"alphaExample",
		"moveLocalExample",
		"rotateAroundExample",
		"colorExample"
	};

	// Token: 0x0400009A RID: 154
	public bool useEstimatedTime = true;

	// Token: 0x0400009B RID: 155
	private Transform ltLogo;

	// Token: 0x0400009C RID: 156
	private TestingZLegacyExt.TimingType timingType;

	// Token: 0x0400009D RID: 157
	private int descrTimeScaleChangeId;

	// Token: 0x0400009E RID: 158
	private Vector3 origin;

	// Token: 0x0200027D RID: 637
	// (Invoke) Token: 0x06000D87 RID: 3463
	public delegate void NextFunc();

	// Token: 0x0200027E RID: 638
	public enum TimingType
	{
		// Token: 0x040009EC RID: 2540
		SteadyNormalTime,
		// Token: 0x040009ED RID: 2541
		IgnoreTimeScale,
		// Token: 0x040009EE RID: 2542
		HalfTimeScale,
		// Token: 0x040009EF RID: 2543
		VariableTimeScale,
		// Token: 0x040009F0 RID: 2544
		Length
	}
}
