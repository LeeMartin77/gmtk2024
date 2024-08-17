using Godot;
using System;
using System.Linq;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

  	[Export]
	public double TickRateSeconds = 1.0d;

	private double _timeSinceLastTick = 0.0d;

	public float IncomePerTick = 0.0f;
	public float ExpensesPerTick = 0.0f;

  	[Export]
	public float Account = 10_000.0f;

	private Rack _rack;

	private Contracts _contracts;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		// Get Contracts
    	_rack = GetNode<Rack>("/root/Root/Game/Rack");
		_contracts = GetNode<Contracts>("/root/Root/UI/Contracts");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		_timeSinceLastTick += delta;
		if (_timeSinceLastTick > TickRateSeconds) {
			// total up servers and contracts
			ExpensesPerTick = _rack.GetServers().Aggregate(0.0f, (acc, nxt) => {
				return acc + nxt.ExpensesPerTick;
			});
			IncomePerTick = _contracts.GetContracts().Aggregate(0.0f, (acc, nxt) => {
				return acc + nxt.IncomePerTick;
			});
			_timeSinceLastTick -= TickRateSeconds;
			Account += IncomePerTick;
			Account -= ExpensesPerTick;
		}
	}
}
