using System;
using System.Collections;
using Scenes;
using TMPro;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class LineText : MonoBehaviour
{
	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000354 RID: 852 RVA: 0x0000C894 File Offset: 0x0000AA94
	// (set) Token: 0x06000355 RID: 853 RVA: 0x0000C89C File Offset: 0x0000AA9C
	public bool finished { get; private set; }

	// Token: 0x06000356 RID: 854 RVA: 0x0000C8A5 File Offset: 0x0000AAA5
	private void Awake()
	{
		this.Hide();
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
	public void Display(string text, float duration)
	{
		if (this._displayCoroutine != null)
		{
			this._displayCoroutine.GetValueOrDefault().Stop();
		}
		if (Scene<GameBase>.instance.uiManager.npcConversation.visible)
		{
			return;
		}
		this._displayCoroutine = new CoroutineReference?(this.StartCoroutineWithReference(this.CDisplay(text, duration)));
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0000C90B File Offset: 0x0000AB0B
	public IEnumerator CDisplay(string text, float duration)
	{
		if (Scene<GameBase>.instance.uiManager.npcConversation.visible)
		{
			yield break;
		}
		this.Show(text);
		yield return Chronometer.global.WaitForSeconds(duration);
		this.Hide();
		yield break;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0000C928 File Offset: 0x0000AB28
	private void Show(string text)
	{
		this.finished = false;
		this._text.text = text;
		this._speechBubble.size = this.ResizeDisplayField(Mathf.Clamp(this._minWidth, this._text.preferredWidth + 0.5f, this._maxWidth), Mathf.Max(this._minHeight, this._text.preferredHeight + 0.5f));
		this._text.gameObject.SetActive(true);
		this._speechBubble.gameObject.SetActive(true);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0000C9B9 File Offset: 0x0000ABB9
	public void Hide()
	{
		this.finished = true;
		this._text.text = "";
		this._text.gameObject.SetActive(false);
		this._speechBubble.gameObject.SetActive(false);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0000C8A5 File Offset: 0x0000AAA5
	private void OnDisable()
	{
		this.Hide();
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0000B70A File Offset: 0x0000990A
	private Vector2 ResizeDisplayField(float width, float height)
	{
		return new Vector2(width, height);
	}

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	private TextMeshPro _text;

	// Token: 0x040002B7 RID: 695
	[SerializeField]
	private SpriteRenderer _speechBubble;

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	private float _minWidth = 2f;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	private float _maxWidth = 40f;

	// Token: 0x040002BA RID: 698
	[SerializeField]
	private float _minHeight = 0.78125f;

	// Token: 0x040002BC RID: 700
	private CoroutineReference? _displayCoroutine;
}
