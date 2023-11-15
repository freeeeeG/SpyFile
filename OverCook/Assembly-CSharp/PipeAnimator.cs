using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class PipeAnimator : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06001256 RID: 4694 RVA: 0x000674EC File Offset: 0x000658EC
	private void Awake()
	{
		this.m_propertyBlocks = new MaterialPropertyBlock[this.m_datas.Length];
		for (int i = 0; i < this.m_propertyBlocks.Length; i++)
		{
			this.m_propertyBlocks[i] = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00067534 File Offset: 0x00065934
	private void Update()
	{
		for (int i = 0; i < this.m_datas.Length; i++)
		{
			PipeAnimatorData pipeAnimatorData = this.m_datas[i];
			Vector3 v = pipeAnimatorData.m_path.Evaluate(Mathf.Clamp01(pipeAnimatorData.m_position));
			this.m_propertyBlocks[i].Clear();
			this.m_propertyBlocks[i].SetVector(this.m_materialPipePosPropertyString, v);
			this.m_propertyBlocks[i].SetVector(this.m_materialPipeParamsPropertyString, new Vector4(pipeAnimatorData.m_falloff, pipeAnimatorData.m_displacement, 0f, 0f));
		}
		for (int j = 0; j < this.m_rendererRanges.Length; j++)
		{
			PipeAnimator.IntRange intRange = this.m_rendererRanges[j];
			int num = intRange.m_first + intRange.m_count;
			for (int k = intRange.m_first; k < num; k++)
			{
				this.m_renderers[k].SetPropertyBlock(this.m_propertyBlocks[j]);
			}
		}
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x00067645 File Offset: 0x00065A45
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x00067648 File Offset: 0x00065A48
	public void OnBeforeSerialize()
	{
		List<Renderer> list = new List<Renderer>();
		this.m_rendererRanges = new PipeAnimator.IntRange[this.m_datas.Length];
		int num = 0;
		while (this.m_datas != null && num < this.m_datas.Length)
		{
			PipeAnimatorData pipeAnimatorData = this.m_datas[num];
			if (pipeAnimatorData != null)
			{
				this.m_rendererRanges[num].m_first = list.Count;
				Renderer renderer = pipeAnimatorData.gameObject.RequestComponent<Renderer>();
				if (renderer != null)
				{
					list.Add(renderer);
				}
				int childCount = this.m_datas[num].transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = this.m_datas[num].transform.GetChild(i);
					renderer = child.gameObject.RequestComponent<Renderer>();
					if (renderer != null)
					{
						list.Add(renderer);
					}
				}
				this.m_rendererRanges[num].m_count = Mathf.Max(0, list.Count - this.m_rendererRanges[num].m_first);
			}
			num++;
		}
		this.m_renderers = list.ToArray();
	}

	// Token: 0x04000E4F RID: 3663
	[SerializeField]
	private PipeAnimatorData[] m_datas = new PipeAnimatorData[0];

	// Token: 0x04000E50 RID: 3664
	[SerializeField]
	private string m_materialPipePosPropertyString = "_PipePos";

	// Token: 0x04000E51 RID: 3665
	[SerializeField]
	private string m_materialPipeParamsPropertyString = "_PipeParams";

	// Token: 0x04000E52 RID: 3666
	private MaterialPropertyBlock[] m_propertyBlocks = new MaterialPropertyBlock[0];

	// Token: 0x04000E53 RID: 3667
	[ReadOnly]
	[SerializeField]
	private Renderer[] m_renderers = new Renderer[0];

	// Token: 0x04000E54 RID: 3668
	[ReadOnly]
	[SerializeField]
	private PipeAnimator.IntRange[] m_rendererRanges = new PipeAnimator.IntRange[0];

	// Token: 0x020003E4 RID: 996
	[Serializable]
	private struct IntRange
	{
		// Token: 0x04000E55 RID: 3669
		public int m_first;

		// Token: 0x04000E56 RID: 3670
		public int m_count;
	}
}
