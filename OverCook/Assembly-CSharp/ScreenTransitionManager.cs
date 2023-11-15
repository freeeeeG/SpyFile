using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AFD RID: 2813
public class ScreenTransitionManager : Manager
{
	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060038DE RID: 14558 RVA: 0x0010C905 File Offset: 0x0010AD05
	public bool IsIdle
	{
		get
		{
			return this.m_UpDownTransitionInfos.Count == 0 && ScreenTransitionManager.TransitionState.eDown == this.m_CurrentState;
		}
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x0010C923 File Offset: 0x0010AD23
	private ScreenTransitionManager.UpDownTransition GetLastTransition()
	{
		if (this.m_UpDownTransitionInfos.Count != 0)
		{
			return this.m_UpDownTransitionInfos[this.m_UpDownTransitionInfos.Count - 1];
		}
		return null;
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x0010C94F File Offset: 0x0010AD4F
	private ScreenTransitionManager.UpDownTransition GetCurrentTransition()
	{
		if (this.m_UpDownTransitionInfos.Count != 0)
		{
			return this.m_UpDownTransitionInfos[0];
		}
		return null;
	}

	// Token: 0x060038E1 RID: 14561 RVA: 0x0010C970 File Offset: 0x0010AD70
	private void Awake()
	{
		if (this.m_TransitionImage == null)
		{
			this.m_TransitionImage = base.gameObject.RequireComponentRecursive<T17Image>();
		}
		this.m_TransitionMaterial = this.m_TransitionImage.material;
		this.m_TransitionImage.gameObject.SetActive(false);
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x0010C9C4 File Offset: 0x0010ADC4
	public void Update()
	{
		ScreenTransitionManager.UpDownTransition currentTransition = this.GetCurrentTransition();
		if (currentTransition != null && !currentTransition.enumerator.MoveNext())
		{
			this.m_UpDownTransitionInfos.RemoveAt(0);
		}
		if (this.m_TransitionLoads.Count > 0 && !this.m_TransitionLoads.Peek().MoveNext())
		{
			this.m_TransitionLoads.Dequeue();
		}
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x0010CA2C File Offset: 0x0010AE2C
	public bool StartTransitionUp(CallbackVoid OnTransitionUp = null)
	{
		ScreenTransitionManager.UpDownTransition lastTransition = this.GetLastTransition();
		ScreenTransitionManager.UpDownTransition currentTransition = this.GetCurrentTransition();
		if (this.m_CurrentState != ScreenTransitionManager.TransitionState.eTransitionDown)
		{
			if (this.m_CurrentState == ScreenTransitionManager.TransitionState.eUp)
			{
				if (OnTransitionUp != null)
				{
					OnTransitionUp();
				}
				return true;
			}
		}
		if (lastTransition != null && lastTransition.state == ScreenTransitionManager.TransitionState.eUp)
		{
			ScreenTransitionManager.UpDownTransition upDownTransition = lastTransition;
			upDownTransition.callback = (CallbackVoid)Delegate.Combine(upDownTransition.callback, OnTransitionUp);
			return true;
		}
		this.m_UpDownTransitionInfos.Add(new ScreenTransitionManager.UpDownTransition(ScreenTransitionManager.TransitionState.eUp, this.TransitionUp(), OnTransitionUp));
		return true;
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x0010CAB8 File Offset: 0x0010AEB8
	public bool StartTransitionDown(CallbackVoid OnTransitionDown = null)
	{
		ScreenTransitionManager.UpDownTransition lastTransition = this.GetLastTransition();
		ScreenTransitionManager.UpDownTransition currentTransition = this.GetCurrentTransition();
		if (this.m_CurrentState != ScreenTransitionManager.TransitionState.eTransitionUp)
		{
			if (this.m_CurrentState == ScreenTransitionManager.TransitionState.eDown)
			{
				if (OnTransitionDown != null)
				{
					OnTransitionDown();
				}
				return true;
			}
		}
		if (lastTransition != null && lastTransition.state == ScreenTransitionManager.TransitionState.eDown)
		{
			ScreenTransitionManager.UpDownTransition upDownTransition = lastTransition;
			upDownTransition.callback = (CallbackVoid)Delegate.Combine(upDownTransition.callback, OnTransitionDown);
			return true;
		}
		this.m_UpDownTransitionInfos.Add(new ScreenTransitionManager.UpDownTransition(ScreenTransitionManager.TransitionState.eDown, this.TransitionDown(), OnTransitionDown));
		return true;
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0010CB40 File Offset: 0x0010AF40
	private IEnumerator TransitionUp()
	{
		this.m_CurrentState = ScreenTransitionManager.TransitionState.eTransitionUp;
		FastList<User> userList = ClientUserSystem.m_Users;
		for (int i = 0; i < userList.Count; i++)
		{
			User user = userList._items[i];
			if (user.Engagement == EngagementSlot.One)
			{
				if (user != null && user.GamepadUser != null)
				{
					T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user.GamepadUser);
					if (eventSystemForGamepadUser != null && this.m_eventSystemSuppressor == null)
					{
						this.m_eventSystemSuppressor = eventSystemForGamepadUser.Disable(this);
					}
				}
				break;
			}
		}
		this.m_TransitionImage.gameObject.SetActive(true);
		this.m_TransitionMaterial.SetColor(ScreenTransitionManager.Uniforms._FillColour, this.m_FillColour);
		this.m_TransitionMaterial.SetTexture(ScreenTransitionManager.Uniforms._MaskTexture, this.PickRandomMaskTexture());
		this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._Cutoff, this.m_MaskCutoff);
		this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._MaxScale, this.m_MaxScaleDown);
		this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._Fade, 0f);
		yield return null;
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIScreenOut, base.gameObject.layer);
		float fCurrentFade = 0f;
		float fTime = 0f;
		while (fTime <= 1f)
		{
			fTime += Time.deltaTime;
			fCurrentFade = this.m_TransitionCurve.Evaluate(fTime) * this.m_MaxScaleDown;
			this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._Fade, fCurrentFade);
			yield return null;
		}
		this.m_CurrentState = ScreenTransitionManager.TransitionState.eUp;
		ScreenTransitionManager.UpDownTransition currentTransition = this.GetCurrentTransition();
		if (currentTransition.callback != null)
		{
			currentTransition.callback();
		}
		yield break;
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x0010CB5C File Offset: 0x0010AF5C
	private IEnumerator TransitionDown()
	{
		this.m_CurrentState = ScreenTransitionManager.TransitionState.eTransitionDown;
		FastList<User> userList = ClientUserSystem.m_Users;
		this.m_TransitionImage.gameObject.SetActive(true);
		this.m_TransitionMaterial.SetColor(ScreenTransitionManager.Uniforms._FillColour, this.m_FillColour);
		this.m_TransitionMaterial.SetTexture(ScreenTransitionManager.Uniforms._MaskTexture, this.PickRandomMaskTexture());
		this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._Cutoff, this.m_MaskCutoff);
		this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._MaxScale, this.m_MaxScaleDown);
		this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._Fade, this.m_MaxScaleDown);
		yield return null;
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIScreenIn, base.gameObject.layer);
		float fCurrentFade = this.m_MaxScaleDown;
		float fTime = 0f;
		while (fTime <= 1f)
		{
			fTime += Time.deltaTime;
			fCurrentFade = this.m_TransitionCurve.Evaluate(1f - fTime) * this.m_MaxScaleDown;
			this.m_TransitionMaterial.SetFloat(ScreenTransitionManager.Uniforms._Fade, fCurrentFade);
			yield return null;
		}
		this.m_CurrentState = ScreenTransitionManager.TransitionState.eDown;
		if (this.m_eventSystemSuppressor != null)
		{
			this.m_eventSystemSuppressor.Release();
			this.m_eventSystemSuppressor = null;
		}
		ScreenTransitionManager.UpDownTransition currentTransition = this.GetCurrentTransition();
		if (currentTransition.callback != null)
		{
			currentTransition.callback();
		}
		yield break;
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x0010CB78 File Offset: 0x0010AF78
	private Texture PickRandomMaskTexture()
	{
		int index = UnityEngine.Random.Range(0, this.m_MaskTextures.Count);
		return this.m_MaskTextures[index];
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x0010CBA3 File Offset: 0x0010AFA3
	public void TransitionLoad(string _sceneName)
	{
		this.m_TransitionLoads.Enqueue(this.TransitionLoadRoutine(_sceneName));
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x0010CBB8 File Offset: 0x0010AFB8
	private IEnumerator TransitionLoadRoutine(string _sceneName)
	{
		SpinnerIconManager spinnerMan = SpinnerIconManager.Instance;
		Suppressor suppressor = null;
		if (spinnerMan != null)
		{
			suppressor = spinnerMan.Show(SpinnerIconManager.SpinnerIconType.Load, this, true);
		}
		AssetBundleLoadLevelOperationBase loadOp = null;
		AsyncOperation async = null;
		this.StartTransitionUp(delegate
		{
			loadOp = AssetBundleManager.LoadLevelAsync(_sceneName.ToLowerInvariant(), _sceneName, false);
			loadOp.OnAsyncOperationStarted = delegate(AsyncOperation op)
			{
				async = op;
			};
		});
		while (loadOp == null || loadOp.MoveNext())
		{
			yield return null;
		}
		while (async == null || !async.isDone)
		{
			yield return null;
		}
		if (suppressor != null)
		{
			suppressor.Release();
		}
		bool bDown = false;
		this.StartTransitionDown(delegate
		{
			bDown = true;
		});
		while (!bDown)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002D9B RID: 11675
	[SerializeField]
	private T17Image m_TransitionImage;

	// Token: 0x04002D9C RID: 11676
	[SerializeField]
	private float m_MaskCutoff = 0.4f;

	// Token: 0x04002D9D RID: 11677
	[SerializeField]
	private float m_MaxScaleDown = 50f;

	// Token: 0x04002D9E RID: 11678
	[SerializeField]
	private Color m_FillColour = Color.black;

	// Token: 0x04002D9F RID: 11679
	[SerializeField]
	private List<Texture2D> m_MaskTextures = new List<Texture2D>();

	// Token: 0x04002DA0 RID: 11680
	[SerializeField]
	private AnimationCurve m_TransitionCurve;

	// Token: 0x04002DA1 RID: 11681
	private Material m_TransitionMaterial;

	// Token: 0x04002DA2 RID: 11682
	private Suppressor m_eventSystemSuppressor;

	// Token: 0x04002DA3 RID: 11683
	private Queue<IEnumerator> m_TransitionLoads = new Queue<IEnumerator>();

	// Token: 0x04002DA4 RID: 11684
	private List<ScreenTransitionManager.UpDownTransition> m_UpDownTransitionInfos = new List<ScreenTransitionManager.UpDownTransition>();

	// Token: 0x04002DA5 RID: 11685
	private ScreenTransitionManager.TransitionState m_CurrentState;

	// Token: 0x02000AFE RID: 2814
	private class UpDownTransition
	{
		// Token: 0x060038EA RID: 14570 RVA: 0x0010CBDA File Offset: 0x0010AFDA
		public UpDownTransition(ScreenTransitionManager.TransitionState _state, IEnumerator _enumerator, CallbackVoid _callback)
		{
			this.state = _state;
			this.enumerator = _enumerator;
			if (_callback != null)
			{
				this.callback = (CallbackVoid)Delegate.Combine(this.callback, _callback);
			}
		}

		// Token: 0x04002DA6 RID: 11686
		public ScreenTransitionManager.TransitionState state;

		// Token: 0x04002DA7 RID: 11687
		public IEnumerator enumerator;

		// Token: 0x04002DA8 RID: 11688
		public CallbackVoid callback;
	}

	// Token: 0x02000AFF RID: 2815
	private static class Uniforms
	{
		// Token: 0x04002DA9 RID: 11689
		internal static readonly int _FillColour = Shader.PropertyToID("_FillColour");

		// Token: 0x04002DAA RID: 11690
		internal static readonly int _MaskTexture = Shader.PropertyToID("_MaskTexture");

		// Token: 0x04002DAB RID: 11691
		internal static readonly int _Fade = Shader.PropertyToID("_Fade");

		// Token: 0x04002DAC RID: 11692
		internal static readonly int _Cutoff = Shader.PropertyToID("_Cutoff");

		// Token: 0x04002DAD RID: 11693
		internal static readonly int _MaxScale = Shader.PropertyToID("_MaxScale");
	}

	// Token: 0x02000B00 RID: 2816
	private enum TransitionState
	{
		// Token: 0x04002DAF RID: 11695
		eDown,
		// Token: 0x04002DB0 RID: 11696
		eUp,
		// Token: 0x04002DB1 RID: 11697
		eTransitionUp,
		// Token: 0x04002DB2 RID: 11698
		eTransitionDown
	}
}
