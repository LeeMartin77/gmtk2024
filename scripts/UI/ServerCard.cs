using Godot;
using System;
using System.Collections.Generic;

public class ServerCard : MarginContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public ServerCreationArgs CreationArgs;

    public Game _game;


	private Dictionary<string, Label> _labels;

    private Button _button;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
		_game = GetNode<Game>("/root/Root");


		_labels = new Dictionary<string, Label>{
			{"DailyCost", GetNode<Label>("MarginContainer/VBoxContainer/Cost/DailyCostValue")},
			{"PortValue", GetNode<Label>("MarginContainer/VBoxContainer/Daily/Ports/PortValue")},
			{"CoreValue", GetNode<Label>("MarginContainer/VBoxContainer/Daily/Cores/CoreValue")},
		};

        _button = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/BuyButton");
        _button.Text = $"BUY £{CreationArgs.Cost}";

        _labels["DailyCost"].Text = $"{CreationArgs.ExpensesPerTick}";
        _labels["PortValue"].Text = $"x{CreationArgs.NumberOfPorts}";
        _labels["CoreValue"].Text = $"x{CreationArgs.NumberOfCores}";
		
    }

    public void _on_Button_Release() {
        _game.BuyServer(CreationArgs);
    }
}
