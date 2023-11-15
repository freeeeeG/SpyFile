using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class TriggerAnimatorVariableMessage : Serialisable
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600066A RID: 1642 RVA: 0x0002CD44 File Offset: 0x0002B144
	public TriggerAnimatorVariableMessage.RandomValueType Type
	{
		get
		{
			return this.m_type;
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002CD4C File Offset: 0x0002B14C
	public void Initialise()
	{
		this.m_type = TriggerAnimatorVariableMessage.RandomValueType.None;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0002CD55 File Offset: 0x0002B155
	public void InitRandomFloat(float _min, float _max)
	{
		this.m_type = TriggerAnimatorVariableMessage.RandomValueType.Float;
		this.m_randomValue = UnityEngine.Random.Range(_min, _max);
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0002CD70 File Offset: 0x0002B170
	public void InitRandomInt(int _min, int _max)
	{
		this.m_type = TriggerAnimatorVariableMessage.RandomValueType.Int;
		this.m_randomValue = UnityEngine.Random.Range(_min, _max);
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0002CD8B File Offset: 0x0002B18B
	public void InitRandomBool()
	{
		this.m_type = TriggerAnimatorVariableMessage.RandomValueType.Bool;
		this.m_randomValue = (UnityEngine.Random.Range(0, 1) == 1);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0002CDB4 File Offset: 0x0002B1B4
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_type, 2);
		TriggerAnimatorVariableMessage.RandomValueType type = this.m_type;
		if (type != TriggerAnimatorVariableMessage.RandomValueType.Bool)
		{
			if (type != TriggerAnimatorVariableMessage.RandomValueType.Float)
			{
				if (type == TriggerAnimatorVariableMessage.RandomValueType.Int)
				{
					writer.Write((float)((int)this.m_randomValue));
				}
			}
			else
			{
				writer.Write((float)this.m_randomValue);
			}
		}
		else
		{
			writer.Write((bool)this.m_randomValue);
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0002CE34 File Offset: 0x0002B234
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_type = (TriggerAnimatorVariableMessage.RandomValueType)reader.ReadUInt32(2);
		TriggerAnimatorVariableMessage.RandomValueType type = this.m_type;
		if (type != TriggerAnimatorVariableMessage.RandomValueType.Bool)
		{
			if (type != TriggerAnimatorVariableMessage.RandomValueType.Float)
			{
				if (type == TriggerAnimatorVariableMessage.RandomValueType.Int)
				{
					this.m_randomValue = (int)reader.ReadFloat32();
				}
			}
			else
			{
				this.m_randomValue = reader.ReadFloat32();
			}
		}
		else
		{
			this.m_randomValue = reader.ReadBit();
		}
		return true;
	}

	// Token: 0x04000557 RID: 1367
	private TriggerAnimatorVariableMessage.RandomValueType m_type;

	// Token: 0x04000558 RID: 1368
	private const int m_kBitsPerType = 2;

	// Token: 0x04000559 RID: 1369
	public object m_randomValue;

	// Token: 0x0200016E RID: 366
	public enum RandomValueType
	{
		// Token: 0x0400055B RID: 1371
		None,
		// Token: 0x0400055C RID: 1372
		Bool,
		// Token: 0x0400055D RID: 1373
		Int,
		// Token: 0x0400055E RID: 1374
		Float
	}
}
