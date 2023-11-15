using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Database;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;

// Token: 0x02000A86 RID: 2694
public static class SimMessages
{
	// Token: 0x06005226 RID: 21030 RVA: 0x001D5F90 File Offset: 0x001D4190
	public unsafe static void AddElementConsumer(int gameCell, ElementConsumer.Configuration configuration, SimHashes element, byte radius, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.AddElementConsumerMessage* ptr = stackalloc SimMessages.AddElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementConsumerMessage))];
		ptr->cellIdx = gameCell;
		ptr->configuration = (byte)configuration;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(2024405073, sizeof(SimMessages.AddElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x001D5FFC File Offset: 0x001D41FC
	public unsafe static void SetElementConsumerData(int sim_handle, int cell, float consumptionRate)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementConsumerDataMessage* ptr = stackalloc SimMessages.SetElementConsumerDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementConsumerDataMessage))];
		ptr->handle = sim_handle;
		ptr->cell = cell;
		ptr->consumptionRate = consumptionRate;
		Sim.SIM_HandleMessage(1575539738, sizeof(SimMessages.SetElementConsumerDataMessage), (byte*)ptr);
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x001D6048 File Offset: 0x001D4248
	public unsafe static void RemoveElementConsumer(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementConsumerMessage* ptr = stackalloc SimMessages.RemoveElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementConsumerMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(894417742, sizeof(SimMessages.RemoveElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x06005229 RID: 21033 RVA: 0x001D6098 File Offset: 0x001D4298
	public unsafe static void AddElementEmitter(float max_pressure, int on_registered, int on_blocked = -1, int on_unblocked = -1)
	{
		SimMessages.AddElementEmitterMessage* ptr = stackalloc SimMessages.AddElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementEmitterMessage))];
		ptr->maxPressure = max_pressure;
		ptr->callbackIdx = on_registered;
		ptr->onBlockedCB = on_blocked;
		ptr->onUnblockedCB = on_unblocked;
		Sim.SIM_HandleMessage(-505471181, sizeof(SimMessages.AddElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600522A RID: 21034 RVA: 0x001D60E0 File Offset: 0x001D42E0
	public unsafe static void ModifyElementEmitter(int sim_handle, int game_cell, int max_depth, SimHashes element, float emit_interval, float emit_mass, float emit_temperature, float max_pressure, byte disease_idx, int disease_count)
	{
		Debug.Assert(Grid.IsValidCell(game_cell));
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.ModifyElementEmitterMessage* ptr = stackalloc SimMessages.ModifyElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cellIdx = game_cell;
		ptr->emitInterval = emit_interval;
		ptr->emitMass = emit_mass;
		ptr->emitTemperature = emit_temperature;
		ptr->maxPressure = max_pressure;
		ptr->elementIdx = elementIndex;
		ptr->maxDepth = (byte)max_depth;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(403589164, sizeof(SimMessages.ModifyElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600522B RID: 21035 RVA: 0x001D6174 File Offset: 0x001D4374
	public unsafe static void RemoveElementEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementEmitterMessage* ptr = stackalloc SimMessages.RemoveElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-1524118282, sizeof(SimMessages.RemoveElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600522C RID: 21036 RVA: 0x001D61C4 File Offset: 0x001D43C4
	public unsafe static void AddRadiationEmitter(int on_registered, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		SimMessages.AddRadiationEmitterMessage* ptr = stackalloc SimMessages.AddRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddRadiationEmitterMessage))];
		ptr->callbackIdx = on_registered;
		ptr->cell = game_cell;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-1505895314, sizeof(SimMessages.AddRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600522D RID: 21037 RVA: 0x001D623C File Offset: 0x001D443C
	public unsafe static void ModifyRadiationEmitter(int sim_handle, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ModifyRadiationEmitterMessage* ptr = stackalloc SimMessages.ModifyRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyRadiationEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cell = game_cell;
		ptr->callbackIdx = -1;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-503965465, sizeof(SimMessages.ModifyRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600522E RID: 21038 RVA: 0x001D62C4 File Offset: 0x001D44C4
	public unsafe static void RemoveRadiationEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveRadiationEmitterMessage* ptr = stackalloc SimMessages.RemoveRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveRadiationEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-704259919, sizeof(SimMessages.RemoveRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600522F RID: 21039 RVA: 0x001D6314 File Offset: 0x001D4514
	public unsafe static void AddElementChunk(int gameCell, SimHashes element, float mass, float temperature, float surface_area, float thickness, float ground_transfer_scale, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (mass * temperature > 0f)
		{
			ushort elementIndex = ElementLoader.GetElementIndex(element);
			SimMessages.AddElementChunkMessage* ptr = stackalloc SimMessages.AddElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementChunkMessage))];
			ptr->gameCell = gameCell;
			ptr->callbackIdx = cb_handle;
			ptr->mass = mass;
			ptr->temperature = temperature;
			ptr->surfaceArea = surface_area;
			ptr->thickness = thickness;
			ptr->groundTransferScale = ground_transfer_scale;
			ptr->elementIdx = elementIndex;
			Sim.SIM_HandleMessage(1445724082, sizeof(SimMessages.AddElementChunkMessage), (byte*)ptr);
		}
	}

	// Token: 0x06005230 RID: 21040 RVA: 0x001D63A0 File Offset: 0x001D45A0
	public unsafe static void RemoveElementChunk(int sim_handle, int cb_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementChunkMessage* ptr = stackalloc SimMessages.RemoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementChunkMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-912908555, sizeof(SimMessages.RemoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06005231 RID: 21041 RVA: 0x001D63F0 File Offset: 0x001D45F0
	public unsafe static void SetElementChunkData(int sim_handle, float temperature, float heat_capacity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementChunkDataMessage* ptr = stackalloc SimMessages.SetElementChunkDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementChunkDataMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		Sim.SIM_HandleMessage(-435115907, sizeof(SimMessages.SetElementChunkDataMessage), (byte*)ptr);
	}

	// Token: 0x06005232 RID: 21042 RVA: 0x001D643C File Offset: 0x001D463C
	public unsafe static void MoveElementChunk(int sim_handle, int cell)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.MoveElementChunkMessage* ptr = stackalloc SimMessages.MoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MoveElementChunkMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		Sim.SIM_HandleMessage(-374911358, sizeof(SimMessages.MoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06005233 RID: 21043 RVA: 0x001D648C File Offset: 0x001D468C
	public unsafe static void ModifyElementChunkEnergy(int sim_handle, float delta_kj)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkEnergyMessage* ptr = stackalloc SimMessages.ModifyElementChunkEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkEnergyMessage))];
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		Sim.SIM_HandleMessage(1020555667, sizeof(SimMessages.ModifyElementChunkEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06005234 RID: 21044 RVA: 0x001D64DC File Offset: 0x001D46DC
	public unsafe static void ModifyElementChunkTemperatureAdjuster(int sim_handle, float temperature, float heat_capacity, float thermal_conductivity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkAdjusterMessage* ptr = stackalloc SimMessages.ModifyElementChunkAdjusterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkAdjusterMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		ptr->thermalConductivity = thermal_conductivity;
		Sim.SIM_HandleMessage(-1387601379, sizeof(SimMessages.ModifyElementChunkAdjusterMessage), (byte*)ptr);
	}

	// Token: 0x06005235 RID: 21045 RVA: 0x001D6538 File Offset: 0x001D4738
	public unsafe static void AddBuildingHeatExchange(Extents extents, float mass, float temperature, float thermal_conductivity, float operating_kw, ushort elem_idx, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(Grid.XYToCell(extents.x, extents.y)))
		{
			return;
		}
		int num = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		if (!Grid.IsValidCell(num))
		{
			Debug.LogErrorFormat("Invalid Cell [{0}] Extents [{1},{2}] [{3},{4}]", new object[]
			{
				num,
				extents.x,
				extents.y,
				extents.width,
				extents.height
			});
		}
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		SimMessages.AddBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callbackIdx;
		ptr->elemIdx = elem_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = float.MaxValue;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1739021608, sizeof(SimMessages.AddBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005236 RID: 21046 RVA: 0x001D6674 File Offset: 0x001D4874
	public unsafe static void ModifyBuildingHeatExchange(int sim_handle, Extents extents, float mass, float temperature, float thermal_conductivity, float overheat_temperature, float operating_kw, ushort element_idx)
	{
		int cell = Grid.XYToCell(extents.x, extents.y);
		Debug.Assert(Grid.IsValidCell(cell));
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		Debug.Assert(Grid.IsValidCell(cell2));
		if (!Grid.IsValidCell(cell2))
		{
			return;
		}
		SimMessages.ModifyBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.ModifyBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingHeatExchangeMessage))];
		ptr->callbackIdx = sim_handle;
		ptr->elemIdx = element_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = overheat_temperature;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1818001569, sizeof(SimMessages.ModifyBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005237 RID: 21047 RVA: 0x001D6768 File Offset: 0x001D4968
	public unsafe static void RemoveBuildingHeatExchange(int sim_handle, int callbackIdx = -1)
	{
		SimMessages.RemoveBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingHeatExchangeMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-456116629, sizeof(SimMessages.RemoveBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005238 RID: 21048 RVA: 0x001D67AC File Offset: 0x001D49AC
	public unsafe static void ModifyBuildingEnergy(int sim_handle, float delta_kj, float min_temperature, float max_temperature)
	{
		SimMessages.ModifyBuildingEnergyMessage* ptr = stackalloc SimMessages.ModifyBuildingEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingEnergyMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		ptr->minTemperature = min_temperature;
		ptr->maxTemperature = max_temperature;
		Sim.SIM_HandleMessage(-1348791658, sizeof(SimMessages.ModifyBuildingEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06005239 RID: 21049 RVA: 0x001D6800 File Offset: 0x001D4A00
	public unsafe static void RegisterBuildingToBuildingHeatExchange(int structureTemperatureHandler, int callbackIdx = -1)
	{
		SimMessages.RegisterBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RegisterBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage))];
		ptr->structureTemperatureHandler = structureTemperatureHandler;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1338718217, sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600523A RID: 21050 RVA: 0x001D683C File Offset: 0x001D4A3C
	public unsafe static void AddBuildingToBuildingHeatExchange(int selfHandler, int buildingInContact, int cellsInContact)
	{
		SimMessages.AddBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingInContactHandle = buildingInContact;
		ptr->cellsInContact = cellsInContact;
		Sim.SIM_HandleMessage(-1586724321, sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600523B RID: 21051 RVA: 0x001D687C File Offset: 0x001D4A7C
	public unsafe static void RemoveBuildingInContactFromBuildingToBuildingHeatExchange(int selfHandler, int buildingToRemove)
	{
		SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingNoLongerInContactHandler = buildingToRemove;
		Sim.SIM_HandleMessage(-1993857213, sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600523C RID: 21052 RVA: 0x001D68B8 File Offset: 0x001D4AB8
	public unsafe static void RemoveBuildingToBuildingHeatExchange(int selfHandler, int callback = -1)
	{
		SimMessages.RemoveBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callback;
		ptr->selfHandler = selfHandler;
		Sim.SIM_HandleMessage(697100730, sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x001D68F4 File Offset: 0x001D4AF4
	public unsafe static void AddDiseaseEmitter(int callbackIdx)
	{
		SimMessages.AddDiseaseEmitterMessage* ptr = stackalloc SimMessages.AddDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddDiseaseEmitterMessage))];
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(1486783027, sizeof(SimMessages.AddDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600523E RID: 21054 RVA: 0x001D6928 File Offset: 0x001D4B28
	public unsafe static void ModifyDiseaseEmitter(int sim_handle, int cell, byte range, byte disease_idx, float emit_interval, int emit_count)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.ModifyDiseaseEmitterMessage* ptr = stackalloc SimMessages.ModifyDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		ptr->maxDepth = range;
		ptr->diseaseIdx = disease_idx;
		ptr->emitInterval = emit_interval;
		ptr->emitCount = emit_count;
		Sim.SIM_HandleMessage(-1899123924, sizeof(SimMessages.ModifyDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600523F RID: 21055 RVA: 0x001D698C File Offset: 0x001D4B8C
	public unsafe static void RemoveDiseaseEmitter(int cb_handle, int sim_handle)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveDiseaseEmitterMessage* ptr = stackalloc SimMessages.RemoveDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(468135926, sizeof(SimMessages.RemoveDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005240 RID: 21056 RVA: 0x001D69D0 File Offset: 0x001D4BD0
	public unsafe static void SetSavedOptionValue(SimMessages.SimSavedOptions option, int zero_or_one)
	{
		SimMessages.SetSavedOptionsMessage* ptr = stackalloc SimMessages.SetSavedOptionsMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetSavedOptionsMessage))];
		if (zero_or_one == 0)
		{
			SimMessages.SetSavedOptionsMessage* ptr2 = ptr;
			ptr2->clearBits = (ptr2->clearBits | (byte)option);
			ptr->setBits = 0;
		}
		else
		{
			ptr->clearBits = 0;
			SimMessages.SetSavedOptionsMessage* ptr3 = ptr;
			ptr3->setBits = (ptr3->setBits | (byte)option);
		}
		Sim.SIM_HandleMessage(1154135737, sizeof(SimMessages.SetSavedOptionsMessage), (byte*)ptr);
	}

	// Token: 0x06005241 RID: 21057 RVA: 0x001D6A28 File Offset: 0x001D4C28
	private static void WriteKleiString(this BinaryWriter writer, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		writer.Write(bytes.Length);
		if (bytes.Length != 0)
		{
			writer.Write(bytes);
		}
	}

	// Token: 0x06005242 RID: 21058 RVA: 0x001D6A58 File Offset: 0x001D4C58
	public unsafe static void CreateSimElementsTable(List<Element> elements)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Element)) * elements.Count);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		Debug.Assert(elements.Count < 65535, "SimDLL internals assume there are fewer than 65535 elements");
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < elements.Count; i++)
		{
			Sim.Element element = new Sim.Element(elements[i], elements);
			element.Write(binaryWriter);
		}
		for (int j = 0; j < elements.Count; j++)
		{
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(elements[j].name));
		}
		byte[] buffer = memoryStream.GetBuffer();
		byte[] array;
		byte* msg;
		if ((array = buffer) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(1108437482, buffer.Length, msg);
		array = null;
	}

	// Token: 0x06005243 RID: 21059 RVA: 0x001D6B48 File Offset: 0x001D4D48
	public unsafe static void CreateDiseaseTable(Diseases diseases)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(diseases.Count);
		List<Element> elements = ElementLoader.elements;
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < diseases.Count; i++)
		{
			Disease disease = diseases[i];
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(disease.Name));
			binaryWriter.Write(disease.id.GetHashCode());
			binaryWriter.Write(disease.strength);
			disease.temperatureRange.Write(binaryWriter);
			disease.temperatureHalfLives.Write(binaryWriter);
			disease.pressureRange.Write(binaryWriter);
			disease.pressureHalfLives.Write(binaryWriter);
			binaryWriter.Write(disease.radiationKillRate);
			for (int j = 0; j < elements.Count; j++)
			{
				ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[j];
				elemGrowthInfo.Write(binaryWriter);
			}
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(825301935, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06005244 RID: 21060 RVA: 0x001D6C84 File Offset: 0x001D4E84
	public unsafe static void DefineWorldOffsets(List<SimMessages.WorldOffsetData> worldOffsets)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(worldOffsets.Count);
		foreach (SimMessages.WorldOffsetData worldOffsetData in worldOffsets)
		{
			binaryWriter.Write(worldOffsetData.worldOffsetX);
			binaryWriter.Write(worldOffsetData.worldOffsetY);
			binaryWriter.Write(worldOffsetData.worldSizeX);
			binaryWriter.Write(worldOffsetData.worldSizeY);
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(-895846551, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06005245 RID: 21061 RVA: 0x001D6D54 File Offset: 0x001D4F54
	public static void SimDataInitializeFromCells(int width, int height, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, bool headless)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Cell)) * width * height + Marshal.SizeOf(typeof(float)) * width * height + Marshal.SizeOf(typeof(Sim.DiseaseCell)) * width * height);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		bool value = Sim.IsRadiationEnabled();
		binaryWriter.Write(value);
		binaryWriter.Write(headless);
		int num = width * height;
		for (int i = 0; i < num; i++)
		{
			cells[i].Write(binaryWriter);
		}
		for (int j = 0; j < num; j++)
		{
			binaryWriter.Write(bgTemp[j]);
		}
		for (int k = 0; k < num; k++)
		{
			dc[k].Write(binaryWriter);
		}
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_InitializeFromCells, buffer.Length, buffer);
	}

	// Token: 0x06005246 RID: 21062 RVA: 0x001D6E60 File Offset: 0x001D5060
	public static void SimDataResizeGridAndInitializeVacuumCells(Vector2I grid_size, int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(grid_size.x);
		binaryWriter.Write(grid_size.y);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_ResizeAndInitializeVacuumCells, buffer.Length, buffer);
	}

	// Token: 0x06005247 RID: 21063 RVA: 0x001D6EE0 File Offset: 0x001D50E0
	public static void SimDataFreeCells(int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_FreeCells, buffer.Length, buffer);
	}

	// Token: 0x06005248 RID: 21064 RVA: 0x001D6F48 File Offset: 0x001D5148
	public unsafe static void Dig(int gameCell, int callbackIdx = -1, bool skipEvent = false)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.DigMessage* ptr = stackalloc SimMessages.DigMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.DigMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->skipEvent = skipEvent;
		Sim.SIM_HandleMessage(833038498, sizeof(SimMessages.DigMessage), (byte*)ptr);
	}

	// Token: 0x06005249 RID: 21065 RVA: 0x001D6F94 File Offset: 0x001D5194
	public unsafe static void SetInsulation(int gameCell, float value)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		ptr->value = value;
		Sim.SIM_HandleMessage(-898773121, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x0600524A RID: 21066 RVA: 0x001D6FD8 File Offset: 0x001D51D8
	public unsafe static void SetStrength(int gameCell, int weight, float strengthMultiplier)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		int num = (int)(strengthMultiplier * 4f) & 127;
		int num2 = (weight & 1) << 7 | num;
		ptr->value = (float)((byte)num2);
		Sim.SIM_HandleMessage(1593243982, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x0600524B RID: 21067 RVA: 0x001D7030 File Offset: 0x001D5230
	public unsafe static void SetCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 1;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x0600524C RID: 21068 RVA: 0x001D707C File Offset: 0x001D527C
	public unsafe static void ClearCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 0;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x0600524D RID: 21069 RVA: 0x001D70C8 File Offset: 0x001D52C8
	public unsafe static void ModifyCell(int gameCell, ushort elementIdx, float temperature, float mass, byte disease_idx, int disease_count, SimMessages.ReplaceType replace_type = SimMessages.ReplaceType.None, bool do_vertical_solid_displacement = false, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		if (element.maxMass == 0f && mass > element.maxMass)
		{
			Debug.LogWarningFormat("Invalid cell modification (mass greater than element maximum): Cell={0}, EIdx={1}, T={2}, M={3}, {4} max mass = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.maxMass
			});
			mass = element.maxMass;
		}
		if (temperature < 0f || temperature > 10000f)
		{
			Debug.LogWarningFormat("Invalid cell modification (temp out of bounds): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		if (temperature == 0f && mass > 0f)
		{
			Debug.LogWarningFormat("Invalid cell modification (zero temp with non-zero mass): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		SimMessages.ModifyCellMessage* ptr = stackalloc SimMessages.ModifyCellMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->temperature = temperature;
		ptr->mass = mass;
		ptr->elementIdx = elementIdx;
		ptr->replaceType = (byte)replace_type;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		ptr->addSubType = (do_vertical_solid_displacement ? 0 : 1);
		Sim.SIM_HandleMessage(-1252920804, sizeof(SimMessages.ModifyCellMessage), (byte*)ptr);
	}

	// Token: 0x0600524E RID: 21070 RVA: 0x001D72A8 File Offset: 0x001D54A8
	public unsafe static void ModifyDiseaseOnCell(int gameCell, byte disease_idx, int disease_delta)
	{
		SimMessages.CellDiseaseModification* ptr = stackalloc SimMessages.CellDiseaseModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellDiseaseModification))];
		ptr->cellIdx = gameCell;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_delta;
		Sim.SIM_HandleMessage(-1853671274, sizeof(SimMessages.CellDiseaseModification), (byte*)ptr);
	}

	// Token: 0x0600524F RID: 21071 RVA: 0x001D72E8 File Offset: 0x001D54E8
	public unsafe static void ModifyRadiationOnCell(int gameCell, float radiationDelta, int callbackIdx = -1)
	{
		SimMessages.CellRadiationModification* ptr = stackalloc SimMessages.CellRadiationModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellRadiationModification))];
		ptr->cellIdx = gameCell;
		ptr->radiationDelta = radiationDelta;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1914877797, sizeof(SimMessages.CellRadiationModification), (byte*)ptr);
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x001D7328 File Offset: 0x001D5528
	public unsafe static void ModifyRadiationParams(RadiationParams type, float value)
	{
		SimMessages.RadiationParamsModification* ptr = stackalloc SimMessages.RadiationParamsModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RadiationParamsModification))];
		ptr->RadiationParamsType = (int)type;
		ptr->value = value;
		Sim.SIM_HandleMessage(377112707, sizeof(SimMessages.RadiationParamsModification), (byte*)ptr);
	}

	// Token: 0x06005251 RID: 21073 RVA: 0x001D7361 File Offset: 0x001D5561
	public static ushort GetElementIndex(SimHashes element)
	{
		return ElementLoader.GetElementIndex(element);
	}

	// Token: 0x06005252 RID: 21074 RVA: 0x001D736C File Offset: 0x001D556C
	public unsafe static void ConsumeMass(int gameCell, SimHashes element, float mass, byte radius, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.MassConsumptionMessage* ptr = stackalloc SimMessages.MassConsumptionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassConsumptionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		Sim.SIM_HandleMessage(1727657959, sizeof(SimMessages.MassConsumptionMessage), (byte*)ptr);
	}

	// Token: 0x06005253 RID: 21075 RVA: 0x001D73CC File Offset: 0x001D55CC
	public unsafe static void EmitMass(int gameCell, ushort element_idx, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.MassEmissionMessage* ptr = stackalloc SimMessages.MassEmissionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassEmissionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->elementIdx = element_idx;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(797274363, sizeof(SimMessages.MassEmissionMessage), (byte*)ptr);
	}

	// Token: 0x06005254 RID: 21076 RVA: 0x001D7434 File Offset: 0x001D5634
	public unsafe static void ConsumeDisease(int game_cell, float percent_to_consume, int max_to_consume, int callback_idx)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ConsumeDiseaseMessage* ptr = stackalloc SimMessages.ConsumeDiseaseMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ConsumeDiseaseMessage))];
		ptr->callbackIdx = callback_idx;
		ptr->gameCell = game_cell;
		ptr->percentToConsume = percent_to_consume;
		ptr->maxToConsume = max_to_consume;
		Sim.SIM_HandleMessage(-1019841536, sizeof(SimMessages.ConsumeDiseaseMessage), (byte*)ptr);
	}

	// Token: 0x06005255 RID: 21077 RVA: 0x001D7484 File Offset: 0x001D5684
	public static void AddRemoveSubstance(int gameCell, SimHashes new_element, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		SimMessages.AddRemoveSubstance(gameCell, elementIndex, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x06005256 RID: 21078 RVA: 0x001D74AC File Offset: 0x001D56AC
	public static void AddRemoveSubstance(int gameCell, ushort elementIdx, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		if (elementIdx == 65535)
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
		SimMessages.ModifyCell(gameCell, elementIdx, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x06005257 RID: 21079 RVA: 0x001D74FC File Offset: 0x001D56FC
	public static void ReplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte diseaseIdx = 255, int diseaseCount = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, diseaseIdx, diseaseCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
		}
	}

	// Token: 0x06005258 RID: 21080 RVA: 0x001D7550 File Offset: 0x001D5750
	public static void ReplaceAndDisplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte disease_idx = 255, int disease_count = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.ReplaceAndDisplace, false, callbackIdx);
		}
	}

	// Token: 0x06005259 RID: 21081 RVA: 0x001D75A4 File Offset: 0x001D57A4
	public unsafe static void ModifyEnergy(int gameCell, float kilojoules, float max_temperature, SimMessages.EnergySourceID id)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (max_temperature <= 0f)
		{
			Debug.LogError("invalid max temperature for cell energy modification");
			return;
		}
		SimMessages.ModifyCellEnergyMessage* ptr = stackalloc SimMessages.ModifyCellEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellEnergyMessage))];
		ptr->cellIdx = gameCell;
		ptr->kilojoules = kilojoules;
		ptr->maxTemperature = max_temperature;
		ptr->id = (int)id;
		Sim.SIM_HandleMessage(818320644, sizeof(SimMessages.ModifyCellEnergyMessage), (byte*)ptr);
	}

	// Token: 0x0600525A RID: 21082 RVA: 0x001D7608 File Offset: 0x001D5808
	public static void ModifyMass(int gameCell, float mass, byte disease_idx, int disease_count, CellModifyMassEvent ev, float temperature = -1f, SimHashes element = SimHashes.Vacuum)
	{
		if (element != SimHashes.Vacuum)
		{
			ushort elementIndex = SimMessages.GetElementIndex(element);
			if (elementIndex != 65535)
			{
				if (temperature == -1f)
				{
					temperature = ElementLoader.elements[(int)elementIndex].defaultValues.temperature;
				}
				SimMessages.ModifyCell(gameCell, elementIndex, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
				return;
			}
		}
		else
		{
			SimMessages.ModifyCell(gameCell, 0, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
		}
	}

	// Token: 0x0600525B RID: 21083 RVA: 0x001D7670 File Offset: 0x001D5870
	public unsafe static void CreateElementInteractions(SimMessages.ElementInteraction[] interactions)
	{
		fixed (SimMessages.ElementInteraction[] array = interactions)
		{
			SimMessages.ElementInteraction* interactions2;
			if (interactions == null || array.Length == 0)
			{
				interactions2 = null;
			}
			else
			{
				interactions2 = &array[0];
			}
			SimMessages.CreateElementInteractionsMsg* ptr = stackalloc SimMessages.CreateElementInteractionsMsg[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CreateElementInteractionsMsg))];
			ptr->numInteractions = interactions.Length;
			ptr->interactions = interactions2;
			Sim.SIM_HandleMessage(-930289787, sizeof(SimMessages.CreateElementInteractionsMsg), (byte*)ptr);
		}
	}

	// Token: 0x0600525C RID: 21084 RVA: 0x001D76C8 File Offset: 0x001D58C8
	public unsafe static void NewGameFrame(float elapsed_seconds, List<Game.SimActiveRegion> activeRegions)
	{
		Debug.Assert(activeRegions.Count > 0, "NewGameFrame cannot be called with zero activeRegions");
		Sim.NewGameFrame* ptr = stackalloc Sim.NewGameFrame[checked(unchecked((UIntPtr)activeRegions.Count) * (UIntPtr)sizeof(Sim.NewGameFrame))];
		Sim.NewGameFrame* ptr2 = ptr;
		foreach (Game.SimActiveRegion simActiveRegion in activeRegions)
		{
			Pair<Vector2I, Vector2I> region = simActiveRegion.region;
			region.first = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, simActiveRegion.region.first.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.first.y));
			region.second = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells, simActiveRegion.region.second.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.second.y));
			ptr2->elapsedSeconds = elapsed_seconds;
			ptr2->minX = region.first.x;
			ptr2->minY = region.first.y;
			ptr2->maxX = region.second.x;
			ptr2->maxY = region.second.y;
			ptr2->currentSunlightIntensity = simActiveRegion.currentSunlightIntensity;
			ptr2->currentCosmicRadiationIntensity = simActiveRegion.currentCosmicRadiationIntensity;
			ptr2++;
		}
		Sim.SIM_HandleMessage(-775326397, sizeof(Sim.NewGameFrame) * activeRegions.Count, (byte*)ptr);
	}

	// Token: 0x0600525D RID: 21085 RVA: 0x001D7864 File Offset: 0x001D5A64
	public unsafe static void SetDebugProperties(Sim.DebugProperties properties)
	{
		Sim.DebugProperties* ptr = stackalloc Sim.DebugProperties[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(Sim.DebugProperties))];
		*ptr = properties;
		ptr->buildingTemperatureScale = properties.buildingTemperatureScale;
		ptr->buildingToBuildingTemperatureScale = properties.buildingToBuildingTemperatureScale;
		Sim.SIM_HandleMessage(-1683118492, sizeof(Sim.DebugProperties), (byte*)ptr);
	}

	// Token: 0x0600525E RID: 21086 RVA: 0x001D78B0 File Offset: 0x001D5AB0
	public unsafe static void ModifyCellWorldZone(int cell, byte zone_id)
	{
		SimMessages.CellWorldZoneModification* ptr = stackalloc SimMessages.CellWorldZoneModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellWorldZoneModification))];
		ptr->cell = cell;
		ptr->zoneID = zone_id;
		Sim.SIM_HandleMessage(-449718014, sizeof(SimMessages.CellWorldZoneModification), (byte*)ptr);
	}

	// Token: 0x0400360F RID: 13839
	public const int InvalidCallback = -1;

	// Token: 0x04003610 RID: 13840
	public const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x02001971 RID: 6513
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementConsumerMessage
	{
		// Token: 0x040075FC RID: 30204
		public int cellIdx;

		// Token: 0x040075FD RID: 30205
		public int callbackIdx;

		// Token: 0x040075FE RID: 30206
		public byte radius;

		// Token: 0x040075FF RID: 30207
		public byte configuration;

		// Token: 0x04007600 RID: 30208
		public ushort elementIdx;
	}

	// Token: 0x02001972 RID: 6514
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementConsumerDataMessage
	{
		// Token: 0x04007601 RID: 30209
		public int handle;

		// Token: 0x04007602 RID: 30210
		public int cell;

		// Token: 0x04007603 RID: 30211
		public float consumptionRate;
	}

	// Token: 0x02001973 RID: 6515
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementConsumerMessage
	{
		// Token: 0x04007604 RID: 30212
		public int handle;

		// Token: 0x04007605 RID: 30213
		public int callbackIdx;
	}

	// Token: 0x02001974 RID: 6516
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementEmitterMessage
	{
		// Token: 0x04007606 RID: 30214
		public float maxPressure;

		// Token: 0x04007607 RID: 30215
		public int callbackIdx;

		// Token: 0x04007608 RID: 30216
		public int onBlockedCB;

		// Token: 0x04007609 RID: 30217
		public int onUnblockedCB;
	}

	// Token: 0x02001975 RID: 6517
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementEmitterMessage
	{
		// Token: 0x0400760A RID: 30218
		public int handle;

		// Token: 0x0400760B RID: 30219
		public int cellIdx;

		// Token: 0x0400760C RID: 30220
		public float emitInterval;

		// Token: 0x0400760D RID: 30221
		public float emitMass;

		// Token: 0x0400760E RID: 30222
		public float emitTemperature;

		// Token: 0x0400760F RID: 30223
		public float maxPressure;

		// Token: 0x04007610 RID: 30224
		public int diseaseCount;

		// Token: 0x04007611 RID: 30225
		public ushort elementIdx;

		// Token: 0x04007612 RID: 30226
		public byte maxDepth;

		// Token: 0x04007613 RID: 30227
		public byte diseaseIdx;
	}

	// Token: 0x02001976 RID: 6518
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementEmitterMessage
	{
		// Token: 0x04007614 RID: 30228
		public int handle;

		// Token: 0x04007615 RID: 30229
		public int callbackIdx;
	}

	// Token: 0x02001977 RID: 6519
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddRadiationEmitterMessage
	{
		// Token: 0x04007616 RID: 30230
		public int callbackIdx;

		// Token: 0x04007617 RID: 30231
		public int cell;

		// Token: 0x04007618 RID: 30232
		public short emitRadiusX;

		// Token: 0x04007619 RID: 30233
		public short emitRadiusY;

		// Token: 0x0400761A RID: 30234
		public float emitRads;

		// Token: 0x0400761B RID: 30235
		public float emitRate;

		// Token: 0x0400761C RID: 30236
		public float emitSpeed;

		// Token: 0x0400761D RID: 30237
		public float emitDirection;

		// Token: 0x0400761E RID: 30238
		public float emitAngle;

		// Token: 0x0400761F RID: 30239
		public int emitType;
	}

	// Token: 0x02001978 RID: 6520
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyRadiationEmitterMessage
	{
		// Token: 0x04007620 RID: 30240
		public int handle;

		// Token: 0x04007621 RID: 30241
		public int cell;

		// Token: 0x04007622 RID: 30242
		public int callbackIdx;

		// Token: 0x04007623 RID: 30243
		public short emitRadiusX;

		// Token: 0x04007624 RID: 30244
		public short emitRadiusY;

		// Token: 0x04007625 RID: 30245
		public float emitRads;

		// Token: 0x04007626 RID: 30246
		public float emitRate;

		// Token: 0x04007627 RID: 30247
		public float emitSpeed;

		// Token: 0x04007628 RID: 30248
		public float emitDirection;

		// Token: 0x04007629 RID: 30249
		public float emitAngle;

		// Token: 0x0400762A RID: 30250
		public int emitType;
	}

	// Token: 0x02001979 RID: 6521
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveRadiationEmitterMessage
	{
		// Token: 0x0400762B RID: 30251
		public int handle;

		// Token: 0x0400762C RID: 30252
		public int callbackIdx;
	}

	// Token: 0x0200197A RID: 6522
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementChunkMessage
	{
		// Token: 0x0400762D RID: 30253
		public int gameCell;

		// Token: 0x0400762E RID: 30254
		public int callbackIdx;

		// Token: 0x0400762F RID: 30255
		public float mass;

		// Token: 0x04007630 RID: 30256
		public float temperature;

		// Token: 0x04007631 RID: 30257
		public float surfaceArea;

		// Token: 0x04007632 RID: 30258
		public float thickness;

		// Token: 0x04007633 RID: 30259
		public float groundTransferScale;

		// Token: 0x04007634 RID: 30260
		public ushort elementIdx;

		// Token: 0x04007635 RID: 30261
		public byte pad0;

		// Token: 0x04007636 RID: 30262
		public byte pad1;
	}

	// Token: 0x0200197B RID: 6523
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementChunkMessage
	{
		// Token: 0x04007637 RID: 30263
		public int handle;

		// Token: 0x04007638 RID: 30264
		public int callbackIdx;
	}

	// Token: 0x0200197C RID: 6524
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementChunkDataMessage
	{
		// Token: 0x04007639 RID: 30265
		public int handle;

		// Token: 0x0400763A RID: 30266
		public float temperature;

		// Token: 0x0400763B RID: 30267
		public float heatCapacity;
	}

	// Token: 0x0200197D RID: 6525
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MoveElementChunkMessage
	{
		// Token: 0x0400763C RID: 30268
		public int handle;

		// Token: 0x0400763D RID: 30269
		public int gameCell;
	}

	// Token: 0x0200197E RID: 6526
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkEnergyMessage
	{
		// Token: 0x0400763E RID: 30270
		public int handle;

		// Token: 0x0400763F RID: 30271
		public float deltaKJ;
	}

	// Token: 0x0200197F RID: 6527
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkAdjusterMessage
	{
		// Token: 0x04007640 RID: 30272
		public int handle;

		// Token: 0x04007641 RID: 30273
		public float temperature;

		// Token: 0x04007642 RID: 30274
		public float heatCapacity;

		// Token: 0x04007643 RID: 30275
		public float thermalConductivity;
	}

	// Token: 0x02001980 RID: 6528
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingHeatExchangeMessage
	{
		// Token: 0x04007644 RID: 30276
		public int callbackIdx;

		// Token: 0x04007645 RID: 30277
		public ushort elemIdx;

		// Token: 0x04007646 RID: 30278
		public byte pad0;

		// Token: 0x04007647 RID: 30279
		public byte pad1;

		// Token: 0x04007648 RID: 30280
		public float mass;

		// Token: 0x04007649 RID: 30281
		public float temperature;

		// Token: 0x0400764A RID: 30282
		public float thermalConductivity;

		// Token: 0x0400764B RID: 30283
		public float overheatTemperature;

		// Token: 0x0400764C RID: 30284
		public float operatingKilowatts;

		// Token: 0x0400764D RID: 30285
		public int minX;

		// Token: 0x0400764E RID: 30286
		public int minY;

		// Token: 0x0400764F RID: 30287
		public int maxX;

		// Token: 0x04007650 RID: 30288
		public int maxY;
	}

	// Token: 0x02001981 RID: 6529
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingHeatExchangeMessage
	{
		// Token: 0x04007651 RID: 30289
		public int callbackIdx;

		// Token: 0x04007652 RID: 30290
		public ushort elemIdx;

		// Token: 0x04007653 RID: 30291
		public byte pad0;

		// Token: 0x04007654 RID: 30292
		public byte pad1;

		// Token: 0x04007655 RID: 30293
		public float mass;

		// Token: 0x04007656 RID: 30294
		public float temperature;

		// Token: 0x04007657 RID: 30295
		public float thermalConductivity;

		// Token: 0x04007658 RID: 30296
		public float overheatTemperature;

		// Token: 0x04007659 RID: 30297
		public float operatingKilowatts;

		// Token: 0x0400765A RID: 30298
		public int minX;

		// Token: 0x0400765B RID: 30299
		public int minY;

		// Token: 0x0400765C RID: 30300
		public int maxX;

		// Token: 0x0400765D RID: 30301
		public int maxY;
	}

	// Token: 0x02001982 RID: 6530
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingEnergyMessage
	{
		// Token: 0x0400765E RID: 30302
		public int handle;

		// Token: 0x0400765F RID: 30303
		public float deltaKJ;

		// Token: 0x04007660 RID: 30304
		public float minTemperature;

		// Token: 0x04007661 RID: 30305
		public float maxTemperature;
	}

	// Token: 0x02001983 RID: 6531
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingHeatExchangeMessage
	{
		// Token: 0x04007662 RID: 30306
		public int handle;

		// Token: 0x04007663 RID: 30307
		public int callbackIdx;
	}

	// Token: 0x02001984 RID: 6532
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RegisterBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04007664 RID: 30308
		public int callbackIdx;

		// Token: 0x04007665 RID: 30309
		public int structureTemperatureHandler;
	}

	// Token: 0x02001985 RID: 6533
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04007666 RID: 30310
		public int selfHandler;

		// Token: 0x04007667 RID: 30311
		public int buildingInContactHandle;

		// Token: 0x04007668 RID: 30312
		public int cellsInContact;
	}

	// Token: 0x02001986 RID: 6534
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04007669 RID: 30313
		public int selfHandler;

		// Token: 0x0400766A RID: 30314
		public int buildingNoLongerInContactHandler;
	}

	// Token: 0x02001987 RID: 6535
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x0400766B RID: 30315
		public int callbackIdx;

		// Token: 0x0400766C RID: 30316
		public int selfHandler;
	}

	// Token: 0x02001988 RID: 6536
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddDiseaseEmitterMessage
	{
		// Token: 0x0400766D RID: 30317
		public int callbackIdx;
	}

	// Token: 0x02001989 RID: 6537
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyDiseaseEmitterMessage
	{
		// Token: 0x0400766E RID: 30318
		public int handle;

		// Token: 0x0400766F RID: 30319
		public int gameCell;

		// Token: 0x04007670 RID: 30320
		public byte diseaseIdx;

		// Token: 0x04007671 RID: 30321
		public byte maxDepth;

		// Token: 0x04007672 RID: 30322
		private byte pad0;

		// Token: 0x04007673 RID: 30323
		private byte pad1;

		// Token: 0x04007674 RID: 30324
		public float emitInterval;

		// Token: 0x04007675 RID: 30325
		public int emitCount;
	}

	// Token: 0x0200198A RID: 6538
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveDiseaseEmitterMessage
	{
		// Token: 0x04007676 RID: 30326
		public int handle;

		// Token: 0x04007677 RID: 30327
		public int callbackIdx;
	}

	// Token: 0x0200198B RID: 6539
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetSavedOptionsMessage
	{
		// Token: 0x04007678 RID: 30328
		public byte clearBits;

		// Token: 0x04007679 RID: 30329
		public byte setBits;
	}

	// Token: 0x0200198C RID: 6540
	public enum SimSavedOptions : byte
	{
		// Token: 0x0400767B RID: 30331
		ENABLE_DIAGONAL_FALLING_SAND = 1
	}

	// Token: 0x0200198D RID: 6541
	public struct WorldOffsetData
	{
		// Token: 0x0400767C RID: 30332
		public int worldOffsetX;

		// Token: 0x0400767D RID: 30333
		public int worldOffsetY;

		// Token: 0x0400767E RID: 30334
		public int worldSizeX;

		// Token: 0x0400767F RID: 30335
		public int worldSizeY;
	}

	// Token: 0x0200198E RID: 6542
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct DigMessage
	{
		// Token: 0x04007680 RID: 30336
		public int cellIdx;

		// Token: 0x04007681 RID: 30337
		public int callbackIdx;

		// Token: 0x04007682 RID: 30338
		public bool skipEvent;
	}

	// Token: 0x0200198F RID: 6543
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetCellFloatValueMessage
	{
		// Token: 0x04007683 RID: 30339
		public int cellIdx;

		// Token: 0x04007684 RID: 30340
		public float value;
	}

	// Token: 0x02001990 RID: 6544
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellPropertiesMessage
	{
		// Token: 0x04007685 RID: 30341
		public int cellIdx;

		// Token: 0x04007686 RID: 30342
		public byte properties;

		// Token: 0x04007687 RID: 30343
		public byte set;

		// Token: 0x04007688 RID: 30344
		public byte pad0;

		// Token: 0x04007689 RID: 30345
		public byte pad1;
	}

	// Token: 0x02001991 RID: 6545
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetInsulationValueMessage
	{
		// Token: 0x0400768A RID: 30346
		public int cellIdx;

		// Token: 0x0400768B RID: 30347
		public float value;
	}

	// Token: 0x02001992 RID: 6546
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellMessage
	{
		// Token: 0x0400768C RID: 30348
		public int cellIdx;

		// Token: 0x0400768D RID: 30349
		public int callbackIdx;

		// Token: 0x0400768E RID: 30350
		public float temperature;

		// Token: 0x0400768F RID: 30351
		public float mass;

		// Token: 0x04007690 RID: 30352
		public int diseaseCount;

		// Token: 0x04007691 RID: 30353
		public ushort elementIdx;

		// Token: 0x04007692 RID: 30354
		public byte replaceType;

		// Token: 0x04007693 RID: 30355
		public byte diseaseIdx;

		// Token: 0x04007694 RID: 30356
		public byte addSubType;
	}

	// Token: 0x02001993 RID: 6547
	public enum ReplaceType
	{
		// Token: 0x04007696 RID: 30358
		None,
		// Token: 0x04007697 RID: 30359
		Replace,
		// Token: 0x04007698 RID: 30360
		ReplaceAndDisplace
	}

	// Token: 0x02001994 RID: 6548
	private enum AddSolidMassSubType
	{
		// Token: 0x0400769A RID: 30362
		DoVerticalDisplacement,
		// Token: 0x0400769B RID: 30363
		OnlyIfSameElement
	}

	// Token: 0x02001995 RID: 6549
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellDiseaseModification
	{
		// Token: 0x0400769C RID: 30364
		public int cellIdx;

		// Token: 0x0400769D RID: 30365
		public byte diseaseIdx;

		// Token: 0x0400769E RID: 30366
		public byte pad0;

		// Token: 0x0400769F RID: 30367
		public byte pad1;

		// Token: 0x040076A0 RID: 30368
		public byte pad2;

		// Token: 0x040076A1 RID: 30369
		public int diseaseCount;
	}

	// Token: 0x02001996 RID: 6550
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RadiationParamsModification
	{
		// Token: 0x040076A2 RID: 30370
		public int RadiationParamsType;

		// Token: 0x040076A3 RID: 30371
		public float value;
	}

	// Token: 0x02001997 RID: 6551
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellRadiationModification
	{
		// Token: 0x040076A4 RID: 30372
		public int cellIdx;

		// Token: 0x040076A5 RID: 30373
		public float radiationDelta;

		// Token: 0x040076A6 RID: 30374
		public int callbackIdx;
	}

	// Token: 0x02001998 RID: 6552
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassConsumptionMessage
	{
		// Token: 0x040076A7 RID: 30375
		public int cellIdx;

		// Token: 0x040076A8 RID: 30376
		public int callbackIdx;

		// Token: 0x040076A9 RID: 30377
		public float mass;

		// Token: 0x040076AA RID: 30378
		public ushort elementIdx;

		// Token: 0x040076AB RID: 30379
		public byte radius;
	}

	// Token: 0x02001999 RID: 6553
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassEmissionMessage
	{
		// Token: 0x040076AC RID: 30380
		public int cellIdx;

		// Token: 0x040076AD RID: 30381
		public int callbackIdx;

		// Token: 0x040076AE RID: 30382
		public float mass;

		// Token: 0x040076AF RID: 30383
		public float temperature;

		// Token: 0x040076B0 RID: 30384
		public int diseaseCount;

		// Token: 0x040076B1 RID: 30385
		public ushort elementIdx;

		// Token: 0x040076B2 RID: 30386
		public byte diseaseIdx;
	}

	// Token: 0x0200199A RID: 6554
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConsumeDiseaseMessage
	{
		// Token: 0x040076B3 RID: 30387
		public int gameCell;

		// Token: 0x040076B4 RID: 30388
		public int callbackIdx;

		// Token: 0x040076B5 RID: 30389
		public float percentToConsume;

		// Token: 0x040076B6 RID: 30390
		public int maxToConsume;
	}

	// Token: 0x0200199B RID: 6555
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellEnergyMessage
	{
		// Token: 0x040076B7 RID: 30391
		public int cellIdx;

		// Token: 0x040076B8 RID: 30392
		public float kilojoules;

		// Token: 0x040076B9 RID: 30393
		public float maxTemperature;

		// Token: 0x040076BA RID: 30394
		public int id;
	}

	// Token: 0x0200199C RID: 6556
	public enum EnergySourceID
	{
		// Token: 0x040076BC RID: 30396
		DebugHeat = 1000,
		// Token: 0x040076BD RID: 30397
		DebugCool,
		// Token: 0x040076BE RID: 30398
		FierySkin,
		// Token: 0x040076BF RID: 30399
		Overheatable,
		// Token: 0x040076C0 RID: 30400
		LiquidCooledFan,
		// Token: 0x040076C1 RID: 30401
		ConduitTemperatureManager,
		// Token: 0x040076C2 RID: 30402
		Excavator,
		// Token: 0x040076C3 RID: 30403
		HeatBulb,
		// Token: 0x040076C4 RID: 30404
		WarmBlooded,
		// Token: 0x040076C5 RID: 30405
		StructureTemperature,
		// Token: 0x040076C6 RID: 30406
		Burner,
		// Token: 0x040076C7 RID: 30407
		VacuumRadiator
	}

	// Token: 0x0200199D RID: 6557
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct VisibleCells
	{
		// Token: 0x040076C8 RID: 30408
		public Vector2I min;

		// Token: 0x040076C9 RID: 30409
		public Vector2I max;
	}

	// Token: 0x0200199E RID: 6558
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct WakeCellMessage
	{
		// Token: 0x040076CA RID: 30410
		public int gameCell;
	}

	// Token: 0x0200199F RID: 6559
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementInteraction
	{
		// Token: 0x040076CB RID: 30411
		public uint interactionType;

		// Token: 0x040076CC RID: 30412
		public ushort elemIdx1;

		// Token: 0x040076CD RID: 30413
		public ushort elemIdx2;

		// Token: 0x040076CE RID: 30414
		public ushort elemResultIdx;

		// Token: 0x040076CF RID: 30415
		public byte pad0;

		// Token: 0x040076D0 RID: 30416
		public byte pad1;

		// Token: 0x040076D1 RID: 30417
		public float minMass;

		// Token: 0x040076D2 RID: 30418
		public float interactionProbability;

		// Token: 0x040076D3 RID: 30419
		public float elem1MassDestructionPercent;

		// Token: 0x040076D4 RID: 30420
		public float elem2MassRequiredMultiplier;

		// Token: 0x040076D5 RID: 30421
		public float elemResultMassCreationMultiplier;
	}

	// Token: 0x020019A0 RID: 6560
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CreateElementInteractionsMsg
	{
		// Token: 0x040076D6 RID: 30422
		public int numInteractions;

		// Token: 0x040076D7 RID: 30423
		public unsafe SimMessages.ElementInteraction* interactions;
	}

	// Token: 0x020019A1 RID: 6561
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeChange
	{
		// Token: 0x040076D8 RID: 30424
		public int cell;

		// Token: 0x040076D9 RID: 30425
		public byte layer;

		// Token: 0x040076DA RID: 30426
		public byte pad0;

		// Token: 0x040076DB RID: 30427
		public byte pad1;

		// Token: 0x040076DC RID: 30428
		public byte pad2;

		// Token: 0x040076DD RID: 30429
		public float mass;

		// Token: 0x040076DE RID: 30430
		public float temperature;

		// Token: 0x040076DF RID: 30431
		public int elementHash;
	}

	// Token: 0x020019A2 RID: 6562
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellWorldZoneModification
	{
		// Token: 0x040076E0 RID: 30432
		public int cell;

		// Token: 0x040076E1 RID: 30433
		public byte zoneID;
	}
}
