using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000364 RID: 868
public class ConfirmDialogueController : UIControllerBase
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06001097 RID: 4247 RVA: 0x0005F978 File Offset: 0x0005DD78
	// (remove) Token: 0x06001098 RID: 4248 RVA: 0x0005F9B0 File Offset: 0x0005DDB0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<bool> ResultCallback = delegate(bool _result)
	{
	};

	// Token: 0x06001099 RID: 4249 RVA: 0x0005F9E6 File Offset: 0x0005DDE6
	private void Awake()
	{
		if (this.m_autoBegin)
		{
			this.Begin(this.m_titleText, this.m_explanationText, this.m_pveOptionText, this.m_nveOptionText);
		}
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0005FA11 File Offset: 0x0005DE11
	public void Begin(string _title, string _explanation, string _pveOption, string _nveOption)
	{
		this.m_titleObject.text = _title;
		this.m_explanationTextObject.text = _explanation;
		this.m_iterator = this.Run(this.m_frontendGUI, _pveOption, _nveOption);
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0005FA40 File Offset: 0x0005DE40
	private void Update()
	{
		if (this.m_iterator != null && !this.m_iterator.MoveNext())
		{
			this.m_iterator = null;
		}
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0005FA64 File Offset: 0x0005DE64
	public IEnumerator Run(FrontendGUI _ui, string _pveOption, string _nveOption)
	{
		FrontendListEntry.NameData[] names = new FrontendListEntry.NameData[]
		{
			new FrontendListEntry.NameData(_pveOption, true, false),
			new FrontendListEntry.NameData(_nveOption, true, false)
		};
		_ui.SetNames(names);
		ScrollingListControlsHelper scrollingControlsHelper = new ScrollingListControlsHelper();
		scrollingControlsHelper.Init(_ui, 0.12f);
		bool finished = false;
		scrollingControlsHelper.RegisterSelectionCallback(delegate(int _s)
		{
			finished = true;
			this.ResultCallback(_s == 0);
		});
		scrollingControlsHelper.RegisterCancelCallback(delegate
		{
			finished = true;
			this.ResultCallback(false);
		});
		while (!finished)
		{
			scrollingControlsHelper.Update();
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000CC5 RID: 3269
	[SerializeField]
	[AssignChild("ConfirmTitle", Editorbility.NonEditable)]
	private Text m_titleObject;

	// Token: 0x04000CC6 RID: 3270
	[SerializeField]
	[AssignChild("ExplainationText", Editorbility.NonEditable)]
	private Text m_explanationTextObject;

	// Token: 0x04000CC7 RID: 3271
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	private FrontendGUI m_frontendGUI;

	// Token: 0x04000CC8 RID: 3272
	[SerializeField]
	private bool m_autoBegin;

	// Token: 0x04000CC9 RID: 3273
	[SerializeField]
	[HideInInspectorTest("m_autoBegin", true)]
	private string m_titleText;

	// Token: 0x04000CCA RID: 3274
	[SerializeField]
	[HideInInspectorTest("m_autoBegin", true)]
	private string m_explanationText;

	// Token: 0x04000CCB RID: 3275
	[SerializeField]
	[HideInInspectorTest("m_autoBegin", true)]
	private string m_pveOptionText;

	// Token: 0x04000CCC RID: 3276
	[SerializeField]
	[HideInInspectorTest("m_autoBegin", true)]
	private string m_nveOptionText;

	// Token: 0x04000CCE RID: 3278
	private IEnumerator m_iterator;
}
