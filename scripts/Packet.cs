using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Packet : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public double TimeoutTicks = 20.0f;

    [Export]
    public double Work = 3.0f;

    public float MovementSpeed = 650.0f;

    public bool Processable = false;
    public double WorkRate = 0.0f;

    public Node2D Destination;
    public Node2D Source;

    public Contract Contract;

    private List<Node2D> _joints;
    private int _jointIndex = 0;
    private int _jointLength;
    private int _direction = 1;

    private Node2D _target;

    private Game _game;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Destination = GetParent().GetNode<Node2D>("../Destination");
        Source = GetParent().GetNode<Node2D>("../Source");

        _joints = new List<Node2D>
        {
            Source
        };
        _joints.AddRange(GetParent().GetNode("../Cable").GetChildren().OfType<RigidBody2D>().ToArray());
            
        _jointLength = _joints.Count();
        _target = _joints[_jointIndex];

        _game = GetNode<Game>("/root/Root");
    }


    public override void _Process(float delta)
    {
        TimeoutTicks -= _game.TickRateSeconds * delta;

        if (_jointIndex + 1 == _jointLength && !Processable) {
            Processable = true;
        }

        if (Contract == null) {
            QueueFree();
        }

        if (TimeoutTicks <= 0) {
            Contract.FailPacket();
            QueueFree();
        }

        if (Processable && Work > 0.0f) {
            Work -= WorkRate * (_game.TickRateSeconds * delta);
        }

        if (Work <= 0.0f) {
            // done work, return up the cable
            _direction = -1;
        }

        base._Process(delta);
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        var additionalVelocity = new Vector2();

        if (_target.GetType() == typeof(RigidBody2D)) {
            additionalVelocity = (_target as RigidBody2D).LinearVelocity;
        }
        if (Position.DistanceTo(_target.Position) > 9.0f)
        {
            LinearVelocity = Position.DirectionTo(_target.Position) * MovementSpeed * ((float)_game.TickRateSeconds) + additionalVelocity;
        } else if (_jointIndex + _direction > 0 && _jointIndex + _direction < _jointLength) {
            _jointIndex += _direction;
            _target = _joints[_jointIndex];
            LinearVelocity = Position.DirectionTo(_target.Position) * MovementSpeed * ((float)_game.TickRateSeconds) + additionalVelocity;
        } else {

            LinearVelocity = new Vector2();
        }
        base._IntegrateForces(state);
    }
}
