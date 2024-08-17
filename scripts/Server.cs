using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Server : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

  	[Export]
	public float ExpensesPerTick = 110.0f;

    [Export]
    public float WorkPerTick = 0.4f;

    [Export]
    public int NumberOfCores = 2;

    private Label _coresLabel;
    private Label _infoLabel;

    private Game _game;

    private double[] coreStates;

    private Stack<Packet> pending;

    private Area2D[] _ports;

    public override void _Ready()
    {
        _coresLabel = GetNode<Label>("Cores/Label");
        _infoLabel = GetNode<Label>("Info/Label");

        _game = GetNode<Game>("/root/Root");

        _ports = GetNode<Node2D>("Ports").GetChildren().Cast<Area2D>().ToArray();

        var initialStates = new List<double>();
        var numcores = NumberOfCores;
        while (numcores > 0) {
            numcores -= 1;
            initialStates.Add(0.0f);
        }
        coreStates = initialStates.ToArray();
        pending = new Stack<Packet>();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
            // work off "debt" on core
            if (state >= 0.0f) {
                coreStates[i] = state - (WorkPerTick * _game.TickRateSeconds * delta);
            }
        }
        foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
            if (state <= 0.0f) {
                // core is available to work
                if (pending.Count > 0) {
                    var pkt = pending.Pop();
                    coreStates[i] = pkt.Work;
                    pkt.WorkRate = WorkPerTick;
                }
            }
        }

        foreach(var port in _ports) {
            var overlapping = port.GetOverlappingBodies();
            // lolfuk
            foreach(var overlap in overlapping) {
                if (overlap.GetType()  == typeof(Packet)) {
                    if ((overlap as Packet).Processable && (overlap as Packet).WorkRate <= 0.0f){
                        pending.Push(overlap as Packet);
                    }
                }
            }
        }

    }
}
