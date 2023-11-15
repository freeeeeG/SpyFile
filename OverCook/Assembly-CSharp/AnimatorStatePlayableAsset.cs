using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000656 RID: 1622
[Serializable]
public class AnimatorStatePlayableAsset : PlayableAsset
{
	// Token: 0x06001EE8 RID: 7912 RVA: 0x000973D8 File Offset: 0x000957D8
	public object GetValue()
	{
		AnimatorVariableType variableType = this.m_variableType;
		if (variableType == AnimatorVariableType.Bool)
		{
			return this.m_boolValue;
		}
		if (variableType == AnimatorVariableType.Int)
		{
			return this.m_intValue;
		}
		if (variableType != AnimatorVariableType.Float)
		{
			return null;
		}
		return this.m_floatValue;
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x0009742C File Offset: 0x0009582C
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<AnimatorStatePlayableBehaviour> playable = ScriptPlayable<AnimatorStatePlayableBehaviour>.Create(graph, 0);
		AnimatorStatePlayableBehaviour behaviour = playable.GetBehaviour();
		behaviour.Setup(this.m_variableName, this.m_variableType, this.GetValue());
		return playable;
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001EEA RID: 7914 RVA: 0x00097467 File Offset: 0x00095867
	public override double duration
	{
		get
		{
			return base.duration;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001EEB RID: 7915 RVA: 0x0009746F File Offset: 0x0009586F
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x040017AA RID: 6058
	[SerializeField]
	private string m_variableName;

	// Token: 0x040017AB RID: 6059
	[SerializeField]
	private AnimatorVariableType m_variableType;

	// Token: 0x040017AC RID: 6060
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Bool)]
	private bool m_boolValue;

	// Token: 0x040017AD RID: 6061
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Int)]
	private int m_intValue;

	// Token: 0x040017AE RID: 6062
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Float)]
	private float m_floatValue;
}
