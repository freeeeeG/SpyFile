using System;
using FX;
using UnityEngine;

// Token: 0x02000032 RID: 50
public sealed class EffectPoolInstance : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060000BB RID: 187 RVA: 0x0000466C File Offset: 0x0000286C
	// (remove) Token: 0x060000BC RID: 188 RVA: 0x000046A4 File Offset: 0x000028A4
	public event Action OnStop;

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000BD RID: 189 RVA: 0x000046D9 File Offset: 0x000028D9
	// (set) Token: 0x060000BE RID: 190 RVA: 0x000046E1 File Offset: 0x000028E1
	public EffectPoolInstance.State state { get; private set; }

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000BF RID: 191 RVA: 0x000046EA File Offset: 0x000028EA
	public SpriteRenderer renderer
	{
		get
		{
			return this._renderer;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000C0 RID: 192 RVA: 0x000046F2 File Offset: 0x000028F2
	public Animator animator
	{
		get
		{
			return this._animator;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000C1 RID: 193 RVA: 0x000046FA File Offset: 0x000028FA
	// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004702 File Offset: 0x00002902
	public ChronometerBase chronometer { get; set; }

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000470B File Offset: 0x0000290B
	// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004713 File Offset: 0x00002913
	public float delay { get; set; }

	// Token: 0x1700001A RID: 26
	// (set) Token: 0x060000C5 RID: 197 RVA: 0x0000471C File Offset: 0x0000291C
	public int hue
	{
		set
		{
			this.renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetInt(EffectInfo.huePropertyID, value);
			this.renderer.SetPropertyBlock(this._propertyBlock);
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00004751 File Offset: 0x00002951
	private void Awake()
	{
		this._propertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000475E File Offset: 0x0000295E
	private void OnDestroy()
	{
		this._renderer.sprite = null;
		this._renderer = null;
		this._animator.runtimeAnimatorController = null;
		this._animator = null;
		this.OnStop = null;
		this.state = EffectPoolInstance.State.Destroyed;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00004794 File Offset: 0x00002994
	public void Copy(ReusableChronoSpriteEffect to)
	{
		to.animator.runtimeAnimatorController = this.animator.runtimeAnimatorController;
		to.animator.speed = this.animator.speed;
		to.animator.updateMode = this.animator.updateMode;
		to.animator.cullingMode = this.animator.cullingMode;
		to.renderer.sprite = this.renderer.sprite;
		to.renderer.color = this.renderer.color;
		to.renderer.flipX = this.renderer.flipX;
		to.renderer.flipY = this.renderer.flipY;
		to.renderer.drawMode = this.renderer.drawMode;
		to.renderer.sortingLayerID = this.renderer.sortingLayerID;
		to.renderer.sortingOrder = this.renderer.sortingOrder;
		to.renderer.maskInteraction = this.renderer.maskInteraction;
		to.renderer.spriteSortPoint = this.renderer.spriteSortPoint;
		to.renderer.renderingLayerMask = this.renderer.renderingLayerMask;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x000048D8 File Offset: 0x00002AD8
	public void Play(RuntimeAnimatorController animation, float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		if (animation == null)
		{
			Debug.LogError("Animation of effect is null!");
			this.Stop();
			return;
		}
		if (fadeOutCurve == null)
		{
			fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}
		this._animator.runtimeAnimatorController = animation;
		this._delay = delay;
		if (loop)
		{
			this._duration = float.PositiveInfinity;
		}
		else
		{
			this._duration = duration;
		}
		this._fadingTime = 0f;
		this._fadingCurve = fadeOutCurve;
		this._fadingDuration = fadeOutDuration;
		if (this._delay > 0f)
		{
			this.state = EffectPoolInstance.State.Delaying;
			return;
		}
		this.StartPlay();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00004980 File Offset: 0x00002B80
	public void UpdateEffect()
	{
		float deltaTime = this.chronometer.DeltaTime();
		switch (this.state)
		{
		case EffectPoolInstance.State.Delaying:
			this.ProcessDelay(deltaTime);
			return;
		case EffectPoolInstance.State.Playing:
			this.ProcessPlay(deltaTime);
			return;
		case EffectPoolInstance.State.Fading:
			this.ProcessFade(deltaTime);
			return;
		default:
			return;
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000049CC File Offset: 0x00002BCC
	private void ProcessDelay(float deltaTime)
	{
		this._renderer.enabled = false;
		this._delay -= deltaTime;
		if (this._delay > 0f)
		{
			return;
		}
		this.StartPlay();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000049FC File Offset: 0x00002BFC
	private void StartPlay()
	{
		if (this._renderer == null)
		{
			Debug.LogError("Renderer is null!");
			this.Stop();
			return;
		}
		if (this._animator == null)
		{
			Debug.LogError("Animator is null!");
			this.Stop();
			return;
		}
		this.state = EffectPoolInstance.State.Playing;
		if (this._animator.runtimeAnimatorController != null)
		{
			if (!this._animator.enabled)
			{
				this._animator.enabled = true;
			}
			this._animator.Play(0, 0, 0f);
			this._animator.Update(0f);
			if (this._duration == 0f)
			{
				this._duration = this._animator.GetCurrentAnimatorStateInfo(0).length;
			}
		}
		this._duration -= this._fadingDuration;
		this._animator.enabled = false;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00004AE4 File Offset: 0x00002CE4
	private void ProcessPlay(float deltaTime)
	{
		this._duration -= deltaTime;
		this._animator.Update(deltaTime);
		if (this._duration > 0f)
		{
			return;
		}
		if (this._fadingDuration > 0f)
		{
			this._initialFadeAlpha = this._renderer.color.a;
			this.state = EffectPoolInstance.State.Fading;
			return;
		}
		this.Stop();
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00004B4C File Offset: 0x00002D4C
	private void ProcessFade(float deltaTime)
	{
		if (this._fadingTime > this._fadingDuration)
		{
			this.Stop();
			return;
		}
		this._fadingTime += deltaTime;
		this._animator.Update(deltaTime);
		Color color = this._renderer.color;
		color.a = this._initialFadeAlpha * (1f - this._fadingCurve.Evaluate(this._fadingTime / this._fadingDuration));
		this._renderer.color = color;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00004BCC File Offset: 0x00002DCC
	public void Stop()
	{
		if (base.transform.parent != null)
		{
			base.transform.SetParent(null, false);
		}
		this._renderer.enabled = false;
		this._animator.enabled = false;
		this._renderer.sprite = null;
		this._animator.runtimeAnimatorController = null;
		this.state = EffectPoolInstance.State.Stopped;
		Action onStop = this.OnStop;
		if (onStop == null)
		{
			return;
		}
		onStop();
	}

	// Token: 0x040000A5 RID: 165
	[GetComponent]
	[SerializeField]
	private SpriteRenderer _renderer;

	// Token: 0x040000A6 RID: 166
	[GetComponent]
	[SerializeField]
	private Animator _animator;

	// Token: 0x040000A7 RID: 167
	private MaterialPropertyBlock _propertyBlock;

	// Token: 0x040000A8 RID: 168
	private float _delay;

	// Token: 0x040000A9 RID: 169
	private float _duration;

	// Token: 0x040000AA RID: 170
	private float _fadingTime;

	// Token: 0x040000AB RID: 171
	private float _fadingDuration;

	// Token: 0x040000AC RID: 172
	private AnimationCurve _fadingCurve;

	// Token: 0x040000AD RID: 173
	private float _initialFadeAlpha;

	// Token: 0x02000033 RID: 51
	public enum State
	{
		// Token: 0x040000B3 RID: 179
		Stopped,
		// Token: 0x040000B4 RID: 180
		Delaying,
		// Token: 0x040000B5 RID: 181
		Playing,
		// Token: 0x040000B6 RID: 182
		Fading,
		// Token: 0x040000B7 RID: 183
		Destroyed
	}
}
