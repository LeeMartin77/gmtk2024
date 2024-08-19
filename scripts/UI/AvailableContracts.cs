using Godot;
using System;
using System.Collections.Generic;

public class AvailableContracts : VBoxContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private Game _game;


	private float _contractScale = 1.0f;
	private int _contractTier = 1;

	private Dictionary<int, string[]> _scaleCompanyNames = new Dictionary<int, string[]>{
{1, new string[]{"Sunrise",
	"GreenLeaf",
	"TechHaven",
	"Bloom Florists",
	"Crafty",
	"Urban Café",
	"Pixel Design",
	"Healthy Market",
	"Book Nook",
	"Spoon Bakery",}},
		{2, new string[]{"NexGen",
	"BrightPath",
	"Visionary",
	"Pinnacle",
	"Quantum",
	"Innovative",
	"Summit",
	"Strategic",
	"Vertex",
	"Blue Horizon"}},
		{3, new string[]{ "Synergy",
	"Evergreen.",
	"TriStar",
	"Aspire",
	"Horizon",
	"Stellar",
	"PrimeWave",
	"MetroTech",
	"Continental",
	"Northern"}},
		{4, new string[]{  "OmniCorp",
	"GlobalTech",
	"NovaSynthesis",
	"Titanium",
	"QuantumSphere",
	"Eclipse",
	"Imperial",
	"Hyperion",
	"AstraNova",
	"Zenith"}},
	};

	private string[] _things = new string[]{ "Website",
	"Database",
	"API",
	"Microservice",
	"Cache",
	"Application",
	"Backup",
	"Repository",
	"Queue",
	"Proxy"};

	private List<ContractCreationArgs> _baseContracts = new List<ContractCreationArgs>(){
		new ContractCreationArgs(){
			ContractName = "Low",
			ContractId = Guid.NewGuid().ToString(),
			IncomePerTick = 1,
			PacketsPerTick =  0.4f,
			ContractSigningPay = 1000,
			ContractLeavingFee = 1200,
			ContractFlatFailureFee = 200,
			MaxLostPackets = 30,
		},
		new ContractCreationArgs(){
			ContractName = "Mid",
			ContractId = Guid.NewGuid().ToString(),
			IncomePerTick = 4,
			PacketsPerTick =  0.8f,
			ContractSigningPay = 1800,
			ContractLeavingFee = 2400,
			ContractFlatFailureFee = 400,
			MaxLostPackets = 20,
		},
		new ContractCreationArgs(){
			ContractName = "High",
			ContractId = Guid.NewGuid().ToString(),
			IncomePerTick = 12,
			PacketsPerTick =  1.2f,
			ContractSigningPay = 3600,
			ContractLeavingFee = 4800,
			ContractFlatFailureFee = 800,
			MaxLostPackets = 10,
		}	
	};


	
	public override void _Ready()
	{
		
		_game = GetNode<Game>("/root/Root");
		_containerTemplate = GD.Load<PackedScene>("res://AvailableContractCard.tscn");
		_createValidServerOptions();
	}


		PackedScene _containerTemplate;
	private void _createValidServerOptions() {

			foreach(Node chld in GetChildren()) {
				chld.QueueFree();
			}

			foreach(ContractCreationArgs cca in _baseContracts) {
				var instance = _containerTemplate.Instance<AvailableContract>();
				ContractCreationArgs cpy = cca;
				cpy.ContractId = Guid.NewGuid().ToString();
				var company = _scaleCompanyNames[_contractTier][new Random().Next(_scaleCompanyNames[_contractTier].Length)];
				var thing = _things[new Random().Next(_things.Length)];
				cpy.ContractName = $"{company} {thing}";
				cpy.IncomePerTick *= _contractScale * Math.Min(_contractTier, 1);
				cpy.PacketsPerTick *= _contractScale * Math.Min(_contractTier, 1);
				cpy.ContractSigningPay *= _contractScale * Math.Min(_contractTier, 1);
				cpy.ContractLeavingFee *= _contractScale * Math.Min(_contractTier, 1);
				cpy.ContractFlatFailureFee *= _contractScale * Math.Min(_contractTier, 1);
				cpy.MaxLostPackets *= (int)_contractScale * Math.Min(_contractTier, 1);
				instance.Args = cpy;
				AddChild(instance);
			}
	}

	private int _lastTick = 0;
	public override void _Process(float delta)
	{
		if (_game.TickNumber != _lastTick && _game.TickNumber % 10 == 0) {
			_contractScale *= 1.05f;
			_contractTier = Math.Max(Math.Min((int)_game.RequestsHandled / 100, 4), 1);
			_createValidServerOptions();
			_lastTick = _game.TickNumber;
		}
	}
}
