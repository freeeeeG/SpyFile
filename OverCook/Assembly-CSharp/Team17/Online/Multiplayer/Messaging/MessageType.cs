using System;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008CF RID: 2255
	public enum MessageType
	{
		// Token: 0x04002305 RID: 8965
		Example,
		// Token: 0x04002306 RID: 8966
		LevelLoadByIndex,
		// Token: 0x04002307 RID: 8967
		LevelLoadByName,
		// Token: 0x04002308 RID: 8968
		EntitySynchronisation,
		// Token: 0x04002309 RID: 8969
		EntityEvent,
		// Token: 0x0400230A RID: 8970
		SpawnEntity,
		// Token: 0x0400230B RID: 8971
		DestroyEntity,
		// Token: 0x0400230C RID: 8972
		ChefOwnership,
		// Token: 0x0400230D RID: 8973
		UsersChanged,
		// Token: 0x0400230E RID: 8974
		UsersAdded,
		// Token: 0x0400230F RID: 8975
		Input,
		// Token: 0x04002310 RID: 8976
		GameState,
		// Token: 0x04002311 RID: 8977
		ChefAvatar,
		// Token: 0x04002312 RID: 8978
		LatencyMeasure,
		// Token: 0x04002313 RID: 8979
		TimeSync,
		// Token: 0x04002314 RID: 8980
		ControllerSettings,
		// Token: 0x04002315 RID: 8981
		TriggerEvent,
		// Token: 0x04002316 RID: 8982
		LobbyServer,
		// Token: 0x04002317 RID: 8983
		LobbyClient,
		// Token: 0x04002318 RID: 8984
		ChefEvent,
		// Token: 0x04002319 RID: 8985
		ChefEffect,
		// Token: 0x0400231A RID: 8986
		MapAvatar,
		// Token: 0x0400231B RID: 8987
		MapAvatarHorn,
		// Token: 0x0400231C RID: 8988
		DynamicLevel,
		// Token: 0x0400231D RID: 8989
		GameSetup,
		// Token: 0x0400231E RID: 8990
		GameProgressData,
		// Token: 0x0400231F RID: 8991
		EmoteWheel,
		// Token: 0x04002320 RID: 8992
		SetupCoopSession,
		// Token: 0x04002321 RID: 8993
		FlowInput,
		// Token: 0x04002322 RID: 8994
		Achievement,
		// Token: 0x04002323 RID: 8995
		TriggerAudio,
		// Token: 0x04002324 RID: 8996
		SpawnPhysicalAttachment,
		// Token: 0x04002325 RID: 8997
		ResumeWorldObjectSync,
		// Token: 0x04002326 RID: 8998
		ResumeChefPositionSync,
		// Token: 0x04002327 RID: 8999
		ResumePhysicsObjectSync,
		// Token: 0x04002328 RID: 9000
		BossLevel,
		// Token: 0x04002329 RID: 9001
		DestroyChef,
		// Token: 0x0400232A RID: 9002
		HighScores,
		// Token: 0x0400232B RID: 9003
		DestroyEntities,
		// Token: 0x0400232C RID: 9004
		ResumeEntitySync,
		// Token: 0x0400232D RID: 9005
		SessionConfigSync,
		// Token: 0x0400232E RID: 9006
		HordeSpawn,
		// Token: 0x0400232F RID: 9007
		COUNT
	}
}
