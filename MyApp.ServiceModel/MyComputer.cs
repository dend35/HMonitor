using System;

namespace MyApp.ServiceModel
{
	public class MyComputer
	{
		public float CpuLoad { get; set; }
		public float CpuTemp { get; set; }
		public float RamLoad { get; set; }
		public float RamAvailable { get; set; }
		public float RamUsed { get; set; }
		public float GpuLoad { get; set; }
		public float GpuTemp { get; set; }
		public float Fps { get; set; }
		
		public float UpSpeed { get; set; }
		public float DownSpeed { get; set; }
		public float UpBytes { get; set; }
		public float DownBytes { get; set; }

	}
}