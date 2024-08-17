using Godot;
using System;

public class Destination : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private bool _attachedToMouse = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Destination_input_event(Node _, InputEvent evnt, int shapeidx) {
        if  (evnt is InputEventMouseButton && evnt.IsPressed() && !_attachedToMouse) {
            GD.Print(evnt);
            _attachedToMouse = true;
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
        if (_attachedToMouse) {
            GD.Print("shouldmove");
            Position = (GetParent() as Node2D).ToLocal(GetGlobalMousePosition());
            var t = Position;

            t.x = Position.x;

            t.y = Position.y;

            Position = t;
            if (Input.IsActionJustReleased("click"))
            {
                GD.Print("detatching");
                _attachedToMouse = false;
            }
        }
        if (_attachedToMouse) {
        }
    }
}
