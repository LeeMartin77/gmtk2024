using Godot;
using System;

public class StartScreen : Node2D
{
	public override void _Ready()
	{
		
	}
	public void _on_Button_button_up() {
		
	  GetTree().ChangeScene("res://Game.tscn");
	}

    public void _on_HowToPlay_button_up() {
        GetNode<Control>("HowToPlay").Visible = true;
    }


    public void _on_Dismiss_button_up() {
        GetNode<Control>("HowToPlay").Visible = false;
    }
}
