using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000B9E RID: 2974
public class TriggerAudioMessage : Serialisable
{
	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06003CF4 RID: 15604 RVA: 0x001235F2 File Offset: 0x001219F2
	public GameOneShotAudioTag AudioTag
	{
		get
		{
			return this.m_audioTag;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06003CF5 RID: 15605 RVA: 0x001235FA File Offset: 0x001219FA
	public int Layer
	{
		get
		{
			return this.m_layer;
		}
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x00123602 File Offset: 0x00121A02
	public void Initialise(GameOneShotAudioTag _audioTag, int _layer)
	{
		this.m_audioTag = _audioTag;
		this.m_layer = _layer;
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x00123612 File Offset: 0x00121A12
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_audioTag = (GameOneShotAudioTag)reader.ReadUInt32(this.m_kBitsPerTag);
		this.m_layer = (int)reader.ReadUInt32(this.m_kBitsPerLayer);
		return true;
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x00123639 File Offset: 0x00121A39
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_audioTag, this.m_kBitsPerTag);
		writer.Write((uint)this.m_layer, this.m_kBitsPerLayer);
	}

	// Token: 0x04003109 RID: 12553
	private int m_kBitsPerTag = GameUtils.GetRequiredBitCount(319);

	// Token: 0x0400310A RID: 12554
	private GameOneShotAudioTag m_audioTag;

	// Token: 0x0400310B RID: 12555
	private int m_kBitsPerLayer = 5;

	// Token: 0x0400310C RID: 12556
	private int m_layer;
}
