using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class LTSeq
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000205 RID: 517 RVA: 0x0000DD0A File Offset: 0x0000BF0A
	public int id
	{
		get
		{
			return (int)(this._id | this.counter << 16);
		}
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000DD1C File Offset: 0x0000BF1C
	public void reset()
	{
		this.previous = null;
		this.tween = null;
		this.totalDelay = 0f;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000DD37 File Offset: 0x0000BF37
	public void init(uint id, uint global_counter)
	{
		this.reset();
		this._id = id;
		this.counter = global_counter;
		this.current = this;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000DD54 File Offset: 0x0000BF54
	private LTSeq addOn()
	{
		this.current.toggle = true;
		LTSeq ltseq = this.current;
		this.current = LeanTween.sequence(true);
		this.current.previous = ltseq;
		ltseq.toggle = false;
		this.current.totalDelay = ltseq.totalDelay;
		this.current.debugIter = ltseq.debugIter + 1;
		return this.current;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
	private float addPreviousDelays()
	{
		LTSeq ltseq = this.current.previous;
		if (ltseq != null && ltseq.tween != null)
		{
			return this.current.totalDelay + ltseq.tween.time;
		}
		return this.current.totalDelay;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000DE07 File Offset: 0x0000C007
	public LTSeq append(float delay)
	{
		this.current.totalDelay += delay;
		return this.current;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000DE24 File Offset: 0x0000C024
	public LTSeq append(Action callback)
	{
		LTDescr ltdescr = LeanTween.delayedCall(0f, callback);
		return this.append(ltdescr);
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000DE44 File Offset: 0x0000C044
	public LTSeq append(Action<object> callback, object obj)
	{
		this.append(LeanTween.delayedCall(0f, callback).setOnCompleteParam(obj));
		return this.addOn();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000DE64 File Offset: 0x0000C064
	public LTSeq append(GameObject gameObject, Action callback)
	{
		this.append(LeanTween.delayedCall(gameObject, 0f, callback));
		return this.addOn();
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000DE7F File Offset: 0x0000C07F
	public LTSeq append(GameObject gameObject, Action<object> callback, object obj)
	{
		this.append(LeanTween.delayedCall(gameObject, 0f, callback).setOnCompleteParam(obj));
		return this.addOn();
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000DEA0 File Offset: 0x0000C0A0
	public LTSeq append(LTDescr tween)
	{
		this.current.tween = tween;
		this.current.totalDelay = this.addPreviousDelays();
		tween.setDelay(this.current.totalDelay);
		return this.addOn();
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000DED7 File Offset: 0x0000C0D7
	public LTSeq insert(LTDescr tween)
	{
		this.current.tween = tween;
		tween.setDelay(this.addPreviousDelays());
		return this.addOn();
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
	public LTSeq setScale(float timeScale)
	{
		this.setScaleRecursive(this.current, timeScale, 500);
		return this.addOn();
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000DF14 File Offset: 0x0000C114
	private void setScaleRecursive(LTSeq seq, float timeScale, int count)
	{
		if (count > 0)
		{
			this.timeScale = timeScale;
			seq.totalDelay *= timeScale;
			if (seq.tween != null)
			{
				if (seq.tween.time != 0f)
				{
					seq.tween.setTime(seq.tween.time * timeScale);
				}
				seq.tween.setDelay(seq.tween.delay * timeScale);
			}
			if (seq.previous != null)
			{
				this.setScaleRecursive(seq.previous, timeScale, count - 1);
			}
		}
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000DF9E File Offset: 0x0000C19E
	public LTSeq reverse()
	{
		return this.addOn();
	}

	// Token: 0x040000E7 RID: 231
	public LTSeq previous;

	// Token: 0x040000E8 RID: 232
	public LTSeq current;

	// Token: 0x040000E9 RID: 233
	public LTDescr tween;

	// Token: 0x040000EA RID: 234
	public float totalDelay;

	// Token: 0x040000EB RID: 235
	public float timeScale;

	// Token: 0x040000EC RID: 236
	private int debugIter;

	// Token: 0x040000ED RID: 237
	public uint counter;

	// Token: 0x040000EE RID: 238
	public bool toggle;

	// Token: 0x040000EF RID: 239
	private uint _id;
}
