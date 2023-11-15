using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x0200019E RID: 414
[Serializable]
public class JSON
{
	// Token: 0x170000AD RID: 173
	public object this[string fieldName]
	{
		get
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return this.fields[fieldName];
			}
			return null;
		}
		set
		{
			if (this.fields.ContainsKey(fieldName))
			{
				this.fields[fieldName] = value;
				return;
			}
			this.fields.Add(fieldName, value);
		}
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0002A705 File Offset: 0x00028905
	public string ToString(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToString(this.fields[fieldName]);
		}
		return "";
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0002A72C File Offset: 0x0002892C
	public decimal ToDecimal(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToDecimal(this.fields[fieldName]);
		}
		return 0m;
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0002A753 File Offset: 0x00028953
	public byte ToByte(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToByte(this.fields[fieldName]);
		}
		return 0;
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0002A776 File Offset: 0x00028976
	public sbyte ToSByte(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToSByte(this.fields[fieldName]);
		}
		return 0;
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0002A799 File Offset: 0x00028999
	public short ToInt16(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToInt16(this.fields[fieldName]);
		}
		return 0;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0002A7BC File Offset: 0x000289BC
	public int ToInt(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToInt32(this.fields[fieldName]);
		}
		return 0;
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0002A7DF File Offset: 0x000289DF
	public long ToInt64(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToInt64(this.fields[fieldName]);
		}
		return 0L;
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x0002A803 File Offset: 0x00028A03
	public float ToFloat(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToSingle(this.fields[fieldName]);
		}
		return 0f;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0002A82A File Offset: 0x00028A2A
	public double ToDouble(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName))
		{
			return Convert.ToDouble(this.fields[fieldName]);
		}
		return 0.0;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0002A855 File Offset: 0x00028A55
	public bool ToBoolean(string fieldName)
	{
		return this.fields.ContainsKey(fieldName) && Convert.ToBoolean(this.fields[fieldName]);
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0002A878 File Offset: 0x00028A78
	// (set) Token: 0x06000B09 RID: 2825 RVA: 0x0002A880 File Offset: 0x00028A80
	public string serialized
	{
		get
		{
			return JSON._JSON.Serialize(this);
		}
		set
		{
			JSON json = JSON._JSON.Deserialize(value);
			if (json != null)
			{
				this.fields = json.fields;
			}
		}
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0002A8A3 File Offset: 0x00028AA3
	public JSON ToJSON(string fieldName)
	{
		if (!this.fields.ContainsKey(fieldName))
		{
			this.fields.Add(fieldName, new JSON());
		}
		return (JSON)this[fieldName];
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0002A8D0 File Offset: 0x00028AD0
	public bool ContainsKey(string fieldName)
	{
		return this.fields.ContainsKey(fieldName);
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0002A8DE File Offset: 0x00028ADE
	public int Count()
	{
		return this.fields.Count;
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0002A8EB File Offset: 0x00028AEB
	public static implicit operator Vector2(JSON value)
	{
		return new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]));
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0002A917 File Offset: 0x00028B17
	public static explicit operator JSON(Vector2 value)
	{
		JSON json = new JSON();
		json["x"] = value.x;
		json["y"] = value.y;
		return json;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0002A94A File Offset: 0x00028B4A
	public static implicit operator Vector3(JSON value)
	{
		return new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), Convert.ToSingle(value["z"]));
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0002A984 File Offset: 0x00028B84
	public static explicit operator JSON(Vector3 value)
	{
		JSON json = new JSON();
		json["x"] = value.x;
		json["y"] = value.y;
		json["z"] = value.z;
		return json;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0002A9D8 File Offset: 0x00028BD8
	public static implicit operator Quaternion(JSON value)
	{
		return new Quaternion(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), Convert.ToSingle(value["z"]), Convert.ToSingle(value["w"]));
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0002AA2C File Offset: 0x00028C2C
	public static explicit operator JSON(Quaternion value)
	{
		JSON json = new JSON();
		json["x"] = value.x;
		json["y"] = value.y;
		json["z"] = value.z;
		json["w"] = value.w;
		return json;
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0002AA98 File Offset: 0x00028C98
	public static implicit operator Color(JSON value)
	{
		return new Color(Convert.ToSingle(value["r"]), Convert.ToSingle(value["g"]), Convert.ToSingle(value["b"]), Convert.ToSingle(value["a"]));
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0002AAEC File Offset: 0x00028CEC
	public static explicit operator JSON(Color value)
	{
		JSON json = new JSON();
		json["r"] = value.r;
		json["g"] = value.g;
		json["b"] = value.b;
		json["a"] = value.a;
		return json;
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0002AB58 File Offset: 0x00028D58
	public static implicit operator Color32(JSON value)
	{
		return new Color32(Convert.ToByte(value["r"]), Convert.ToByte(value["g"]), Convert.ToByte(value["b"]), Convert.ToByte(value["a"]));
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0002ABAC File Offset: 0x00028DAC
	public static explicit operator JSON(Color32 value)
	{
		JSON json = new JSON();
		json["r"] = value.r;
		json["g"] = value.g;
		json["b"] = value.b;
		json["a"] = value.a;
		return json;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0002AC18 File Offset: 0x00028E18
	public static implicit operator Rect(JSON value)
	{
		return new Rect((float)Convert.ToByte(value["left"]), (float)Convert.ToByte(value["top"]), (float)Convert.ToByte(value["width"]), (float)Convert.ToByte(value["height"]));
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0002AC70 File Offset: 0x00028E70
	public static explicit operator JSON(Rect value)
	{
		JSON json = new JSON();
		json["left"] = value.xMin;
		json["top"] = value.yMax;
		json["width"] = value.width;
		json["height"] = value.height;
		return json;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0002ACE0 File Offset: 0x00028EE0
	public T[] ToArray<T>(string fieldName)
	{
		if (this.fields.ContainsKey(fieldName) && this.fields[fieldName] is IEnumerable)
		{
			List<T> list = new List<T>();
			foreach (object obj in (this.fields[fieldName] as IEnumerable))
			{
				if (list is List<string>)
				{
					(list as List<string>).Add(Convert.ToString(obj));
				}
				else if (list is List<int>)
				{
					(list as List<int>).Add(Convert.ToInt32(obj));
				}
				else if (list is List<float>)
				{
					(list as List<float>).Add(Convert.ToSingle(obj));
				}
				else if (list is List<double>)
				{
					(list as List<double>).Add(Convert.ToDouble(obj));
				}
				else if (list is List<long>)
				{
					(list as List<long>).Add(Convert.ToInt64(obj));
				}
				else if (list is List<short>)
				{
					(list as List<short>).Add(Convert.ToInt16(obj));
				}
				else if (list is List<decimal>)
				{
					(list as List<decimal>).Add(Convert.ToDecimal(obj));
				}
				else if (list is List<byte>)
				{
					(list as List<byte>).Add(Convert.ToByte(obj));
				}
				else if (list is List<char>)
				{
					(list as List<char>).Add(Convert.ToChar(obj));
				}
				else if (list is List<bool>)
				{
					(list as List<bool>).Add(Convert.ToBoolean(obj));
				}
				else if (list is List<Vector2>)
				{
					(list as List<Vector2>).Add((JSON)obj);
				}
				else if (list is List<Vector3>)
				{
					(list as List<Vector3>).Add((JSON)obj);
				}
				else if (list is List<Rect>)
				{
					(list as List<Rect>).Add((JSON)obj);
				}
				else if (list is List<Color>)
				{
					(list as List<Color>).Add((JSON)obj);
				}
				else if (list is List<Color32>)
				{
					(list as List<Color32>).Add((JSON)obj);
				}
				else if (list is List<Quaternion>)
				{
					(list as List<Quaternion>).Add((JSON)obj);
				}
				else if (list is List<JSON>)
				{
					(list as List<JSON>).Add((JSON)obj);
				}
			}
			return list.ToArray();
		}
		return new T[0];
	}

	// Token: 0x040008EE RID: 2286
	public Dictionary<string, object> fields = new Dictionary<string, object>();

	// Token: 0x020002BD RID: 701
	private sealed class _JSON
	{
		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003957E File Offset: 0x0003777E
		public static JSON Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return JSON._JSON.Parser.Parse(json);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003958B File Offset: 0x0003778B
		public static string Serialize(JSON obj)
		{
			return JSON._JSON.Serializer.Serialize(obj);
		}

		// Token: 0x020002C9 RID: 713
		private sealed class Parser : IDisposable
		{
			// Token: 0x06000FC1 RID: 4033 RVA: 0x00039B84 File Offset: 0x00037D84
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06000FC2 RID: 4034 RVA: 0x00039B98 File Offset: 0x00037D98
			public static JSON Parse(string jsonString)
			{
				JSON result;
				using (JSON._JSON.Parser parser = new JSON._JSON.Parser(jsonString))
				{
					result = (parser.ParseValue() as JSON);
				}
				return result;
			}

			// Token: 0x06000FC3 RID: 4035 RVA: 0x00039BD8 File Offset: 0x00037DD8
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06000FC4 RID: 4036 RVA: 0x00039BEC File Offset: 0x00037DEC
			private JSON ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				JSON json = new JSON();
				json.fields = dictionary;
				this.json.Read();
				for (;;)
				{
					JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == JSON._JSON.Parser.TOKEN.NONE)
					{
						break;
					}
					if (nextToken == JSON._JSON.Parser.TOKEN.CURLY_CLOSE)
					{
						return json;
					}
					if (nextToken != JSON._JSON.Parser.TOKEN.COMMA)
					{
						string text = this.ParseString();
						if (text == null)
						{
							goto Block_4;
						}
						if (this.NextToken != JSON._JSON.Parser.TOKEN.COLON)
						{
							goto Block_5;
						}
						this.json.Read();
						dictionary[text] = this.ParseValue();
					}
				}
				return null;
				Block_4:
				return null;
				Block_5:
				return null;
			}

			// Token: 0x06000FC5 RID: 4037 RVA: 0x00039C64 File Offset: 0x00037E64
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == JSON._JSON.Parser.TOKEN.NONE)
					{
						return null;
					}
					if (nextToken != JSON._JSON.Parser.TOKEN.SQUARED_CLOSE)
					{
						if (nextToken != JSON._JSON.Parser.TOKEN.COMMA)
						{
							object item = this.ParseByToken(nextToken);
							list.Add(item);
						}
					}
					else
					{
						flag = false;
					}
				}
				return list;
			}

			// Token: 0x06000FC6 RID: 4038 RVA: 0x00039CB4 File Offset: 0x00037EB4
			private object ParseValue()
			{
				JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06000FC7 RID: 4039 RVA: 0x00039CD0 File Offset: 0x00037ED0
			private object ParseByToken(JSON._JSON.Parser.TOKEN token)
			{
				switch (token)
				{
				case JSON._JSON.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case JSON._JSON.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case JSON._JSON.Parser.TOKEN.STRING:
					return this.ParseString();
				case JSON._JSON.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case JSON._JSON.Parser.TOKEN.TRUE:
					return true;
				case JSON._JSON.Parser.TOKEN.FALSE:
					return false;
				case JSON._JSON.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x06000FC8 RID: 4040 RVA: 0x00039D40 File Offset: 0x00037F40
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					if (nextChar != '"')
					{
						if (nextChar != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							if (nextChar <= '\\')
							{
								if (nextChar == '"' || nextChar == '/' || nextChar == '\\')
								{
									stringBuilder.Append(nextChar);
								}
							}
							else if (nextChar <= 'f')
							{
								if (nextChar != 'b')
								{
									if (nextChar == 'f')
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
							}
							else if (nextChar != 'n')
							{
								switch (nextChar)
								{
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									StringBuilder stringBuilder2 = new StringBuilder();
									for (int i = 0; i < 4; i++)
									{
										stringBuilder2.Append(this.NextChar);
									}
									stringBuilder.Append((char)Convert.ToInt32(stringBuilder2.ToString(), 16));
									break;
								}
								}
							}
							else
							{
								stringBuilder.Append('\n');
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06000FC9 RID: 4041 RVA: 0x00039E94 File Offset: 0x00038094
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x06000FCA RID: 4042 RVA: 0x00039ED2 File Offset: 0x000380D2
			private void EatWhitespace()
			{
				while (" \t\n\r".IndexOf(this.PeekChar) != -1)
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00039F03 File Offset: 0x00038103
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00039F15 File Offset: 0x00038115
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x170001B3 RID: 435
			// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00039F28 File Offset: 0x00038128
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (" \t\n\r{}[],:\"".IndexOf(this.PeekChar) == -1)
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00039F74 File Offset: 0x00038174
			private JSON._JSON.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return JSON._JSON.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					if (peekChar <= '[')
					{
						switch (peekChar)
						{
						case '"':
							return JSON._JSON.Parser.TOKEN.STRING;
						case '#':
						case '$':
						case '%':
						case '&':
						case '\'':
						case '(':
						case ')':
						case '*':
						case '+':
						case '.':
						case '/':
							break;
						case ',':
							this.json.Read();
							return JSON._JSON.Parser.TOKEN.COMMA;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							return JSON._JSON.Parser.TOKEN.NUMBER;
						case ':':
							return JSON._JSON.Parser.TOKEN.COLON;
						default:
							if (peekChar == '[')
							{
								return JSON._JSON.Parser.TOKEN.SQUARED_OPEN;
							}
							break;
						}
					}
					else
					{
						if (peekChar == ']')
						{
							this.json.Read();
							return JSON._JSON.Parser.TOKEN.SQUARED_CLOSE;
						}
						if (peekChar == '{')
						{
							return JSON._JSON.Parser.TOKEN.CURLY_OPEN;
						}
						if (peekChar == '}')
						{
							this.json.Read();
							return JSON._JSON.Parser.TOKEN.CURLY_CLOSE;
						}
					}
					string nextWord = this.NextWord;
					if (nextWord == "false")
					{
						return JSON._JSON.Parser.TOKEN.FALSE;
					}
					if (nextWord == "true")
					{
						return JSON._JSON.Parser.TOKEN.TRUE;
					}
					if (!(nextWord == "null"))
					{
						return JSON._JSON.Parser.TOKEN.NONE;
					}
					return JSON._JSON.Parser.TOKEN.NULL;
				}
			}

			// Token: 0x04000CFA RID: 3322
			private const string WHITE_SPACE = " \t\n\r";

			// Token: 0x04000CFB RID: 3323
			private const string WORD_BREAK = " \t\n\r{}[],:\"";

			// Token: 0x04000CFC RID: 3324
			private StringReader json;

			// Token: 0x020002CB RID: 715
			private enum TOKEN
			{
				// Token: 0x04000CFF RID: 3327
				NONE,
				// Token: 0x04000D00 RID: 3328
				CURLY_OPEN,
				// Token: 0x04000D01 RID: 3329
				CURLY_CLOSE,
				// Token: 0x04000D02 RID: 3330
				SQUARED_OPEN,
				// Token: 0x04000D03 RID: 3331
				SQUARED_CLOSE,
				// Token: 0x04000D04 RID: 3332
				COLON,
				// Token: 0x04000D05 RID: 3333
				COMMA,
				// Token: 0x04000D06 RID: 3334
				STRING,
				// Token: 0x04000D07 RID: 3335
				NUMBER,
				// Token: 0x04000D08 RID: 3336
				TRUE,
				// Token: 0x04000D09 RID: 3337
				FALSE,
				// Token: 0x04000D0A RID: 3338
				NULL
			}
		}

		// Token: 0x020002CA RID: 714
		private sealed class Serializer
		{
			// Token: 0x06000FCF RID: 4047 RVA: 0x0003A096 File Offset: 0x00038296
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06000FD0 RID: 4048 RVA: 0x0003A0A9 File Offset: 0x000382A9
			public static string Serialize(JSON obj)
			{
				JSON._JSON.Serializer serializer = new JSON._JSON.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06000FD1 RID: 4049 RVA: 0x0003A0C4 File Offset: 0x000382C4
			private void SerializeValue(object value)
			{
				if (value == null)
				{
					this.builder.Append("null");
					return;
				}
				if (value is string)
				{
					this.SerializeString(value as string);
					return;
				}
				if (value is bool)
				{
					this.builder.Append(value.ToString().ToLower());
					return;
				}
				if (value is JSON)
				{
					this.SerializeObject(value as JSON);
					return;
				}
				if (value is IDictionary)
				{
					this.SerializeDictionary(value as IDictionary);
					return;
				}
				if (value is IList)
				{
					this.SerializeArray(value as IList);
					return;
				}
				if (value is char)
				{
					this.SerializeString(value.ToString());
					return;
				}
				this.SerializeOther(value);
			}

			// Token: 0x06000FD2 RID: 4050 RVA: 0x0003A176 File Offset: 0x00038376
			private void SerializeObject(JSON obj)
			{
				this.SerializeDictionary(obj.fields);
			}

			// Token: 0x06000FD3 RID: 4051 RVA: 0x0003A184 File Offset: 0x00038384
			private void SerializeDictionary(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x06000FD4 RID: 4052 RVA: 0x0003A22C File Offset: 0x0003842C
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object value in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x06000FD5 RID: 4053 RVA: 0x0003A2AC File Offset: 0x000384AC
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				int i = 0;
				while (i < array.Length)
				{
					char c = array[i];
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					case '\v':
						goto IL_DD;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_DD;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_123:
					i++;
					continue;
					IL_DD:
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						this.builder.Append(c);
						goto IL_123;
					}
					this.builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
					goto IL_123;
				}
				this.builder.Append('"');
			}

			// Token: 0x06000FD6 RID: 4054 RVA: 0x0003A3F8 File Offset: 0x000385F8
			private void SerializeOther(object value)
			{
				if (value is float || value is int || value is uint || value is long || value is double || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
				{
					this.builder.Append(value.ToString());
					return;
				}
				this.SerializeString(value.ToString());
			}

			// Token: 0x04000CFD RID: 3325
			private StringBuilder builder;
		}
	}
}
