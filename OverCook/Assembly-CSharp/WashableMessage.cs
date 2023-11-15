using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000626 RID: 1574
public class WashableMessage : Serialisable
{
	// Token: 0x06001DCD RID: 7629 RVA: 0x00090A17 File Offset: 0x0008EE17
	public void Initialise_Progress(float _progress, float _targetProgress)
	{
		this.m_msgType = WashableMessage.MsgType.Progress;
		this.m_progress = _progress;
		this.m_target = _targetProgress;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x00090A2E File Offset: 0x0008EE2E
	public void Initialise_Rate(float _rate, float _currentProgress)
	{
		this.m_msgType = WashableMessage.MsgType.Rate;
		this.m_rate = _rate;
		this.m_progress = _currentProgress;
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x00090A48 File Offset: 0x0008EE48
	public void Serialise(BitStreamWriter _writer)
	{
		_writer.Write((uint)this.m_msgType, 1);
		WashableMessage.MsgType msgType = this.m_msgType;
		if (msgType != WashableMessage.MsgType.Progress)
		{
			if (msgType == WashableMessage.MsgType.Rate)
			{
				_writer.Write(this.m_rate);
				_writer.Write(this.m_progress);
			}
		}
		else
		{
			_writer.Write(this.m_progress);
			_writer.Write(this.m_target);
		}
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x00090AB8 File Offset: 0x0008EEB8
	public bool Deserialise(BitStreamReader _reader)
	{
		this.m_msgType = (WashableMessage.MsgType)_reader.ReadUInt32(1);
		WashableMessage.MsgType msgType = this.m_msgType;
		if (msgType != WashableMessage.MsgType.Progress)
		{
			if (msgType == WashableMessage.MsgType.Rate)
			{
				this.m_rate = _reader.ReadFloat32();
				this.m_progress = _reader.ReadFloat32();
			}
		}
		else
		{
			this.m_progress = _reader.ReadFloat32();
			this.m_target = _reader.ReadFloat32();
		}
		return true;
	}

	// Token: 0x040016FA RID: 5882
	private const int c_msgTypeBits = 1;

	// Token: 0x040016FB RID: 5883
	public WashableMessage.MsgType m_msgType;

	// Token: 0x040016FC RID: 5884
	public float m_progress;

	// Token: 0x040016FD RID: 5885
	public float m_target;

	// Token: 0x040016FE RID: 5886
	public float m_rate;

	// Token: 0x02000627 RID: 1575
	public enum MsgType
	{
		// Token: 0x04001700 RID: 5888
		Progress,
		// Token: 0x04001701 RID: 5889
		Rate
	}
}
