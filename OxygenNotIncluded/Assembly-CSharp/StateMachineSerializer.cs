using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;

// Token: 0x0200043A RID: 1082
public class StateMachineSerializer
{
	// Token: 0x060016DC RID: 5852 RVA: 0x000765BC File Offset: 0x000747BC
	public void Serialize(List<StateMachine.Instance> state_machines, BinaryWriter writer)
	{
		writer.Write(StateMachineSerializer.SERIALIZER_VERSION);
		long position = writer.BaseStream.Position;
		writer.Write(0);
		long position2 = writer.BaseStream.Position;
		try
		{
			int num = (int)writer.BaseStream.Position;
			int num2 = 0;
			writer.Write(num2);
			using (List<StateMachine.Instance>.Enumerator enumerator = state_machines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (StateMachineSerializer.Entry.TrySerialize(enumerator.Current, writer))
					{
						num2++;
					}
				}
			}
			int num3 = (int)writer.BaseStream.Position;
			writer.BaseStream.Position = (long)num;
			writer.Write(num2);
			writer.BaseStream.Position = (long)num3;
		}
		catch (Exception obj)
		{
			Debug.Log("StateMachines: ");
			foreach (StateMachine.Instance instance in state_machines)
			{
				Debug.Log(instance.ToString());
			}
			Debug.LogError(obj);
		}
		long position3 = writer.BaseStream.Position;
		long num4 = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num4);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x0007671C File Offset: 0x0007491C
	public void Deserialize(IReader reader)
	{
		int num = reader.ReadInt32();
		int length = reader.ReadInt32();
		if (num < 10)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"State machine serializer version mismatch: ",
				num.ToString(),
				"!=",
				StateMachineSerializer.SERIALIZER_VERSION.ToString(),
				"\nDiscarding data."
			}));
			reader.SkipBytes(length);
			return;
		}
		if (num < 12)
		{
			this.entries = StateMachineSerializer.OldEntryV11.DeserializeOldEntries(reader, num);
			return;
		}
		int num2 = reader.ReadInt32();
		this.entries = new List<StateMachineSerializer.Entry>(num2);
		for (int i = 0; i < num2; i++)
		{
			StateMachineSerializer.Entry entry = StateMachineSerializer.Entry.Deserialize(reader, num);
			if (entry != null)
			{
				this.entries.Add(entry);
			}
		}
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x000767D0 File Offset: 0x000749D0
	private static string TrimAssemblyInfo(string type_name)
	{
		int num = type_name.IndexOf("[[");
		if (num != -1)
		{
			return type_name.Substring(0, num);
		}
		return type_name;
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x000767F8 File Offset: 0x000749F8
	public bool Restore(StateMachine.Instance instance)
	{
		Type type = instance.GetType();
		for (int i = 0; i < this.entries.Count; i++)
		{
			StateMachineSerializer.Entry entry = this.entries[i];
			if (entry.type == type && instance.serializationSuffix == entry.typeSuffix)
			{
				this.entries.RemoveAt(i);
				return entry.Restore(instance);
			}
		}
		return false;
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x00076865 File Offset: 0x00074A65
	private static bool DoesVersionHaveTypeSuffix(int version)
	{
		return version >= 20 || version == 11;
	}

	// Token: 0x04000CAE RID: 3246
	public const int SERIALIZER_PRE_DLC1 = 10;

	// Token: 0x04000CAF RID: 3247
	public const int SERIALIZER_TYPE_SUFFIX = 11;

	// Token: 0x04000CB0 RID: 3248
	public const int SERIALIZER_OPTIMIZE_BUFFERS = 12;

	// Token: 0x04000CB1 RID: 3249
	public const int SERIALIZER_EXPANSION1 = 20;

	// Token: 0x04000CB2 RID: 3250
	private static int SERIALIZER_VERSION = 20;

	// Token: 0x04000CB3 RID: 3251
	private const string TargetParameterName = "TargetParameter";

	// Token: 0x04000CB4 RID: 3252
	private List<StateMachineSerializer.Entry> entries = new List<StateMachineSerializer.Entry>();

	// Token: 0x020010C8 RID: 4296
	private class Entry
	{
		// Token: 0x06007774 RID: 30580 RVA: 0x002D391C File Offset: 0x002D1B1C
		public static bool TrySerialize(StateMachine.Instance smi, BinaryWriter writer)
		{
			if (!smi.IsRunning())
			{
				return false;
			}
			int num = (int)writer.BaseStream.Position;
			writer.Write(0);
			writer.WriteKleiString(smi.GetType().FullName);
			writer.WriteKleiString(smi.serializationSuffix);
			writer.WriteKleiString(smi.GetCurrentState().name);
			int num2 = (int)writer.BaseStream.Position;
			writer.Write(0);
			int num3 = (int)writer.BaseStream.Position;
			Serializer.SerializeTypeless(smi, writer);
			if (smi.GetStateMachine().serializable == StateMachine.SerializeType.ParamsOnly || smi.GetStateMachine().serializable == StateMachine.SerializeType.Both_DEPRECATED)
			{
				StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
				writer.Write(parameterContexts.Length);
				foreach (StateMachine.Parameter.Context context in parameterContexts)
				{
					long position = (long)((int)writer.BaseStream.Position);
					writer.Write(0);
					long num4 = (long)((int)writer.BaseStream.Position);
					writer.WriteKleiString(context.GetType().FullName);
					writer.WriteKleiString(context.parameter.name);
					context.Serialize(writer);
					long num5 = (long)((int)writer.BaseStream.Position);
					writer.BaseStream.Position = position;
					long num6 = num5 - num4;
					writer.Write((int)num6);
					writer.BaseStream.Position = num5;
				}
			}
			int num7 = (int)writer.BaseStream.Position - num3;
			if (num7 > 0)
			{
				int num8 = (int)writer.BaseStream.Position;
				writer.BaseStream.Position = (long)num2;
				writer.Write(num7);
				writer.BaseStream.Position = (long)num8;
				return true;
			}
			writer.BaseStream.Position = (long)num;
			writer.BaseStream.SetLength((long)num);
			return false;
		}

		// Token: 0x06007775 RID: 30581 RVA: 0x002D3ADC File Offset: 0x002D1CDC
		public static StateMachineSerializer.Entry Deserialize(IReader reader, int serializerVersion)
		{
			StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
			reader.ReadInt32();
			entry.version = serializerVersion;
			string typeName = reader.ReadKleiString();
			entry.type = Type.GetType(typeName);
			entry.typeSuffix = (StateMachineSerializer.DoesVersionHaveTypeSuffix(serializerVersion) ? reader.ReadKleiString() : null);
			entry.currentState = reader.ReadKleiString();
			int length = reader.ReadInt32();
			entry.entryData = new FastReader(reader.ReadBytes(length));
			if (entry.type == null)
			{
				return null;
			}
			return entry;
		}

		// Token: 0x06007776 RID: 30582 RVA: 0x002D3B60 File Offset: 0x002D1D60
		public bool Restore(StateMachine.Instance smi)
		{
			if (Manager.HasDeserializationMapping(smi.GetType()))
			{
				Deserializer.DeserializeTypeless(smi, this.entryData);
			}
			StateMachine.SerializeType serializable = smi.GetStateMachine().serializable;
			if (serializable == StateMachine.SerializeType.Never)
			{
				return false;
			}
			if ((serializable == StateMachine.SerializeType.Both_DEPRECATED || serializable == StateMachine.SerializeType.ParamsOnly) && !this.entryData.IsFinished)
			{
				StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
				int num = this.entryData.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int num2 = this.entryData.ReadInt32();
					int position = this.entryData.Position;
					string text = this.entryData.ReadKleiString();
					text = text.Replace("Version=2.0.0.0", "Version=4.0.0.0");
					string b = this.entryData.ReadKleiString();
					foreach (StateMachine.Parameter.Context context in parameterContexts)
					{
						if (context.parameter.name == b && (this.version > 10 || !(context.parameter.GetType().Name == "TargetParameter")) && context.GetType().FullName == text)
						{
							context.Deserialize(this.entryData, smi);
							break;
						}
					}
					this.entryData.SkipBytes(num2 - (this.entryData.Position - position));
				}
			}
			if (serializable == StateMachine.SerializeType.Both_DEPRECATED || serializable == StateMachine.SerializeType.CurrentStateOnly_DEPRECATED)
			{
				StateMachine.BaseState state = smi.GetStateMachine().GetState(this.currentState);
				if (state != null)
				{
					smi.GoTo(state);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005A0C RID: 23052
		public int version;

		// Token: 0x04005A0D RID: 23053
		public Type type;

		// Token: 0x04005A0E RID: 23054
		public string typeSuffix;

		// Token: 0x04005A0F RID: 23055
		public string currentState;

		// Token: 0x04005A10 RID: 23056
		public FastReader entryData;
	}

	// Token: 0x020010C9 RID: 4297
	private class OldEntryV11
	{
		// Token: 0x06007778 RID: 30584 RVA: 0x002D3CEB File Offset: 0x002D1EEB
		public OldEntryV11(int version, int dataPos, Type type, string typeSuffix, string currentState)
		{
			this.version = version;
			this.dataPos = dataPos;
			this.type = type;
			this.typeSuffix = typeSuffix;
			this.currentState = currentState;
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x002D3D18 File Offset: 0x002D1F18
		public static List<StateMachineSerializer.Entry> DeserializeOldEntries(IReader reader, int serializerVersion)
		{
			Debug.Assert(serializerVersion < 12);
			List<StateMachineSerializer.OldEntryV11> list = StateMachineSerializer.OldEntryV11.ReadEntries(reader, serializerVersion);
			byte[] bytes = StateMachineSerializer.OldEntryV11.ReadEntryData(reader);
			List<StateMachineSerializer.Entry> list2 = new List<StateMachineSerializer.Entry>(list.Count);
			foreach (StateMachineSerializer.OldEntryV11 oldEntryV in list)
			{
				StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
				entry.version = serializerVersion;
				entry.type = oldEntryV.type;
				entry.typeSuffix = oldEntryV.typeSuffix;
				entry.currentState = oldEntryV.currentState;
				entry.entryData = new FastReader(bytes);
				entry.entryData.SkipBytes(oldEntryV.dataPos);
				list2.Add(entry);
			}
			return list2;
		}

		// Token: 0x0600777A RID: 30586 RVA: 0x002D3DE0 File Offset: 0x002D1FE0
		private static StateMachineSerializer.OldEntryV11 Deserialize(IReader reader, int serializerVersion)
		{
			int num = reader.ReadInt32();
			int num2 = reader.ReadInt32();
			string typeName = reader.ReadKleiString();
			string text = StateMachineSerializer.DoesVersionHaveTypeSuffix(serializerVersion) ? reader.ReadKleiString() : null;
			string text2 = reader.ReadKleiString();
			Type left = Type.GetType(typeName);
			if (left == null)
			{
				return null;
			}
			return new StateMachineSerializer.OldEntryV11(num, num2, left, text, text2);
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x002D3E38 File Offset: 0x002D2038
		private static List<StateMachineSerializer.OldEntryV11> ReadEntries(IReader reader, int serializerVersion)
		{
			List<StateMachineSerializer.OldEntryV11> list = new List<StateMachineSerializer.OldEntryV11>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				StateMachineSerializer.OldEntryV11 oldEntryV = StateMachineSerializer.OldEntryV11.Deserialize(reader, serializerVersion);
				if (oldEntryV != null)
				{
					list.Add(oldEntryV);
				}
			}
			return list;
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x002D3E74 File Offset: 0x002D2074
		private static byte[] ReadEntryData(IReader reader)
		{
			int length = reader.ReadInt32();
			return reader.ReadBytes(length);
		}

		// Token: 0x04005A11 RID: 23057
		public int version;

		// Token: 0x04005A12 RID: 23058
		public int dataPos;

		// Token: 0x04005A13 RID: 23059
		public Type type;

		// Token: 0x04005A14 RID: 23060
		public string typeSuffix;

		// Token: 0x04005A15 RID: 23061
		public string currentState;
	}
}
