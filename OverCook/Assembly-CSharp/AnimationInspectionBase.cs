using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011D RID: 285
public abstract class AnimationInspectionBase : MonoBehaviour
{
	// Token: 0x06000547 RID: 1351 RVA: 0x00029C1C File Offset: 0x0002801C
	protected void AddDatastream(string _animationName, string _datastreamName, AnimationInspectionBase.FloatDatastreamCalculator _calculator)
	{
		AnimationInspectionBase.DatastreamData datastreamData;
		datastreamData.m_nameHash = Animator.StringToHash(_animationName);
		datastreamData.m_streamName = _datastreamName;
		datastreamData.m_calculator = _calculator;
		Array.Resize<AnimationInspectionBase.DatastreamData>(ref this.m_datastreams, this.m_datastreams.Length + 1);
		this.m_datastreams[this.m_datastreams.Length - 1] = datastreamData;
		if (!this.m_streamNameValidator.ContainsKey(_datastreamName))
		{
			this.m_streamNameValidator.Add(_datastreamName, true);
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00029C94 File Offset: 0x00028094
	public float GetDatastreamFloat(string _id)
	{
		float result = 0f;
		for (int i = 0; i < this.m_animator.layerCount; i++)
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(i);
			int nNameHash = currentAnimatorStateInfo.nameHash;
			AnimationInspectionBase.DatastreamData[] array = Array.FindAll<AnimationInspectionBase.DatastreamData>(this.m_datastreams, (AnimationInspectionBase.DatastreamData _d) => _d.m_nameHash == nNameHash && _d.m_streamName == _id);
			foreach (AnimationInspectionBase.DatastreamData datastreamData in array)
			{
				result = Mathf.Max(new float[]
				{
					datastreamData.m_calculator(currentAnimatorStateInfo.normalizedTime)
				});
			}
		}
		return result;
	}

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	[AssignComponentRecursive(Visibility.Show)]
	public Animator m_animator;

	// Token: 0x04000491 RID: 1169
	private AnimationInspectionBase.DatastreamData[] m_datastreams = new AnimationInspectionBase.DatastreamData[0];

	// Token: 0x04000492 RID: 1170
	private Dictionary<string, bool> m_streamNameValidator = new Dictionary<string, bool>();

	// Token: 0x0200011E RID: 286
	private struct DatastreamData
	{
		// Token: 0x04000493 RID: 1171
		public int m_nameHash;

		// Token: 0x04000494 RID: 1172
		public string m_streamName;

		// Token: 0x04000495 RID: 1173
		public AnimationInspectionBase.FloatDatastreamCalculator m_calculator;
	}

	// Token: 0x0200011F RID: 287
	// (Invoke) Token: 0x0600054A RID: 1354
	protected delegate float FloatDatastreamCalculator(float _animPropThrough);
}
