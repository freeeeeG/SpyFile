using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000664 RID: 1636
[Serializable]
public class DialogPlayableAsset : PlayableAsset
{
	// Token: 0x06001F2E RID: 7982 RVA: 0x00098940 File Offset: 0x00096D40
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<DialogPlayableBehaviour> playable = ScriptPlayable<DialogPlayableBehaviour>.Create(graph, 0);
		DialogPlayableBehaviour behaviour = playable.GetBehaviour();
		Transform transform = this.m_followObject.Resolve(playable.GetGraph<ScriptPlayable<DialogPlayableBehaviour>>().GetResolver());
		if (transform != null)
		{
			behaviour.Setup(this.m_dialogue, transform);
		}
		else
		{
			behaviour.Setup(this.m_dialogue, this.m_anchor, this.m_pivot, this.m_rotation);
		}
		return playable;
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000989B9 File Offset: 0x00096DB9
	public override double duration
	{
		get
		{
			return base.duration;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06001F30 RID: 7984 RVA: 0x000989C1 File Offset: 0x00096DC1
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x040017D3 RID: 6099
	[Header("Dialogue")]
	[SerializeField]
	private DialogueController.Dialogue m_dialogue;

	// Token: 0x040017D4 RID: 6100
	[Header("Positioning")]
	[SerializeField]
	private ExposedReference<Transform> m_followObject;

	// Token: 0x040017D5 RID: 6101
	[SerializeField]
	private Vector2 m_anchor = new Vector2(0.5f, 0.5f);

	// Token: 0x040017D6 RID: 6102
	[SerializeField]
	private Vector2 m_pivot = new Vector2(0.5f, 0.5f);

	// Token: 0x040017D7 RID: 6103
	[SerializeField]
	private float m_rotation;
}
