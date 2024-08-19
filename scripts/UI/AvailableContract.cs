using Godot;
using System;
using System.Collections.Generic;


public class AvailableContract : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	public ContractCreationArgs Args;
	private Game _game;
	public Dictionary<string, Label> _labels;
	public Button _contractButton;

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		
		_labels = new Dictionary<string, Label>{
			{"ContractName", GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/NameOfContract")},
			{"AllowableLost", GetNode<Label>("MarginContainer/VBoxContainer/AvailableContractMetrics/AllowableLoss/AllowableLossValue")},
			{"DailyPay", GetNode<Label>("MarginContainer/VBoxContainer/Daily/DailyPay/DailyPayValue")},
			{"DailyRequests", GetNode<Label>("MarginContainer/VBoxContainer/Daily/DailyRequests/DailyRequestsValue")},
		};
		_contractButton = GetNode<Button>("MarginContainer/VBoxContainer/ContractButton");
		
		_contractButton.Text = $"TAKE CONTRACT (£{+Args.ContractSigningPay:#,##0})";
		_labels["ContractName"].Text = Args.ContractName;
		_labels["AllowableLost"].Text = $"{Args.MaxLostPackets}";
		_labels["DailyPay"].Text = $"{DayTime.ToDailyMoney(Args.IncomePerTick):#,##0}";
		_labels["DailyRequests"].Text = $"{DayTime.ToDailyRequests(Args.PacketsPerTick):#,##0}";

	}

	public void _on_Button_Release() {
		_game.AcceptContract(Args);
		QueueFree();
	}

}
