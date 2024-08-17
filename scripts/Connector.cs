using Godot;
using System;
using System.Data;

public class Connector : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    private bool _attachedToMouse = false;

    public override void _Ready()
    {
        
    }

    public void _on_Connector_input_event(Node _, InputEvent evnt, int shapeidx) {
        if  (evnt is InputEventMouseButton && evnt.IsPressed() && !_attachedToMouse) {
            _attachedToMouse = true;
        }
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

        if (_attachedToMouse) {
            if (Input.IsActionJustReleased("click"))
            {
                _attachedToMouse = false;
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state) {

        if (_attachedToMouse) {
            Position = (GetParent() as Node2D).ToLocal(GetGlobalMousePosition());
            var t = state.Transform;

            t.origin.x = Position.x;

            t.origin.y = Position.y;

            state.Transform = t;
        }
    }
    // if reset_state:
    //     state.transform = Transform2D(0.0, Vector2(100, 100))
    //     reset_state = false
}
