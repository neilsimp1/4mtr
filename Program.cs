﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;

namespace _4mtr {
    public class Program {

		private static List<string> ignores = new List<string>{ "node_modules", "bower_components" };
		
		public static int Main(string[] args) {
			var result = CommandLine.Parser.Default.ParseArguments<Options>(args).MapResult((Options options) => Execute(options), errs => 1);

			return result;
		}

		private static int Execute(Options options) {
			var files = GetTextFileList(options);

			foreach (var file in files) Console.WriteLine(file);
			Console.WriteLine("\nRun 4mtr on these files? y/n");

			var choice = Console.ReadKey();
			if(choice.Key.ToString() == "Y"){
				foreach(var file in files) Format(file, options);
				Console.WriteLine("\n\nDone");
			}
			else Console.WriteLine("\n\nExiting");

			return 0;
		}

		private static void Format(string file, Options options) {
			StringBuilder sb = new StringBuilder();
			string line;
			
			using(StreamReader stream = File.OpenText(file)){
				while((line = stream.ReadLine()) != null){
					sb.Append(line.TrimEnd() + options.LineEnding);
				}
			}

			var length = options.LineEnding.Length;
			while(sb.ToString().EndsWith(options.LineEnding)){
				var asd = sb.ToString();
				sb.Remove(sb.Length - length, length);
			}
			sb.Append(options.LineEnding);

			try{
				File.WriteAllText(file, sb.ToString());
			}
			catch(Exception ex){
				Console.WriteLine(ex.Message);
			}
		}

		private static List<string> GetTextFileList(Options options) {
			var inputs = options.Inputs.ToList<string>();
			if(inputs.Count == 0) inputs.Add(Directory.GetCurrentDirectory());

			var Scan = new Func<List<string>>(() => {
				var files = new List<string>();
				foreach(var input in inputs){
					if(Directory.Exists(input)) files.AddRange(LoopDir(input, ignores));
					else if(File.Exists(input)){
						if(IsTextFile(input)) files.Add(Path.GetFullPath(input));
					}
				}

				return files;
			});


			return Scan();
		}

		private static List<string> LoopDir(string dir, List<string> ignores) {
			var files = new List<string>();

			foreach(var subdir in Directory.GetDirectories(dir)){
				var subdirName = new DirectoryInfo(subdir).Name;
				if(!ignores.Contains(subdirName) & subdirName[0] != '.') files.AddRange(LoopDir(subdir, ignores));
			}
			
			foreach(var file in Directory.GetFiles(dir)){
				if(IsTextFile(file) & new DirectoryInfo(file).Name[0] != '.') files.Add(file);
			}

			return files;
		}

		private static bool IsTextFile(string file) {
			try{
				using(StreamReader stream = File.OpenText(file)){
					int ch;
					var counter = 0;
					while((ch = stream.Read()) != -1 && counter < 100){
						counter++;
						if(IsControlChar(ch)) return false;
					}
				}
				return true;
			}
			catch(Exception){
				return false;
			}
		}

		private static bool IsControlChar(int ch) {
			return (ch > ControlChars.NUL && ch < ControlChars.BS) || (ch > ControlChars.CR && ch < ControlChars.SUB);
		}

		private static class ControlChars {
			public static char NUL = (char)0; // Null char
			public static char BS = (char)8; // Back Space
			public static char CR = (char)13; // Carriage Return
			public static char SUB = (char)26; // Substitute
		}

    }
}
