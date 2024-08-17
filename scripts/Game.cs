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

	public int TickNumber = 0;

	private double _timeSinceLastTick = 0.0d;

	public float IncomePerTick = 0.0f;
	public float ExpensesPerTick = 0.0f;

  	[Export]
	public float Account = 10_000.0f;

	public Rack Rack;

	public Contracts Contracts;

	public Firewall Firewall;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		// Get Contracts
    	Rack = GetNode<Rack>("/root/Root/Game/Rack");
		Contracts = GetNode<Contracts>("/root/Root/UI/Contracts");
		Firewall = GetNode<Firewall>("/root/Root/Game/Firewall");
    }

	public void AcceptContract(ContractCreationArgs cca) {
		Contracts.AddContract(cca);
		Firewall.AddConnection(new ConnectionCreationArgs{
			ContractId = cca.ContractId
		});
	}

	public void BuyServer(ServerCreationArgs sca) {
		if (Account >= sca.Cost) {
			Account -= sca.Cost;
			Rack.AddServer(sca);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		_timeSinceLastTick += delta;
		if (_timeSinceLastTick > TickRateSeconds) {
			// total up servers and contracts
			ExpensesPerTick = Rack.GetServers().Aggregate(0.0f, (acc, nxt) => {
				return acc + nxt.ExpensesPerTick;
			});
			IncomePerTick = Contracts.GetContracts().Aggregate(0.0f, (acc, nxt) => {
				return acc + nxt.IncomePerTick;
			});
			_timeSinceLastTick -= TickRateSeconds;
			Account += IncomePerTick;
			Account -= ExpensesPerTick;
			TickNumber += 1;
		}
	}
}
