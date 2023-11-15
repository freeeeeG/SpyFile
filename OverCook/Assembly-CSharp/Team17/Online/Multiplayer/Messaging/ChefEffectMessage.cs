using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008A4 RID: 2212
	public class ChefEffectMessage : Serialisable
	{
		// Token: 0x06002B21 RID: 11041 RVA: 0x000CA2A6 File Offset: 0x000C86A6
		public void Initalise(uint _ChefEntityID, ChefEffectMessage.EffectType _effectType, Vector3 _relativePosition)
		{
			this.ChefEntityID = _ChefEntityID;
			this.Effect = _effectType;
			this.RelativePosition = _relativePosition;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000CA2BD File Offset: 0x000C86BD
		public bool Deserialise(BitStreamReader _reader)
		{
			this.ChefEntityID = _reader.ReadUInt32(10);
			_reader.ReadVector3(ref this.RelativePosition);
			this.Effect = (ChefEffectMessage.EffectType)_reader.ReadUInt32(ChefEffectMessage.kEffectTypeBitCount);
			return true;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000CA2EB File Offset: 0x000C86EB
		public void Serialise(BitStreamWriter _writer)
		{
			_writer.Write(this.ChefEntityID, 10);
			_writer.Write(ref this.RelativePosition);
			_writer.Write((uint)this.Effect, ChefEffectMessage.kEffectTypeBitCount);
		}

		// Token: 0x04002214 RID: 8724
		private static readonly int kEffectTypeBitCount = GameUtils.GetRequiredBitCount(2);

		// Token: 0x04002215 RID: 8725
		public uint ChefEntityID;

		// Token: 0x04002216 RID: 8726
		public Vector3 RelativePosition;

		// Token: 0x04002217 RID: 8727
		public ChefEffectMessage.EffectType Effect;

		// Token: 0x020008A5 RID: 2213
		public enum EffectType
		{
			// Token: 0x04002219 RID: 8729
			Impact,
			// Token: 0x0400221A RID: 8730
			Dash,
			// Token: 0x0400221B RID: 8731
			COUNT
		}
	}
}
