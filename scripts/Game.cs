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

	private Camera2D _camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get Contracts
		_camera = GetNode<Camera2D>("/root/Root/Game/Camera2D");
		Rack = GetNode<Rack>("/root/Root/Game/Rack");
		// for note of observers - this is horrific, don't do this ever
		Contracts = GetNode<Contracts>("/root/Root/Game/Camera2D/GUI/MarginContainer/Menu/TabContainer/CONTRACTS/contents/ActiveContracts");
		Firewall = GetNode<Firewall>("/root/Root/Game/Firewall");
	}

	public void AcceptContract(ContractCreationArgs cca) {
		Contracts.AddContract(cca);
		Firewall.AddConnection(new ConnectionCreationArgs{
			ContractId = cca.ContractId
		});
	}

	public void BuyServer(ServerCreationArgs sca) {
		if (Account >= sca.Cost && Rack.HasSpace()) {
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

	public void AdjustMoney(float amount) {
		Account += amount;
		// TODO: Logic for good/bad
	}

	private float _scrollSpeed = 10f;
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.GetType() == typeof(InputEventPanGesture)) {
			var dlta = (inputEvent as InputEventPanGesture).Delta;
			_scrollBy((dlta.y * -1) * _scrollSpeed);
		}
		if (inputEvent.IsActionPressed("scroll_up"))
		{
			_scrollBy(-1 * _scrollSpeed);
		} else if (inputEvent.IsActionPressed("scroll_down")) {

			_scrollBy(+1 * _scrollSpeed);
		}
	}

	private float _cameraLimit = 1200f;
	private void _scrollBy(float amount) {
		if (_camera.Position.y > 0 && amount < 0) {
			var newpos = _camera.Position;
			newpos.y = Math.Max(0, newpos.y + amount);
			_camera.Position = newpos;
		} else if (_camera.Position.y < _cameraLimit && amount > 0) {
			var newpos = _camera.Position;
			newpos.y = Math.Min(_cameraLimit, newpos.y + amount);
			_camera.Position = newpos;
		}
	}
}
