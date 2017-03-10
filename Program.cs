using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _4mtr {
    public class Program {

        public static void Main(string[] args) {
			var files = GetTextFileList(args);

			foreach(var file in files){
				Console.WriteLine(file);
			}
			Console.WriteLine("\nRun 4mtr on these files? y/n");
			var choice = Console.ReadKey();

			if(choice.Key.ToString() == "Y"){
				Console.WriteLine("\n\nDone");
			}
			else{
				Console.WriteLine("\n\nExiting");
			}
        }

		private static List<string> GetTextFileList(string[] args) {
			var files = new List<string>();

			var inputs = args.ToList();			
			if(inputs.Count == 0) inputs.Add(Directory.GetCurrentDirectory());

			foreach(var input in inputs){
				if(Directory.Exists(input)){
					files.AddRange(
						Directory.GetFiles(input, "*.*", SearchOption.AllDirectories)
							.Select(file => Path.GetFullPath(file))
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
					while((ch = stream.Read()) != -1){
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
