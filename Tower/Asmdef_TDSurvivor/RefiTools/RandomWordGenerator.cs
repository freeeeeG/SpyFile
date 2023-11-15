using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001B4 RID: 436
	public static class RandomWordGenerator
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x0002D7C6 File Offset: 0x0002B9C6
		public static string GetRandomItemWithModifier()
		{
			return RandomWordGenerator.arr_Adjs[Random.Range(0, RandomWordGenerator.arr_Adjs.Length)] + " " + RandomWordGenerator.arr_Items[Random.Range(0, RandomWordGenerator.arr_Items.Length)];
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
		public static string GetRandomString(int wordCount)
		{
			string text = "";
			for (int i = 0; i < wordCount; i++)
			{
				text += RandomWordGenerator.arr_Items[Random.Range(0, RandomWordGenerator.arr_Items.Length)];
				if (i != wordCount - 1)
				{
					text += " ";
				}
			}
			return text;
		}

		// Token: 0x0400093F RID: 2367
		private static string[] arr_Adjs = new string[]
		{
			"powerful",
			"defensive",
			"offensive",
			"magical",
			"holy",
			"cursed",
			"ancient",
			"legendary",
			"rare",
			"epic",
			"unique",
			"enchanted",
			"mythic",
			"legendary",
			"rare",
			"enchanted",
			"mythic",
			"ancient",
			"cursed",
			"holy",
			"powerful",
			"offensive",
			"magical",
			"defensive",
			"unique",
			"rare",
			"epic",
			"enchanted",
			"mythic",
			"ancient",
			"cursed",
			"holy",
			"powerful",
			"offensive",
			"magical",
			"defensive",
			"unique",
			"rare",
			"epic",
			"enchanted",
			"mythic",
			"ancient",
			"cursed",
			"holy",
			"powerful",
			"offensive",
			"magical"
		};

		// Token: 0x04000940 RID: 2368
		private static string[] arr_Items = new string[]
		{
			"towers",
			"walls",
			"traps",
			"soldiers",
			"archers",
			"mages",
			"cannons",
			"knights",
			"goblins",
			"dragons",
			"skeletons",
			"zombies",
			"golems",
			"ogres",
			"wolves",
			"bears",
			"griffins",
			"giants",
			"trolls",
			"wyverns",
			"wizards",
			"banshees",
			"werewolves",
			"vampires",
			"demons",
			"angels",
			"elementals",
			"plants",
			"flowers",
			"poison",
			"fire",
			"ice",
			"lightning",
			"earth",
			"air",
			"water",
			"holy",
			"dark",
			"light",
			"necrotic",
			"healing",
			"shields",
			"potions",
			"artifacts",
			"relics",
			"scrolls",
			"runes",
			"talismans",
			"charms"
		};
	}
}
