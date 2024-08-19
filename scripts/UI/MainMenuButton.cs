using Godot;
using System;

public class MainMenuButton : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_HomePage_button_up() {
			GetTree().ChangeScene("res://Start.tscn");
    }
    
}
