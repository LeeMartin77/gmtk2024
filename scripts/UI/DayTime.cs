using Godot;
using System;

public class DayTime : Control
{
	private Game _game;
	
	private Label _day;
	private Label _time;
	
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_day = GetNode<Label>("day-label");
		_time = GetNode<Label>("time-label");
	}

	public static long TicksToMinutes(int tickNumber) {
		return tickNumber * 15;
	}

	public static float ToDailyRequests(float requestsPerTick) {
		float requestsAMinute = requestsPerTick / 15;
		return requestsAMinute * 60 * 24;
	}

	public static float ToDailyMoney(float moneyPerTick) {
		float moneyAMinute = moneyPerTick / 15;
		return moneyAMinute * 60 * 24;
	}

	public override void _Process(float delta)
	{
		long tickAsMinutes = TicksToMinutes(_game.TickNumber);
		
		long minutes = tickAsMinutes % 60;
		long hours = (tickAsMinutes / 60) % 24;
		long day = (tickAsMinutes / 60 / 24);
		
		_day.Text = $"Day: {day}";
		_time.Text = $"Time: {hours.ToString("00")}:{minutes.ToString("00")}";
	}
}

