using System;
using System.Collections;
using FX;
using UnityEngine;

// Token: 0x02000068 RID: 104
public sealed class ReusableChronoSpriteEffect : MonoBehaviour, IUseChronometer, IDelayable, IPoolObjectCopiable<ReusableChronoSpriteEffect>
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060001DE RID: 478 RVA: 0x00008719 File Offset: 0x00006919
	public PoolObject reusable
	{
		get
		{
			return this._reusable;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060001DF RID: 479 RVA: 0x00008721 File Offset: 0x00006921
	public SpriteRenderer renderer
	{
		get
		{
			return this._renderer;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x00008729 File Offset: 0x00006929
	public Animator animator
	{
		get
		{
			return this._animator;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x00008731 File Offset: 0x00006931
	// (set) Token: 0x060001E2 RID: 482 RVA: 0x00008739 File Offset: 0x00006939
	public ChronometerBase chronometer { get; set; }

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060001E3 RID: 483 RVA: 0x00008742 File Offset: 0x00006942
	// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000874A File Offset: 0x0000694A
	public float delay { get; set; }

	// Token: 0x17000049 RID: 73
	// (set) Token: 0x060001E5 RID: 485 RVA: 0x00008753 File Offset: 0x00006953
	public int hue
	{
		set
		{
			this.renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetInt(EffectInfo.huePropertyID, value);
			this.renderer.SetPropertyBlock(this._propertyBlock);
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00008788 File Offset: 0x00006988
	private void Awake()
	{
		this._propertyBlock = new MaterialPropertyBlock();
		this._reusable.onDespawn += this.OnDespawn;
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x000087AC File Offset: 0x000069AC
	private void OnDespawn()
	{
		this._cPlayReference.Stop();
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x000087BC File Offset: 0x000069BC
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

	// Token: 0x060001E9 RID: 489 RVA: 0x000088FD File Offset: 0x00006AFD
	public void Play(float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		this._cPlayReference = CoroutineProxy.instance.StartCoroutineWithReference(this.CPlay(delay, duration, loop, fadeOutCurve, fadeOutDuration));
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000891C File Offset: 0x00006B1C
	private IEnumerator CPlay(float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		float remain;
		float num;
		for (remain = delay; remain > 1E-45f; remain -= num)
		{
			this._renderer.enabled = false;
			yield return null;
			num = this.chronometer.DeltaTime();
		}
		this._renderer.enabled = true;
		if (this._animator.runtimeAnimatorController != null)
		{
			if (!this._animator.enabled)
			{
				this._animator.enabled = true;
			}
			this._animator.Play(0, 0, 0f);
		}
		if (loop)
		{
			yield break;
		}
		if (duration == 0f)
		{
			duration = this._animator.GetCurrentAnimatorStateInfo(0).length;
		}
		duration -= fadeOutDuration;
		if (duration <= 0f)
		{
			Debug.LogError("Duration - Fade out duration이 0이하입니다.");
			this._cPlayReference.Clear();
			this._reusable.Despawn();
			yield break;
		}
		remain += duration;
		this._animator.enabled = false;
		while (remain > 1E-45f)
		{
			yield return null;
			float num2 = this.chronometer.DeltaTime();
			this._animator.Update(num2);
			remain -= num2;
		}
		if (fadeOutDuration > 0f)
		{
			yield return this.CFadeOut(-remain, fadeOutDuration, fadeOutCurve);
		}
		this._animator.enabled = true;
		this._cPlayReference.Clear();
		this._reusable.Despawn();
		yield break;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00008950 File Offset: 0x00006B50
	private IEnumerator CFadeOut(float time, float duration, AnimationCurve fadeOutCurve)
	{
		Color color = this._renderer.color;
		float alpha = color.a;
		while (time < duration)
		{
			yield return null;
			float num = this.chronometer.DeltaTime();
			time += num;
			this._animator.Update(num);
			color.a = alpha * (1f - fadeOutCurve.Evaluate(time / duration));
			this._renderer.color = color;
		}
		yield break;
	}

	// Token: 0x0400019D RID: 413
	[SerializeField]
	private PoolObject _reusable;

	// Token: 0x0400019E RID: 414
	[GetComponent]
	[SerializeField]
	private SpriteRenderer _renderer;

	// Token: 0x0400019F RID: 415
	[GetComponent]
	[SerializeField]
	private Animator _animator;

	// Token: 0x040001A0 RID: 416
	private MaterialPropertyBlock _propertyBlock;

	// Token: 0x040001A1 RID: 417
	private CoroutineReference _cPlayReference;
}
