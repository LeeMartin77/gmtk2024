using Godot;
using System;
using System.Collections.Generic;

public struct ContractCreationArgs {
	public float IncomePerTick;
	public float PacketsPerTick;
	public float PacketTimeoutTime;
	public string ContractName;
	public float ContractSigningPay;
	public float ContractLeavingFee;
	public float ContractFlatFailureFee;
	public int MaxLostPackets;
	public string ContractId;
}

public class Contract : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.


  	[Export]
	public float IncomePerTick = 100.0f;

	[Export]
	// TODO: We need this to be non-flat rates in the future
	public float PacketsPerTick = 0.2f;

	[Export]
	public float PacketTimeoutTime = 20f;

	[Export]
	public string ContractId = "a832c168-a98f-475d-825b-9f5803dfa3de";


	[Export]
	public string ContractName = "Your Mum's Website";
	[Export]
	public float ContractSigningPay = 100.0f;
	[Export]
	public float ContractLeavingFee = 120.0f;
	[Export]
	public float ContractFlatFailureFee = 10.0f;
	[Export]
	public int MaxLostPackets = 50;

	[Export]
	public bool Active = false;

	private Connection _connection;
	private Game _game;
	private int _lastTick;

	private float _pendingPackets = 1.0f;

	public int SentPackets = 0;
	public int ReceivedPackets = 0;
	public int FailedPackets = 0;

	public Dictionary<string, Label> _labels;

	public Button _contractButton;

	public async override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		
		_labels = new Dictionary<string, Label>{
			{"ContractName", GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/NameOfContract")},
			{"RequestsLost", GetNode<Label>("MarginContainer/VBoxContainer/ActiveContractMetrics/RequestsLost/RequestsLostValue")},
			{"AllowableLost", GetNode<Label>("MarginContainer/VBoxContainer/AvailableContractMetrics/AllowableLoss/AllowableLossValue")},
			{"DailyPay", GetNode<Label>("MarginContainer/VBoxContainer/Daily/DailyPay/DailyPayValue")},
			{"DailyRequests", GetNode<Label>("MarginContainer/VBoxContainer/Daily/DailyRequests/DailyRequestsValue")},
		};
		
		_labels["ContractName"].Text = ContractName;
		_labels["AllowableLost"].Text = $"{MaxLostPackets}";
		_labels["DailyPay"].Text = $"{IncomePerTick}";
		_labels["DailyRequests"].Text = $"{PacketsPerTick}";

		_contractButton = GetNode<Button>("MarginContainer/VBoxContainer/ContractButton");



		if (Active) {
			_contractButton.Text = $"END CONTRACT (£{-ContractLeavingFee})";
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
			try {
				_connection = GetNode<Firewall>("/root/Root/Game/Firewall").GetConnectionByContractId(ContractId);
			} catch {
				GD.Print("Connection doesn't exist!");
				QueueFree();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!Active) {
			return;
		}
		if (_lastTick != _game.TickNumber) {
			_lastTick = _game.TickNumber;
			_pendingPackets += PacketsPerTick;
			while (_pendingPackets >= 1.0f && _connection != null) {
				_pendingPackets -= 1.0f;
				SentPackets += 1;
				_connection.CreatePacket(PacketTimeoutTime);
			}

			if (Active && _game.TickNumber > 0 && _game.TickNumber % 10 == 0) {
				ContractLeavingFee *= 0.95f;
				PacketsPerTick *= 1.1f;
				_contractButton.Text = $"END CONTRACT (£{-ContractLeavingFee})";
			}
		}

		if (FailedPackets > MaxLostPackets) {
			_game.AdjustMoney(-ContractFlatFailureFee);
			EndContract();
		}

		// requests lost
		_labels["RequestsLost"].Text = $"{FailedPackets}";
		_labels["DailyRequests"].Text = $"{PacketsPerTick}";


	}

	public void ReceivePacket() {
		ReceivedPackets += 1;
	}

	public void FailPacket() {
		FailedPackets += 1;
	}

	public void _on_Button_Release() {
		if (Active) {
			EndContract();
		} else {
			// TODO: Trigger add contract
		}
	}

	public void EndContract() {
		_game.AdjustMoney(-ContractLeavingFee);
		_connection.QueueFree();
		QueueFree();
	}
}
