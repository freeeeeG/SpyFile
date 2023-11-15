using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class FloatingText : MonoBehaviour
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000305 RID: 773 RVA: 0x0000BA79 File Offset: 0x00009C79
	public PoolObject poolObject
	{
		get
		{
			return this._poolObject;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000306 RID: 774 RVA: 0x0000BA81 File Offset: 0x00009C81
	// (set) Token: 0x06000307 RID: 775 RVA: 0x0000BA8E File Offset: 0x00009C8E
	public Color color
	{
		get
		{
			return this._text.color;
		}
		set
		{
			this._text.color = value;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000308 RID: 776 RVA: 0x0000BA9C File Offset: 0x00009C9C
	// (set) Token: 0x06000309 RID: 777 RVA: 0x0000BAA9 File Offset: 0x00009CA9
	public string text
	{
		get
		{
			return this._text.text;
		}
		set
		{
			this._text.text = value;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600030A RID: 778 RVA: 0x0000BAB7 File Offset: 0x00009CB7
	// (set) Token: 0x0600030B RID: 779 RVA: 0x0000BAC4 File Offset: 0x00009CC4
	public int sortingOrder
	{
		get
		{
			return this._text.sortingOrder;
		}
		set
		{
			this._text.sortingOrder = value;
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000B6BD File Offset: 0x000098BD
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000BAD2 File Offset: 0x00009CD2
	public void SetGradientColor(Color a, Color b)
	{
		this._text.color = Color.white;
		this._text.colorGradient = new VertexGradient(a, a, b, b);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000BAF8 File Offset: 0x00009CF8
	public FloatingText Spawn()
	{
		return this._poolObject.Spawn(true).GetComponent<FloatingText>();
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0000BB0B File Offset: 0x00009D0B
	public void Despawn()
	{
		this._poolObject.Despawn();
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0000BB18 File Offset: 0x00009D18
	public void Despawn(float seconds)
	{
		base.StartCoroutine(this.CDespawn(seconds));
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000BB28 File Offset: 0x00009D28
	private IEnumerator CDespawn(float seconds)
	{
		yield return Chronometer.global.WaitForSeconds(seconds);
		this.Despawn();
		yield break;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0000BB3E File Offset: 0x00009D3E
	public void FadeOut(float duration)
	{
		base.StartCoroutine(this.CFadeOut(duration));
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0000BB4E File Offset: 0x00009D4E
	private IEnumerator CFadeOut(float duration)
	{
		float t = duration;
		this.SetFadeAlpha(1f);
		yield return null;
		while (t > 0f)
		{
			this.SetFadeAlpha(t / duration);
			yield return null;
			t -= Chronometer.global.deltaTime;
		}
		this.SetFadeAlpha(0f);
		yield break;
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0000BB64 File Offset: 0x00009D64
	private void SetFadeAlpha(float alpha)
	{
		Color color = this.color;
		color.a = alpha;
		this.color = color;
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0000BB88 File Offset: 0x00009D88
	public void Initialize(string text, Vector3 position)
	{
		base.StopAllCoroutines();
		this._text.text = text;
		base.transform.position = position;
		base.transform.localScale = Vector3.one;
		this._text.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0000BBF3 File Offset: 0x00009DF3
	public void Initialize(string text, Vector3 position, Color color)
	{
		this.Initialize(text, position);
		this._text.color = color;
	}

	// Token: 0x0400027D RID: 637
	private const float duration = 0.5f;

	// Token: 0x0400027E RID: 638
	private static readonly WaitForSeconds waitForDuration = new WaitForSeconds(0.5f);

	// Token: 0x0400027F RID: 639
	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	// Token: 0x04000280 RID: 640
	[SerializeField]
	private TextMeshPro _text;
}
