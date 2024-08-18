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

	public override void _Process(float delta)
	{
		long tickAsMinutes = _game.TickNumber * 15;
		
		long minutes = tickAsMinutes % 60;
		long hours = (tickAsMinutes / 60) % 24;
		long day = (tickAsMinutes / 60 / 24);
		
		
		_day.Text = $"Day: {day}";
		_time.Text = $"Time: {hours.ToString("00")}:{minutes.ToString("00")}";
	}
}

