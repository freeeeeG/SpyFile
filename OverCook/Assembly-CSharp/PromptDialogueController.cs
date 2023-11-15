using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000369 RID: 873
public class PromptDialogueController : UIControllerBase
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060010B3 RID: 4275 RVA: 0x0005FFAC File Offset: 0x0005E3AC
	// (remove) Token: 0x060010B4 RID: 4276 RVA: 0x0005FFE4 File Offset: 0x0005E3E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<bool> ResultCallback = delegate(bool _result)
	{
	};

	// Token: 0x060010B5 RID: 4277 RVA: 0x0006001A File Offset: 0x0005E41A
	private void Awake()
	{
		if (this.m_autoBegin)
		{
			this.Begin(this.m_titleText, this.m_explanationText);
		}
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x00060039 File Offset: 0x0005E439
	public void Begin(string _title, string _explanation)
	{
		this.m_titleObject.text = _title;
		this.m_explanationTextObject.text = _explanation;
		this.m_iterator = this.Run(this.m_frontendGUI);
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x00060065 File Offset: 0x0005E465
	private void Update()
	{
		if (this.m_iterator != null && !this.m_iterator.MoveNext())
		{
			this.m_iterator = null;
		}
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0006008C File Offset: 0x0005E48C
	public IEnumerator Run(FrontendGUI _ui)
	{
		ScrollingListControlsHelper scrollingControlsHelper = new ScrollingListControlsHelper();
		scrollingControlsHelper.Init(_ui, 0f);
		bool finished = false;
		scrollingControlsHelper.RegisterSelectionCallback(delegate(int _s)
		{
			finished = true;
		});
		while (!finished)
		{
			scrollingControlsHelper.Update();
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000CDC RID: 3292
	[SerializeField]
	[AssignChild("Title", Editorbility.NonEditable)]
	private Text m_titleObject;

	// Token: 0x04000CDD RID: 3293
	[SerializeField]
	[AssignChild("ExplainationText", Editorbility.NonEditable)]
	private Text m_explanationTextObject;

	// Token: 0x04000CDE RID: 3294
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	private FrontendGUI m_frontendGUI;

	// Token: 0x04000CDF RID: 3295
	[SerializeField]
	private bool m_autoBegin;

	// Token: 0x04000CE0 RID: 3296
	[SerializeField]
	[HideInInspectorTest("m_autoBegin", true)]
	private string m_titleText;

	// Token: 0x04000CE1 RID: 3297
	[SerializeField]
	[HideInInspectorTest("m_autoBegin", true)]
	private string m_explanationText;

	// Token: 0x04000CE3 RID: 3299
	private IEnumerator m_iterator;
}
