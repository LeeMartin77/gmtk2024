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
	public float Account = 100.0f;

	public float LastDelta = 0.0f;

	[Export]
	public float GameOverAmount = -1_000f;

	public bool GameOver = false;

	public float MoneyMade = 0.0f;
	public int RequestsHandled = 0;

	public Rack Rack;

	public Contracts Contracts;

	public Firewall Firewall;

	private Camera2D _camera;

	private Panel _gameOverPanel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get Contracts
		_camera = GetNode<Camera2D>("/root/Root/Game/Camera2D");
		_gameOverPanel = GetNode<Panel>("/root/Root/Game/Camera2D/GameOverPanel");
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
		if (GameOver) {
			return;
		}
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
			AdjustMoney(IncomePerTick);
			AdjustMoney(-ExpensesPerTick);
			LastDelta = IncomePerTick - ExpensesPerTick;
			TickNumber += 1;
		}
	}

	public void AdjustMoney(float amount) {
		if (GameOver) {
			return;
		}
		Account += amount;
		if (amount > 0){
			MoneyMade += amount;
		}
		if (Account <= GameOverAmount) {
			// Game is over!
			GameOver = true;
			_gameOverPanel.Visible = true;
		}
	}

	public void AddPackets(int packetcount) {
		if (GameOver) {
			return;
		}
		RequestsHandled += packetcount;
	}

	private float _scrollSpeed = 10f;
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.GetType() == typeof(InputEventPanGesture) && (inputEvent as InputEventPanGesture).Position.x > 400) {
			var dlta = (inputEvent as InputEventPanGesture).Delta;
			_scrollBy((dlta.y * -1) * _scrollSpeed);
		}
		if (inputEvent.IsActionPressed("scroll_up") && GetViewport().GetMousePosition().x > 400)
		{
			_scrollBy(-1 * _scrollSpeed);
		} else if (inputEvent.IsActionPressed("scroll_down") && GetViewport().GetMousePosition().x > 400) {

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
