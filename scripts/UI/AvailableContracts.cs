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


	private List<ContractCreationArgs> _allContracts = new List<ContractCreationArgs>(){
		new ContractCreationArgs(){
			ContractName = "Test",
			ContractId = Guid.NewGuid().ToString(),
			IncomePerTick = 1,
			PacketsPerTick =  0.4f,
			ContractSigningPay = 1000,
			ContractLeavingFee = 1200,
			ContractFlatFailureFee = 200,
			MaxLostPackets = 10,
		}	
	};
	
	public override void _Ready()
	{
		
		_game = GetNode<Game>("/root/Root");
		PackedScene containerTemplate = GD.Load<PackedScene>("res://AvailableContractCard.tscn");
		
		foreach(ContractCreationArgs cca in _allContracts) {
			var instance = containerTemplate.Instance<AvailableContract>();
			instance.Args = cca;
			AddChild(instance);
		}
	}
}
