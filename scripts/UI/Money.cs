using Godot;
using System;

public class Money : Control
{
	private Game _game;
	private Label _balance;
	
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_balance = GetNode<Label>("balance-value");
	}

	public override void _Process(float delta)
	{
		_balance.Text = $"£{_game.Account.ToString("#,##0.00")}";
	}
}
