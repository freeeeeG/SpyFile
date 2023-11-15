using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public static class ConstructHelper
{
	// Token: 0x060004B4 RID: 1204 RVA: 0x0000CA0D File Offset: 0x0000AC0D
	public static void Initialize()
	{
		ConstructHelper.m_TileFactory = Singleton<StaticData>.Instance.TileFactory;
		ConstructHelper.m_ShapeFactory = Singleton<StaticData>.Instance.ShapeFactory;
		ConstructHelper.m_ContentFactory = Singleton<StaticData>.Instance.ContentFactory;
		ConstructHelper.m_StrategyFactory = Singleton<StaticData>.Instance.StrategyFactory;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0000CA4C File Offset: 0x0000AC4C
	public static TileShape GetRandomShape()
	{
		ShapeType[] array = Enum.GetValues(typeof(ShapeType)) as ShapeType[];
		float[] array2 = new float[5];
		for (int i = 0; i < 5; i++)
		{
			array2[i] = StaticData.QualityChances[GameRes.SystemLevel - 1, i];
		}
		ShapeInfo shapeInfo = new ShapeInfo();
		shapeInfo.ShapeType = (int)array[Random.Range(0, array.Length - 1)];
		shapeInfo.Element = Random.Range(0, 5);
		shapeInfo.Quality = StaticData.RandomNumber(array2) + 1;
		shapeInfo.TurretPos = Random.Range(0, 3);
		shapeInfo.TurretDir = Random.Range(0, 3);
		TileShape shape = ConstructHelper.m_ShapeFactory.GetShape(shapeInfo);
		GameTile elementTurret = ConstructHelper.GetElementTurret((ElementType)shapeInfo.Element, shapeInfo.Quality);
		shape.SetTile(elementTurret, shapeInfo.TurretPos, shapeInfo.TurretDir);
		return shape;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0000CB1C File Offset: 0x0000AD1C
	public static GameTile GetNormalTile(GameTileContentType type)
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		GameTileContent basicContent = ConstructHelper.m_ContentFactory.GetBasicContent(type);
		basicTile.SetContent(basicContent);
		return basicTile;
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0000CB46 File Offset: 0x0000AD46
	public static GroundTile GetGroundTile()
	{
		return ConstructHelper.m_TileFactory.GetGroundTile();
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0000CB52 File Offset: 0x0000AD52
	public static TurretAttribute GetElementAttribute(ElementType element)
	{
		return ConstructHelper.m_ContentFactory.GetElementAttribute(element);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0000CB60 File Offset: 0x0000AD60
	public static GameTile GetRandomTrap()
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		GameTileContent randomTrapContent = ConstructHelper.m_ContentFactory.GetRandomTrapContent();
		basicTile.SetContent(randomTrapContent);
		return basicTile;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0000CB8C File Offset: 0x0000AD8C
	public static GameTile GetSpecificTrap(string trapName)
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		GameTileContent trapContentByName = ConstructHelper.m_ContentFactory.GetTrapContentByName(trapName, false);
		basicTile.SetContent(trapContentByName);
		return basicTile;
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
	public static GameTile GetSpawnPoint()
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		GameTileContent spawnPoint = ConstructHelper.m_ContentFactory.GetSpawnPoint();
		basicTile.SetContent(spawnPoint);
		return basicTile;
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
	public static GameTile GetDestinationPoint()
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		GameTileContent destinationPoint = ConstructHelper.m_ContentFactory.GetDestinationPoint();
		basicTile.SetContent(destinationPoint);
		return basicTile;
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0000CC10 File Offset: 0x0000AE10
	public static RefactorStrategy GetRandomRefactorStrategy()
	{
		TurretAttribute randomCompositeAtt = ConstructHelper.m_ContentFactory.GetRandomCompositeAtt();
		RefactorStrategy randomRefactorStrategy = ConstructHelper.m_StrategyFactory.GetRandomRefactorStrategy(randomCompositeAtt);
		randomRefactorStrategy.AddElementSkill(ConstructHelper.GetElementSkillBaseOnComposition(randomRefactorStrategy.Compositions));
		TurretSkillFactory.AddGlobalSkillToStrategy(randomRefactorStrategy);
		return randomRefactorStrategy;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0000CC4C File Offset: 0x0000AE4C
	public static ElementSkill GetElementSkillBaseOnComposition(List<Composition> compositions)
	{
		List<int> list = new List<int>();
		foreach (Composition composition in compositions)
		{
			list.Add(composition.elementRequirement);
		}
		return TurretSkillFactory.GetElementSkill(list);
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0000CCAC File Offset: 0x0000AEAC
	public static TileShape GetRefactorTurretByStrategy(RefactorStrategy strategy)
	{
		TileShape dshape = ConstructHelper.m_ShapeFactory.GetDShape();
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		RefactorTurret refactorTurret = ConstructHelper.m_ContentFactory.GetRefactorTurret(strategy);
		basicTile.SetContent(refactorTurret);
		dshape.SetTile(basicTile, -1, -1);
		return dshape;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0000CCEA File Offset: 0x0000AEEA
	public static GameTile GetTileWithContent(GameTileContent content)
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		basicTile.SetContent(content);
		return basicTile;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0000CCFD File Offset: 0x0000AEFD
	public static TileShape GetTrapShapeByName(string name)
	{
		TileShape dshape = ConstructHelper.m_ShapeFactory.GetDShape();
		dshape.SetTile(ConstructHelper.GetTrap(name, true), -1, -1);
		return dshape;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0000CD18 File Offset: 0x0000AF18
	public static TileShape GetRefactorTurretByNameAndElement(string name, int e1, int e2, int e3)
	{
		TurretAttribute refactorByString = ConstructHelper.m_ContentFactory.GetRefactorByString(name);
		List<int> elements = new List<int>
		{
			e1,
			e2,
			e3
		};
		List<int> qualities = new List<int>
		{
			1,
			1,
			1
		};
		RefactorStrategy specificRefactorStrategy = ConstructHelper.m_StrategyFactory.GetSpecificRefactorStrategy(refactorByString, elements, qualities, 1);
		ElementSkill elementSkill = TurretSkillFactory.GetElementSkill(elements);
		specificRefactorStrategy.AddElementSkill(elementSkill);
		TurretSkillFactory.AddGlobalSkillToStrategy(specificRefactorStrategy);
		return ConstructHelper.GetRefactorTurretByStrategy(specificRefactorStrategy);
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0000CD90 File Offset: 0x0000AF90
	public static TileShape GetElementTurretByQualityAndElement(ElementType element, int quality)
	{
		TileShape dshape = ConstructHelper.m_ShapeFactory.GetDShape();
		GameTile elementTurret = ConstructHelper.GetElementTurret(element, quality);
		dshape.SetTile(elementTurret, -1, -1);
		return dshape;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
	public static TileShape GetShapeByContent(GameTileContent content)
	{
		TileShape dshape = ConstructHelper.m_ShapeFactory.GetDShape();
		dshape.SetTile(content.m_GameTile, -1, -1);
		return dshape;
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
	public static TileShape GetTutorialShape(ShapeInfo shapeInfo, bool levelDown = false)
	{
		TileShape shape = ConstructHelper.m_ShapeFactory.GetShape(shapeInfo);
		GameTile elementTurret = ConstructHelper.GetElementTurret((ElementType)shapeInfo.Element, levelDown ? (shapeInfo.Quality - 1) : shapeInfo.Quality);
		shape.SetTile(elementTurret, shapeInfo.TurretPos, shapeInfo.TurretDir);
		return shape;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0000CE20 File Offset: 0x0000B020
	public static RefactorStrategy GetSpecificStrategyByString(string name, List<int> elements, List<int> qualities, int quality = 1)
	{
		TurretAttribute refactorByString = ConstructHelper.m_ContentFactory.GetRefactorByString(name);
		RefactorStrategy specificRefactorStrategy = ConstructHelper.m_StrategyFactory.GetSpecificRefactorStrategy(refactorByString, elements, qualities, quality);
		TurretSkillFactory.AddGlobalSkillToStrategy(specificRefactorStrategy);
		return specificRefactorStrategy;
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0000CE50 File Offset: 0x0000B050
	public static GameTile GetElementTurret(ElementType element, int quality)
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		ElementTurret elementTurret = ConstructHelper.m_ContentFactory.GetElementTurret(element, quality);
		basicTile.SetContent(elementTurret);
		return basicTile;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0000CE7C File Offset: 0x0000B07C
	public static GameTile GetElementTurret(ContentStruct contentStruct)
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		ElementTurret elementTurret = ConstructHelper.m_ContentFactory.GetElementTurret((ElementType)contentStruct.Element, contentStruct.Quality);
		basicTile.SetContent(elementTurret);
		elementTurret.Strategy.TotalDamage = long.Parse(contentStruct.TotalDamage);
		return basicTile;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
	public static GameTile GetTrap(string name, bool isReveal)
	{
		GameTileContent trapContentByName = ConstructHelper.m_ContentFactory.GetTrapContentByName(name, isReveal);
		if (trapContentByName == null)
		{
			return null;
		}
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		basicTile.SetContent(trapContentByName);
		return basicTile;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0000CF00 File Offset: 0x0000B100
	public static GameTile GetRefactorTurret(ContentStruct contentStruct)
	{
		GameTile basicTile = ConstructHelper.m_TileFactory.GetBasicTile();
		RefactorStrategy specificStrategyByString = ConstructHelper.GetSpecificStrategyByString(contentStruct.ContentName, new List<int>
		{
			1,
			1,
			1
		}, new List<int>
		{
			1,
			1,
			1
		}, contentStruct.Quality);
		RefactorTurret refactorTurret = ConstructHelper.m_ContentFactory.GetRefactorTurret(specificStrategyByString);
		specificStrategyByString.PrivateExtraSlot = contentStruct.ExtraSlot;
		basicTile.SetContent(refactorTurret);
		for (int i = 0; i < contentStruct.SkillList.Count; i++)
		{
			ElementSkill elementSkill = TurretSkillFactory.GetElementSkill(contentStruct.SkillList[(i + 1).ToString()]);
			elementSkill.Elements = contentStruct.ElementsList[(i + 1).ToString()];
			elementSkill.IsException = contentStruct.IsException[(i + 1).ToString()];
			refactorTurret.Strategy.AddElementSkill(elementSkill);
			((BasicTile)basicTile).EquipTurret(contentStruct.SkillList.Count);
		}
		specificStrategyByString.TotalDamage = long.Parse(contentStruct.TotalDamage);
		return basicTile;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0000D028 File Offset: 0x0000B228
	public static TileShape GetSaveDragingContent(ContentStruct content)
	{
		GameTile specialTile;
		switch (content.ContentType)
		{
		case 3:
			specialTile = ConstructHelper.GetElementTurret(content);
			break;
		case 4:
			specialTile = ConstructHelper.GetRefactorTurret(content);
			break;
		case 5:
			specialTile = ConstructHelper.GetTrap(content.ContentName, content.TrapRevealed);
			break;
		default:
			Debug.Log("错误的保存类型");
			specialTile = ConstructHelper.m_TileFactory.GetBasicTile();
			break;
		}
		TileShape dshape = ConstructHelper.m_ShapeFactory.GetDShape();
		dshape.SetTile(specialTile, -1, -1);
		return dshape;
	}

	// Token: 0x040001CC RID: 460
	private static TileFactory m_TileFactory;

	// Token: 0x040001CD RID: 461
	private static TileShapeFactory m_ShapeFactory;

	// Token: 0x040001CE RID: 462
	private static TileContentFactory m_ContentFactory;

	// Token: 0x040001CF RID: 463
	private static TurretStrategyFactory m_StrategyFactory;
}
