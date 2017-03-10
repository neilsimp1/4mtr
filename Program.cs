using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _4mtr {
    public class Program {

        public static void Main(string[] args) {
			Console.WriteLine(DateTime.Now.ToString("0:MM/dd/yyy HH:mm:ss.fff") + "\n");////////////////////////
			var files = GetTextFileList(args);
			Console.WriteLine(DateTime.Now.ToString("0:MM/dd/yyy HH:mm:ss.fff") + "\n");////////////////////////

			foreach(var file in files) Console.WriteLine(file);
			Console.WriteLine("\nRun 4mtr on these files? y/n");

			var choice = Console.ReadKey();
			if(choice.Key.ToString() == "Y"){
				foreach(var file in files) Format(file);
				Console.WriteLine("\n\nDone");
			}
			else{
				Console.WriteLine("\n\nExiting");
			}
		}

		private static void Format(string file) {
			StringBuilder sb = new StringBuilder();
			string line;
			
			using(StreamReader stream = File.OpenText(file)){
				while((line = stream.ReadLine()) != null){
					sb.AppendLine(line.TrimEnd());
				}
			}
			
			if(sb[sb.Length - 1].ToString() != Environment.NewLine) sb.Append(Environment.NewLine);

			try{
				File.WriteAllText(file, sb.ToString());
			}
			catch(Exception ex){
				Console.WriteLine(ex.Message);
			}
		}

		private static List<string> GetTextFileList(string[] args) {
			var ignores = new List<string>();
			ignores.Add("node_modules");

			var files = new List<string>();

			var inputs = args.ToList();
			if(inputs.Count == 0) inputs.Add(Directory.GetCurrentDirectory());

			foreach(var input in inputs){
				var filename = new DirectoryInfo(input).Name;
				if(Directory.Exists(input)){
					files.AddRange(
						Directory.GetFiles(input, "*.*", SearchOption.AllDirectories)
							.Select(file => Path.GetFullPath(file))
							.Where(file => !ignores.Contains(filename))
							.Where(file => !file.Contains(@"\.") && !file.Contains("/."))
							.Where(file => IsTextFile(file))
							.ToList()
					);
				}
				else if(File.Exists(input)){
					if(IsTextFile(input)) files.Add(Path.GetFullPath(input));
				}
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
