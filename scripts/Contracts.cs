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

	public override void _Ready()
	{
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
		nc.IncomePerTick = args.IncomePerTick;
		nc.PacketsPerTick = args.PacketsPerTick;
		nc.PacketTimeoutTime = args.PacketTimeoutTime;
		nc.ContractId = args.ContractId;
		AddChild(nc);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
