using Godot;
using System;

public class Money : Control
{
	private Game _game;
	private Label _balance;
	private Label _delta;
	
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_balance = GetNode<Label>("balance");
		_delta = GetNode<Label>("delta");
	}

	public override void _Process(float delta)
	{
		_balance.Text = $"BLN: £{_game.Account:#,##0}";
		_delta.Text = $"{(_game.LastDelta > 0 ? "+" : "-")}£{_game.LastDelta:#,##0}";
	}
}
