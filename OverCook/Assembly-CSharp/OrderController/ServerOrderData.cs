using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace OrderController
{
	// Token: 0x02000757 RID: 1879
	public class ServerOrderData : Serialisable
	{
		// Token: 0x0600241F RID: 9247 RVA: 0x000AC3CF File Offset: 0x000AA7CF
		public ServerOrderData()
		{
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000AC3E2 File Offset: 0x000AA7E2
		public ServerOrderData(OrderID _id, RecipeList.Entry _entry, float _lifetime)
		{
			this.ID = _id;
			this.RecipeListEntry = _entry;
			this.Lifetime = _lifetime;
			this.Remaining = this.Lifetime;
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000AC416 File Offset: 0x000AA816
		public void Serialise(BitStreamWriter writer)
		{
			this.ID.Serialise(writer);
			this.RecipeListEntry.Serialise(writer);
			writer.Write(this.Lifetime);
			writer.Write(this.Remaining);
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000AC448 File Offset: 0x000AA848
		public bool Deserialise(BitStreamReader reader)
		{
			this.ID.Deserialise(reader);
			if (this.RecipeListEntry.Deserialise(reader))
			{
				this.Lifetime = reader.ReadFloat32();
				this.Remaining = reader.ReadFloat32();
				return true;
			}
			return false;
		}

		// Token: 0x04001B96 RID: 7062
		public OrderID ID;

		// Token: 0x04001B97 RID: 7063
		public RecipeList.Entry RecipeListEntry = new RecipeList.Entry();

		// Token: 0x04001B98 RID: 7064
		public float Lifetime;

		// Token: 0x04001B99 RID: 7065
		public float Remaining;
	}
}
