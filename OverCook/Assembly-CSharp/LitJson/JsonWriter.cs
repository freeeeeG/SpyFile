using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x0200024D RID: 589
	public class JsonWriter
	{
		// Token: 0x06000AB1 RID: 2737 RVA: 0x00039678 File Offset: 0x00037A78
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000396A2 File Offset: 0x00037AA2
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000396B0 File Offset: 0x00037AB0
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x000396D6 File Offset: 0x00037AD6
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x000396DE File Offset: 0x00037ADE
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x000396FC File Offset: 0x00037AFC
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00039704 File Offset: 0x00037B04
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0003970D File Offset: 0x00037B0D
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00039715 File Offset: 0x00037B15
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0003971D File Offset: 0x00037B1D
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00039728 File Offset: 0x00037B28
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0003988C File Offset: 0x00037C8C
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000398F0 File Offset: 0x00037CF0
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0003993D File Offset: 0x00037D3D
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00039960 File Offset: 0x00037D60
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000399B8 File Offset: 0x00037DB8
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000399C4 File Offset: 0x00037DC4
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00039A30 File Offset: 0x00037E30
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						if (str[i] >= ' ' && str[i] <= '~')
						{
							this.writer.Write(str[i]);
						}
						else
						{
							JsonWriter.IntToHex((int)str[i], this.hex_seq);
							this.writer.Write("\\u");
							this.writer.Write(this.hex_seq);
						}
					}
					else
					{
						this.writer.Write('\\');
						this.writer.Write(str[i]);
					}
					break;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				}
			}
			this.writer.Write('"');
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00039BAB File Offset: 0x00037FAB
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00039BCB File Offset: 0x00037FCB
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00039BEC File Offset: 0x00037FEC
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00039C4A File Offset: 0x0003804A
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put((!boolean) ? "false" : "true");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00039C80 File Offset: 0x00038080
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00039CAC File Offset: 0x000380AC
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00039D11 File Offset: 0x00038111
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00039D3D File Offset: 0x0003813D
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00039D69 File Offset: 0x00038169
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00039DA1 File Offset: 0x000381A1
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00039DD0 File Offset: 0x000381D0
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00039E44 File Offset: 0x00038244
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00039E98 File Offset: 0x00038298
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00039F0C File Offset: 0x0003830C
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00039F60 File Offset: 0x00038360
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x0400085A RID: 2138
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x0400085B RID: 2139
		private WriterContext context;

		// Token: 0x0400085C RID: 2140
		private Stack<WriterContext> ctx_stack;

		// Token: 0x0400085D RID: 2141
		private bool has_reached_end;

		// Token: 0x0400085E RID: 2142
		private char[] hex_seq;

		// Token: 0x0400085F RID: 2143
		private int indentation;

		// Token: 0x04000860 RID: 2144
		private int indent_value;

		// Token: 0x04000861 RID: 2145
		private StringBuilder inst_string_builder;

		// Token: 0x04000862 RID: 2146
		private bool pretty_print;

		// Token: 0x04000863 RID: 2147
		private bool validate;

		// Token: 0x04000864 RID: 2148
		private TextWriter writer;
	}
}
