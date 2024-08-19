using Godot;
using System;

public class MoneyMade : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

	private Game _game;
	
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Text = $"£{_game.MoneyMade:#,###,##0} MADE\n";
    }
}
