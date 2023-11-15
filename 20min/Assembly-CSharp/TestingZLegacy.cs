using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class TestingZLegacy : MonoBehaviour
{
	// Token: 0x06000065 RID: 101 RVA: 0x00002F51 File Offset: 0x00001151
	private void Awake()
	{
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00006B32 File Offset: 0x00004D32
	private void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo");
		LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
		this.origin = this.ltLogo.transform.position;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00006B71 File Offset: 0x00004D71
	private void pauseNow()
	{
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00006B88 File Offset: 0x00004D88
	private void OnGUI()
	{
		string text = this.useEstimatedTime ? "useEstimatedTime" : ("timeScale:" + Time.timeScale);
		GUI.Label(new Rect(0.03f * (float)Screen.width, 0.03f * (float)Screen.height, 0.5f * (float)Screen.width, 0.3f * (float)Screen.height), text);
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00006BF4 File Offset: 0x00004DF4
	private void endlessCallback()
	{
		Debug.Log("endless");
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00006C00 File Offset: 0x00004E00
	private void cycleThroughExamples()
	{
		if (this.exampleIter == 0)
		{
			int num = (int)(this.timingType + 1);
			if (num > 4)
			{
				num = 0;
			}
			this.timingType = (TestingZLegacy.TimingType)num;
			this.useEstimatedTime = (this.timingType == TestingZLegacy.TimingType.IgnoreTimeScale);
			Time.timeScale = (this.useEstimatedTime ? 0f : 1f);
			if (this.timingType == TestingZLegacy.TimingType.HalfTimeScale)
			{
				Time.timeScale = 0.5f;
			}
			if (this.timingType == TestingZLegacy.TimingType.VariableTimeScale)
			{
				this.descrTimeScaleChangeId = LeanTween.value(base.gameObject, 0.01f, 10f, 3f).setOnUpdate(delegate(float val)
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
		LeanTween.delayedCall(base.gameObject, delayTime, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
		this.exampleIter = ((this.exampleIter + 1 >= this.exampleFunctions.Length) ? 0 : (this.exampleIter + 1));
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00006D4C File Offset: 0x00004F4C
	public void updateValue3Example()
	{
		Debug.Log("updateValue3Example Time:" + Time.time);
		LeanTween.value(base.gameObject, new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0f, 270f, 0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00002F51 File Offset: 0x00001151
	public void updateValue3ExampleUpdate(Vector3 val)
	{
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00006DE5 File Offset: 0x00004FE5
	public void updateValue3ExampleCallback(Vector3 val)
	{
		this.ltLogo.transform.eulerAngles = val;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00006DF8 File Offset: 0x00004FF8
	public void loopTestClamp()
	{
		Debug.Log("loopTestClamp Time:" + Time.time);
		GameObject gameObject = GameObject.Find("Cube1");
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		LeanTween.scaleZ(gameObject, 4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00006E74 File Offset: 0x00005074
	public void loopTestPingPong()
	{
		Debug.Log("loopTestPingPong Time:" + Time.time);
		GameObject gameObject = GameObject.Find("Cube2");
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		LeanTween.scaleY(gameObject, 4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00006EEC File Offset: 0x000050EC
	public void colorExample()
	{
		LeanTween.color(GameObject.Find("LCharacter"), new Color(1f, 0f, 0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00006F44 File Offset: 0x00005144
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
		LeanTween.move(this.ltLogo, to, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00007020 File Offset: 0x00005220
	public void customTweenExample()
	{
		Debug.Log(string.Concat(new object[]
		{
			"customTweenExample starting pos:",
			this.ltLogo.transform.position,
			" origin:",
			this.origin
		}));
		LeanTween.moveX(this.ltLogo, -10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.moveX(this.ltLogo, 0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000070D8 File Offset: 0x000052D8
	public void moveExample()
	{
		Debug.Log("moveExample");
		LeanTween.move(this.ltLogo, new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.move(this.ltLogo, this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0000714C File Offset: 0x0000534C
	public void rotateExample()
	{
		Debug.Log("rotateExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("yo", 5.0);
		LeanTween.rotate(this.ltLogo, new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam(hashtable).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00002F51 File Offset: 0x00001151
	public void rotateOnUpdate(float val)
	{
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000071E0 File Offset: 0x000053E0
	public void rotateFinished(object hash)
	{
		Hashtable hashtable = hash as Hashtable;
		Debug.Log("rotateFinished hash:" + hashtable["yo"]);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00007210 File Offset: 0x00005410
	public void scaleExample()
	{
		Debug.Log("scaleExample");
		Vector3 localScale = this.ltLogo.transform.localScale;
		LeanTween.scale(this.ltLogo, new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00007284 File Offset: 0x00005484
	public void updateValueExample()
	{
		Debug.Log("updateValueExample");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("message", "hi");
		LeanTween.value(base.gameObject, new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.transform.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(hashtable).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00007300 File Offset: 0x00005500
	public void updateValueExampleCallback(float val, object hash)
	{
		Vector3 eulerAngles = this.ltLogo.transform.eulerAngles;
		eulerAngles.y = val;
		this.ltLogo.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00007337 File Offset: 0x00005537
	public void delayedCallExample()
	{
		Debug.Log("delayedCallExample");
		LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00007368 File Offset: 0x00005568
	public void delayedCallExampleCallback()
	{
		Debug.Log("Delayed function was called");
		Vector3 localScale = this.ltLogo.transform.localScale;
		LeanTween.scale(this.ltLogo, new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000073DC File Offset: 0x000055DC
	public void alphaExample()
	{
		Debug.Log("alphaExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		LeanTween.alpha(gameObject, 0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.alpha(gameObject, 1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00007440 File Offset: 0x00005640
	public void moveLocalExample()
	{
		Debug.Log("moveLocalExample");
		GameObject gameObject = GameObject.Find("LCharacter");
		Vector3 localPosition = gameObject.transform.localPosition;
		LeanTween.moveLocal(gameObject, new Vector3(0f, 2f, 0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
		LeanTween.moveLocal(gameObject, localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000074B9 File Offset: 0x000056B9
	public void rotateAroundExample()
	{
		Debug.Log("rotateAroundExample");
		LeanTween.rotateAround(GameObject.Find("LCharacter"), Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000074EF File Offset: 0x000056EF
	public void loopPause()
	{
		LeanTween.pause(GameObject.Find("Cube1"));
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00007500 File Offset: 0x00005700
	public void loopResume()
	{
		LeanTween.resume(GameObject.Find("Cube1"));
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00007511 File Offset: 0x00005711
	public void punchTest()
	{
		LeanTween.moveX(this.ltLogo, 7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
	}

	// Token: 0x04000085 RID: 133
	public AnimationCurve customAnimationCurve;

	// Token: 0x04000086 RID: 134
	public Transform pt1;

	// Token: 0x04000087 RID: 135
	public Transform pt2;

	// Token: 0x04000088 RID: 136
	public Transform pt3;

	// Token: 0x04000089 RID: 137
	public Transform pt4;

	// Token: 0x0400008A RID: 138
	public Transform pt5;

	// Token: 0x0400008B RID: 139
	private int exampleIter;

	// Token: 0x0400008C RID: 140
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

	// Token: 0x0400008D RID: 141
	public bool useEstimatedTime = true;

	// Token: 0x0400008E RID: 142
	private GameObject ltLogo;

	// Token: 0x0400008F RID: 143
	private TestingZLegacy.TimingType timingType;

	// Token: 0x04000090 RID: 144
	private int descrTimeScaleChangeId;

	// Token: 0x04000091 RID: 145
	private Vector3 origin;

	// Token: 0x0200027A RID: 634
	// (Invoke) Token: 0x06000D80 RID: 3456
	public delegate void NextFunc();

	// Token: 0x0200027B RID: 635
	public enum TimingType
	{
		// Token: 0x040009E4 RID: 2532
		SteadyNormalTime,
		// Token: 0x040009E5 RID: 2533
		IgnoreTimeScale,
		// Token: 0x040009E6 RID: 2534
		HalfTimeScale,
		// Token: 0x040009E7 RID: 2535
		VariableTimeScale,
		// Token: 0x040009E8 RID: 2536
		Length
	}
}
