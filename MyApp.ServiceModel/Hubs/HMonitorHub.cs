using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenHardwareMonitor.Hardware;

namespace MyApp.ServiceModel.Hubs
{
	public class HMonitorHub : Hub
	{
		private static Computer _computer;
		private static bool isRun = false;
		public HMonitorHub()
		{
			_computer = new Computer()
			{
				GPUEnabled = true,
				CPUEnabled = true,
				RAMEnabled = true,
				MainboardEnabled = true,
				FanControllerEnabled = true,
				HDDEnabled = true,
			};
			_computer.Open();
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
				await Task.Delay(100);
			}
		}

		private MyComputer UpdateSensors()
		{
			var result = new MyComputer();
			foreach (var hardware in _computer.Hardware)
			{
				hardware.Update();
				switch (hardware.HardwareType)
				{
					case HardwareType.CPU:
						result.CpuLoad = hardware.Sensors.FirstOrDefault(i =>i.SensorType == SensorType.Load &&  i.Name == "CPU Total" )?.Value ?? 0;
						result.CpuTemp = hardware.Sensors.FirstOrDefault(i =>i.SensorType == SensorType.Temperature && i.Name.Contains("CPU Package"))?.Value ?? 0;
						break;
					case HardwareType.RAM:
						result.RamLoad = hardware.Sensors.FirstOrDefault(i => i.SensorType == SensorType.Load)?.Value ?? 0;
						break;
					case HardwareType.GpuAti:
					case HardwareType.GpuNvidia:
						result.GpuLoad = hardware.Sensors.FirstOrDefault(i => i.SensorType == SensorType.Load)?.Value ?? 0;
						result.GpuTemp = hardware.Sensors.FirstOrDefault(i => i.SensorType == SensorType.Temperature)?.Value ?? 0;
						break;
				}
				
			}

			return result;
		}
	}
}