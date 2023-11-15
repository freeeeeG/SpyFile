using System;

namespace Characters
{
	// Token: 0x02000748 RID: 1864
	public enum Key
	{
		// Token: 0x04002041 RID: 8257
		Unspecified,
		// Token: 0x04002042 RID: 8258
		Player1,
		// Token: 0x04002043 RID: 8259
		Player2,
		// Token: 0x04002044 RID: 8260
		SmallProp = 101,
		// Token: 0x04002045 RID: 8261
		LargeProp,
		// Token: 0x04002046 RID: 8262
		BrutalityAltar = 201,
		// Token: 0x04002047 RID: 8263
		EnergyAltar,
		// Token: 0x04002048 RID: 8264
		RageAltar,
		// Token: 0x04002049 RID: 8265
		SteelAltar,
		// Token: 0x0400204A RID: 8266
		MinionCarleonRecruit = 501,
		// Token: 0x0400204B RID: 8267
		MinionCarleonArcher,
		// Token: 0x0400204C RID: 8268
		MinionCarleonAssassin,
		// Token: 0x0400204D RID: 8269
		MinionCarleonManAtArms,
		// Token: 0x0400204E RID: 8270
		Spatialist = 1001,
		// Token: 0x0400204F RID: 8271
		Occultist,
		// Token: 0x04002050 RID: 8272
		Silentist,
		// Token: 0x04002051 RID: 8273
		Dimensionist,
		// Token: 0x04002052 RID: 8274
		Yggdrasil = 2001,
		// Token: 0x04002053 RID: 8275
		LeianaShortHair,
		// Token: 0x04002054 RID: 8276
		LeianaLongHair,
		// Token: 0x04002055 RID: 8277
		AwakenLeiana,
		// Token: 0x04002056 RID: 8278
		Chimera,
		// Token: 0x04002057 RID: 8279
		Pope,
		// Token: 0x04002058 RID: 8280
		FirstHero1,
		// Token: 0x04002059 RID: 8281
		FirstHero2,
		// Token: 0x0400205A RID: 8282
		FirstHero3,
		// Token: 0x0400205B RID: 8283
		FirstDarkHero,
		// Token: 0x0400205C RID: 8284
		CarleonRecruit = 10001,
		// Token: 0x0400205D RID: 8285
		CarleonArcher,
		// Token: 0x0400205E RID: 8286
		CarleonAssassin,
		// Token: 0x0400205F RID: 8287
		CarleonManAtArms,
		// Token: 0x04002060 RID: 8288
		CarleonLowClassWizard,
		// Token: 0x04002061 RID: 8289
		FlameWizard,
		// Token: 0x04002062 RID: 8290
		GlacialWizard,
		// Token: 0x04002063 RID: 8291
		Ent = 10101,
		// Token: 0x04002064 RID: 8292
		RootEnt,
		// Token: 0x04002065 RID: 8293
		ForestKeeper,
		// Token: 0x04002066 RID: 8294
		BlossomEnt,
		// Token: 0x04002067 RID: 8295
		GiantMushroomEnt = 15001,
		// Token: 0x04002068 RID: 8296
		CannonSpecialist,
		// Token: 0x04002069 RID: 8297
		LandWizard,
		// Token: 0x0400206A RID: 8298
		Chaser,
		// Token: 0x0400206B RID: 8299
		Hound,
		// Token: 0x0400206C RID: 8300
		ForestRootKeeper,
		// Token: 0x0400206D RID: 8301
		AssassinMercenary,
		// Token: 0x0400206E RID: 8302
		EntApocalypse,
		// Token: 0x0400206F RID: 8303
		ThunderCaller,
		// Token: 0x04002070 RID: 8304
		CarleonRecruitInCannon,
		// Token: 0x04002071 RID: 8305
		GoldManeKnight = 20001,
		// Token: 0x04002072 RID: 8306
		GoldManeArcher,
		// Token: 0x04002073 RID: 8307
		GoldManeSpearMan,
		// Token: 0x04002074 RID: 8308
		GoldManeWizard,
		// Token: 0x04002075 RID: 8309
		GoldManeManAtArms,
		// Token: 0x04002076 RID: 8310
		GoldManePriest,
		// Token: 0x04002077 RID: 8311
		ShiningRecruit,
		// Token: 0x04002078 RID: 8312
		ChiefMaid = 20101,
		// Token: 0x04002079 RID: 8313
		BroomMaid,
		// Token: 0x0400207A RID: 8314
		DishMaid,
		// Token: 0x0400207B RID: 8315
		Troublemaker,
		// Token: 0x0400207C RID: 8316
		HammerGiant = 25001,
		// Token: 0x0400207D RID: 8317
		TheBusiestMaid,
		// Token: 0x0400207E RID: 8318
		Butler,
		// Token: 0x0400207F RID: 8319
		GoldManeGuard,
		// Token: 0x04002080 RID: 8320
		WindBand,
		// Token: 0x04002081 RID: 8321
		GoldManeCavalry,
		// Token: 0x04002082 RID: 8322
		WindBandLeader,
		// Token: 0x04002083 RID: 8323
		LordChamberlain,
		// Token: 0x04002084 RID: 8324
		DarkEnt = 30001,
		// Token: 0x04002085 RID: 8325
		DarkOgre,
		// Token: 0x04002086 RID: 8326
		DarkRecruit,
		// Token: 0x04002087 RID: 8327
		DarkGolem,
		// Token: 0x04002088 RID: 8328
		Alchemist = 30101,
		// Token: 0x04002089 RID: 8329
		HighAlchemist,
		// Token: 0x0400208A RID: 8330
		DarkAlchemist,
		// Token: 0x0400208B RID: 8331
		EscapedSubject = 30201,
		// Token: 0x0400208C RID: 8332
		StrangeSubject,
		// Token: 0x0400208D RID: 8333
		Flask = 30301,
		// Token: 0x0400208E RID: 8334
		SpiritInFlask,
		// Token: 0x0400208F RID: 8335
		UnstableFlask,
		// Token: 0x04002090 RID: 8336
		UnstableFlasksSpirit,
		// Token: 0x04002091 RID: 8337
		DarkManAtArms = 35301,
		// Token: 0x04002092 RID: 8338
		Warder,
		// Token: 0x04002093 RID: 8339
		StickySubject,
		// Token: 0x04002094 RID: 8340
		ToxicAlchemist,
		// Token: 0x04002095 RID: 8341
		PerfectSubject,
		// Token: 0x04002096 RID: 8342
		HighAlchemistSummoner,
		// Token: 0x04002097 RID: 8343
		Ultimate,
		// Token: 0x04002098 RID: 8344
		ArmoredGolem,
		// Token: 0x04002099 RID: 8345
		Transcendent,
		// Token: 0x0400209A RID: 8346
		Fanatic = 40001,
		// Token: 0x0400209B RID: 8347
		Catechist,
		// Token: 0x0400209C RID: 8348
		DevoutRecruit,
		// Token: 0x0400209D RID: 8349
		MartyrFanaric,
		// Token: 0x0400209E RID: 8350
		Evangelist,
		// Token: 0x0400209F RID: 8351
		Missionary,
		// Token: 0x040020A0 RID: 8352
		LeoniaOfBrave,
		// Token: 0x040020A1 RID: 8353
		LeoniaOfWisdom,
		// Token: 0x040020A2 RID: 8354
		StatueOfRepentance,
		// Token: 0x040020A3 RID: 8355
		LeoniasKnight,
		// Token: 0x040020A4 RID: 8356
		LeoniasArcher,
		// Token: 0x040020A5 RID: 8357
		LeoniasAssassin,
		// Token: 0x040020A6 RID: 8358
		LeoniasWizard,
		// Token: 0x040020A7 RID: 8359
		LeoniasPriest,
		// Token: 0x040020A8 RID: 8360
		LeoniasSpearman,
		// Token: 0x040020A9 RID: 8361
		LeoniasCrusher,
		// Token: 0x040020AA RID: 8362
		LeoniaOfProtection = 45301,
		// Token: 0x040020AB RID: 8363
		Bombardier,
		// Token: 0x040020AC RID: 8364
		DemonHunter,
		// Token: 0x040020AD RID: 8365
		HereticInquisitor,
		// Token: 0x040020AE RID: 8366
		Executioner,
		// Token: 0x040020AF RID: 8367
		Moderator,
		// Token: 0x040020B0 RID: 8368
		Arbiter,
		// Token: 0x040020B1 RID: 8369
		Awakened,
		// Token: 0x040020B2 RID: 8370
		Sentinel = 50001
	}
}
