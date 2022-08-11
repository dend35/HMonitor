using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR;
using WindowsInput;
using WindowsInput.Native;

namespace MyApp.ServiceModel.Hubs;

public class ControlHub : Hub
{
	private readonly InputSimulator _ins;
	private readonly KeyboardSimulator _ks;
	public ControlHub()
	{
		_ins = new InputSimulator();
		_ks = new KeyboardSimulator(_ins);
	}
	public void Play()
	{
		Press(VirtualKeyCode.MEDIA_PLAY_PAUSE);
	}
	public void Next()
	{
		Press(VirtualKeyCode.MEDIA_NEXT_TRACK);
	}
	public void Prev()
	{
		Press(VirtualKeyCode.MEDIA_PREV_TRACK);
	}
	
	public void PlayDota()
	{
		RunSteamGame(570);
		Console.Beep(37, 300);
	}

	private void Press(VirtualKeyCode key)
	{
		var obj = new object();
		lock (obj)
		{
			_ks.KeyPress(key);
			_ks.Sleep(50);
		}
		
	}

	private void RunSteamGame(int gameId)
	{
		Cmd("start steam://rungameid/" + gameId);
	}
	private void Cmd(string command)
	{
		Process.Start("CMD.exe", "/C " + command);
	}
}
