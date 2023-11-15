using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class LineTextManager : MonoBehaviour
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000364 RID: 868 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
	public HashSet<GameObject> locked
	{
		get
		{
			return this._locked;
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
	private void Awake()
	{
		this._locked = new HashSet<GameObject>();
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
	public LineText Spawn(string text, Vector3 position, float duration)
	{
		LineText lineText;
		if ((this._lineTexts.Count > 0 && this._lineTexts.Peek().finished) || this._lineTexts.Count > 20)
		{
			lineText = this._lineTexts.Dequeue();
			while (lineText == null && this._lineTexts.Count > 0)
			{
				lineText = this._lineTexts.Dequeue();
			}
			if (lineText == null)
			{
				lineText = UnityEngine.Object.Instantiate<LineText>(this._floatingTextPrefab);
			}
		}
		else
		{
			lineText = UnityEngine.Object.Instantiate<LineText>(this._floatingTextPrefab);
		}
		lineText.transform.position = position;
		lineText.Display(text, duration);
		this._lineTexts.Enqueue(lineText);
		return lineText;
	}

	// Token: 0x040002C2 RID: 706
	[SerializeField]
	private LineText _floatingTextPrefab;

	// Token: 0x040002C3 RID: 707
	private Queue<LineText> _lineTexts = new Queue<LineText>(20);

	// Token: 0x040002C4 RID: 708
	private const int _maxFloats = 20;

	// Token: 0x040002C5 RID: 709
	private HashSet<GameObject> _locked;
}
