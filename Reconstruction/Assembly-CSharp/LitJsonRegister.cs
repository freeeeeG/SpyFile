using System;
using System.Runtime.CompilerServices;
using LitJson;
using UnityEngine;

// Token: 0x0200013A RID: 314
public static class LitJsonRegister
{
	// Token: 0x06000817 RID: 2071 RVA: 0x000152CF File Offset: 0x000134CF
	public static void Register()
	{
		LitJsonRegister.RegiterType();
		LitJsonRegister.RegisterFloat();
		LitJsonRegister.RegisterVector3();
		LitJsonRegister.RegisterVector2();
		LitJsonRegister.RegisterVector2Int();
		LitJsonRegister.RegisterQuaternion();
		LitJsonRegister.RegisterGameObject();
		LitJsonRegister.RegisterEnemySequence();
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x000152F9 File Offset: 0x000134F9
	public static void RegiterType()
	{
		JsonMapper.RegisterExporter<Type>(new ExporterFunc<Type>(LitJsonRegister.<RegiterType>g__Exporter|1_0));
		JsonMapper.RegisterImporter<string, Type>(new ImporterFunc<string, Type>(LitJsonRegister.<RegiterType>g__Importer|1_1));
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0001531D File Offset: 0x0001351D
	private static void RegisterFloat()
	{
		JsonMapper.RegisterExporter<float>(new ExporterFunc<float>(LitJsonRegister.<RegisterFloat>g__Exporter|2_0));
		JsonMapper.RegisterImporter<double, float>(new ImporterFunc<double, float>(LitJsonRegister.<RegisterFloat>g__Importer|2_1));
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00015341 File Offset: 0x00013541
	private static void RegisterVector3()
	{
		JsonMapper.RegisterExporter<Vector3>(new ExporterFunc<Vector3>(LitJsonRegister.<RegisterVector3>g__Exporter|3_0));
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00015354 File Offset: 0x00013554
	private static void RegisterVector2()
	{
		JsonMapper.RegisterExporter<Vector2>(new ExporterFunc<Vector2>(LitJsonRegister.<RegisterVector2>g__Exporter|4_0));
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00015367 File Offset: 0x00013567
	private static void RegisterVector2Int()
	{
		JsonMapper.RegisterExporter<Vector2Int>(new ExporterFunc<Vector2Int>(LitJsonRegister.<RegisterVector2Int>g__Exporter|5_0));
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0001537A File Offset: 0x0001357A
	private static void RegisterQuaternion()
	{
		JsonMapper.RegisterExporter<Quaternion>(new ExporterFunc<Quaternion>(LitJsonRegister.<RegisterQuaternion>g__Exporter|6_0));
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x0001538D File Offset: 0x0001358D
	private static void RegisterGameObject()
	{
		JsonMapper.RegisterExporter<GameObject>(new ExporterFunc<GameObject>(LitJsonRegister.<RegisterGameObject>g__Exporter|7_0));
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x000153A0 File Offset: 0x000135A0
	private static void RegisterEnemySequence()
	{
		JsonMapper.RegisterExporter<EnemySequence>(new ExporterFunc<EnemySequence>(LitJsonRegister.<RegisterEnemySequence>g__Exporter|8_0));
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000153B3 File Offset: 0x000135B3
	[CompilerGenerated]
	internal static void <RegiterType>g__Exporter|1_0(Type obj, JsonWriter writer)
	{
		writer.Write(obj.FullName);
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x000153C1 File Offset: 0x000135C1
	[CompilerGenerated]
	internal static Type <RegiterType>g__Importer|1_1(string obj)
	{
		return Type.GetType(obj);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x000153C9 File Offset: 0x000135C9
	[CompilerGenerated]
	internal static void <RegisterFloat>g__Exporter|2_0(float obj, JsonWriter writer)
	{
		writer.Write((double)obj);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x000153D3 File Offset: 0x000135D3
	[CompilerGenerated]
	internal static float <RegisterFloat>g__Importer|2_1(double obj)
	{
		return (float)obj;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x000153D8 File Offset: 0x000135D8
	[CompilerGenerated]
	internal static void <RegisterVector3>g__Exporter|3_0(Vector3 obj, JsonWriter writer)
	{
		writer.WriteObjectStart();
		writer.WritePropertyName("x");
		writer.Write((double)obj.x);
		writer.WritePropertyName("y");
		writer.Write((double)obj.y);
		writer.WritePropertyName("z");
		writer.Write((double)obj.z);
		writer.WriteObjectEnd();
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00015439 File Offset: 0x00013639
	[CompilerGenerated]
	internal static void <RegisterVector2>g__Exporter|4_0(Vector2 obj, JsonWriter writer)
	{
		writer.WriteObjectStart();
		writer.WritePropertyName("x");
		writer.Write((double)obj.x);
		writer.WritePropertyName("y");
		writer.Write((double)obj.y);
		writer.WriteObjectEnd();
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00015477 File Offset: 0x00013677
	[CompilerGenerated]
	internal static void <RegisterVector2Int>g__Exporter|5_0(Vector2Int obj, JsonWriter writer)
	{
		writer.WriteObjectStart();
		writer.WritePropertyName("x");
		writer.Write(obj.x);
		writer.WritePropertyName("y");
		writer.Write(obj.y);
		writer.WriteObjectEnd();
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x000154B8 File Offset: 0x000136B8
	[CompilerGenerated]
	internal static void <RegisterQuaternion>g__Exporter|6_0(Quaternion obj, JsonWriter writer)
	{
		writer.WriteObjectStart();
		writer.WritePropertyName("x");
		writer.Write((double)obj.x);
		writer.WritePropertyName("y");
		writer.Write((double)obj.y);
		writer.WritePropertyName("z");
		writer.Write((double)obj.z);
		writer.WritePropertyName("w");
		writer.Write((double)obj.w);
		writer.WriteObjectEnd();
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00015531 File Offset: 0x00013731
	[CompilerGenerated]
	internal static void <RegisterGameObject>g__Exporter|7_0(GameObject obj, JsonWriter writer)
	{
		writer.WriteObjectStart();
		writer.WritePropertyName("name");
		if (obj != null)
		{
			writer.Write(obj.name);
		}
		else
		{
			writer.Write("Prefab为Null");
		}
		writer.WriteObjectEnd();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0001556C File Offset: 0x0001376C
	[CompilerGenerated]
	internal static void <RegisterEnemySequence>g__Exporter|8_0(EnemySequence obj, JsonWriter writer)
	{
		writer.WriteObjectStart();
		writer.WritePropertyName("EnemyType");
		writer.Write((int)obj.EnemyType);
		writer.WritePropertyName("Amount");
		writer.Write(obj.Amount);
		writer.WritePropertyName("CoolDown");
		writer.Write((double)obj.CoolDown);
		writer.WritePropertyName("Intensify");
		writer.Write((double)obj.Intensify);
		writer.WritePropertyName("IsBoss");
		writer.Write(obj.IsBoss);
		writer.WritePropertyName("Wave");
		writer.Write(obj.Wave);
		writer.WritePropertyName("DmgResist");
		writer.Write((double)obj.DmgResist);
		writer.WriteObjectEnd();
	}
}
