using System;
namespace Ensemble
{
	public class Profile
	{
		public Profile()
		{
		}
		public string Name { get; set; }

		public string Gender { get; set; }
		public string Age { get; set; }
		public string Instr { get; set; }
		public void printto()
		{
			Console.Write("{0}{1}{2}", Name, Gender, Instr);
		}
	}
}
