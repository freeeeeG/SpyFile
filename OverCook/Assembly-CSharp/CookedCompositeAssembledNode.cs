using System;
using BitStream;

// Token: 0x020009BE RID: 2494
public class CookedCompositeAssembledNode : CompositeAssembledNode
{
	// Token: 0x060030D8 RID: 12504 RVA: 0x000E5C50 File Offset: 0x000E4050
	public override void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_cookingStep.m_uID, 32);
		writer.Write((uint)this.m_progress, 4);
		writer.Write(this.m_recordedProgress != null);
		if (this.m_recordedProgress != null)
		{
			writer.Write(this.m_recordedProgress.Value);
		}
		base.Serialise(writer);
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x000E5CB8 File Offset: 0x000E40B8
	public override bool Deserialise(BitStreamReader reader)
	{
		this.m_cookingStep = GameUtils.GetCookingStepData((int)reader.ReadUInt32(32));
		this.m_progress = (CookedCompositeOrderNode.CookingProgress)reader.ReadUInt32(4);
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

	// Token: 0x060030DA RID: 12506 RVA: 0x000E5D1C File Offset: 0x000E411C
	protected override bool IsMatch(AssembledDefinitionNode _subject)
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = _subject as CookedCompositeAssembledNode;
		return cookedCompositeAssembledNode != null && base.AssumeTypeMatch(cookedCompositeAssembledNode) && this.m_cookingStep.m_uID == cookedCompositeAssembledNode.m_cookingStep.m_uID && this.m_progress == cookedCompositeAssembledNode.m_progress;
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x000E5D70 File Offset: 0x000E4170
	public override void ReplaceData(AssembledDefinitionNode _node)
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = _node as CookedCompositeAssembledNode;
		this.m_cookingStep = cookedCompositeAssembledNode.m_cookingStep;
		this.m_progress = cookedCompositeAssembledNode.m_progress;
		this.m_recordedProgress = cookedCompositeAssembledNode.m_recordedProgress;
		base.ReplaceData(_node);
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000E5DB0 File Offset: 0x000E41B0
	public override AssembledDefinitionNode Simpilfy()
	{
		bool flag = this.m_progress != CookedCompositeOrderNode.CookingProgress.Raw;
		CompositeAssembledNode compositeAssembledNode;
		if (flag)
		{
			compositeAssembledNode = new CookedCompositeAssembledNode
			{
				m_cookingStep = this.m_cookingStep,
				m_progress = this.m_progress
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
			if (compositeAssembledNode.m_composition.Length == 1 && compositeAssembledNode.m_composition[0] is CompositeAssembledNode && !(compositeAssembledNode.m_composition[0] is MixedCompositeAssembledNode))
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

	// Token: 0x04002740 RID: 10048
	public CookingStepData m_cookingStep;

	// Token: 0x04002741 RID: 10049
	public CookedCompositeOrderNode.CookingProgress m_progress = CookedCompositeOrderNode.CookingProgress.Cooked;

	// Token: 0x04002742 RID: 10050
	public float? m_recordedProgress = new float?(0f);
}
