using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class TestingPunch : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x000026A1 File Offset: 0x000008A1
	private void Start()
	{
		Debug.Log("exported curve:" + this.curveToString(this.exportCurve));
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000026C0 File Offset: 0x000008C0
	private void Update()
	{
		LeanTween.dtManual = Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Q))
		{
			LeanTween.moveLocalX(base.gameObject, 5f, 1f).setOnComplete(delegate()
			{
				Debug.Log("on complete move local X");
			}).setOnCompleteOnStart(true);
			GameObject gameObject = GameObject.Find("DirectionalLight");
			Light lt = gameObject.GetComponent<Light>();
			LeanTween.value(lt.gameObject, lt.intensity, 0f, 1.5f).setEase(LeanTweenType.linear).setLoopPingPong().setRepeat(-1).setOnUpdate(delegate(float val)
			{
				lt.intensity = val;
			});
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			MonoBehaviour.print("scale punch!");
			TestingPunch.tweenStatically(base.gameObject);
			LeanTween.scale(base.gameObject, new Vector3(1.15f, 1.15f, 1.15f), 0.6f);
			LeanTween.rotateAround(base.gameObject, Vector3.forward, -360f, 0.3f).setOnComplete(delegate()
			{
				LeanTween.rotateAround(base.gameObject, Vector3.forward, -360f, 0.4f).setOnComplete(delegate()
				{
					LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.1f);
					LeanTween.value(base.gameObject, delegate(float v)
					{
					}, 0f, 1f, 0.3f).setDelay(1f);
				});
			});
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			Vector3[] to = new Vector3[]
			{
				new Vector3(-1f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(4f, 0f, 0f),
				new Vector3(20f, 0f, 0f)
			};
			this.descr = LeanTween.move(base.gameObject, to, 15f).setOrientToPath(true).setDirection(1f).setOnComplete(delegate()
			{
				Debug.Log("move path finished");
			});
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			this.descr.setDirection(-this.descr.direction);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			LeanTween.rotateAroundLocal(base.gameObject, base.transform.forward, -80f, 5f).setPoint(new Vector3(1.25f, 0f, 0f));
			MonoBehaviour.print("rotate punch!");
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			MonoBehaviour.print("move punch!");
			Time.timeScale = 0.25f;
			float start = Time.realtimeSinceStartup;
			LeanTween.moveX(base.gameObject, 1f, 1f).setOnComplete(new Action<object>(this.destroyOnComp)).setOnCompleteParam(base.gameObject).setOnComplete(delegate()
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float num = realtimeSinceStartup - start;
				Debug.Log(string.Concat(new object[]
				{
					"start:",
					start,
					" end:",
					realtimeSinceStartup,
					" diff:",
					num,
					" x:",
					this.gameObject.transform.position.x
				}));
			}).setEase(LeanTweenType.easeInBack).setOvershoot(this.overShootValue).setPeriod(0.3f);
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			LeanTween.color(base.gameObject, new Color(1f, 0f, 0f, 0.5f), 1f);
			Color to2 = new Color(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f), 0f);
			LeanTween.color(GameObject.Find("LCharacter"), to2, 4f).setLoopPingPong(1).setEase(LeanTweenType.easeOutBounce);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			LeanTween.delayedCall(base.gameObject, 0.3f, new Action<object>(this.delayedMethod)).setRepeat(4).setOnCompleteOnRepeat(true).setOnCompleteParam("hi");
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			LeanTween.value(base.gameObject, new Action<Color>(this.updateColor), new Color(1f, 0f, 0f, 1f), Color.blue, 4f);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			LeanTween.delayedCall(0.05f, new Action<object>(this.enterMiniGameStart)).setOnCompleteParam(new object[]
			{
				string.Concat(5)
			});
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			LeanTween.value(base.gameObject, delegate(Vector2 val)
			{
				base.transform.position = new Vector3(val.x, base.transform.position.y, base.transform.position.z);
			}, new Vector2(0f, 0f), new Vector2(5f, 100f), 1f).setEase(LeanTweenType.easeOutBounce);
			GameObject l = GameObject.Find("LCharacter");
			Debug.Log(string.Concat(new object[]
			{
				"x:",
				l.transform.position.x,
				" y:",
				l.transform.position.y
			}));
			LeanTween.value(l, new Vector2(l.transform.position.x, l.transform.position.y), new Vector2(l.transform.position.x, l.transform.position.y + 5f), 1f).setOnUpdate(delegate(Vector2 val)
			{
				Debug.Log("tweening vec2 val:" + val);
				l.transform.position = new Vector3(val.x, val.y, this.transform.position.z);
			}, null);
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002C5C File Offset: 0x00000E5C
	private static void tweenStatically(GameObject gameObject)
	{
		Debug.Log("Starting to tween...");
		LeanTween.value(gameObject, delegate(float val)
		{
			Debug.Log("tweening val:" + val);
		}, 0f, 1f, 1f);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002CA8 File Offset: 0x00000EA8
	private void enterMiniGameStart(object val)
	{
		int num = int.Parse((string)((object[])val)[0]);
		Debug.Log("level:" + num);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002CDD File Offset: 0x00000EDD
	private void updateColor(Color c)
	{
		GameObject.Find("LCharacter").GetComponent<Renderer>().material.color = c;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002CFC File Offset: 0x00000EFC
	private void delayedMethod(object myVal)
	{
		string text = myVal as string;
		Debug.Log(string.Concat(new object[]
		{
			"delayed call:",
			Time.time,
			" myVal:",
			text
		}));
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002D41 File Offset: 0x00000F41
	private void destroyOnComp(object p)
	{
		Object.Destroy((GameObject)p);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002D50 File Offset: 0x00000F50
	private string curveToString(AnimationCurve curve)
	{
		string text = "";
		for (int i = 0; i < curve.length; i++)
		{
			text = string.Concat(new object[]
			{
				text,
				"new Keyframe(",
				curve[i].time,
				"f, ",
				curve[i].value,
				"f)"
			});
			if (i < curve.length - 1)
			{
				text += ", ";
			}
		}
		return "new AnimationCurve( " + text + " )";
	}

	// Token: 0x0400000B RID: 11
	public AnimationCurve exportCurve;

	// Token: 0x0400000C RID: 12
	public float overShootValue = 1f;

	// Token: 0x0400000D RID: 13
	private LTDescr descr;
}
