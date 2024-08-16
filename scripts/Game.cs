using Godot;
using System;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

  	[Export]
	public double TickRateSeconds = 1.0d;

	private double _timeSinceLastTick = 0.0d;

  	[Export]
	public float IncomePerTick = 100.0f;
  	[Export]
	public float ExpensesPerTick = 110.0f;
  	[Export]
	public float Account = 10_000.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		_timeSinceLastTick += delta;
		if (_timeSinceLastTick > TickRateSeconds) {
			_timeSinceLastTick -= TickRateSeconds;
			Account += IncomePerTick;
			Account -= ExpensesPerTick;
		}
	}
}
