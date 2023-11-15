using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class BuffText : MonoBehaviour
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000B664 File Offset: 0x00009864
	public PoolObject poolObject
	{
		get
		{
			return this._poolObject;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000B66C File Offset: 0x0000986C
	// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000B679 File Offset: 0x00009879
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

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000B687 File Offset: 0x00009887
	// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000B694 File Offset: 0x00009894
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

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000B6A2 File Offset: 0x000098A2
	// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000B6AF File Offset: 0x000098AF
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

	// Token: 0x060002E7 RID: 743 RVA: 0x0000B6BD File Offset: 0x000098BD
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0000B6CA File Offset: 0x000098CA
	private void ResizeNameField()
	{
		this._background.size = this.ResizeDisplayField(Mathf.Clamp(this._text.preferredWidth + 1f, this._minSize, this._maxSize), this._text.preferredHeight);
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0000B70A File Offset: 0x0000990A
	private Vector2 ResizeDisplayField(float width, float height)
	{
		return new Vector2(width, height);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0000B713 File Offset: 0x00009913
	public BuffText Spawn()
	{
		return this._poolObject.Spawn(true).GetComponent<BuffText>();
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0000B726 File Offset: 0x00009926
	public void Despawn()
	{
		this._poolObject.Despawn();
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0000B733 File Offset: 0x00009933
	public void Despawn(float seconds)
	{
		base.StartCoroutine(this.CDespawn(seconds));
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0000B743 File Offset: 0x00009943
	private IEnumerator CDespawn(float seconds)
	{
		yield return Chronometer.global.WaitForSeconds(seconds);
		this.Despawn();
		yield break;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0000B759 File Offset: 0x00009959
	public void FadeOut(float duration)
	{
		base.StartCoroutine(this.CFadeOut(duration));
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000B769 File Offset: 0x00009969
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

	// Token: 0x060002F0 RID: 752 RVA: 0x0000B780 File Offset: 0x00009980
	private void SetFadeAlpha(float alpha)
	{
		Color color = this.color;
		color.a = alpha;
		this.color = color;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000B7A4 File Offset: 0x000099A4
	public void Initialize(string text, Vector3 position)
	{
		base.StopAllCoroutines();
		this._text.text = text;
		base.transform.position = position;
		base.transform.localScale = Vector3.one;
		base.gameObject.SetActive(true);
		this.ResizeNameField();
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000B7F1 File Offset: 0x000099F1
	public void Initialize(string text, Vector3 position, Color color)
	{
		this.Initialize(text, position);
		this._text.color = color;
		this.ResizeNameField();
	}

	// Token: 0x04000268 RID: 616
	private const float duration = 0.5f;

	// Token: 0x04000269 RID: 617
	private static readonly WaitForSeconds waitForDuration = new WaitForSeconds(0.5f);

	// Token: 0x0400026A RID: 618
	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	// Token: 0x0400026B RID: 619
	[SerializeField]
	private TextMeshPro _text;

	// Token: 0x0400026C RID: 620
	[SerializeField]
	private SpriteRenderer _background;

	// Token: 0x0400026D RID: 621
	[SerializeField]
	private float _minSize = 2.5f;

	// Token: 0x0400026E RID: 622
	[SerializeField]
	private float _maxSize = 10f;

	// Token: 0x0400026F RID: 623
	[SerializeField]
	private int _deltaSize = 20;
}
