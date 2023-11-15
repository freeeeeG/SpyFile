using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
public static class Sim
{
	// Token: 0x06005213 RID: 21011 RVA: 0x001D5C54 File Offset: 0x001D3E54
	public static bool IsRadiationEnabled()
	{
		return DlcManager.FeatureRadiationEnabled();
	}

	// Token: 0x06005214 RID: 21012 RVA: 0x001D5C5B File Offset: 0x001D3E5B
	public static bool IsValidHandle(int h)
	{
		return h != -1 && h != -2;
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x001D5C6B File Offset: 0x001D3E6B
	public static int GetHandleIndex(int h)
	{
		return h & 16777215;
	}

	// Token: 0x06005216 RID: 21014
	[DllImport("SimDLL")]
	public static extern void SIM_Initialize(Sim.GAME_MessageHandler callback);

	// Token: 0x06005217 RID: 21015
	[DllImport("SimDLL")]
	public static extern void SIM_Shutdown();

	// Token: 0x06005218 RID: 21016
	[DllImport("SimDLL")]
	public unsafe static extern IntPtr SIM_HandleMessage(int sim_msg_id, int msg_length, byte* msg);

	// Token: 0x06005219 RID: 21017
	[DllImport("SimDLL")]
	private unsafe static extern byte* SIM_BeginSave(int* size, int x, int y);

	// Token: 0x0600521A RID: 21018
	[DllImport("SimDLL")]
	private static extern void SIM_EndSave();

	// Token: 0x0600521B RID: 21019
	[DllImport("SimDLL")]
	public static extern void SIM_DebugCrash();

	// Token: 0x0600521C RID: 21020 RVA: 0x001D5C74 File Offset: 0x001D3E74
	public unsafe static IntPtr HandleMessage(SimMessageHashes sim_msg_id, int msg_length, byte[] msg)
	{
		IntPtr result;
		fixed (byte[] array = msg)
		{
			byte* msg2;
			if (msg == null || array.Length == 0)
			{
				msg2 = null;
			}
			else
			{
				msg2 = &array[0];
			}
			result = Sim.SIM_HandleMessage((int)sim_msg_id, msg_length, msg2);
		}
		return result;
	}

	// Token: 0x0600521D RID: 21021 RVA: 0x001D5CA4 File Offset: 0x001D3EA4
	public unsafe static void Save(BinaryWriter writer, int x, int y)
	{
		int num;
		void* value = (void*)Sim.SIM_BeginSave(&num, x, y);
		byte[] array = new byte[num];
		Marshal.Copy((IntPtr)value, array, 0, num);
		Sim.SIM_EndSave();
		writer.Write(num);
		writer.Write(array);
	}

	// Token: 0x0600521E RID: 21022 RVA: 0x001D5CE4 File Offset: 0x001D3EE4
	public unsafe static int LoadWorld(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600521F RID: 21023 RVA: 0x001D5D34 File Offset: 0x001D3F34
	public static void AllocateCells(int width, int height, bool headless = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(8))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(width);
				binaryWriter.Write(height);
				bool value = Sim.IsRadiationEnabled();
				binaryWriter.Write(value);
				binaryWriter.Write(headless);
				binaryWriter.Flush();
				Sim.HandleMessage(SimMessageHashes.AllocateCells, (int)memoryStream.Length, memoryStream.GetBuffer());
			}
		}
	}

	// Token: 0x06005220 RID: 21024 RVA: 0x001D5DC4 File Offset: 0x001D3FC4
	public unsafe static int Load(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x001D5E14 File Offset: 0x001D4014
	public unsafe static void Start()
	{
		Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)Sim.SIM_HandleMessage(-931446686, 0, null));
		Grid.elementIdx = ptr->elementIdx;
		Grid.temperature = ptr->temperature;
		Grid.radiation = ptr->radiation;
		Grid.mass = ptr->mass;
		Grid.properties = ptr->properties;
		Grid.strengthInfo = ptr->strengthInfo;
		Grid.insulation = ptr->insulation;
		Grid.diseaseIdx = ptr->diseaseIdx;
		Grid.diseaseCount = ptr->diseaseCount;
		Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
		PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
		PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
		PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
		Grid.InitializeCells();
	}

	// Token: 0x06005222 RID: 21026 RVA: 0x001D5EC8 File Offset: 0x001D40C8
	public static void Shutdown()
	{
		Sim.SIM_Shutdown();
		Grid.mass = null;
	}

	// Token: 0x06005223 RID: 21027
	[DllImport("SimDLL")]
	public unsafe static extern char* SYSINFO_Acquire();

	// Token: 0x06005224 RID: 21028
	[DllImport("SimDLL")]
	public static extern void SYSINFO_Release();

	// Token: 0x06005225 RID: 21029 RVA: 0x001D5ED8 File Offset: 0x001D40D8
	public unsafe static int DLL_MessageHandler(int message_id, IntPtr data)
	{
		if (message_id == 0)
		{
			Sim.DLLExceptionHandlerMessage* ptr = (Sim.DLLExceptionHandlerMessage*)((void*)data);
			string stack_trace = Marshal.PtrToStringAnsi(ptr->callstack);
			string dmp_filename = Marshal.PtrToStringAnsi(ptr->dmpFilename);
			KCrashReporter.ReportSimDLLCrash("SimDLL Crash Dump", stack_trace, dmp_filename);
			return 0;
		}
		if (message_id == 1)
		{
			Sim.DLLReportMessageMessage* ptr2 = (Sim.DLLReportMessageMessage*)((void*)data);
			string msg = "SimMessage: " + Marshal.PtrToStringAnsi(ptr2->message);
			string stack_trace2;
			if (ptr2->callstack != IntPtr.Zero)
			{
				stack_trace2 = Marshal.PtrToStringAnsi(ptr2->callstack);
			}
			else
			{
				string str = Marshal.PtrToStringAnsi(ptr2->file);
				int line = ptr2->line;
				stack_trace2 = str + ":" + line.ToString();
			}
			KCrashReporter.ReportSimDLLCrash(msg, stack_trace2, null);
			return 0;
		}
		return -1;
	}

	// Token: 0x040035F7 RID: 13815
	public const int InvalidHandle = -1;

	// Token: 0x040035F8 RID: 13816
	public const int QueuedRegisterHandle = -2;

	// Token: 0x040035F9 RID: 13817
	public const byte InvalidDiseaseIdx = 255;

	// Token: 0x040035FA RID: 13818
	public const ushort InvalidElementIdx = 65535;

	// Token: 0x040035FB RID: 13819
	public const byte SpaceZoneID = 255;

	// Token: 0x040035FC RID: 13820
	public const byte SolidZoneID = 0;

	// Token: 0x040035FD RID: 13821
	public const int ChunkEdgeSize = 32;

	// Token: 0x040035FE RID: 13822
	public const float StateTransitionEnergy = 3f;

	// Token: 0x040035FF RID: 13823
	public const float ZeroDegreesCentigrade = 273.15f;

	// Token: 0x04003600 RID: 13824
	public const float StandardTemperature = 293.15f;

	// Token: 0x04003601 RID: 13825
	public const float StandardPressure = 101.3f;

	// Token: 0x04003602 RID: 13826
	public const float Epsilon = 0.0001f;

	// Token: 0x04003603 RID: 13827
	public const float MaxTemperature = 10000f;

	// Token: 0x04003604 RID: 13828
	public const float MinTemperature = 0f;

	// Token: 0x04003605 RID: 13829
	public const float MaxRadiation = 9000000f;

	// Token: 0x04003606 RID: 13830
	public const float MinRadiation = 0f;

	// Token: 0x04003607 RID: 13831
	public const float MaxMass = 10000f;

	// Token: 0x04003608 RID: 13832
	public const float MinMass = 1.0001f;

	// Token: 0x04003609 RID: 13833
	private const int PressureUpdateInterval = 1;

	// Token: 0x0400360A RID: 13834
	private const int TemperatureUpdateInterval = 1;

	// Token: 0x0400360B RID: 13835
	private const int LiquidUpdateInterval = 1;

	// Token: 0x0400360C RID: 13836
	private const int LifeUpdateInterval = 1;

	// Token: 0x0400360D RID: 13837
	public const byte ClearSkyGridValue = 253;

	// Token: 0x0400360E RID: 13838
	public const int PACKING_ALIGNMENT = 4;

	// Token: 0x0200194B RID: 6475
	// (Invoke) Token: 0x060094DE RID: 38110
	public delegate int GAME_MessageHandler(int message_id, IntPtr data);

	// Token: 0x0200194C RID: 6476
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLExceptionHandlerMessage
	{
		// Token: 0x04007507 RID: 29959
		public IntPtr callstack;

		// Token: 0x04007508 RID: 29960
		public IntPtr dmpFilename;
	}

	// Token: 0x0200194D RID: 6477
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLReportMessageMessage
	{
		// Token: 0x04007509 RID: 29961
		public IntPtr callstack;

		// Token: 0x0400750A RID: 29962
		public IntPtr message;

		// Token: 0x0400750B RID: 29963
		public IntPtr file;

		// Token: 0x0400750C RID: 29964
		public int line;
	}

	// Token: 0x0200194E RID: 6478
	private enum GameHandledMessages
	{
		// Token: 0x0400750E RID: 29966
		ExceptionHandler,
		// Token: 0x0400750F RID: 29967
		ReportMessage
	}

	// Token: 0x0200194F RID: 6479
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PhysicsData
	{
		// Token: 0x060094E1 RID: 38113 RVA: 0x00337DEB File Offset: 0x00335FEB
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.temperature);
			writer.Write(this.mass);
			writer.Write(this.pressure);
		}

		// Token: 0x04007510 RID: 29968
		public float temperature;

		// Token: 0x04007511 RID: 29969
		public float mass;

		// Token: 0x04007512 RID: 29970
		public float pressure;
	}

	// Token: 0x02001950 RID: 6480
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Cell
	{
		// Token: 0x060094E2 RID: 38114 RVA: 0x00337E14 File Offset: 0x00336014
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.elementIdx);
			writer.Write(0);
			writer.Write(this.insulation);
			writer.Write(0);
			writer.Write(this.pad0);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.temperature);
			writer.Write(this.mass);
		}

		// Token: 0x060094E3 RID: 38115 RVA: 0x00337E85 File Offset: 0x00336085
		public void SetValues(global::Element elem, List<global::Element> elements)
		{
			this.SetValues(elem, elem.defaultValues, elements);
		}

		// Token: 0x060094E4 RID: 38116 RVA: 0x00337E98 File Offset: 0x00336098
		public void SetValues(global::Element elem, Sim.PhysicsData pd, List<global::Element> elements)
		{
			this.elementIdx = (ushort)elements.IndexOf(elem);
			this.temperature = pd.temperature;
			this.mass = pd.mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x060094E5 RID: 38117 RVA: 0x00337F00 File Offset: 0x00336100
		public void SetValues(ushort new_elem_idx, float new_temperature, float new_mass)
		{
			this.elementIdx = new_elem_idx;
			this.temperature = new_temperature;
			this.mass = new_mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x04007513 RID: 29971
		public ushort elementIdx;

		// Token: 0x04007514 RID: 29972
		public byte properties;

		// Token: 0x04007515 RID: 29973
		public byte insulation;

		// Token: 0x04007516 RID: 29974
		public byte strengthInfo;

		// Token: 0x04007517 RID: 29975
		public byte pad0;

		// Token: 0x04007518 RID: 29976
		public byte pad1;

		// Token: 0x04007519 RID: 29977
		public byte pad2;

		// Token: 0x0400751A RID: 29978
		public float temperature;

		// Token: 0x0400751B RID: 29979
		public float mass;

		// Token: 0x02002225 RID: 8741
		public enum Properties
		{
			// Token: 0x040098BB RID: 39099
			GasImpermeable = 1,
			// Token: 0x040098BC RID: 39100
			LiquidImpermeable,
			// Token: 0x040098BD RID: 39101
			SolidImpermeable = 4,
			// Token: 0x040098BE RID: 39102
			Unbreakable = 8,
			// Token: 0x040098BF RID: 39103
			Transparent = 16,
			// Token: 0x040098C0 RID: 39104
			Opaque = 32,
			// Token: 0x040098C1 RID: 39105
			NotifyOnMelt = 64,
			// Token: 0x040098C2 RID: 39106
			ConstructedTile = 128
		}
	}

	// Token: 0x02001951 RID: 6481
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Element
	{
		// Token: 0x060094E6 RID: 38118 RVA: 0x00337F54 File Offset: 0x00336154
		public Element(global::Element e, List<global::Element> elements)
		{
			this.id = e.id;
			this.state = (byte)e.state;
			if (e.HasTag(GameTags.Unstable))
			{
				this.state |= 8;
			}
			int num = elements.FindIndex((global::Element ele) => ele.id == e.lowTempTransitionTarget);
			int num2 = elements.FindIndex((global::Element ele) => ele.id == e.highTempTransitionTarget);
			this.lowTempTransitionIdx = (ushort)((num >= 0) ? num : 65535);
			this.highTempTransitionIdx = (ushort)((num2 >= 0) ? num2 : 65535);
			this.elementsTableIdx = (ushort)elements.IndexOf(e);
			this.specificHeatCapacity = e.specificHeatCapacity;
			this.thermalConductivity = e.thermalConductivity;
			this.solidSurfaceAreaMultiplier = e.solidSurfaceAreaMultiplier;
			this.liquidSurfaceAreaMultiplier = e.liquidSurfaceAreaMultiplier;
			this.gasSurfaceAreaMultiplier = e.gasSurfaceAreaMultiplier;
			this.molarMass = e.molarMass;
			this.strength = e.strength;
			this.flow = e.flow;
			this.viscosity = e.viscosity;
			this.minHorizontalFlow = e.minHorizontalFlow;
			this.minVerticalFlow = e.minVerticalFlow;
			this.maxMass = e.maxMass;
			this.lowTemp = e.lowTemp;
			this.highTemp = e.highTemp;
			this.highTempTransitionOreID = e.highTempTransitionOreID;
			this.highTempTransitionOreMassConversion = e.highTempTransitionOreMassConversion;
			this.lowTempTransitionOreID = e.lowTempTransitionOreID;
			this.lowTempTransitionOreMassConversion = e.lowTempTransitionOreMassConversion;
			this.sublimateIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.sublimateId);
			this.convertIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.convertId);
			this.pack0 = 0;
			if (e.substance == null)
			{
				this.colour = 0U;
			}
			else
			{
				Color32 color = e.substance.colour;
				this.colour = (uint)((int)color.a << 24 | (int)color.b << 16 | (int)color.g << 8 | (int)color.r);
			}
			this.sublimateFX = e.sublimateFX;
			this.sublimateRate = e.sublimateRate;
			this.sublimateEfficiency = e.sublimateEfficiency;
			this.sublimateProbability = e.sublimateProbability;
			this.offGasProbability = e.offGasPercentage;
			this.lightAbsorptionFactor = e.lightAbsorptionFactor;
			this.radiationAbsorptionFactor = e.radiationAbsorptionFactor;
			this.radiationPer1000Mass = e.radiationPer1000Mass;
			this.defaultValues = e.defaultValues;
		}

		// Token: 0x060094E7 RID: 38119 RVA: 0x00338264 File Offset: 0x00336464
		public void Write(BinaryWriter writer)
		{
			writer.Write((int)this.id);
			writer.Write(this.lowTempTransitionIdx);
			writer.Write(this.highTempTransitionIdx);
			writer.Write(this.elementsTableIdx);
			writer.Write(this.state);
			writer.Write(this.pack0);
			writer.Write(this.specificHeatCapacity);
			writer.Write(this.thermalConductivity);
			writer.Write(this.molarMass);
			writer.Write(this.solidSurfaceAreaMultiplier);
			writer.Write(this.liquidSurfaceAreaMultiplier);
			writer.Write(this.gasSurfaceAreaMultiplier);
			writer.Write(this.flow);
			writer.Write(this.viscosity);
			writer.Write(this.minHorizontalFlow);
			writer.Write(this.minVerticalFlow);
			writer.Write(this.maxMass);
			writer.Write(this.lowTemp);
			writer.Write(this.highTemp);
			writer.Write(this.strength);
			writer.Write((int)this.lowTempTransitionOreID);
			writer.Write(this.lowTempTransitionOreMassConversion);
			writer.Write((int)this.highTempTransitionOreID);
			writer.Write(this.highTempTransitionOreMassConversion);
			writer.Write(this.sublimateIndex);
			writer.Write(this.convertIndex);
			writer.Write(this.colour);
			writer.Write((int)this.sublimateFX);
			writer.Write(this.sublimateRate);
			writer.Write(this.sublimateEfficiency);
			writer.Write(this.sublimateProbability);
			writer.Write(this.offGasProbability);
			writer.Write(this.lightAbsorptionFactor);
			writer.Write(this.radiationAbsorptionFactor);
			writer.Write(this.radiationPer1000Mass);
			this.defaultValues.Write(writer);
		}

		// Token: 0x0400751C RID: 29980
		public SimHashes id;

		// Token: 0x0400751D RID: 29981
		public ushort lowTempTransitionIdx;

		// Token: 0x0400751E RID: 29982
		public ushort highTempTransitionIdx;

		// Token: 0x0400751F RID: 29983
		public ushort elementsTableIdx;

		// Token: 0x04007520 RID: 29984
		public byte state;

		// Token: 0x04007521 RID: 29985
		public byte pack0;

		// Token: 0x04007522 RID: 29986
		public float specificHeatCapacity;

		// Token: 0x04007523 RID: 29987
		public float thermalConductivity;

		// Token: 0x04007524 RID: 29988
		public float molarMass;

		// Token: 0x04007525 RID: 29989
		public float solidSurfaceAreaMultiplier;

		// Token: 0x04007526 RID: 29990
		public float liquidSurfaceAreaMultiplier;

		// Token: 0x04007527 RID: 29991
		public float gasSurfaceAreaMultiplier;

		// Token: 0x04007528 RID: 29992
		public float flow;

		// Token: 0x04007529 RID: 29993
		public float viscosity;

		// Token: 0x0400752A RID: 29994
		public float minHorizontalFlow;

		// Token: 0x0400752B RID: 29995
		public float minVerticalFlow;

		// Token: 0x0400752C RID: 29996
		public float maxMass;

		// Token: 0x0400752D RID: 29997
		public float lowTemp;

		// Token: 0x0400752E RID: 29998
		public float highTemp;

		// Token: 0x0400752F RID: 29999
		public float strength;

		// Token: 0x04007530 RID: 30000
		public SimHashes lowTempTransitionOreID;

		// Token: 0x04007531 RID: 30001
		public float lowTempTransitionOreMassConversion;

		// Token: 0x04007532 RID: 30002
		public SimHashes highTempTransitionOreID;

		// Token: 0x04007533 RID: 30003
		public float highTempTransitionOreMassConversion;

		// Token: 0x04007534 RID: 30004
		public ushort sublimateIndex;

		// Token: 0x04007535 RID: 30005
		public ushort convertIndex;

		// Token: 0x04007536 RID: 30006
		public uint colour;

		// Token: 0x04007537 RID: 30007
		public SpawnFXHashes sublimateFX;

		// Token: 0x04007538 RID: 30008
		public float sublimateRate;

		// Token: 0x04007539 RID: 30009
		public float sublimateEfficiency;

		// Token: 0x0400753A RID: 30010
		public float sublimateProbability;

		// Token: 0x0400753B RID: 30011
		public float offGasProbability;

		// Token: 0x0400753C RID: 30012
		public float lightAbsorptionFactor;

		// Token: 0x0400753D RID: 30013
		public float radiationAbsorptionFactor;

		// Token: 0x0400753E RID: 30014
		public float radiationPer1000Mass;

		// Token: 0x0400753F RID: 30015
		public Sim.PhysicsData defaultValues;
	}

	// Token: 0x02001952 RID: 6482
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseCell
	{
		// Token: 0x060094E8 RID: 38120 RVA: 0x00338438 File Offset: 0x00336638
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.diseaseIdx);
			writer.Write(this.reservedInfestationTickCount);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.elementCount);
			writer.Write(this.reservedAccumulatedError);
		}

		// Token: 0x04007540 RID: 30016
		public byte diseaseIdx;

		// Token: 0x04007541 RID: 30017
		private byte reservedInfestationTickCount;

		// Token: 0x04007542 RID: 30018
		private byte pad1;

		// Token: 0x04007543 RID: 30019
		private byte pad2;

		// Token: 0x04007544 RID: 30020
		public int elementCount;

		// Token: 0x04007545 RID: 30021
		private float reservedAccumulatedError;

		// Token: 0x04007546 RID: 30022
		public static readonly Sim.DiseaseCell Invalid = new Sim.DiseaseCell
		{
			diseaseIdx = byte.MaxValue,
			elementCount = 0
		};
	}

	// Token: 0x02001953 RID: 6483
	// (Invoke) Token: 0x060094EB RID: 38123
	public delegate void GAME_Callback();

	// Token: 0x02001954 RID: 6484
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidInfo
	{
		// Token: 0x04007547 RID: 30023
		public int cellIdx;

		// Token: 0x04007548 RID: 30024
		public int isSolid;
	}

	// Token: 0x02001955 RID: 6485
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct LiquidChangeInfo
	{
		// Token: 0x04007549 RID: 30025
		public int cellIdx;
	}

	// Token: 0x02001956 RID: 6486
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidSubstanceChangeInfo
	{
		// Token: 0x0400754A RID: 30026
		public int cellIdx;
	}

	// Token: 0x02001957 RID: 6487
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SubstanceChangeInfo
	{
		// Token: 0x0400754B RID: 30027
		public int cellIdx;

		// Token: 0x0400754C RID: 30028
		public ushort oldElemIdx;

		// Token: 0x0400754D RID: 30029
		public ushort newElemIdx;
	}

	// Token: 0x02001958 RID: 6488
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CallbackInfo
	{
		// Token: 0x0400754E RID: 30030
		public int callbackIdx;
	}

	// Token: 0x02001959 RID: 6489
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameDataUpdate
	{
		// Token: 0x0400754F RID: 30031
		public int numFramesProcessed;

		// Token: 0x04007550 RID: 30032
		public unsafe ushort* elementIdx;

		// Token: 0x04007551 RID: 30033
		public unsafe float* temperature;

		// Token: 0x04007552 RID: 30034
		public unsafe float* mass;

		// Token: 0x04007553 RID: 30035
		public unsafe byte* properties;

		// Token: 0x04007554 RID: 30036
		public unsafe byte* insulation;

		// Token: 0x04007555 RID: 30037
		public unsafe byte* strengthInfo;

		// Token: 0x04007556 RID: 30038
		public unsafe float* radiation;

		// Token: 0x04007557 RID: 30039
		public unsafe byte* diseaseIdx;

		// Token: 0x04007558 RID: 30040
		public unsafe int* diseaseCount;

		// Token: 0x04007559 RID: 30041
		public int numSolidInfo;

		// Token: 0x0400755A RID: 30042
		public unsafe Sim.SolidInfo* solidInfo;

		// Token: 0x0400755B RID: 30043
		public int numLiquidChangeInfo;

		// Token: 0x0400755C RID: 30044
		public unsafe Sim.LiquidChangeInfo* liquidChangeInfo;

		// Token: 0x0400755D RID: 30045
		public int numSolidSubstanceChangeInfo;

		// Token: 0x0400755E RID: 30046
		public unsafe Sim.SolidSubstanceChangeInfo* solidSubstanceChangeInfo;

		// Token: 0x0400755F RID: 30047
		public int numSubstanceChangeInfo;

		// Token: 0x04007560 RID: 30048
		public unsafe Sim.SubstanceChangeInfo* substanceChangeInfo;

		// Token: 0x04007561 RID: 30049
		public int numCallbackInfo;

		// Token: 0x04007562 RID: 30050
		public unsafe Sim.CallbackInfo* callbackInfo;

		// Token: 0x04007563 RID: 30051
		public int numSpawnFallingLiquidInfo;

		// Token: 0x04007564 RID: 30052
		public unsafe Sim.SpawnFallingLiquidInfo* spawnFallingLiquidInfo;

		// Token: 0x04007565 RID: 30053
		public int numDigInfo;

		// Token: 0x04007566 RID: 30054
		public unsafe Sim.SpawnOreInfo* digInfo;

		// Token: 0x04007567 RID: 30055
		public int numSpawnOreInfo;

		// Token: 0x04007568 RID: 30056
		public unsafe Sim.SpawnOreInfo* spawnOreInfo;

		// Token: 0x04007569 RID: 30057
		public int numSpawnFXInfo;

		// Token: 0x0400756A RID: 30058
		public unsafe Sim.SpawnFXInfo* spawnFXInfo;

		// Token: 0x0400756B RID: 30059
		public int numUnstableCellInfo;

		// Token: 0x0400756C RID: 30060
		public unsafe Sim.UnstableCellInfo* unstableCellInfo;

		// Token: 0x0400756D RID: 30061
		public int numWorldDamageInfo;

		// Token: 0x0400756E RID: 30062
		public unsafe Sim.WorldDamageInfo* worldDamageInfo;

		// Token: 0x0400756F RID: 30063
		public int numBuildingTemperatures;

		// Token: 0x04007570 RID: 30064
		public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

		// Token: 0x04007571 RID: 30065
		public int numMassConsumedCallbacks;

		// Token: 0x04007572 RID: 30066
		public unsafe Sim.MassConsumedCallback* massConsumedCallbacks;

		// Token: 0x04007573 RID: 30067
		public int numMassEmittedCallbacks;

		// Token: 0x04007574 RID: 30068
		public unsafe Sim.MassEmittedCallback* massEmittedCallbacks;

		// Token: 0x04007575 RID: 30069
		public int numDiseaseConsumptionCallbacks;

		// Token: 0x04007576 RID: 30070
		public unsafe Sim.DiseaseConsumptionCallback* diseaseConsumptionCallbacks;

		// Token: 0x04007577 RID: 30071
		public int numComponentStateChangedMessages;

		// Token: 0x04007578 RID: 30072
		public unsafe Sim.ComponentStateChangedMessage* componentStateChangedMessages;

		// Token: 0x04007579 RID: 30073
		public int numRemovedMassEntries;

		// Token: 0x0400757A RID: 30074
		public unsafe Sim.ConsumedMassInfo* removedMassEntries;

		// Token: 0x0400757B RID: 30075
		public int numEmittedMassEntries;

		// Token: 0x0400757C RID: 30076
		public unsafe Sim.EmittedMassInfo* emittedMassEntries;

		// Token: 0x0400757D RID: 30077
		public int numElementChunkInfos;

		// Token: 0x0400757E RID: 30078
		public unsafe Sim.ElementChunkInfo* elementChunkInfos;

		// Token: 0x0400757F RID: 30079
		public int numElementChunkMeltedInfos;

		// Token: 0x04007580 RID: 30080
		public unsafe Sim.MeltedInfo* elementChunkMeltedInfos;

		// Token: 0x04007581 RID: 30081
		public int numBuildingOverheatInfos;

		// Token: 0x04007582 RID: 30082
		public unsafe Sim.MeltedInfo* buildingOverheatInfos;

		// Token: 0x04007583 RID: 30083
		public int numBuildingNoLongerOverheatedInfos;

		// Token: 0x04007584 RID: 30084
		public unsafe Sim.MeltedInfo* buildingNoLongerOverheatedInfos;

		// Token: 0x04007585 RID: 30085
		public int numBuildingMeltedInfos;

		// Token: 0x04007586 RID: 30086
		public unsafe Sim.MeltedInfo* buildingMeltedInfos;

		// Token: 0x04007587 RID: 30087
		public int numCellMeltedInfos;

		// Token: 0x04007588 RID: 30088
		public unsafe Sim.CellMeltedInfo* cellMeltedInfos;

		// Token: 0x04007589 RID: 30089
		public int numDiseaseEmittedInfos;

		// Token: 0x0400758A RID: 30090
		public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

		// Token: 0x0400758B RID: 30091
		public int numDiseaseConsumedInfos;

		// Token: 0x0400758C RID: 30092
		public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;

		// Token: 0x0400758D RID: 30093
		public int numRadiationConsumedCallbacks;

		// Token: 0x0400758E RID: 30094
		public unsafe Sim.ConsumedRadiationCallback* radiationConsumedCallbacks;

		// Token: 0x0400758F RID: 30095
		public unsafe float* accumulatedFlow;

		// Token: 0x04007590 RID: 30096
		public IntPtr propertyTextureFlow;

		// Token: 0x04007591 RID: 30097
		public IntPtr propertyTextureLiquid;

		// Token: 0x04007592 RID: 30098
		public IntPtr propertyTextureExposedToSunlight;
	}

	// Token: 0x0200195A RID: 6490
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFallingLiquidInfo
	{
		// Token: 0x04007593 RID: 30099
		public int cellIdx;

		// Token: 0x04007594 RID: 30100
		public ushort elemIdx;

		// Token: 0x04007595 RID: 30101
		public byte diseaseIdx;

		// Token: 0x04007596 RID: 30102
		public byte pad0;

		// Token: 0x04007597 RID: 30103
		public float mass;

		// Token: 0x04007598 RID: 30104
		public float temperature;

		// Token: 0x04007599 RID: 30105
		public int diseaseCount;
	}

	// Token: 0x0200195B RID: 6491
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnOreInfo
	{
		// Token: 0x0400759A RID: 30106
		public int cellIdx;

		// Token: 0x0400759B RID: 30107
		public ushort elemIdx;

		// Token: 0x0400759C RID: 30108
		public byte diseaseIdx;

		// Token: 0x0400759D RID: 30109
		private byte pad0;

		// Token: 0x0400759E RID: 30110
		public float mass;

		// Token: 0x0400759F RID: 30111
		public float temperature;

		// Token: 0x040075A0 RID: 30112
		public int diseaseCount;
	}

	// Token: 0x0200195C RID: 6492
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFXInfo
	{
		// Token: 0x040075A1 RID: 30113
		public int cellIdx;

		// Token: 0x040075A2 RID: 30114
		public int fxHash;

		// Token: 0x040075A3 RID: 30115
		public float rotation;
	}

	// Token: 0x0200195D RID: 6493
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct UnstableCellInfo
	{
		// Token: 0x040075A4 RID: 30116
		public int cellIdx;

		// Token: 0x040075A5 RID: 30117
		public ushort elemIdx;

		// Token: 0x040075A6 RID: 30118
		public byte fallingInfo;

		// Token: 0x040075A7 RID: 30119
		public byte diseaseIdx;

		// Token: 0x040075A8 RID: 30120
		public float mass;

		// Token: 0x040075A9 RID: 30121
		public float temperature;

		// Token: 0x040075AA RID: 30122
		public int diseaseCount;

		// Token: 0x02002227 RID: 8743
		public enum FallingInfo
		{
			// Token: 0x040098C5 RID: 39109
			StartedFalling,
			// Token: 0x040098C6 RID: 39110
			StoppedFalling
		}
	}

	// Token: 0x0200195E RID: 6494
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct NewGameFrame
	{
		// Token: 0x040075AB RID: 30123
		public float elapsedSeconds;

		// Token: 0x040075AC RID: 30124
		public int minX;

		// Token: 0x040075AD RID: 30125
		public int minY;

		// Token: 0x040075AE RID: 30126
		public int maxX;

		// Token: 0x040075AF RID: 30127
		public int maxY;

		// Token: 0x040075B0 RID: 30128
		public float currentSunlightIntensity;

		// Token: 0x040075B1 RID: 30129
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x0200195F RID: 6495
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WorldDamageInfo
	{
		// Token: 0x040075B2 RID: 30130
		public int gameCell;

		// Token: 0x040075B3 RID: 30131
		public int damageSourceOffset;
	}

	// Token: 0x02001960 RID: 6496
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeTemperatureChange
	{
		// Token: 0x040075B4 RID: 30132
		public int cellIdx;

		// Token: 0x040075B5 RID: 30133
		public float temperature;
	}

	// Token: 0x02001961 RID: 6497
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassConsumedCallback
	{
		// Token: 0x040075B6 RID: 30134
		public int callbackIdx;

		// Token: 0x040075B7 RID: 30135
		public ushort elemIdx;

		// Token: 0x040075B8 RID: 30136
		public byte diseaseIdx;

		// Token: 0x040075B9 RID: 30137
		private byte pad0;

		// Token: 0x040075BA RID: 30138
		public float mass;

		// Token: 0x040075BB RID: 30139
		public float temperature;

		// Token: 0x040075BC RID: 30140
		public int diseaseCount;
	}

	// Token: 0x02001962 RID: 6498
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassEmittedCallback
	{
		// Token: 0x040075BD RID: 30141
		public int callbackIdx;

		// Token: 0x040075BE RID: 30142
		public ushort elemIdx;

		// Token: 0x040075BF RID: 30143
		public byte suceeded;

		// Token: 0x040075C0 RID: 30144
		public byte diseaseIdx;

		// Token: 0x040075C1 RID: 30145
		public float mass;

		// Token: 0x040075C2 RID: 30146
		public float temperature;

		// Token: 0x040075C3 RID: 30147
		public int diseaseCount;
	}

	// Token: 0x02001963 RID: 6499
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumptionCallback
	{
		// Token: 0x040075C4 RID: 30148
		public int callbackIdx;

		// Token: 0x040075C5 RID: 30149
		public byte diseaseIdx;

		// Token: 0x040075C6 RID: 30150
		private byte pad0;

		// Token: 0x040075C7 RID: 30151
		private byte pad1;

		// Token: 0x040075C8 RID: 30152
		private byte pad2;

		// Token: 0x040075C9 RID: 30153
		public int diseaseCount;
	}

	// Token: 0x02001964 RID: 6500
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ComponentStateChangedMessage
	{
		// Token: 0x040075CA RID: 30154
		public int callbackIdx;

		// Token: 0x040075CB RID: 30155
		public int simHandle;
	}

	// Token: 0x02001965 RID: 6501
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DebugProperties
	{
		// Token: 0x040075CC RID: 30156
		public float buildingTemperatureScale;

		// Token: 0x040075CD RID: 30157
		public float buildingToBuildingTemperatureScale;

		// Token: 0x040075CE RID: 30158
		public float biomeTemperatureLerpRate;

		// Token: 0x040075CF RID: 30159
		public byte isDebugEditing;

		// Token: 0x040075D0 RID: 30160
		public byte pad0;

		// Token: 0x040075D1 RID: 30161
		public byte pad1;

		// Token: 0x040075D2 RID: 30162
		public byte pad2;
	}

	// Token: 0x02001966 RID: 6502
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct EmittedMassInfo
	{
		// Token: 0x040075D3 RID: 30163
		public ushort elemIdx;

		// Token: 0x040075D4 RID: 30164
		public byte diseaseIdx;

		// Token: 0x040075D5 RID: 30165
		public byte pad0;

		// Token: 0x040075D6 RID: 30166
		public float mass;

		// Token: 0x040075D7 RID: 30167
		public float temperature;

		// Token: 0x040075D8 RID: 30168
		public int diseaseCount;
	}

	// Token: 0x02001967 RID: 6503
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedMassInfo
	{
		// Token: 0x040075D9 RID: 30169
		public int simHandle;

		// Token: 0x040075DA RID: 30170
		public ushort removedElemIdx;

		// Token: 0x040075DB RID: 30171
		public byte diseaseIdx;

		// Token: 0x040075DC RID: 30172
		private byte pad0;

		// Token: 0x040075DD RID: 30173
		public float mass;

		// Token: 0x040075DE RID: 30174
		public float temperature;

		// Token: 0x040075DF RID: 30175
		public int diseaseCount;
	}

	// Token: 0x02001968 RID: 6504
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedDiseaseInfo
	{
		// Token: 0x040075E0 RID: 30176
		public int simHandle;

		// Token: 0x040075E1 RID: 30177
		public byte diseaseIdx;

		// Token: 0x040075E2 RID: 30178
		private byte pad0;

		// Token: 0x040075E3 RID: 30179
		private byte pad1;

		// Token: 0x040075E4 RID: 30180
		private byte pad2;

		// Token: 0x040075E5 RID: 30181
		public int diseaseCount;
	}

	// Token: 0x02001969 RID: 6505
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementChunkInfo
	{
		// Token: 0x040075E6 RID: 30182
		public float temperature;

		// Token: 0x040075E7 RID: 30183
		public float deltaKJ;
	}

	// Token: 0x0200196A RID: 6506
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MeltedInfo
	{
		// Token: 0x040075E8 RID: 30184
		public int handle;
	}

	// Token: 0x0200196B RID: 6507
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellMeltedInfo
	{
		// Token: 0x040075E9 RID: 30185
		public int gameCell;
	}

	// Token: 0x0200196C RID: 6508
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingTemperatureInfo
	{
		// Token: 0x040075EA RID: 30186
		public int handle;

		// Token: 0x040075EB RID: 30187
		public float temperature;
	}

	// Token: 0x0200196D RID: 6509
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingConductivityData
	{
		// Token: 0x040075EC RID: 30188
		public float temperature;

		// Token: 0x040075ED RID: 30189
		public float heatCapacity;

		// Token: 0x040075EE RID: 30190
		public float thermalConductivity;
	}

	// Token: 0x0200196E RID: 6510
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseEmittedInfo
	{
		// Token: 0x040075EF RID: 30191
		public byte diseaseIdx;

		// Token: 0x040075F0 RID: 30192
		private byte pad0;

		// Token: 0x040075F1 RID: 30193
		private byte pad1;

		// Token: 0x040075F2 RID: 30194
		private byte pad2;

		// Token: 0x040075F3 RID: 30195
		public int count;
	}

	// Token: 0x0200196F RID: 6511
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumedInfo
	{
		// Token: 0x040075F4 RID: 30196
		public byte diseaseIdx;

		// Token: 0x040075F5 RID: 30197
		private byte pad0;

		// Token: 0x040075F6 RID: 30198
		private byte pad1;

		// Token: 0x040075F7 RID: 30199
		private byte pad2;

		// Token: 0x040075F8 RID: 30200
		public int count;
	}

	// Token: 0x02001970 RID: 6512
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedRadiationCallback
	{
		// Token: 0x040075F9 RID: 30201
		public int callbackIdx;

		// Token: 0x040075FA RID: 30202
		public int gameCell;

		// Token: 0x040075FB RID: 30203
		public float radiation;
	}
}
