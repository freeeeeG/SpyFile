using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200039A RID: 922
public readonly struct Updater : IEnumerator
{
	// Token: 0x06001327 RID: 4903 RVA: 0x00065199 File Offset: 0x00063399
	public Updater(Func<float, UpdaterResult> fn)
	{
		this.fn = fn;
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x000651A2 File Offset: 0x000633A2
	public UpdaterResult Internal_Update(float deltaTime)
	{
		return this.fn(deltaTime);
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06001329 RID: 4905 RVA: 0x000651B0 File Offset: 0x000633B0
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x000651B3 File Offset: 0x000633B3
	bool IEnumerator.MoveNext()
	{
		return this.fn(Updater.GetDeltaTime()) == UpdaterResult.NotComplete;
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x000651C8 File Offset: 0x000633C8
	void IEnumerator.Reset()
	{
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x000651CA File Offset: 0x000633CA
	public static implicit operator Updater(Promise promise)
	{
		return Updater.Until(() => promise.IsResolved);
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x000651E8 File Offset: 0x000633E8
	public static Updater Until(Func<bool> fn)
	{
		return new Updater(delegate(float dt)
		{
			if (!fn())
			{
				return UpdaterResult.NotComplete;
			}
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x00065206 File Offset: 0x00063406
	public static Updater While(Func<bool> isTrueFn)
	{
		return new Updater(delegate(float dt)
		{
			if (!isTrueFn())
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x00065224 File Offset: 0x00063424
	public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
	{
		Updater whileNotTrueUpdater = Updater.None();
		return new Updater(delegate(float dt)
		{
			if (whileNotTrueUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				if (!isTrueFn())
				{
					return UpdaterResult.Complete;
				}
				whileNotTrueUpdater = getUpdaterWhileNotTrueFn();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x00065254 File Offset: 0x00063454
	public static Updater None()
	{
		return Updater.WaitFrames(0);
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x0006525C File Offset: 0x0006345C
	public static Updater WaitOneFrame()
	{
		return Updater.WaitFrames(1);
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x00065264 File Offset: 0x00063464
	public static Updater WaitFrames(int framesToWait)
	{
		int frame = 0;
		return new Updater(delegate(float dt)
		{
			int frame = frame;
			frame++;
			if (framesToWait <= frame)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x00065289 File Offset: 0x00063489
	public static Updater WaitForSeconds(float secondsToWait)
	{
		float currentSeconds = 0f;
		return new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			if (secondsToWait <= currentSeconds)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x000652B2 File Offset: 0x000634B2
	public static Updater Ease(Action<float> fn, float from, float to, float duration, Easing.EasingFn easing = null)
	{
		return Updater.GenericEase<float>(fn, new Func<float, float, float, float>(Mathf.LerpUnclamped), easing, from, to, duration);
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x000652CB File Offset: 0x000634CB
	public static Updater Ease(Action<Vector2> fn, Vector2 from, Vector2 to, float duration, Easing.EasingFn easing = null)
	{
		return Updater.GenericEase<Vector2>(fn, new Func<Vector2, Vector2, float, Vector2>(Vector2.LerpUnclamped), easing, from, to, duration);
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x000652E4 File Offset: 0x000634E4
	public static Updater Ease(Action<Vector3> fn, Vector3 from, Vector3 to, float duration, Easing.EasingFn easing = null)
	{
		return Updater.GenericEase<Vector3>(fn, new Func<Vector3, Vector3, float, Vector3>(Vector3.LerpUnclamped), easing, from, to, duration);
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x00065300 File Offset: 0x00063500
	public static Updater GenericEase<T>(Action<T> useFn, Func<T, T, float, T> interpolateFn, Easing.EasingFn easingFn, T from, T to, float duration)
	{
		Updater.<>c__DisplayClass18_0<T> CS$<>8__locals1 = new Updater.<>c__DisplayClass18_0<T>();
		CS$<>8__locals1.useFn = useFn;
		CS$<>8__locals1.interpolateFn = interpolateFn;
		CS$<>8__locals1.from = from;
		CS$<>8__locals1.to = to;
		CS$<>8__locals1.easingFn = easingFn;
		CS$<>8__locals1.duration = duration;
		if (CS$<>8__locals1.easingFn == null)
		{
			CS$<>8__locals1.easingFn = Easing.SmoothStep;
		}
		CS$<>8__locals1.currentSeconds = 0f;
		CS$<>8__locals1.<GenericEase>g__UseKeyframeAt|0(0f);
		return new Updater(delegate(float dt)
		{
			CS$<>8__locals1.currentSeconds += dt;
			if (CS$<>8__locals1.currentSeconds < CS$<>8__locals1.duration)
			{
				base.<GenericEase>g__UseKeyframeAt|0(CS$<>8__locals1.currentSeconds / CS$<>8__locals1.duration);
				return UpdaterResult.NotComplete;
			}
			base.<GenericEase>g__UseKeyframeAt|0(1f);
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x00065379 File Offset: 0x00063579
	public static Updater Do(System.Action fn)
	{
		return new Updater(delegate(float dt)
		{
			fn();
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x00065397 File Offset: 0x00063597
	public static Updater Do(Func<Updater> fn)
	{
		bool didInitalize = false;
		Updater target = default(Updater);
		return new Updater(delegate(float dt)
		{
			if (!didInitalize)
			{
				target = fn();
				didInitalize = true;
			}
			return target.Internal_Update(dt);
		});
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x000653C8 File Offset: 0x000635C8
	public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(Option.None, makeUpdaterFns);
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x000653DA File Offset: 0x000635DA
	public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(loopCount, makeUpdaterFns);
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x000653E8 File Offset: 0x000635E8
	public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
	{
		if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
		{
			return Updater.None();
		}
		int completedLoopCount = 0;
		int currentIndex = 0;
		Updater currentUpdater = makeUpdaterFns[currentIndex]();
		return new Updater(delegate(float dt)
		{
			if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				int num = currentIndex;
				currentIndex = num + 1;
				if (currentIndex >= makeUpdaterFns.Length)
				{
					currentIndex -= makeUpdaterFns.Length;
					num = completedLoopCount;
					completedLoopCount = num + 1;
					if (limitLoopCount.IsSome() && completedLoopCount >= limitLoopCount.Unwrap())
					{
						return UpdaterResult.Complete;
					}
				}
				currentUpdater = makeUpdaterFns[currentIndex]();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00065457 File Offset: 0x00063657
	public static Updater Parallel(params Updater[] updaters)
	{
		bool[] isCompleted = new bool[updaters.Length];
		return new Updater(delegate(float dt)
		{
			bool flag = false;
			for (int i = 0; i < updaters.Length; i++)
			{
				if (!isCompleted[i])
				{
					if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
					{
						isCompleted[i] = true;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x00065488 File Offset: 0x00063688
	public static Updater Series(params Updater[] updaters)
	{
		int i = 0;
		return new Updater(delegate(float dt)
		{
			int i;
			if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
			{
				i = i;
				i++;
			}
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x000654B0 File Offset: 0x000636B0
	public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
	{
		Updater.<>c__DisplayClass26_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass26_0();
		CS$<>8__locals1.coroutine = coroutine;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<RunRoutine>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000654E8 File Offset: 0x000636E8
	public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
	{
		return Updater.Run(monoBehaviour, Updater.Series(updaters));
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000654F8 File Offset: 0x000636F8
	public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
	{
		Updater.<>c__DisplayClass28_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass28_0();
		CS$<>8__locals1.updater = updater;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<Run>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x00065530 File Offset: 0x00063730
	public static float GetDeltaTime()
	{
		return Time.unscaledDeltaTime;
	}

	// Token: 0x04000A56 RID: 2646
	public readonly Func<float, UpdaterResult> fn;
}
