using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000660 RID: 1632
public class CutsceneController : MonoBehaviour
{
	// Token: 0x06001F1C RID: 7964 RVA: 0x00098506 File Offset: 0x00096906
	public void RegisterSkipCallback(CallbackVoid _callback)
	{
		this.m_skippedCallback = (CallbackVoid)Delegate.Combine(this.m_skippedCallback, _callback);
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x0009851F File Offset: 0x0009691F
	public void DeregisterSkipCallback(CallbackVoid _callback)
	{
		this.m_skippedCallback = (CallbackVoid)Delegate.Remove(this.m_skippedCallback, _callback);
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x00098538 File Offset: 0x00096938
	public void OnCutsceneSkipped()
	{
		this.m_skippedCallback();
	}

	// Token: 0x040017C5 RID: 6085
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	public PlayableDirector m_director;

	// Token: 0x040017C6 RID: 6086
	[SerializeField]
	[AssignResource("CutsceneSkipUI", Editorbility.NonEditable)]
	public GameObject m_skipUIPrefab;

	// Token: 0x040017C7 RID: 6087
	[SerializeField]
	public GameObject m_gameCamera;

	// Token: 0x040017C8 RID: 6088
	private CallbackVoid m_skippedCallback = delegate()
	{
	};

	// Token: 0x02000661 RID: 1633
	public class SetupData
	{
		// Token: 0x040017CA RID: 6090
		public bool skippable;

		// Token: 0x040017CB RID: 6091
		public bool postplaybackUIEnabled;
	}
}
