using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008BE RID: 2238
public class IngredientContainerMessage : Serialisable
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000CBA0D File Offset: 0x000C9E0D
	public IngredientContainerMessage.MessageType Type
	{
		get
		{
			return this.m_type;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000CBA15 File Offset: 0x000C9E15
	public AssembledDefinitionNode[] Contents
	{
		get
		{
			return this.m_contents;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06002B8C RID: 11148 RVA: 0x000CBA1D File Offset: 0x000C9E1D
	public bool ActiveState
	{
		get
		{
			return this.m_activeState;
		}
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x000CBA25 File Offset: 0x000C9E25
	public void Initialise(AssembledDefinitionNode[] _contents)
	{
		this.m_contents = _contents;
		this.m_type = IngredientContainerMessage.MessageType.ContentsChanged;
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x000CBA35 File Offset: 0x000C9E35
	public void Initialise(bool _activeState)
	{
		this.m_activeState = _activeState;
		this.m_type = IngredientContainerMessage.MessageType.ActiveState;
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x000CBA48 File Offset: 0x000C9E48
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_type, 1);
		IngredientContainerMessage.MessageType type = this.m_type;
		if (type != IngredientContainerMessage.MessageType.ActiveState)
		{
			if (type == IngredientContainerMessage.MessageType.ContentsChanged)
			{
				if (this.m_contents != null)
				{
					CompositeAssembledNode compositeAssembledNode = new CompositeAssembledNode();
					compositeAssembledNode.m_composition = this.m_contents;
					writer.Write((uint)AssembledDefinitionNodeFactory.GetNodeType(compositeAssembledNode), 4);
					compositeAssembledNode.Serialise(writer);
				}
			}
		}
		else
		{
			writer.Write(this.m_activeState);
		}
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x000CBAC4 File Offset: 0x000C9EC4
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_type = (IngredientContainerMessage.MessageType)reader.ReadByte(1);
		IngredientContainerMessage.MessageType type = this.m_type;
		if (type != IngredientContainerMessage.MessageType.ActiveState)
		{
			if (type == IngredientContainerMessage.MessageType.ContentsChanged)
			{
				AssembledDefinitionNode assembledDefinitionNode = AssembledDefinitionNodeFactory.CreateNode((int)reader.ReadUInt32(4));
				CompositeAssembledNode compositeAssembledNode = assembledDefinitionNode as CompositeAssembledNode;
				if (compositeAssembledNode != null)
				{
					compositeAssembledNode.Deserialise(reader);
					this.m_contents = compositeAssembledNode.m_composition;
				}
			}
		}
		else
		{
			this.m_activeState = reader.ReadBit();
		}
		return true;
	}

	// Token: 0x040022B6 RID: 8886
	private const int m_kBitsPerType = 1;

	// Token: 0x040022B7 RID: 8887
	private IngredientContainerMessage.MessageType m_type;

	// Token: 0x040022B8 RID: 8888
	private AssembledDefinitionNode[] m_contents;

	// Token: 0x040022B9 RID: 8889
	private bool m_activeState;

	// Token: 0x020008BF RID: 2239
	public enum MessageType
	{
		// Token: 0x040022BB RID: 8891
		ContentsChanged,
		// Token: 0x040022BC RID: 8892
		ActiveState
	}
}
