using Godot;
using System;

public class Packet : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public double TimeoutTime = 10.0f;

    public float MovementSpeed = 50.0f;


    public bool Processed = false;

    public Node2D Destination;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Destination = GetParent().GetNode<Node2D>("../Destination");
    }


    public override void _Process(float delta)
    {

        TimeoutTime -= delta;

        if (TimeoutTime <= 0) {
            QueueFree();
        }

        base._Process(delta);
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {

        if (Position.DistanceTo(Destination.Position) > 1.0f)
        {        
            LinearVelocity = Position.DirectionTo(Destination.Position) * MovementSpeed;
        } else {
            LinearVelocity = new Vector2();
        }
        base._IntegrateForces(state);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
