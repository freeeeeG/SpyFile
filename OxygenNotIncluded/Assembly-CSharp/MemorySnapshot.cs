using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x0200085E RID: 2142
public class MemorySnapshot
{
	// Token: 0x06003EA1 RID: 16033 RVA: 0x0015BA10 File Offset: 0x00159C10
	public static MemorySnapshot.TypeData GetTypeData(Type type, Dictionary<int, MemorySnapshot.TypeData> types)
	{
		int hashCode = type.GetHashCode();
		MemorySnapshot.TypeData typeData = null;
		if (!types.TryGetValue(hashCode, out typeData))
		{
			typeData = new MemorySnapshot.TypeData(type);
			types[hashCode] = typeData;
		}
		return typeData;
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0015BA44 File Offset: 0x00159C44
	public static void IncrementFieldCount(Dictionary<int, MemorySnapshot.FieldCount> field_counts, string name)
	{
		int hashCode = name.GetHashCode();
		MemorySnapshot.FieldCount fieldCount = null;
		if (!field_counts.TryGetValue(hashCode, out fieldCount))
		{
			fieldCount = new MemorySnapshot.FieldCount();
			fieldCount.name = name;
			field_counts[hashCode] = fieldCount;
		}
		fieldCount.count++;
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0015BA88 File Offset: 0x00159C88
	private void CountReference(MemorySnapshot.ReferenceArgs refArgs)
	{
		if (MemorySnapshot.ShouldExclude(refArgs.reference_type))
		{
			return;
		}
		if (refArgs.reference_type == MemorySnapshot.detailType)
		{
			string text;
			if (refArgs.lineage.obj as UnityEngine.Object != null)
			{
				text = "\"" + ((UnityEngine.Object)refArgs.lineage.obj).name;
			}
			else
			{
				text = "\"" + MemorySnapshot.detailTypeStr;
			}
			if (refArgs.lineage.parent0 != null)
			{
				text += "\",\"";
				text += refArgs.lineage.parent0.ToString();
			}
			if (refArgs.lineage.parent1 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent1.ToString();
			}
			if (refArgs.lineage.parent2 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent2.ToString();
			}
			if (refArgs.lineage.parent3 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent3.ToString();
			}
			if (refArgs.lineage.parent4 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent4.ToString();
			}
			text += "\"\n";
			MemorySnapshot.DetailInfo value;
			this.detailTypeCount.TryGetValue(text, out value);
			value.count++;
			if (typeof(Array).IsAssignableFrom(refArgs.reference_type) && refArgs.lineage.obj != null)
			{
				Array array = refArgs.lineage.obj as Array;
				value.numArrayEntries += ((array != null) ? array.Length : 0);
			}
			this.detailTypeCount[text] = value;
		}
		if (refArgs.reference_type.IsClass)
		{
			MemorySnapshot.GetTypeData(refArgs.reference_type, this.types).refCount++;
			MemorySnapshot.IncrementFieldCount(this.fieldCounts, refArgs.field_name);
		}
		if (refArgs.lineage.obj == null)
		{
			return;
		}
		try
		{
			if (refArgs.lineage.obj.GetType().IsClass && !this.walked.Add(refArgs.lineage.obj))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		MemorySnapshot.TypeData typeData = MemorySnapshot.GetTypeData(refArgs.lineage.obj.GetType(), this.types);
		if (typeData.type.IsClass)
		{
			typeData.instanceCount++;
			if (typeof(Array).IsAssignableFrom(typeData.type))
			{
				Array array2 = refArgs.lineage.obj as Array;
				typeData.numArrayEntries += ((array2 != null) ? array2.Length : 0);
			}
			MemorySnapshot.HierarchyNode key = new MemorySnapshot.HierarchyNode(refArgs.lineage.parent0, refArgs.lineage.parent1, refArgs.lineage.parent2, refArgs.lineage.parent3, refArgs.lineage.parent4);
			int num = 0;
			typeData.hierarchies.TryGetValue(key, out num);
			typeData.hierarchies[key] = num + 1;
		}
		foreach (FieldInfo fieldInfo in typeData.fields)
		{
			this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, new MemorySnapshot.Lineage(refArgs.lineage.obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, fieldInfo.DeclaringType)));
		}
		ICollection collection = refArgs.lineage.obj as ICollection;
		if (collection != null)
		{
			Type type = typeof(object);
			if (collection.GetType().GetElementType() != null)
			{
				type = collection.GetType().GetElementType();
			}
			else if (collection.GetType().GetGenericArguments().Length != 0)
			{
				type = collection.GetType().GetGenericArguments()[0];
			}
			if (!MemorySnapshot.ShouldExclude(type))
			{
				foreach (object obj in collection)
				{
					this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(type, refArgs.field_name + ".Item", new MemorySnapshot.Lineage(obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, collection.GetType())));
				}
			}
		}
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x0015BF90 File Offset: 0x0015A190
	private void CountField(MemorySnapshot.FieldArgs fieldArgs)
	{
		if (MemorySnapshot.ShouldExclude(fieldArgs.field.FieldType))
		{
			return;
		}
		object obj = null;
		try
		{
			if (!fieldArgs.field.FieldType.Name.Contains("*"))
			{
				obj = fieldArgs.field.GetValue(fieldArgs.lineage.obj);
			}
		}
		catch
		{
			obj = null;
		}
		string field_name = fieldArgs.field.DeclaringType.ToString() + "." + fieldArgs.field.Name;
		this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(fieldArgs.field.FieldType, field_name, new MemorySnapshot.Lineage(obj, fieldArgs.lineage.parent3, fieldArgs.lineage.parent2, fieldArgs.lineage.parent1, fieldArgs.lineage.parent0, fieldArgs.field.DeclaringType)));
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x0015C07C File Offset: 0x0015A27C
	private static bool ShouldExclude(Type type)
	{
		return type.IsPrimitive || type.IsEnum || type == typeof(MemorySnapshot);
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x0015C0A0 File Offset: 0x0015A2A0
	private void CountAll()
	{
		while (this.refsToProcess.Count > 0 || this.fieldsToProcess.Count > 0)
		{
			while (this.fieldsToProcess.Count > 0)
			{
				MemorySnapshot.FieldArgs fieldArgs = this.fieldsToProcess[this.fieldsToProcess.Count - 1];
				this.fieldsToProcess.RemoveAt(this.fieldsToProcess.Count - 1);
				this.CountField(fieldArgs);
			}
			while (this.refsToProcess.Count > 0)
			{
				MemorySnapshot.ReferenceArgs refArgs = this.refsToProcess[this.refsToProcess.Count - 1];
				this.refsToProcess.RemoveAt(this.refsToProcess.Count - 1);
				this.CountReference(refArgs);
			}
		}
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x0015C15C File Offset: 0x0015A35C
	public MemorySnapshot()
	{
		MemorySnapshot.Lineage lineage = new MemorySnapshot.Lineage(null, null, null, null, null, null);
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (fieldInfo.IsStatic)
				{
					this.statics.Add(fieldInfo);
					lineage.parent0 = fieldInfo.DeclaringType;
					this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, lineage));
				}
			}
		}
		this.CountAll();
		foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)))
		{
			lineage.obj = @object;
			lineage.parent0 = @object.GetType();
			this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(@object.GetType(), "Object." + @object.name, lineage));
		}
		this.CountAll();
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x0015C2CC File Offset: 0x0015A4CC
	public void WriteTypeDetails(MemorySnapshot compare)
	{
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list = null;
		if (compare != null)
		{
			list = compare.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		}
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list2 = this.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		list2.Sort((KeyValuePair<string, MemorySnapshot.DetailInfo> x, KeyValuePair<string, MemorySnapshot.DetailInfo> y) => y.Value.count - x.Value.count);
		using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("type_details_" + MemorySnapshot.detailTypeStr)))
		{
			streamWriter.WriteLine("Delta,Count,NumArrayEntries,Type");
			foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair in list2)
			{
				int num = keyValuePair.Value.count;
				if (list != null)
				{
					foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair2 in list)
					{
						if (keyValuePair2.Key == keyValuePair.Key)
						{
							num -= keyValuePair2.Value.count;
							break;
						}
					}
				}
				TextWriter textWriter = streamWriter;
				string[] array = new string[7];
				array[0] = num.ToString();
				array[1] = ",";
				int num2 = 2;
				MemorySnapshot.DetailInfo value = keyValuePair.Value;
				array[num2] = value.count.ToString();
				array[3] = ",";
				int num3 = 4;
				value = keyValuePair.Value;
				array[num3] = value.numArrayEntries.ToString();
				array[5] = ",";
				array[6] = keyValuePair.Key;
				textWriter.Write(string.Concat(array));
			}
		}
	}

	// Token: 0x04002894 RID: 10388
	public Dictionary<int, MemorySnapshot.TypeData> types = new Dictionary<int, MemorySnapshot.TypeData>();

	// Token: 0x04002895 RID: 10389
	public Dictionary<int, MemorySnapshot.FieldCount> fieldCounts = new Dictionary<int, MemorySnapshot.FieldCount>();

	// Token: 0x04002896 RID: 10390
	public HashSet<object> walked = new HashSet<object>();

	// Token: 0x04002897 RID: 10391
	public List<FieldInfo> statics = new List<FieldInfo>();

	// Token: 0x04002898 RID: 10392
	public Dictionary<string, MemorySnapshot.DetailInfo> detailTypeCount = new Dictionary<string, MemorySnapshot.DetailInfo>();

	// Token: 0x04002899 RID: 10393
	private static readonly Type detailType = typeof(byte[]);

	// Token: 0x0400289A RID: 10394
	private static readonly string detailTypeStr = MemorySnapshot.detailType.ToString();

	// Token: 0x0400289B RID: 10395
	private List<MemorySnapshot.FieldArgs> fieldsToProcess = new List<MemorySnapshot.FieldArgs>();

	// Token: 0x0400289C RID: 10396
	private List<MemorySnapshot.ReferenceArgs> refsToProcess = new List<MemorySnapshot.ReferenceArgs>();

	// Token: 0x0200162F RID: 5679
	public struct HierarchyNode
	{
		// Token: 0x060089EB RID: 35307 RVA: 0x00312680 File Offset: 0x00310880
		public HierarchyNode(Type parent_0, Type parent_1, Type parent_2, Type parent_3, Type parent_4)
		{
			this.parent0 = parent_0;
			this.parent1 = parent_1;
			this.parent2 = parent_2;
			this.parent3 = parent_3;
			this.parent4 = parent_4;
		}

		// Token: 0x060089EC RID: 35308 RVA: 0x003126A8 File Offset: 0x003108A8
		public bool Equals(MemorySnapshot.HierarchyNode a, MemorySnapshot.HierarchyNode b)
		{
			return a.parent0 == b.parent0 && a.parent1 == b.parent1 && a.parent2 == b.parent2 && a.parent3 == b.parent3 && a.parent4 == b.parent4;
		}

		// Token: 0x060089ED RID: 35309 RVA: 0x00312714 File Offset: 0x00310914
		public override int GetHashCode()
		{
			int num = 0;
			if (this.parent0 != null)
			{
				num += this.parent0.GetHashCode();
			}
			if (this.parent1 != null)
			{
				num += this.parent1.GetHashCode();
			}
			if (this.parent2 != null)
			{
				num += this.parent2.GetHashCode();
			}
			if (this.parent3 != null)
			{
				num += this.parent3.GetHashCode();
			}
			if (this.parent4 != null)
			{
				num += this.parent4.GetHashCode();
			}
			return num;
		}

		// Token: 0x060089EE RID: 35310 RVA: 0x003127B0 File Offset: 0x003109B0
		public override string ToString()
		{
			if (this.parent4 != null)
			{
				return string.Concat(new string[]
				{
					this.parent4.ToString(),
					"--",
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent3 != null)
			{
				return string.Concat(new string[]
				{
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent2 != null)
			{
				return string.Concat(new string[]
				{
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent1 != null)
			{
				return this.parent1.ToString() + "--" + this.parent0.ToString();
			}
			return this.parent0.ToString();
		}

		// Token: 0x04006ADF RID: 27359
		public Type parent0;

		// Token: 0x04006AE0 RID: 27360
		public Type parent1;

		// Token: 0x04006AE1 RID: 27361
		public Type parent2;

		// Token: 0x04006AE2 RID: 27362
		public Type parent3;

		// Token: 0x04006AE3 RID: 27363
		public Type parent4;
	}

	// Token: 0x02001630 RID: 5680
	public class FieldCount
	{
		// Token: 0x04006AE4 RID: 27364
		public string name;

		// Token: 0x04006AE5 RID: 27365
		public int count;
	}

	// Token: 0x02001631 RID: 5681
	public class TypeData
	{
		// Token: 0x060089F0 RID: 35312 RVA: 0x00312940 File Offset: 0x00310B40
		public TypeData(Type type)
		{
			this.type = type;
			this.fields = new List<FieldInfo>();
			this.instanceCount = 0;
			this.refCount = 0;
			this.numArrayEntries = 0;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (!fieldInfo.IsStatic && !MemorySnapshot.ShouldExclude(fieldInfo.FieldType))
				{
					this.fields.Add(fieldInfo);
				}
			}
		}

		// Token: 0x04006AE6 RID: 27366
		public Dictionary<MemorySnapshot.HierarchyNode, int> hierarchies = new Dictionary<MemorySnapshot.HierarchyNode, int>();

		// Token: 0x04006AE7 RID: 27367
		public Type type;

		// Token: 0x04006AE8 RID: 27368
		public List<FieldInfo> fields;

		// Token: 0x04006AE9 RID: 27369
		public int instanceCount;

		// Token: 0x04006AEA RID: 27370
		public int refCount;

		// Token: 0x04006AEB RID: 27371
		public int numArrayEntries;
	}

	// Token: 0x02001632 RID: 5682
	public struct DetailInfo
	{
		// Token: 0x04006AEC RID: 27372
		public int count;

		// Token: 0x04006AED RID: 27373
		public int numArrayEntries;
	}

	// Token: 0x02001633 RID: 5683
	private struct Lineage
	{
		// Token: 0x060089F1 RID: 35313 RVA: 0x003129C1 File Offset: 0x00310BC1
		public Lineage(object obj, Type parent4, Type parent3, Type parent2, Type parent1, Type parent0)
		{
			this.obj = obj;
			this.parent0 = parent0;
			this.parent1 = parent1;
			this.parent2 = parent2;
			this.parent3 = parent3;
			this.parent4 = parent4;
		}

		// Token: 0x04006AEE RID: 27374
		public object obj;

		// Token: 0x04006AEF RID: 27375
		public Type parent0;

		// Token: 0x04006AF0 RID: 27376
		public Type parent1;

		// Token: 0x04006AF1 RID: 27377
		public Type parent2;

		// Token: 0x04006AF2 RID: 27378
		public Type parent3;

		// Token: 0x04006AF3 RID: 27379
		public Type parent4;
	}

	// Token: 0x02001634 RID: 5684
	private struct ReferenceArgs
	{
		// Token: 0x060089F2 RID: 35314 RVA: 0x003129F0 File Offset: 0x00310BF0
		public ReferenceArgs(Type reference_type, string field_name, MemorySnapshot.Lineage lineage)
		{
			this.reference_type = reference_type;
			this.lineage = lineage;
			this.field_name = field_name;
		}

		// Token: 0x04006AF4 RID: 27380
		public Type reference_type;

		// Token: 0x04006AF5 RID: 27381
		public string field_name;

		// Token: 0x04006AF6 RID: 27382
		public MemorySnapshot.Lineage lineage;
	}

	// Token: 0x02001635 RID: 5685
	private struct FieldArgs
	{
		// Token: 0x060089F3 RID: 35315 RVA: 0x00312A07 File Offset: 0x00310C07
		public FieldArgs(FieldInfo field, MemorySnapshot.Lineage lineage)
		{
			this.field = field;
			this.lineage = lineage;
		}

		// Token: 0x04006AF7 RID: 27383
		public FieldInfo field;

		// Token: 0x04006AF8 RID: 27384
		public MemorySnapshot.Lineage lineage;
	}
}
