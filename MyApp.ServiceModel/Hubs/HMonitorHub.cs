using System.Linq;
using System.Threading.Tasks;
using HardwareProviders.CPU;
using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;

namespace MyApp.ServiceModel.Hubs
{
	public class HMonitorHub : Hub
	{
		private static Computer _computer;

		private static bool isRun = false;
		public HMonitorHub()
		{
			_computer = new Computer
			{
				IsGpuEnabled = true,
				IsCpuEnabled = true,
				IsMemoryEnabled = true,
				IsNetworkEnabled = true
				// MainboardEnabled = true,
				// FanControllerEnabled = true,
				// HDDEnabled = true,
			};
			
			// _computer.Open();
		}

		public async Task RunHardwareInfoSender()
		{
			if(!isRun)
				await HardwareInfoSender();
		}
		public async Task HardwareInfoSender()
		{
			isRun = true;
			while(true)
			{
				await Clients.All.SendAsync(nameof(HardwareInfoSender), UpdateSensors());
				await Task.Delay(500);
			}
		}

		private MyComputer UpdateSensors()
		{
			var result = new MyComputer();
			if(!_computer.Hardware.Any())
				_computer.Open();
			foreach (var hardware in _computer.Hardware)
			{
				hardware.Update();
				switch (hardware.HardwareType)
				{
					case HardwareType.Cpu:
						result.CpuLoad = hardware.Sensors.FirstOrDefault(i => i.Name == "CPU Total")?.Value ?? 0;
						result.CpuTemp = hardware.Sensors.FirstOrDefault(i => i.Name == "Core Max")?.Value ?? 0;
						break;
					case HardwareType.Memory:
						result.RamLoad = hardware.Sensors.FirstOrDefault(i => i.SensorType == SensorType.Load)?.Value ?? 0;
						result.RamUsed = hardware.Sensors.FirstOrDefault(i => i.Name == "Memory Used")?.Value ?? 0;
						result.RamAvailable = hardware.Sensors.FirstOrDefault(i => i.Name == "Memory Available")?.Value ?? 0;
						break;
					case HardwareType.GpuAmd:
					case HardwareType.GpuNvidia:
						result.GpuLoad = hardware.Sensors.FirstOrDefault(i => i.SensorType == SensorType.Load)?.Value ?? 0;
						result.GpuTemp = hardware.Sensors.FirstOrDefault(i => i.SensorType == SensorType.Temperature)?.Value ?? 0;
						result.Fps = hardware.Sensors.FirstOrDefault(i => i.Name == "Fullscreen FPS")?.Value ?? 0;
						break;
					case HardwareType.Network:
						if(hardware.Name == "Ethernet")
						{
							result.DownSpeed = hardware.Sensors.FirstOrDefault(i => i.Name == "Download Speed")?.Value ?? 0;
							result.UpSpeed = hardware.Sensors.FirstOrDefault(i => i.Name == "Upload Speed")?.Value ?? 0;
							result.DownBytes = hardware.Sensors.FirstOrDefault(i => i.Name == "Data Downloaded")?.Value ?? 0;
							result.UpBytes = hardware.Sensors.FirstOrDefault(i => i.Name == "Data Uploaded")?.Value ?? 0;
						}
						break;
				}
				
			}

			return result;
		}
	}
}