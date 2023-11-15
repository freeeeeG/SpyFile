using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008C9 RID: 2249
// (Invoke) Token: 0x06002BAF RID: 11183
public delegate void UnorderedMessageReceivedCallback(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message, uint uSequence);
