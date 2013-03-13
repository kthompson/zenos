﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Mono.Cecil;

using Cecil.Decompiler;
using Cecil.Decompiler.Languages;

using NUnit.Framework;

namespace Cecil.Decompiler.Tests {

	public abstract class DecompilationTestFixture : DecompilerTestFixture {

		static AssemblyDefinition current = AssemblyDefinition.ReadAssembly(typeof (DecompilationTestFixture).Module.Assembly.Location);

		public string DecompileMethod (string method_name, ILanguage language)
		{
			var str = new StringWriter ();
			var method = GetMethod (method_name);
			var writer = language.GetWriter (new PlainTextFormatter (str));

			var block = method.Body.Decompile (language);

			writer.Write (block);

			return str.ToString ();
		}

		public ILanguage CSharpV0 {
            get { return Languages.CSharp.GetLanguage(CSharpVersion.None); }
		}

		public ILanguage CSharpV1 {
            get { return Languages.CSharp.GetLanguage(CSharpVersion.V1); }
		}

		public ILanguage CSharpV2 {
            get { return Languages.CSharp.GetLanguage(CSharpVersion.V2); }
		}

		public ILanguage CSharpV3 {
            get { return Languages.CSharp.GetLanguage(CSharpVersion.V3); }
		}

		public MethodDefinition GetMethod (string method)
		{
		    return GetTypeDefinition().Methods.First(m => m.Name == method);			
		}

		public TypeDefinition GetTypeDefinition ()
		{
			return current.MainModule.GetType(NormalizeTypeName (GetType ().FullName));
		}

		static string NormalizeTypeName (string name)
		{
			return name.Replace ('+', '/');
		}

		public void AssertMethod (string method_name, ILanguage language, string expected)
		{
			Assert.AreEqual (NormalizeResult (expected),
				NormalizeResult (DecompileMethod (method_name, language)));
		}

		static string NormalizeResult (string str)
		{
			var start = str.IndexOf ('{');
			var end = str.LastIndexOf ('}');
			return Normalize (str.Substring (
				start,
				end - start));
		}
	}
}
