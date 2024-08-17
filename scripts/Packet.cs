using Godot;
using System;

public class Packet : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public double TimeoutTicks = 20.0f;

    [Export]
    public double Work = 3.0f;

    public float MovementSpeed = 75.0f;

    public bool Processable = false;
    public double WorkRate = 0.0f;

    public Node2D Destination;
    public Node2D Source;

    private Node2D _target;

    private Game _game;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Destination = GetParent().GetNode<Node2D>("../Destination");
        Source = GetParent().GetNode<Node2D>("../Source");
        _target = Destination;
        _game = GetNode<Game>("/root/Root");
    }


    public override void _Process(float delta)
    {
        TimeoutTicks -= _game.TickRateSeconds * delta;

        if (TimeoutTicks <= 0) {
            QueueFree();
        }

        if (Processable && Work > 0.0f) {
            Work -= WorkRate * (_game.TickRateSeconds * delta);
        }

        if (Work <= 0.0f) {
            // done work, return to source
            _target = Source;
        }

        base._Process(delta);
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {

        if (Position.DistanceTo(_target.Position) > 0.0f)
        {        
            LinearVelocity = Position.DirectionTo(_target.Position) * MovementSpeed;
        } else {
            LinearVelocity = new Vector2();
        }
        base._IntegrateForces(state);
    }
}
