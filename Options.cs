using System.Collections.Generic;
using CommandLine;

namespace _4mtr {
	public class Options {

		public enum LineEndingEnum { N, R, RN }
		private LineEndingEnum _lineEnding;

		[Value(0)]
		public IEnumerable<string> Inputs { get; set; }

		[Option('e', "lineending", HelpText = @"Specify a line ending <n|r|rn>. If not specified, will use system default.", Default = "")]
		public string LineEnding {
			get {
				if(_lineEnding == LineEndingEnum.R) return "\r";
				if(_lineEnding == LineEndingEnum.N) return "\n";
				return "\r\n";
			}
			set {
				if(value == "r" || value == "n" || value == "rn"){
					if(value == "r") _lineEnding = LineEndingEnum.R;
					else if(value == "n") _lineEnding = LineEndingEnum.N;
					else _lineEnding = LineEndingEnum.RN;
				}
				else{
					if(System.Environment.NewLine == "\r") _lineEnding = LineEndingEnum.R;
					else if(System.Environment.NewLine == "\n") _lineEnding = LineEndingEnum.N;
					else _lineEnding = LineEndingEnum.RN;
				}
			}
		}

	}
}
