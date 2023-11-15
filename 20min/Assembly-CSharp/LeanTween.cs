using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000025 RID: 37
public class LeanTween : MonoBehaviour
{
	// Token: 0x0600024B RID: 587 RVA: 0x0000EFC1 File Offset: 0x0000D1C1
	public static void init()
	{
		LeanTween.init(LeanTween.maxTweens);
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600024C RID: 588 RVA: 0x0000EFCD File Offset: 0x0000D1CD
	public static int maxSearch
	{
		get
		{
			return LeanTween.tweenMaxSearch;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600024D RID: 589 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
	public static int maxSimulataneousTweens
	{
		get
		{
			return LeanTween.maxTweens;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600024E RID: 590 RVA: 0x0000EFDC File Offset: 0x0000D1DC
	public static int tweensRunning
	{
		get
		{
			int num = 0;
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000F00E File Offset: 0x0000D20E
	public static void init(int maxSimultaneousTweens)
	{
		LeanTween.init(maxSimultaneousTweens, LeanTween.maxSequences);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000F01C File Offset: 0x0000D21C
	public static void init(int maxSimultaneousTweens, int maxSimultaneousSequences)
	{
		if (LeanTween.tweens == null)
		{
			LeanTween.maxTweens = maxSimultaneousTweens;
			LeanTween.tweens = new LTDescr[LeanTween.maxTweens];
			LeanTween.tweensFinished = new int[LeanTween.maxTweens];
			LeanTween.tweensFinishedIds = new int[LeanTween.maxTweens];
			LeanTween._tweenEmpty = new GameObject();
			LeanTween._tweenEmpty.name = "~LeanTween";
			LeanTween._tweenEmpty.AddComponent(typeof(LeanTween));
			LeanTween._tweenEmpty.isStatic = true;
			LeanTween._tweenEmpty.hideFlags = HideFlags.HideAndDontSave;
			Object.DontDestroyOnLoad(LeanTween._tweenEmpty);
			for (int i = 0; i < LeanTween.maxTweens; i++)
			{
				LeanTween.tweens[i] = new LTDescr();
			}
			SceneManager.sceneLoaded += LeanTween.onLevelWasLoaded54;
			LeanTween.sequences = new LTSeq[maxSimultaneousSequences];
			for (int j = 0; j < maxSimultaneousSequences; j++)
			{
				LeanTween.sequences[j] = new LTSeq();
			}
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000F108 File Offset: 0x0000D308
	public static void reset()
	{
		if (LeanTween.tweens != null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i] != null)
				{
					LeanTween.tweens[i].toggle = false;
				}
			}
		}
		LeanTween.tweens = null;
		Object.Destroy(LeanTween._tweenEmpty);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000F152 File Offset: 0x0000D352
	public void Update()
	{
		LeanTween.update();
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000F159 File Offset: 0x0000D359
	private static void onLevelWasLoaded54(Scene scene, LoadSceneMode mode)
	{
		LeanTween.internalOnLevelWasLoaded(scene.buildIndex);
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000F167 File Offset: 0x0000D367
	private static void internalOnLevelWasLoaded(int lvl)
	{
		LTGUI.reset();
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000F170 File Offset: 0x0000D370
	public static void update()
	{
		if (LeanTween.frameRendered != Time.frameCount)
		{
			LeanTween.init();
			LeanTween.dtEstimated = ((LeanTween.dtEstimated < 0f) ? 0f : (LeanTween.dtEstimated = Time.unscaledDeltaTime));
			LeanTween.dtActual = Time.deltaTime;
			LeanTween.maxTweenReached = 0;
			LeanTween.finishedCnt = 0;
			int num = 0;
			while (num <= LeanTween.tweenMaxSearch && num < LeanTween.maxTweens)
			{
				LeanTween.tween = LeanTween.tweens[num];
				if (LeanTween.tween.toggle)
				{
					LeanTween.maxTweenReached = num;
					if (LeanTween.tween.updateInternal())
					{
						LeanTween.tweensFinished[LeanTween.finishedCnt] = num;
						LeanTween.tweensFinishedIds[LeanTween.finishedCnt] = LeanTween.tweens[num].id;
						LeanTween.finishedCnt++;
					}
				}
				num++;
			}
			LeanTween.tweenMaxSearch = LeanTween.maxTweenReached;
			LeanTween.frameRendered = Time.frameCount;
			for (int i = 0; i < LeanTween.finishedCnt; i++)
			{
				LeanTween.j = LeanTween.tweensFinished[i];
				LeanTween.tween = LeanTween.tweens[LeanTween.j];
				if (LeanTween.tween.id == LeanTween.tweensFinishedIds[i])
				{
					LeanTween.removeTween(LeanTween.j);
					if (LeanTween.tween.hasExtraOnCompletes && LeanTween.tween.trans != null)
					{
						LeanTween.tween.callOnCompletes();
					}
				}
			}
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000F2C0 File Offset: 0x0000D4C0
	public static void removeTween(int i, int uniqueId)
	{
		if (LeanTween.tweens[i].uniqueId == uniqueId)
		{
			LeanTween.removeTween(i);
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000F2D8 File Offset: 0x0000D4D8
	public static void removeTween(int i)
	{
		if (LeanTween.tweens[i].toggle)
		{
			LeanTween.tweens[i].toggle = false;
			LeanTween.tweens[i].counter = uint.MaxValue;
			if (LeanTween.tweens[i].destroyOnComplete)
			{
				if (LeanTween.tweens[i]._optional.ltRect != null)
				{
					LTGUI.destroy(LeanTween.tweens[i]._optional.ltRect.id);
				}
				else if (LeanTween.tweens[i].trans != null && LeanTween.tweens[i].trans.gameObject != LeanTween._tweenEmpty)
				{
					Object.Destroy(LeanTween.tweens[i].trans.gameObject);
				}
			}
			LeanTween.startSearch = i;
			if (i + 1 >= LeanTween.tweenMaxSearch)
			{
				LeanTween.startSearch = 0;
			}
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000F3AC File Offset: 0x0000D5AC
	public static Vector3[] add(Vector3[] a, Vector3 b)
	{
		Vector3[] array = new Vector3[a.Length];
		LeanTween.i = 0;
		while (LeanTween.i < a.Length)
		{
			array[LeanTween.i] = a[LeanTween.i] + b;
			LeanTween.i++;
		}
		return array;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000F400 File Offset: 0x0000D600
	public static float closestRot(float from, float to)
	{
		float num = 0f - (360f - to);
		float num2 = 360f + to;
		float num3 = Mathf.Abs(to - from);
		float num4 = Mathf.Abs(num - from);
		float num5 = Mathf.Abs(num2 - from);
		if (num3 < num4 && num3 < num5)
		{
			return to;
		}
		if (num4 < num5)
		{
			return num;
		}
		return num2;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000F452 File Offset: 0x0000D652
	public static void cancelAll()
	{
		LeanTween.cancelAll(false);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000F45C File Offset: 0x0000D65C
	public static void cancelAll(bool callComplete)
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans != null)
			{
				if (callComplete && LeanTween.tweens[i].optional.onComplete != null)
				{
					LeanTween.tweens[i].optional.onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
	public static void cancel(GameObject gameObject)
	{
		LeanTween.cancel(gameObject, false);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
	public static void cancel(GameObject gameObject, bool callOnComplete)
	{
		LeanTween.init();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LTDescr ltdescr = LeanTween.tweens[i];
			if (ltdescr != null && ltdescr.toggle && ltdescr.trans == transform)
			{
				if (callOnComplete && ltdescr.optional.onComplete != null)
				{
					ltdescr.optional.onComplete();
				}
				LeanTween.removeTween(i);
			}
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000F540 File Offset: 0x0000D740
	public static void cancel(RectTransform rect)
	{
		LeanTween.cancel(rect.gameObject, false);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000F550 File Offset: 0x0000D750
	public static void cancel(GameObject gameObject, int uniqueId, bool callOnComplete = false)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num].trans == null || (LeanTween.tweens[num].trans.gameObject == gameObject && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2)))
			{
				if (callOnComplete && LeanTween.tweens[num].optional.onComplete != null)
				{
					LeanTween.tweens[num].optional.onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
	public static void cancel(LTRect ltRect, int uniqueId)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num]._optional.ltRect == ltRect && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000F636 File Offset: 0x0000D836
	public static void cancel(int uniqueId)
	{
		LeanTween.cancel(uniqueId, false);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000F640 File Offset: 0x0000D840
	public static void cancel(int uniqueId, bool callOnComplete)
	{
		if (uniqueId >= 0)
		{
			LeanTween.init();
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (num > LeanTween.tweens.Length - 1)
			{
				int num3 = num - LeanTween.tweens.Length;
				LTSeq ltseq = LeanTween.sequences[num3];
				for (int i = 0; i < LeanTween.maxSequences; i++)
				{
					if (ltseq.current.tween != null)
					{
						LeanTween.removeTween(ltseq.current.tween.uniqueId & 65535);
					}
					if (ltseq.current.previous == null)
					{
						return;
					}
					ltseq.current = ltseq.current.previous;
				}
				return;
			}
			if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				if (callOnComplete && LeanTween.tweens[num].optional.onComplete != null)
				{
					LeanTween.tweens[num].optional.onComplete();
				}
				LeanTween.removeTween(num);
			}
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000F724 File Offset: 0x0000D924
	public static LTDescr descr(int uniqueId)
	{
		LeanTween.init();
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if (LeanTween.tweens[num] != null && LeanTween.tweens[num].uniqueId == uniqueId && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			return LeanTween.tweens[num];
		}
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].uniqueId == uniqueId && (ulong)LeanTween.tweens[i].counter == (ulong)((long)num2))
			{
				return LeanTween.tweens[i];
			}
		}
		return null;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000F7AD File Offset: 0x0000D9AD
	public static LTDescr description(int uniqueId)
	{
		return LeanTween.descr(uniqueId);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
	public static LTDescr[] descriptions(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			return null;
		}
		List<LTDescr> list = new List<LTDescr>();
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
			{
				list.Add(LeanTween.tweens[i]);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000F822 File Offset: 0x0000DA22
	[Obsolete("Use 'pause( id )' instead")]
	public static void pause(GameObject gameObject, int uniqueId)
	{
		LeanTween.pause(uniqueId);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000F82C File Offset: 0x0000DA2C
	public static void pause(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].pause();
		}
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000F864 File Offset: 0x0000DA64
	public static void pause(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].pause();
			}
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000F8AC File Offset: 0x0000DAAC
	public static void pauseAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].pause();
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000F8DC File Offset: 0x0000DADC
	public static void resumeAll()
	{
		LeanTween.init();
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			LeanTween.tweens[i].resume();
		}
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000F90B File Offset: 0x0000DB0B
	[Obsolete("Use 'resume( id )' instead")]
	public static void resume(GameObject gameObject, int uniqueId)
	{
		LeanTween.resume(uniqueId);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000F914 File Offset: 0x0000DB14
	public static void resume(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
		{
			LeanTween.tweens[num].resume();
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000F94C File Offset: 0x0000DB4C
	public static void resume(GameObject gameObject)
	{
		Transform transform = gameObject.transform;
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].trans == transform)
			{
				LeanTween.tweens[i].resume();
			}
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000F994 File Offset: 0x0000DB94
	public static bool isPaused(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (object.Equals(LeanTween.tweens[i].direction, 0f))
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (object.Equals(LeanTween.tweens[j].direction, 0f) && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000FA30 File Offset: 0x0000DC30
	public static bool isPaused(RectTransform rect)
	{
		return LeanTween.isTweening(rect.gameObject);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000FA40 File Offset: 0x0000DC40
	public static bool isPaused(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && object.Equals(LeanTween.tweens[LeanTween.i].direction, 0f));
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
	public static bool isTweening(GameObject gameObject = null)
	{
		if (gameObject == null)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle)
				{
					return true;
				}
			}
			return false;
		}
		Transform transform = gameObject.transform;
		for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
		{
			if (LeanTween.tweens[j].toggle && LeanTween.tweens[j].trans == transform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000FA30 File Offset: 0x0000DC30
	public static bool isTweening(RectTransform rect)
	{
		return LeanTween.isTweening(rect.gameObject);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000FB18 File Offset: 0x0000DD18
	public static bool isTweening(int uniqueId)
	{
		int num = uniqueId & 65535;
		int num2 = uniqueId >> 16;
		return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && LeanTween.tweens[num].toggle);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000FB64 File Offset: 0x0000DD64
	public static bool isTweening(LTRect ltRect)
	{
		for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
		{
			if (LeanTween.tweens[i].toggle && LeanTween.tweens[i]._optional.ltRect == ltRect)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000FBA8 File Offset: 0x0000DDA8
	public static void drawBezierPath(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize = 0f, Transform arrowTransform = null)
	{
		Vector3 vector = a;
		Vector3 a2 = -a + 3f * (b - c) + d;
		Vector3 b2 = 3f * (a + c) - 6f * b;
		Vector3 b3 = 3f * (b - a);
		if (arrowSize > 0f)
		{
			Vector3 position = arrowTransform.position;
			Quaternion rotation = arrowTransform.rotation;
			float num = 0f;
			for (float num2 = 1f; num2 <= 120f; num2 += 1f)
			{
				float num3 = num2 / 120f;
				Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
				Gizmos.DrawLine(vector, vector2);
				num += (vector2 - vector).magnitude;
				if (num > 1f)
				{
					num -= 1f;
					arrowTransform.position = vector2;
					arrowTransform.LookAt(vector, Vector3.forward);
					Vector3 a3 = arrowTransform.TransformDirection(Vector3.right);
					Vector3 normalized = (vector - vector2).normalized;
					Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
					a3 = arrowTransform.TransformDirection(-Vector3.right);
					Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
				}
				vector = vector2;
			}
			arrowTransform.position = position;
			arrowTransform.rotation = rotation;
			return;
		}
		for (float num4 = 1f; num4 <= 30f; num4 += 1f)
		{
			float num3 = num4 / 30f;
			Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000FDAA File Offset: 0x0000DFAA
	public static object logError(string error)
	{
		if (LeanTween.throwErrors)
		{
			Debug.LogError(error);
		}
		else
		{
			Debug.Log(error);
		}
		return null;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000FDC2 File Offset: 0x0000DFC2
	public static LTDescr options(LTDescr seed)
	{
		Debug.LogError("error this function is no longer used");
		return null;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000FDD0 File Offset: 0x0000DFD0
	public static LTDescr options()
	{
		LeanTween.init();
		bool flag = false;
		LeanTween.j = 0;
		LeanTween.i = LeanTween.startSearch;
		while (LeanTween.j <= LeanTween.maxTweens)
		{
			if (LeanTween.j >= LeanTween.maxTweens)
			{
				return LeanTween.logError("LeanTween - You have run out of available spaces for tweening. To avoid this error increase the number of spaces to available for tweening when you initialize the LeanTween class ex: LeanTween.init( " + LeanTween.maxTweens * 2 + " );") as LTDescr;
			}
			if (LeanTween.i >= LeanTween.maxTweens)
			{
				LeanTween.i = 0;
			}
			if (!LeanTween.tweens[LeanTween.i].toggle)
			{
				if (LeanTween.i + 1 > LeanTween.tweenMaxSearch && LeanTween.i + 1 < LeanTween.maxTweens)
				{
					LeanTween.tweenMaxSearch = LeanTween.i + 1;
				}
				LeanTween.startSearch = LeanTween.i + 1;
				flag = true;
				break;
			}
			LeanTween.j++;
			LeanTween.i++;
		}
		if (!flag)
		{
			LeanTween.logError("no available tween found!");
		}
		LeanTween.tweens[LeanTween.i].reset();
		LeanTween.global_counter += 1U;
		if (LeanTween.global_counter > 32768U)
		{
			LeanTween.global_counter = 0U;
		}
		LeanTween.tweens[LeanTween.i].setId((uint)LeanTween.i, LeanTween.global_counter);
		return LeanTween.tweens[LeanTween.i];
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000279 RID: 633 RVA: 0x0000FF10 File Offset: 0x0000E110
	public static GameObject tweenEmpty
	{
		get
		{
			LeanTween.init(LeanTween.maxTweens);
			return LeanTween._tweenEmpty;
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000FF24 File Offset: 0x0000E124
	private static LTDescr pushNewTween(GameObject gameObject, Vector3 to, float time, LTDescr tween)
	{
		LeanTween.init(LeanTween.maxTweens);
		if (gameObject == null || tween == null)
		{
			return null;
		}
		tween.trans = gameObject.transform;
		tween.to = to;
		tween.time = time;
		if (tween.time <= 0f)
		{
			tween.updateInternal();
		}
		return tween;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000FF78 File Offset: 0x0000E178
	public static LTDescr play(RectTransform rectTransform, Sprite[] sprites)
	{
		float time = 0.25f * (float)sprites.Length;
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3((float)sprites.Length - 1f, 0f, 0f), time, LeanTween.options().setCanvasPlaySprite().setSprites(sprites).setRepeat(-1));
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000FFCC File Offset: 0x0000E1CC
	public static LTSeq sequence(bool initSequence = true)
	{
		LeanTween.init(LeanTween.maxTweens);
		for (int i = 0; i < LeanTween.sequences.Length; i++)
		{
			if ((LeanTween.sequences[i].tween == null || !LeanTween.sequences[i].tween.toggle) && !LeanTween.sequences[i].toggle)
			{
				LTSeq ltseq = LeanTween.sequences[i];
				if (initSequence)
				{
					ltseq.init((uint)(i + LeanTween.tweens.Length), LeanTween.global_counter);
					LeanTween.global_counter += 1U;
					if (LeanTween.global_counter > 32768U)
					{
						LeanTween.global_counter = 0U;
					}
				}
				else
				{
					ltseq.reset();
				}
				return ltseq;
			}
		}
		return null;
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00010070 File Offset: 0x0000E270
	public static LTDescr alpha(GameObject gameObject, float to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setAlpha());
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x000100AC File Offset: 0x0000E2AC
	public static LTDescr alpha(LTRect ltRect, float to, float time)
	{
		ltRect.alphaEnabled = true;
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setGUIAlpha().setRect(ltRect));
	}

	// Token: 0x0600027F RID: 639 RVA: 0x000100E0 File Offset: 0x0000E2E0
	public static LTDescr textAlpha(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setTextAlpha());
	}

	// Token: 0x06000280 RID: 640 RVA: 0x000100E0 File Offset: 0x0000E2E0
	public static LTDescr alphaText(RectTransform rectTransform, float to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setTextAlpha());
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00010108 File Offset: 0x0000E308
	public static LTDescr alphaCanvas(CanvasGroup canvasGroup, float to, float time)
	{
		return LeanTween.pushNewTween(canvasGroup.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasGroupAlpha());
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00010130 File Offset: 0x0000E330
	public static LTDescr alphaVertex(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setAlphaVertex());
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00010154 File Offset: 0x0000E354
	public static LTDescr color(GameObject gameObject, Color to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setColor().setPoint(new Vector3(to.r, to.g, to.b)));
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000101B4 File Offset: 0x0000E3B4
	public static LTDescr textColor(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setTextColor().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00010208 File Offset: 0x0000E408
	public static LTDescr colorText(RectTransform rectTransform, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setTextColor().setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0001025C File Offset: 0x0000E45C
	public static LTDescr delayedCall(float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0001027E File Offset: 0x0000E47E
	public static LTDescr delayedCall(float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000288 RID: 648 RVA: 0x000102A0 File Offset: 0x0000E4A0
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000102BE File Offset: 0x0000E4BE
	public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action<object> callback)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, delayTime, LeanTween.options().setCallback().setOnComplete(callback));
	}

	// Token: 0x0600028A RID: 650 RVA: 0x000102DC File Offset: 0x0000E4DC
	public static LTDescr destroyAfter(LTRect rect, float delayTime)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.zero, delayTime, LeanTween.options().setCallback().setRect(rect).setDestroyOnComplete(true));
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00010304 File Offset: 0x0000E504
	public static LTDescr move(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setMove());
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00010318 File Offset: 0x0000E518
	public static LTDescr move(GameObject gameObject, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, gameObject.transform.position.z), time, LeanTween.options().setMove());
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0001034C File Offset: 0x0000E54C
	public static LTDescr move(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurved();
		if (LeanTween.d.optional.path == null)
		{
			LeanTween.d.optional.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.optional.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x000103C8 File Offset: 0x0000E5C8
	public static LTDescr move(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurved();
		LeanTween.d.optional.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00010414 File Offset: 0x0000E614
	public static LTDescr move(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00010460 File Offset: 0x0000E660
	public static LTDescr moveSpline(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x000104B4 File Offset: 0x0000E6B4
	public static LTDescr moveSpline(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSpline();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00010500 File Offset: 0x0000E700
	public static LTDescr moveSplineLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSplineLocal();
		LeanTween.d.optional.spline = new LTSpline(to);
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00010551 File Offset: 0x0000E751
	public static LTDescr move(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIMove().setRect(ltRect));
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00010574 File Offset: 0x0000E774
	public static LTDescr moveMargin(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIMoveMargin().setRect(ltRect));
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00010597 File Offset: 0x0000E797
	public static LTDescr moveX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveX());
	}

	// Token: 0x06000296 RID: 662 RVA: 0x000105BA File Offset: 0x0000E7BA
	public static LTDescr moveY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveY());
	}

	// Token: 0x06000297 RID: 663 RVA: 0x000105DD File Offset: 0x0000E7DD
	public static LTDescr moveZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveZ());
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00010600 File Offset: 0x0000E800
	public static LTDescr moveLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setMoveLocal());
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00010614 File Offset: 0x0000E814
	public static LTDescr moveLocal(GameObject gameObject, Vector3[] to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurvedLocal();
		if (LeanTween.d.optional.path == null)
		{
			LeanTween.d.optional.path = new LTBezierPath(to);
		}
		else
		{
			LeanTween.d.optional.path.setPoints(to);
		}
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0001068D File Offset: 0x0000E88D
	public static LTDescr moveLocalX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalX());
	}

	// Token: 0x0600029B RID: 667 RVA: 0x000106B0 File Offset: 0x0000E8B0
	public static LTDescr moveLocalY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalY());
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000106D3 File Offset: 0x0000E8D3
	public static LTDescr moveLocalZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setMoveLocalZ());
	}

	// Token: 0x0600029D RID: 669 RVA: 0x000106F8 File Offset: 0x0000E8F8
	public static LTDescr moveLocal(GameObject gameObject, LTBezierPath to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveCurvedLocal();
		LeanTween.d.optional.path = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00010744 File Offset: 0x0000E944
	public static LTDescr moveLocal(GameObject gameObject, LTSpline to, float time)
	{
		LeanTween.d = LeanTween.options().setMoveSplineLocal();
		LeanTween.d.optional.spline = to;
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, LeanTween.d);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00010790 File Offset: 0x0000E990
	public static LTDescr move(GameObject gameObject, Transform to, float time)
	{
		return LeanTween.pushNewTween(gameObject, Vector3.zero, time, LeanTween.options().setTo(to).setMoveToTransform());
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x000107AE File Offset: 0x0000E9AE
	public static LTDescr rotate(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setRotate());
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x000107C2 File Offset: 0x0000E9C2
	public static LTDescr rotate(LTRect ltRect, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setGUIRotate().setRect(ltRect));
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x000107EF File Offset: 0x0000E9EF
	public static LTDescr rotateLocal(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setRotateLocal());
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00010803 File Offset: 0x0000EA03
	public static LTDescr rotateX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateX());
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00010826 File Offset: 0x0000EA26
	public static LTDescr rotateY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateY());
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00010849 File Offset: 0x0000EA49
	public static LTDescr rotateZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setRotateZ());
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0001086C File Offset: 0x0000EA6C
	public static LTDescr rotateAround(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, LeanTween.options().setAxis(axis).setRotateAround());
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00010895 File Offset: 0x0000EA95
	public static LTDescr rotateAroundLocal(GameObject gameObject, Vector3 axis, float add, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, LeanTween.options().setRotateAroundLocal().setAxis(axis));
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x000108BE File Offset: 0x0000EABE
	public static LTDescr scale(GameObject gameObject, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setScale());
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x000108D2 File Offset: 0x0000EAD2
	public static LTDescr scale(LTRect ltRect, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, LeanTween.options().setGUIScale().setRect(ltRect));
	}

	// Token: 0x060002AA RID: 682 RVA: 0x000108F5 File Offset: 0x0000EAF5
	public static LTDescr scaleX(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleX());
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00010918 File Offset: 0x0000EB18
	public static LTDescr scaleY(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleY());
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0001093B File Offset: 0x0000EB3B
	public static LTDescr scaleZ(GameObject gameObject, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setScaleZ());
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0001095E File Offset: 0x0000EB5E
	public static LTDescr value(GameObject gameObject, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00010996 File Offset: 0x0000EB96
	public static LTDescr value(float from, float to, float time)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setFrom(new Vector3(from, 0f, 0f)));
	}

	// Token: 0x060002AF RID: 687 RVA: 0x000109D4 File Offset: 0x0000EBD4
	public static LTDescr value(GameObject gameObject, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, LeanTween.options().setValue3().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)));
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00010A3E File Offset: 0x0000EC3E
	public static LTDescr value(GameObject gameObject, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setValue3().setFrom(from));
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00010A58 File Offset: 0x0000EC58
	public static LTDescr value(GameObject gameObject, Color from, Color to, float time)
	{
		LTDescr ltdescr = LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setFromColor(from).setHasInitialized(false));
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		ltdescr.spriteRen = component;
		return ltdescr;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00010AC4 File Offset: 0x0000ECC4
	public static LTDescr value(GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate));
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00010B24 File Offset: 0x0000ED24
	public static LTDescr value(GameObject gameObject, Action<float, float> callOnUpdateRatio, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdateRatio(callOnUpdateRatio));
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00010B84 File Offset: 0x0000ED84
	public static LTDescr value(GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00010C18 File Offset: 0x0000EE18
	public static LTDescr value(GameObject gameObject, Action<Color, object> callOnUpdate, Color from, Color to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCallbackColor().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00010CAC File Offset: 0x0000EEAC
	public static LTDescr value(GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, LeanTween.options().setValue3().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)).setOnUpdateVector2(callOnUpdate));
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00010D1D File Offset: 0x0000EF1D
	public static LTDescr value(GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(gameObject, to, time, LeanTween.options().setValue3().setTo(to).setFrom(from).setOnUpdateVector3(callOnUpdate));
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00010D44 File Offset: 0x0000EF44
	public static LTDescr value(GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
	{
		return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCallback().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate, gameObject));
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00010DA4 File Offset: 0x0000EFA4
	public static LTDescr delayedSound(AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(LeanTween.tweenEmpty, pos, 0f, LeanTween.options().setDelayedSound().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00010DE1 File Offset: 0x0000EFE1
	public static LTDescr delayedSound(GameObject gameObject, AudioClip audio, Vector3 pos, float volume)
	{
		return LeanTween.pushNewTween(gameObject, pos, 0f, LeanTween.options().setDelayedSound().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00010E1A File Offset: 0x0000F01A
	public static LTDescr move(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasMove().setRect(rectTrans));
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00010E39 File Offset: 0x0000F039
	public static LTDescr moveX(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveX().setRect(rectTrans));
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00010E67 File Offset: 0x0000F067
	public static LTDescr moveY(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveY().setRect(rectTrans));
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00010E95 File Offset: 0x0000F095
	public static LTDescr moveZ(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasMoveZ().setRect(rectTrans));
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00010EC3 File Offset: 0x0000F0C3
	public static LTDescr rotate(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00010EFB File Offset: 0x0000F0FB
	public static LTDescr rotate(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(Vector3.forward));
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00010F24 File Offset: 0x0000F124
	public static LTDescr rotateAround(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAround().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00010F58 File Offset: 0x0000F158
	public static LTDescr rotateAroundLocal(RectTransform rectTrans, Vector3 axis, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasRotateAroundLocal().setRect(rectTrans).setAxis(axis));
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00010F8C File Offset: 0x0000F18C
	public static LTDescr scale(RectTransform rectTrans, Vector3 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasScale().setRect(rectTrans));
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00010FAB File Offset: 0x0000F1AB
	public static LTDescr size(RectTransform rectTrans, Vector2 to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, to, time, LeanTween.options().setCanvasSizeDelta().setRect(rectTrans));
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00010FCF File Offset: 0x0000F1CF
	public static LTDescr alpha(RectTransform rectTrans, float to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, LeanTween.options().setCanvasAlpha().setRect(rectTrans));
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00011000 File Offset: 0x0000F200
	public static LTDescr color(RectTransform rectTrans, Color to, float time)
	{
		return LeanTween.pushNewTween(rectTrans.gameObject, new Vector3(1f, to.a, 0f), time, LeanTween.options().setCanvasColor().setRect(rectTrans).setPoint(new Vector3(to.r, to.g, to.b)));
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001105A File Offset: 0x0000F25A
	public static float tweenOnCurve(LTDescr tweenDescr, float ratioPassed)
	{
		return tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.optional.animationCurve.Evaluate(ratioPassed);
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00011088 File Offset: 0x0000F288
	public static Vector3 tweenOnCurveVector(LTDescr tweenDescr, float ratioPassed)
	{
		return new Vector3(tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.optional.animationCurve.Evaluate(ratioPassed), tweenDescr.from.y + tweenDescr.diff.y * tweenDescr.optional.animationCurve.Evaluate(ratioPassed), tweenDescr.from.z + tweenDescr.diff.z * tweenDescr.optional.animationCurve.Evaluate(ratioPassed));
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00011115 File Offset: 0x0000F315
	public static float easeOutQuadOpt(float start, float diff, float ratioPassed)
	{
		return -diff * ratioPassed * (ratioPassed - 2f) + start;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00011125 File Offset: 0x0000F325
	public static float easeInQuadOpt(float start, float diff, float ratioPassed)
	{
		return diff * ratioPassed * ratioPassed + start;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00011130 File Offset: 0x0000F330
	public static float easeInOutQuadOpt(float start, float diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00011180 File Offset: 0x0000F380
	public static Vector3 easeInOutQuadOpt(Vector3 start, Vector3 diff, float ratioPassed)
	{
		ratioPassed /= 0.5f;
		if (ratioPassed < 1f)
		{
			return diff / 2f * ratioPassed * ratioPassed + start;
		}
		ratioPassed -= 1f;
		return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
	}

	// Token: 0x060002CD RID: 717 RVA: 0x000111EF File Offset: 0x0000F3EF
	public static float linear(float start, float end, float val)
	{
		return Mathf.Lerp(start, end, val);
	}

	// Token: 0x060002CE RID: 718 RVA: 0x000111FC File Offset: 0x0000F3FC
	public static float clerp(float start, float end, float val)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * val;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * val;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * val;
		}
		return result;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00011268 File Offset: 0x0000F468
	public static float spring(float start, float end, float val)
	{
		val = Mathf.Clamp01(val);
		val = (Mathf.Sin(val * 3.1415927f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
		return start + (end - start) * val;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x000112CC File Offset: 0x0000F4CC
	public static float easeInQuad(float start, float end, float val)
	{
		end -= start;
		return end * val * val + start;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000112DA File Offset: 0x0000F4DA
	public static float easeOutQuad(float start, float end, float val)
	{
		end -= start;
		return -end * val * (val - 2f) + start;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000112F0 File Offset: 0x0000F4F0
	public static float easeInOutQuad(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val + start;
		}
		val -= 1f;
		return -end / 2f * (val * (val - 2f) - 1f) + start;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00011344 File Offset: 0x0000F544
	public static float easeInOutQuadOpt2(float start, float diffBy2, float val, float val2)
	{
		val /= 0.5f;
		if (val < 1f)
		{
			return diffBy2 * val2 + start;
		}
		val -= 1f;
		return -diffBy2 * (val2 - 2f - 1f) + start;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00011378 File Offset: 0x0000F578
	public static float easeInCubic(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val + start;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00011388 File Offset: 0x0000F588
	public static float easeOutCubic(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val + 1f) + start;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000113A8 File Offset: 0x0000F5A8
	public static float easeInOutCubic(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val + 2f) + start;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x000113F9 File Offset: 0x0000F5F9
	public static float easeInQuart(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val + start;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0001140B File Offset: 0x0000F60B
	public static float easeOutQuart(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return -end * (val * val * val * val - 1f) + start;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00011430 File Offset: 0x0000F630
	public static float easeInOutQuart(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val + start;
		}
		val -= 2f;
		return -end / 2f * (val * val * val * val - 2f) + start;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00011486 File Offset: 0x0000F686
	public static float easeInQuint(float start, float end, float val)
	{
		end -= start;
		return end * val * val * val * val * val + start;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001149A File Offset: 0x0000F69A
	public static float easeOutQuint(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * (val * val * val * val * val + 1f) + start;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x000114C0 File Offset: 0x0000F6C0
	public static float easeInOutQuint(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * val * val * val * val * val + start;
		}
		val -= 2f;
		return end / 2f * (val * val * val * val * val + 2f) + start;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00011519 File Offset: 0x0000F719
	public static float easeInSine(float start, float end, float val)
	{
		end -= start;
		return -end * Mathf.Cos(val / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00011539 File Offset: 0x0000F739
	public static float easeOutSine(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Sin(val / 1f * 1.5707964f) + start;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00011556 File Offset: 0x0000F756
	public static float easeInOutSine(float start, float end, float val)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * val / 1f) - 1f) + start;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00011580 File Offset: 0x0000F780
	public static float easeInExpo(float start, float end, float val)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x000115A8 File Offset: 0x0000F7A8
	public static float easeOutExpo(float start, float end, float val)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000115D4 File Offset: 0x0000F7D4
	public static float easeInOutExpo(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
		}
		val -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * val) + 2f) + start;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00011644 File Offset: 0x0000F844
	public static float easeInCirc(float start, float end, float val)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - val * val) - 1f) + start;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00011664 File Offset: 0x0000F864
	public static float easeOutCirc(float start, float end, float val)
	{
		val -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - val * val) + start;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00011688 File Offset: 0x0000F888
	public static float easeInOutCirc(float start, float end, float val)
	{
		val /= 0.5f;
		end -= start;
		if (val < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}
		val -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x000116F4 File Offset: 0x0000F8F4
	public static float easeInBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		return end - LeanTween.easeOutBounce(0f, end, num - val) + start;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00011720 File Offset: 0x0000F920
	public static float easeOutBounce(float start, float end, float val)
	{
		val /= 1f;
		end -= start;
		if (val < 0.36363637f)
		{
			return end * (7.5625f * val * val) + start;
		}
		if (val < 0.72727275f)
		{
			val -= 0.54545456f;
			return end * (7.5625f * val * val + 0.75f) + start;
		}
		if ((double)val < 0.9090909090909091)
		{
			val -= 0.8181818f;
			return end * (7.5625f * val * val + 0.9375f) + start;
		}
		val -= 0.95454544f;
		return end * (7.5625f * val * val + 0.984375f) + start;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x000117BC File Offset: 0x0000F9BC
	public static float easeInOutBounce(float start, float end, float val)
	{
		end -= start;
		float num = 1f;
		if (val < num / 2f)
		{
			return LeanTween.easeInBounce(0f, end, val * 2f) * 0.5f + start;
		}
		return LeanTween.easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00011820 File Offset: 0x0000FA20
	public static float easeInBack(float start, float end, float val, float overshoot = 1f)
	{
		end -= start;
		val /= 1f;
		float num = 1.70158f * overshoot;
		return end * val * val * ((num + 1f) * val - num) + start;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00011858 File Offset: 0x0000FA58
	public static float easeOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val = val / 1f - 1f;
		return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001189C File Offset: 0x0000FA9C
	public static float easeInOutBack(float start, float end, float val, float overshoot = 1f)
	{
		float num = 1.70158f * overshoot;
		end -= start;
		val /= 0.5f;
		if (val < 1f)
		{
			num *= 1.525f * overshoot;
			return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
		}
		val -= 2f;
		num *= 1.525f * overshoot;
		return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00011920 File Offset: 0x0000FB20
	public static float easeInElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val > 0.6f)
		{
			overshoot = 1f + (1f - val) / 0.4f * (overshoot - 1f);
		}
		val -= 1f;
		return start - num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000119E4 File Offset: 0x0000FBE4
	public static float easeOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		if (val == 1f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f && val < 0.4f)
		{
			overshoot = 1f + val / 0.4f * (overshoot - 1f);
		}
		return start + end + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * overshoot;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00011A9C File Offset: 0x0000FC9C
	public static float easeInOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
	{
		end -= start;
		float num = 0f;
		if (val == 0f)
		{
			return start;
		}
		val /= 0.5f;
		if (val == 2f)
		{
			return start + end;
		}
		float num2;
		if (num == 0f || num < Mathf.Abs(end))
		{
			num = end;
			num2 = period / 4f;
		}
		else
		{
			num2 = period / 6.2831855f * Mathf.Asin(end / num);
		}
		if (overshoot > 1f)
		{
			if (val < 0.2f)
			{
				overshoot = 1f + val / 0.2f * (overshoot - 1f);
			}
			else if (val > 0.8f)
			{
				overshoot = 1f + (1f - val) / 0.2f * (overshoot - 1f);
			}
		}
		if (val < 1f)
		{
			val -= 1f;
			return start - 0.5f * (num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period)) * overshoot;
		}
		val -= 1f;
		return end + start + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.2831855f / period) * 0.5f * overshoot;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00011BD4 File Offset: 0x0000FDD4
	public static LTDescr followDamp(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.diff = d.trans.position;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.damp(d.optional.axis, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.position = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.damp(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.damp(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.damp(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.damp(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.damp(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.damp(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.damp(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.damp(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.damp(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00011D9C File Offset: 0x0000FF9C
	public static LTDescr followSpring(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.diff = d.trans.position;
			d.easeInternal = delegate()
			{
				d.diff = LeanSmooth.spring(d.diff, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.position = d.diff;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.spring(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.spring(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.spring(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.spring(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.spring(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.spring(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.spring(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.spring(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.spring(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00011F74 File Offset: 0x00010174
	public static LTDescr followBounceOut(Transform trans, Transform target, LeanProp prop, float smoothTime, float maxSpeed = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.bounceOut(d.optional.axis, d.toTrans.position, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.position = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.bounceOut(d.optional.axis, d.toTrans.localPosition, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.bounceOut(d.trans.position.x, d.toTrans.position.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.bounceOut(d.trans.position.y, d.toTrans.position.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.bounceOut(d.trans.position.z, d.toTrans.position.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.bounceOut(d.trans.localPosition.x, d.toTrans.localPosition.x, ref d.fromInternal.x, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.bounceOut(d.trans.localPosition.y, d.toTrans.localPosition.y, ref d.fromInternal.y, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.bounceOut(d.trans.localPosition.z, d.toTrans.localPosition.z, ref d.fromInternal.z, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.bounceOut(d.trans.localScale, d.toTrans.localScale, ref d.fromInternal, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.bounceOut(d.trans.LeanColor(), d.toTrans.LeanColor(), ref d.optional.color, smoothTime, maxSpeed, Time.deltaTime, friction, accelRate, hitDamping);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00012138 File Offset: 0x00010338
	public static LTDescr followLinear(Transform trans, Transform target, LeanProp prop, float moveSpeed)
	{
		LTDescr d = LeanTween.pushNewTween(trans.gameObject, Vector3.zero, float.MaxValue, LeanTween.options().setFollow().setTarget(target));
		switch (prop)
		{
		case LeanProp.position:
			d.easeInternal = delegate()
			{
				d.trans.position = LeanSmooth.linear(d.trans.position, d.toTrans.position, moveSpeed, -1f);
			};
			break;
		case LeanProp.localPosition:
			d.optional.axis = d.trans.localPosition;
			d.easeInternal = delegate()
			{
				d.optional.axis = LeanSmooth.linear(d.optional.axis, d.toTrans.localPosition, moveSpeed, -1f);
				d.trans.localPosition = d.optional.axis + d.toInternal;
			};
			break;
		case LeanProp.x:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosX(LeanSmooth.linear(d.trans.position.x, d.toTrans.position.x, moveSpeed, -1f));
			};
			break;
		case LeanProp.y:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosY(LeanSmooth.linear(d.trans.position.y, d.toTrans.position.y, moveSpeed, -1f));
			};
			break;
		case LeanProp.z:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetPosZ(LeanSmooth.linear(d.trans.position.z, d.toTrans.position.z, moveSpeed, -1f));
			};
			break;
		case LeanProp.localX:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosX(LeanSmooth.linear(d.trans.localPosition.x, d.toTrans.localPosition.x, moveSpeed, -1f));
			};
			break;
		case LeanProp.localY:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosY(LeanSmooth.linear(d.trans.localPosition.y, d.toTrans.localPosition.y, moveSpeed, -1f));
			};
			break;
		case LeanProp.localZ:
			d.easeInternal = delegate()
			{
				d.trans.LeanSetLocalPosZ(LeanSmooth.linear(d.trans.localPosition.z, d.toTrans.localPosition.z, moveSpeed, -1f));
			};
			break;
		case LeanProp.scale:
			d.easeInternal = delegate()
			{
				d.trans.localScale = LeanSmooth.linear(d.trans.localScale, d.toTrans.localScale, moveSpeed, -1f);
			};
			break;
		case LeanProp.color:
			d.easeInternal = delegate()
			{
				Color color = LeanSmooth.linear(d.trans.LeanColor(), d.toTrans.LeanColor(), moveSpeed);
				d.trans.GetComponent<Renderer>().material.color = color;
			};
			break;
		}
		return d;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000122DA File Offset: 0x000104DA
	public static void addListener(int eventId, Action<LTEvent> callback)
	{
		LeanTween.addListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000122E8 File Offset: 0x000104E8
	public static void addListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		if (LeanTween.eventListeners == null)
		{
			LeanTween.INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
			LeanTween.eventListeners = new Action<LTEvent>[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
			LeanTween.goListeners = new GameObject[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
		}
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.INIT_LISTENERS_MAX)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == null || LeanTween.eventListeners[num] == null)
			{
				LeanTween.eventListeners[num] = callback;
				LeanTween.goListeners[num] = caller;
				if (LeanTween.i >= LeanTween.eventsMaxSearch)
				{
					LeanTween.eventsMaxSearch = LeanTween.i + 1;
				}
				return;
			}
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				return;
			}
			LeanTween.i++;
		}
		Debug.LogError("You ran out of areas to add listeners, consider increasing LISTENERS_MAX, ex: LeanTween.LISTENERS_MAX = " + LeanTween.LISTENERS_MAX * 2);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000123DC File Offset: 0x000105DC
	public static bool removeListener(int eventId, Action<LTEvent> callback)
	{
		return LeanTween.removeListener(LeanTween.tweenEmpty, eventId, callback);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x000123EC File Offset: 0x000105EC
	public static bool removeListener(int eventId)
	{
		int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
		LeanTween.eventListeners[num] = null;
		LeanTween.goListeners[num] = null;
		return true;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00012418 File Offset: 0x00010618
	public static bool removeListener(GameObject caller, int eventId, Action<LTEvent> callback)
	{
		LeanTween.i = 0;
		while (LeanTween.i < LeanTween.eventsMaxSearch)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
			if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
			{
				LeanTween.eventListeners[num] = null;
				LeanTween.goListeners[num] = null;
				return true;
			}
			LeanTween.i++;
		}
		return false;
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00012484 File Offset: 0x00010684
	public static void dispatchEvent(int eventId)
	{
		LeanTween.dispatchEvent(eventId, null);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00012490 File Offset: 0x00010690
	public static void dispatchEvent(int eventId, object data)
	{
		for (int i = 0; i < LeanTween.eventsMaxSearch; i++)
		{
			int num = eventId * LeanTween.INIT_LISTENERS_MAX + i;
			if (LeanTween.eventListeners[num] != null)
			{
				if (LeanTween.goListeners[num])
				{
					LeanTween.eventListeners[num](new LTEvent(eventId, data));
				}
				else
				{
					LeanTween.eventListeners[num] = null;
				}
			}
		}
	}

	// Token: 0x0400016E RID: 366
	public static bool throwErrors = true;

	// Token: 0x0400016F RID: 367
	public static float tau = 6.2831855f;

	// Token: 0x04000170 RID: 368
	public static float PI_DIV2 = 1.5707964f;

	// Token: 0x04000171 RID: 369
	private static LTSeq[] sequences;

	// Token: 0x04000172 RID: 370
	private static LTDescr[] tweens;

	// Token: 0x04000173 RID: 371
	private static int[] tweensFinished;

	// Token: 0x04000174 RID: 372
	private static int[] tweensFinishedIds;

	// Token: 0x04000175 RID: 373
	private static LTDescr tween;

	// Token: 0x04000176 RID: 374
	private static int tweenMaxSearch = -1;

	// Token: 0x04000177 RID: 375
	private static int maxTweens = 400;

	// Token: 0x04000178 RID: 376
	private static int maxSequences = 400;

	// Token: 0x04000179 RID: 377
	private static int frameRendered = -1;

	// Token: 0x0400017A RID: 378
	private static GameObject _tweenEmpty;

	// Token: 0x0400017B RID: 379
	public static float dtEstimated = -1f;

	// Token: 0x0400017C RID: 380
	public static float dtManual;

	// Token: 0x0400017D RID: 381
	public static float dtActual;

	// Token: 0x0400017E RID: 382
	private static uint global_counter = 0U;

	// Token: 0x0400017F RID: 383
	private static int i;

	// Token: 0x04000180 RID: 384
	private static int j;

	// Token: 0x04000181 RID: 385
	private static int finishedCnt;

	// Token: 0x04000182 RID: 386
	public static AnimationCurve punch = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.112586f, 0.9976035f),
		new Keyframe(0.3120486f, -0.1720615f),
		new Keyframe(0.4316337f, 0.07030682f),
		new Keyframe(0.5524869f, -0.03141804f),
		new Keyframe(0.6549395f, 0.003909959f),
		new Keyframe(0.770987f, -0.009817753f),
		new Keyframe(0.8838775f, 0.001939224f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04000183 RID: 387
	public static AnimationCurve shake = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f),
		new Keyframe(0.75f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04000184 RID: 388
	private static int maxTweenReached;

	// Token: 0x04000185 RID: 389
	public static int startSearch = 0;

	// Token: 0x04000186 RID: 390
	public static LTDescr d;

	// Token: 0x04000187 RID: 391
	private static Action<LTEvent>[] eventListeners;

	// Token: 0x04000188 RID: 392
	private static GameObject[] goListeners;

	// Token: 0x04000189 RID: 393
	private static int eventsMaxSearch = 0;

	// Token: 0x0400018A RID: 394
	public static int EVENTS_MAX = 10;

	// Token: 0x0400018B RID: 395
	public static int LISTENERS_MAX = 10;

	// Token: 0x0400018C RID: 396
	private static int INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
}
