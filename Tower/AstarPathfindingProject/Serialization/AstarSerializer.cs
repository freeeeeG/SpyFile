using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Zip;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x020000B5 RID: 181
	public class AstarSerializer
	{
		// Token: 0x060007EF RID: 2031 RVA: 0x00034BCC File Offset: 0x00032DCC
		private static StringBuilder GetStringBuilder()
		{
			AstarSerializer._stringBuilder.Length = 0;
			return AstarSerializer._stringBuilder;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00034BDE File Offset: 0x00032DDE
		public AstarSerializer(AstarData data, GameObject contextRoot) : this(data, SerializeSettings.Settings, contextRoot)
		{
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00034BED File Offset: 0x00032DED
		public AstarSerializer(AstarData data, SerializeSettings settings, GameObject contextRoot)
		{
			this.data = data;
			this.contextRoot = contextRoot;
			this.settings = settings;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00034C1C File Offset: 0x00032E1C
		public void SetGraphIndexOffset(int offset)
		{
			this.graphIndexOffset = offset;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00034C25 File Offset: 0x00032E25
		private void AddChecksum(byte[] bytes)
		{
			this.checksum = Checksum.GetChecksum(bytes, this.checksum);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00034C39 File Offset: 0x00032E39
		private void AddEntry(string name, byte[] bytes)
		{
			this.zip.AddEntry(name, bytes);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00034C49 File Offset: 0x00032E49
		public uint GetChecksum()
		{
			return this.checksum;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00034C54 File Offset: 0x00032E54
		public void OpenSerialize()
		{
			this.zipStream = new MemoryStream();
			this.zip = new ZipFile();
			this.zip.AlternateEncoding = Encoding.UTF8;
			this.zip.AlternateEncodingUsage = ZipOption.Always;
			this.zip.ParallelDeflateThreshold = -1L;
			this.meta = new GraphMeta();
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00034CAC File Offset: 0x00032EAC
		public byte[] CloseSerialize()
		{
			byte[] array = this.SerializeMeta();
			this.AddChecksum(array);
			this.AddEntry("meta.json", array);
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			foreach (ZipEntry zipEntry in this.zip.Entries)
			{
				zipEntry.AccessedTime = dateTime;
				zipEntry.CreationTime = dateTime;
				zipEntry.LastModified = dateTime;
				zipEntry.ModifiedTime = dateTime;
			}
			this.zip.Save(this.zipStream);
			this.zip.Dispose();
			array = this.zipStream.ToArray();
			this.zip = null;
			this.zipStream = null;
			return array;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00034D74 File Offset: 0x00032F74
		public void SerializeGraphs(NavGraph[] _graphs)
		{
			if (this.graphs != null)
			{
				throw new InvalidOperationException("Cannot serialize graphs multiple times.");
			}
			this.graphs = _graphs;
			if (this.zip == null)
			{
				throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
			}
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					byte[] bytes = this.Serialize(this.graphs[i]);
					this.AddChecksum(bytes);
					this.AddEntry("graph" + i.ToString() + ".json", bytes);
				}
			}
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00034E14 File Offset: 0x00033014
		private byte[] SerializeMeta()
		{
			if (this.graphs == null)
			{
				throw new Exception("No call to SerializeGraphs has been done");
			}
			this.meta.version = AstarPath.Version;
			this.meta.graphs = this.graphs.Length;
			this.meta.guids = new List<string>();
			this.meta.typeNames = new List<string>();
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.meta.guids.Add(this.graphs[i].guid.ToString());
					this.meta.typeNames.Add(this.graphs[i].GetType().FullName);
				}
				else
				{
					this.meta.guids.Add(null);
					this.meta.typeNames.Add(null);
				}
			}
			StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
			TinyJsonSerializer.Serialize(this.meta, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00034F28 File Offset: 0x00033128
		public byte[] Serialize(NavGraph graph)
		{
			StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
			TinyJsonSerializer.Serialize(graph, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00034F53 File Offset: 0x00033153
		[Obsolete("Not used anymore. You can safely remove the call to this function.")]
		public void SerializeNodes()
		{
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00034F58 File Offset: 0x00033158
		private static int GetMaxNodeIndexInAllGraphs(NavGraph[] graphs)
		{
			int maxIndex = 0;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							maxIndex = Math.Max(node.NodeIndex, maxIndex);
							if (node.NodeIndex == -1)
							{
								Debug.LogError("Graph contains destroyed nodes. This is a bug.");
							}
						});
					}
					navGraph.GetNodes(action);
				}
			}
			return maxIndex;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00034FB4 File Offset: 0x000331B4
		private static byte[] SerializeNodeIndices(NavGraph[] graphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			int maxNodeIndexInAllGraphs = AstarSerializer.GetMaxNodeIndexInAllGraphs(graphs);
			writer.Write(maxNodeIndexInAllGraphs);
			int maxNodeIndex2 = 0;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							maxNodeIndex2 = Math.Max(node.NodeIndex, maxNodeIndex2);
							writer.Write(node.NodeIndex);
						});
					}
					navGraph.GetNodes(action);
				}
			}
			if (maxNodeIndex2 != maxNodeIndexInAllGraphs)
			{
				throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
			}
			byte[] result = memoryStream.ToArray();
			writer.Close();
			return result;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00035054 File Offset: 0x00033254
		private static byte[] SerializeGraphExtraInfo(NavGraph graph)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter);
			((IGraphInternals)graph).SerializeExtraInfo(ctx);
			byte[] result = memoryStream.ToArray();
			binaryWriter.Close();
			return result;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00035088 File Offset: 0x00033288
		private static byte[] SerializeGraphNodeReferences(NavGraph graph)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter);
			graph.GetNodes(delegate(GraphNode node)
			{
				node.SerializeReferences(ctx);
			});
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000350D0 File Offset: 0x000332D0
		public void SerializeExtraInfo()
		{
			if (!this.settings.nodes)
			{
				return;
			}
			if (this.graphs == null)
			{
				throw new InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
			}
			byte[] bytes = AstarSerializer.SerializeNodeIndices(this.graphs);
			this.AddChecksum(bytes);
			this.AddEntry("graph_references.binary", bytes);
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					bytes = AstarSerializer.SerializeGraphExtraInfo(this.graphs[i]);
					this.AddChecksum(bytes);
					this.AddEntry("graph" + i.ToString() + "_extra.binary", bytes);
					bytes = AstarSerializer.SerializeGraphNodeReferences(this.graphs[i]);
					this.AddChecksum(bytes);
					this.AddEntry("graph" + i.ToString() + "_references.binary", bytes);
				}
			}
			bytes = this.SerializeNodeLinks();
			this.AddChecksum(bytes);
			this.AddEntry("node_link2.binary", bytes);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000351B8 File Offset: 0x000333B8
		private byte[] SerializeNodeLinks()
		{
			MemoryStream memoryStream = new MemoryStream();
			NodeLink2.SerializeReferences(new GraphSerializationContext(new BinaryWriter(memoryStream)));
			return memoryStream.ToArray();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x000351D4 File Offset: 0x000333D4
		private ZipEntry GetEntry(string name)
		{
			return this.zip[name];
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000351E2 File Offset: 0x000333E2
		private bool ContainsEntry(string name)
		{
			return this.GetEntry(name) != null;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000351F0 File Offset: 0x000333F0
		public bool OpenDeserialize(byte[] bytes)
		{
			this.zipStream = new MemoryStream();
			this.zipStream.Write(bytes, 0, bytes.Length);
			this.zipStream.Position = 0L;
			try
			{
				this.zip = ZipFile.Read(this.zipStream);
				this.zip.ParallelDeflateThreshold = -1L;
			}
			catch (Exception ex)
			{
				string str = "Caught exception when loading from zip\n";
				Exception ex2 = ex;
				Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
				this.zipStream.Dispose();
				return false;
			}
			if (this.ContainsEntry("meta.json"))
			{
				this.meta = this.DeserializeMeta(this.GetEntry("meta.json"));
			}
			else
			{
				if (!this.ContainsEntry("meta.binary"))
				{
					throw new Exception("No metadata found in serialized data.");
				}
				this.meta = this.DeserializeBinaryMeta(this.GetEntry("meta.binary"));
			}
			if (AstarSerializer.FullyDefinedVersion(this.meta.version) > AstarSerializer.FullyDefinedVersion(AstarPath.Version))
			{
				string[] array = new string[5];
				array[0] = "Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ";
				int num = 1;
				Version version = AstarPath.Version;
				array[num] = ((version != null) ? version.ToString() : null);
				array[2] = " Data version: ";
				int num2 = 3;
				Version version2 = this.meta.version;
				array[num2] = ((version2 != null) ? version2.ToString() : null);
				array[4] = "\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n";
				Debug.LogWarning(string.Concat(array));
			}
			return true;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00035358 File Offset: 0x00033558
		private static Version FullyDefinedVersion(Version v)
		{
			return new Version(Mathf.Max(v.Major, 0), Mathf.Max(v.Minor, 0), Mathf.Max(v.Build, 0), Mathf.Max(v.Revision, 0));
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0003538F File Offset: 0x0003358F
		public void CloseDeserialize()
		{
			this.zipStream.Dispose();
			this.zip.Dispose();
			this.zip = null;
			this.zipStream = null;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000353B8 File Offset: 0x000335B8
		private NavGraph DeserializeGraph(int zipIndex, int graphIndex, Type[] availableGraphTypes)
		{
			Type graphType = this.meta.GetGraphType(zipIndex, availableGraphTypes);
			if (object.Equals(graphType, null))
			{
				return null;
			}
			NavGraph navGraph = this.data.CreateGraph(graphType);
			navGraph.graphIndex = (uint)graphIndex;
			string name = "graph" + zipIndex.ToString() + ".json";
			string name2 = "graph" + zipIndex.ToString() + ".binary";
			if (this.ContainsEntry(name))
			{
				TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(this.GetEntry(name)), graphType, navGraph, this.contextRoot);
			}
			else
			{
				if (!this.ContainsEntry(name2))
				{
					throw new FileNotFoundException(string.Concat(new string[]
					{
						"Could not find data for graph ",
						zipIndex.ToString(),
						" in zip. Entry 'graph",
						zipIndex.ToString(),
						".json' does not exist"
					}));
				}
				GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(this.GetEntry(name2)), null, navGraph.graphIndex, this.meta);
				((IGraphInternals)navGraph).DeserializeSettingsCompatibility(ctx);
			}
			if (navGraph.guid.ToString() != this.meta.guids[zipIndex])
			{
				string str = "Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n";
				Guid guid = navGraph.guid;
				throw new Exception(str + guid.ToString() + " != " + this.meta.guids[zipIndex]);
			}
			return navGraph;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0003551C File Offset: 0x0003371C
		public NavGraph[] DeserializeGraphs(Type[] availableGraphTypes)
		{
			List<NavGraph> list = new List<NavGraph>();
			this.graphIndexInZip = new Dictionary<NavGraph, int>();
			for (int i = 0; i < this.meta.graphs; i++)
			{
				int graphIndex = list.Count + this.graphIndexOffset;
				NavGraph navGraph = this.DeserializeGraph(i, graphIndex, availableGraphTypes);
				if (navGraph != null)
				{
					list.Add(navGraph);
					this.graphIndexInZip[navGraph] = i;
				}
			}
			this.graphs = list.ToArray();
			return this.graphs;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00035594 File Offset: 0x00033794
		private bool DeserializeExtraInfo(NavGraph graph)
		{
			ZipEntry entry = this.GetEntry("graph" + this.graphIndexInZip[graph].ToString() + "_extra.binary");
			if (entry == null)
			{
				return false;
			}
			GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), null, graph.graphIndex, this.meta);
			((IGraphInternals)graph).DeserializeExtraInfo(ctx);
			return true;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000355F4 File Offset: 0x000337F4
		private bool AnyDestroyedNodesInGraphs()
		{
			bool result = false;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				NavGraph navGraph = this.graphs[i];
				Action<GraphNode> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(GraphNode node)
					{
						if (node.Destroyed)
						{
							result = true;
						}
					});
				}
				navGraph.GetNodes(action);
			}
			return result;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00035654 File Offset: 0x00033854
		private GraphNode[] DeserializeNodeReferenceMap()
		{
			ZipEntry entry = this.GetEntry("graph_references.binary");
			if (entry == null)
			{
				throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader reader = AstarSerializer.GetBinaryReader(entry);
			int num = reader.ReadInt32();
			GraphNode[] int2Node = new GraphNode[num + 1];
			try
			{
				Action<GraphNode> <>9__0;
				for (int i = 0; i < this.graphs.Length; i++)
				{
					NavGraph navGraph = this.graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							int num2 = reader.ReadInt32();
							int2Node[num2] = node;
						});
					}
					navGraph.GetNodes(action);
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", innerException);
			}
			if (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				throw new Exception((reader.BaseStream.Length / 4L).ToString() + " nodes were serialized, but only data for " + (reader.BaseStream.Position / 4L).ToString() + " nodes was found. The data looks corrupt.");
			}
			reader.Close();
			return int2Node;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0003578C File Offset: 0x0003398C
		private void DeserializeNodeReferences(NavGraph graph, GraphNode[] int2Node)
		{
			int num = this.graphIndexInZip[graph];
			ZipEntry entry = this.GetEntry("graph" + num.ToString() + "_references.binary");
			if (entry == null)
			{
				throw new Exception("Node references for graph " + num.ToString() + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader binaryReader = AstarSerializer.GetBinaryReader(entry);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryReader, int2Node, graph.graphIndex, this.meta);
			graph.GetNodes(delegate(GraphNode node)
			{
				node.DeserializeReferences(ctx);
			});
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00035818 File Offset: 0x00033A18
		public void DeserializeExtraInfo()
		{
			bool flag = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				flag |= this.DeserializeExtraInfo(this.graphs[i]);
			}
			if (!flag)
			{
				return;
			}
			if (this.AnyDestroyedNodesInGraphs())
			{
				Debug.LogError("Graph contains destroyed nodes. This is a bug.");
			}
			GraphNode[] int2Node = this.DeserializeNodeReferenceMap();
			for (int j = 0; j < this.graphs.Length; j++)
			{
				this.DeserializeNodeReferences(this.graphs[j], int2Node);
			}
			this.DeserializeNodeLinks(int2Node);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00035894 File Offset: 0x00033A94
		private void DeserializeNodeLinks(GraphNode[] int2Node)
		{
			ZipEntry entry = this.GetEntry("node_link2.binary");
			if (entry == null)
			{
				return;
			}
			NodeLink2.DeserializeReferences(new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), int2Node, 0U, this.meta));
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000358CC File Offset: 0x00033ACC
		public void PostDeserialization()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				GraphSerializationContext ctx = new GraphSerializationContext(null, null, 0U, this.meta);
				((IGraphInternals)this.graphs[i]).PostDeserialization(ctx);
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0003590C File Offset: 0x00033B0C
		public void DeserializeEditorSettingsCompatibility()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				ZipEntry entry = this.GetEntry("graph" + this.graphIndexInZip[this.graphs[i]].ToString() + "_editor.json");
				if (entry != null)
				{
					((IGraphInternals)this.graphs[i]).SerializedEditorSettings = AstarSerializer.GetString(entry);
				}
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00035974 File Offset: 0x00033B74
		private static BinaryReader GetBinaryReader(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			return new BinaryReader(memoryStream);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0003599C File Offset: 0x00033B9C
		private static string GetString(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			streamReader.Dispose();
			return result;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000359D1 File Offset: 0x00033BD1
		private GraphMeta DeserializeMeta(ZipEntry entry)
		{
			return TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(entry), typeof(GraphMeta), null, null) as GraphMeta;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000359F0 File Offset: 0x00033BF0
		private GraphMeta DeserializeBinaryMeta(ZipEntry entry)
		{
			GraphMeta graphMeta = new GraphMeta();
			BinaryReader binaryReader = AstarSerializer.GetBinaryReader(entry);
			if (binaryReader.ReadString() != "A*")
			{
				throw new Exception("Invalid magic number in saved data");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			int num3 = binaryReader.ReadInt32();
			int num4 = binaryReader.ReadInt32();
			if (num < 0)
			{
				graphMeta.version = new Version(0, 0);
			}
			else if (num2 < 0)
			{
				graphMeta.version = new Version(num, 0);
			}
			else if (num3 < 0)
			{
				graphMeta.version = new Version(num, num2);
			}
			else if (num4 < 0)
			{
				graphMeta.version = new Version(num, num2, num3);
			}
			else
			{
				graphMeta.version = new Version(num, num2, num3, num4);
			}
			graphMeta.graphs = binaryReader.ReadInt32();
			graphMeta.guids = new List<string>();
			int num5 = binaryReader.ReadInt32();
			for (int i = 0; i < num5; i++)
			{
				graphMeta.guids.Add(binaryReader.ReadString());
			}
			graphMeta.typeNames = new List<string>();
			num5 = binaryReader.ReadInt32();
			for (int j = 0; j < num5; j++)
			{
				graphMeta.typeNames.Add(binaryReader.ReadString());
			}
			binaryReader.Close();
			return graphMeta;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00035B24 File Offset: 0x00033D24
		public static void SaveToFile(string path, byte[] data)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(data, 0, data.Length);
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00035B60 File Offset: 0x00033D60
		public static byte[] LoadFromFile(string path)
		{
			byte[] result;
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				byte[] array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				result = array;
			}
			return result;
		}

		// Token: 0x040004B3 RID: 1203
		private AstarData data;

		// Token: 0x040004B4 RID: 1204
		private ZipFile zip;

		// Token: 0x040004B5 RID: 1205
		private MemoryStream zipStream;

		// Token: 0x040004B6 RID: 1206
		private GraphMeta meta;

		// Token: 0x040004B7 RID: 1207
		private SerializeSettings settings;

		// Token: 0x040004B8 RID: 1208
		private GameObject contextRoot;

		// Token: 0x040004B9 RID: 1209
		private NavGraph[] graphs;

		// Token: 0x040004BA RID: 1210
		private Dictionary<NavGraph, int> graphIndexInZip;

		// Token: 0x040004BB RID: 1211
		private int graphIndexOffset;

		// Token: 0x040004BC RID: 1212
		private const string binaryExt = ".binary";

		// Token: 0x040004BD RID: 1213
		private const string jsonExt = ".json";

		// Token: 0x040004BE RID: 1214
		private uint checksum = uint.MaxValue;

		// Token: 0x040004BF RID: 1215
		private UTF8Encoding encoding = new UTF8Encoding();

		// Token: 0x040004C0 RID: 1216
		private static StringBuilder _stringBuilder = new StringBuilder();

		// Token: 0x040004C1 RID: 1217
		public static readonly Version V3_8_3 = new Version(3, 8, 3);

		// Token: 0x040004C2 RID: 1218
		public static readonly Version V3_9_0 = new Version(3, 9, 0);

		// Token: 0x040004C3 RID: 1219
		public static readonly Version V4_1_0 = new Version(4, 1, 0);
	}
}
