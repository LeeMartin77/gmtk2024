using Godot;
using System;

public class UI : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	private Game _game;

	private Label _account;
	private Label _income;
	private Label _expenses;
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");

		_account = GetNode<Label>("Funds");
		_income = GetNode<Label>("Income");
		_expenses = GetNode<Label>("Expenses");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		_account.Text = $"${_game.Account}";
		_income.Text = $"${_game.IncomePerTick}";
		_expenses.Text = $"${_game.ExpensesPerTick}";
	}
}
