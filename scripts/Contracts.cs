using Godot;
using System;
using System.Linq;

public class Contracts : VBoxContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private PackedScene _contractScene;
	private Game _game;

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_contractScene = GD.Load<PackedScene>("res://ContractCard.tscn");
	}

	public Contract[] GetContracts() {
		var children = GetChildren();
		return children.Cast<Contract>().ToArray();
	}

	// will throw if contract does not exist
	public Contract GetContractById(string id) {
		return GetContracts().First((x) => x.ContractId == id);
	}

	public void AddContract(ContractCreationArgs args) {
		var nc = _contractScene.Instance() as Contract;
		_game.AdjustMoney(args.ContractSigningPay);
		nc.ContractId = args.ContractId;
		nc.ContractName = args.ContractName;

		nc.IncomePerTick = args.IncomePerTick;
		nc.PacketsPerTick = args.PacketsPerTick;

		nc.ContractSigningPay = args.ContractSigningPay;
		nc.ContractLeavingFee = args.ContractLeavingFee;
		nc.ContractFlatFailureFee = args.ContractFlatFailureFee;
		nc.MaxLostPackets = args.MaxLostPackets;
		AddChild(nc);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
