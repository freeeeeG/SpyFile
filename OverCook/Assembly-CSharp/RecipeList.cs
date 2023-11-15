using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000766 RID: 1894
[Serializable]
public class RecipeList : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x0600246F RID: 9327 RVA: 0x000ACEE2 File Offset: 0x000AB2E2
	public void OnAfterDeserialize()
	{
		this.Validate();
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000ACEEA File Offset: 0x000AB2EA
	public void OnBeforeSerialize()
	{
		this.Validate();
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000ACEF2 File Offset: 0x000AB2F2
	private void Validate()
	{
	}

	// Token: 0x04001BDE RID: 7134
	[SerializeField]
	public RecipeList.Entry[] m_recipes;

	// Token: 0x04001BDF RID: 7135
	[SerializeField]
	public RecipeList.Entry[] m_freestyle;

	// Token: 0x02000767 RID: 1895
	[Serializable]
	public class Entry : IWeight, Serialisable
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x000ACF03 File Offset: 0x000AB303
		public float Weight
		{
			get
			{
				return this.m_weight;
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000ACF0B File Offset: 0x000AB30B
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_order.m_uID, 32);
			writer.Write((uint)this.m_scoreForMeal, 10);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000ACF30 File Offset: 0x000AB330
		public bool Deserialise(BitStreamReader reader)
		{
			int id = (int)reader.ReadUInt32(32);
			this.m_order = GameUtils.GetOrderDefinitionNode(id);
			this.m_scoreForMeal = (int)reader.ReadUInt32(10);
			return true;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000ACF61 File Offset: 0x000AB361
		public void Copy(RecipeList.Entry entry)
		{
			this.m_order = entry.m_order;
			this.m_weight = entry.m_weight;
			this.m_scoreForMeal = entry.m_scoreForMeal;
		}

		// Token: 0x04001BE0 RID: 7136
		public OrderDefinitionNode m_order;

		// Token: 0x04001BE1 RID: 7137
		public float m_weight;

		// Token: 0x04001BE2 RID: 7138
		public int m_scoreForMeal = 1;

		// Token: 0x04001BE3 RID: 7139
		private const int kBitsPerOrderNode = 32;

		// Token: 0x04001BE4 RID: 7140
		private const int kBitsPerScore = 10;
	}
}
