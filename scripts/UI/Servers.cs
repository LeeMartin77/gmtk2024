using Godot;
using System;
using System.Collections.Generic;

public class Servers : Node
{
	private Game _game;
	
	private VBoxContainer _serversContainer;
	private List<ServerCreationArgs> _allServers = new List<ServerCreationArgs>(){
		new ServerCreationArgs(){
			Name = "HELL OPTIPLEX X1",
			Cost = 100,
			ExpensesPerTick = 10,
			WorkPerTick =  0.4f,
			NumberOfCores = 1,
			NumberOfPorts = 1,
			AvailableAtMoneyMade = 0,
			UnavailableAtMoneyMade = 999999999999,
		},

		// 100 - Dell
		new ServerCreationArgs(){
			Name = "HELL X100",
			Cost = 100,
			ExpensesPerTick = 10,
			WorkPerTick =  0.4f,
			NumberOfCores = 2,
			NumberOfPorts = 2,
			AvailableAtMoneyMade = 100,
			UnavailableAtMoneyMade = 1000,
		},
		new ServerCreationArgs(){
			Name = "HELL X200",
			Cost = 200,
			ExpensesPerTick = 15,
			WorkPerTick =  0.8f,
			NumberOfCores = 4,
			NumberOfPorts = 2,
			AvailableAtMoneyMade = 200,
			UnavailableAtMoneyMade = 2000,
		},
		new ServerCreationArgs(){
			Name = "HELL X300",
			Cost = 300,
			ExpensesPerTick = 20,
			WorkPerTick =  1.2f,
			NumberOfCores = 4,
			NumberOfPorts = 2,
			AvailableAtMoneyMade = 300,
			UnavailableAtMoneyMade = 3000,
		},

		// 1_000 - Tsujifu
		new ServerCreationArgs(){
			Name = "TSUJIFU Z1000",
			Cost = 1000,
			ExpensesPerTick = 100,
			WorkPerTick =  0.8f,
			NumberOfCores = 4,
			NumberOfPorts = 2,
			AvailableAtMoneyMade = 1000,
			UnavailableAtMoneyMade = 10000,
		},
		new ServerCreationArgs(){
			Name = "TSUJIFU Z2000",
			Cost = 2000,
			ExpensesPerTick = 150,
			WorkPerTick =  1.6f,
			NumberOfCores = 6,
			NumberOfPorts = 2,
			AvailableAtMoneyMade = 2000,
			UnavailableAtMoneyMade = 20000,
		},
		new ServerCreationArgs(){
			Name = "TSUJIFU Z3000",
			Cost = 3000,
			ExpensesPerTick = 200,
			WorkPerTick =  2f,
			NumberOfCores = 6,
			NumberOfPorts = 4,
			AvailableAtMoneyMade = 3000,
			UnavailableAtMoneyMade = 30000,
		},

		// 10_000
		new ServerCreationArgs(){
			Name = "COSCI L10000",
			Cost = 10000,
			ExpensesPerTick = 1000,
			WorkPerTick =  2f,
			NumberOfCores = 6,
			NumberOfPorts = 2,
			AvailableAtMoneyMade = 10000,
			UnavailableAtMoneyMade = 100000,
		},
		new ServerCreationArgs(){
			Name = "COSCI L20000",
			Cost = 20000,
			ExpensesPerTick = 1500,
			WorkPerTick =  3f,
			NumberOfCores = 8,
			NumberOfPorts = 4,
			AvailableAtMoneyMade = 20000,
			UnavailableAtMoneyMade = 200000,
		},
		new ServerCreationArgs(){
			Name = "COSCI L30000",
			Cost = 30000,
			ExpensesPerTick = 2000,
			WorkPerTick =  4f,
			NumberOfCores = 10,
			NumberOfPorts = 6,
			AvailableAtMoneyMade = 30000,
			UnavailableAtMoneyMade = 300000,
		},

		new ServerCreationArgs(){
			Name = "MBI FAR RED 9000",
			Cost = 1_000_000,
			ExpensesPerTick = 100_000,
			WorkPerTick =  20f,
			NumberOfCores = 12,
			NumberOfPorts = 6,
			AvailableAtMoneyMade = 200000,
			UnavailableAtMoneyMade = 999_999_999_999,
		}
	};
	
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_serversContainer = GetNode<VBoxContainer>("AllServers");
			
		_containerTemplate = GD.Load<PackedScene>("res://ServerCard.tscn");
		_createValidServerOptions();
		
	}

		PackedScene _containerTemplate;
	private void _createValidServerOptions() {

			foreach(Node chld in _serversContainer.GetChildren()) {
				chld.QueueFree();
			}
			foreach(ServerCreationArgs serverCreationArgs in _allServers) {
				if (serverCreationArgs.AvailableAtMoneyMade < _game.MoneyMade && serverCreationArgs.UnavailableAtMoneyMade > _game.MoneyMade) {
					var instance = _containerTemplate.Instance<ServerCard>();
					instance.CreationArgs = serverCreationArgs;
					_serversContainer.AddChild(instance);
				}
			}
	}

	public override void _Process(float delta)
	{
		if (_game.TickNumber % 10 == 0) {
			_createValidServerOptions();
		}
	}
}
