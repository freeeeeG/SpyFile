using System;
using BitStream;

// Token: 0x020009C5 RID: 2501
public class MixedCompositeAssembledNode : CompositeAssembledNode
{
	// Token: 0x060030F8 RID: 12536 RVA: 0x000E6320 File Offset: 0x000E4720
	public override void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_progress, 4);
		writer.Write(this.m_recordedProgress != null);
		if (this.m_recordedProgress != null)
		{
			writer.Write(this.m_recordedProgress.Value);
		}
		base.Serialise(writer);
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x000E6374 File Offset: 0x000E4774
	public override bool Deserialise(BitStreamReader reader)
	{
		this.m_progress = (MixedCompositeOrderNode.MixingProgress)reader.ReadUInt32(4);
		if (reader.ReadBit())
		{
			this.m_recordedProgress = new float?(reader.ReadFloat32());
		}
		else
		{
			this.m_recordedProgress = null;
		}
		return base.Deserialise(reader);
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000E63C8 File Offset: 0x000E47C8
	protected override bool IsMatch(AssembledDefinitionNode _subject)
	{
		MixedCompositeAssembledNode mixedCompositeAssembledNode = _subject as MixedCompositeAssembledNode;
		return mixedCompositeAssembledNode != null && base.AssumeTypeMatch(mixedCompositeAssembledNode) && this.m_progress == mixedCompositeAssembledNode.m_progress;
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x000E6400 File Offset: 0x000E4800
	public override void ReplaceData(AssembledDefinitionNode _node)
	{
		MixedCompositeAssembledNode mixedCompositeAssembledNode = _node as MixedCompositeAssembledNode;
		this.m_progress = mixedCompositeAssembledNode.m_progress;
		this.m_recordedProgress = mixedCompositeAssembledNode.m_recordedProgress;
		base.ReplaceData(_node);
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000E6434 File Offset: 0x000E4834
	public override AssembledDefinitionNode Simpilfy()
	{
		bool flag = this.m_progress != MixedCompositeOrderNode.MixingProgress.Unmixed;
		CompositeAssembledNode compositeAssembledNode;
		if (flag)
		{
			compositeAssembledNode = new MixedCompositeAssembledNode
			{
				m_progress = this.m_progress,
				m_recordedProgress = this.m_recordedProgress
			};
		}
		else
		{
			compositeAssembledNode = new CompositeAssembledNode();
		}
		for (int i = 0; i < this.m_composition.Length; i++)
		{
			AssembledDefinitionNode assembledDefinitionNode = this.m_composition[i].Simpilfy();
			if (assembledDefinitionNode != AssembledDefinitionNode.NullNode)
			{
				ArrayUtils.PushBack<AssembledDefinitionNode>(ref compositeAssembledNode.m_composition, assembledDefinitionNode);
			}
		}
		for (int j = 0; j < this.m_optional.Length; j++)
		{
			AssembledDefinitionNode assembledDefinitionNode2 = this.m_optional[j].Simpilfy();
			if (assembledDefinitionNode2 != AssembledDefinitionNode.NullNode)
			{
				ArrayUtils.PushBack<AssembledDefinitionNode>(ref compositeAssembledNode.m_optional, assembledDefinitionNode2);
			}
		}
		compositeAssembledNode.m_permittedEntries = this.m_permittedEntries;
		if (compositeAssembledNode.m_optional.Length == 0)
		{
			if (compositeAssembledNode.m_composition.Length == 1 && !flag)
			{
				return compositeAssembledNode.m_composition[0];
			}
			if (compositeAssembledNode.m_composition.Length == 1 && compositeAssembledNode.m_composition[0] is CompositeAssembledNode)
			{
				CompositeAssembledNode compositeAssembledNode2 = compositeAssembledNode.m_composition[0] as CompositeAssembledNode;
				compositeAssembledNode.m_composition = compositeAssembledNode2.m_composition;
				compositeAssembledNode.m_optional = compositeAssembledNode.m_optional.Union(compositeAssembledNode2.m_optional);
				return compositeAssembledNode;
			}
			if (compositeAssembledNode.m_composition.Length == 0)
			{
				return AssembledDefinitionNode.NullNode;
			}
		}
		return compositeAssembledNode;
	}

	// Token: 0x04002753 RID: 10067
	public MixedCompositeOrderNode.MixingProgress m_progress = MixedCompositeOrderNode.MixingProgress.Mixed;

	// Token: 0x04002754 RID: 10068
	public float? m_recordedProgress = new float?(0f);
}
