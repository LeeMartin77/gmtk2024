using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public struct ServerCreationArgs {
    public float Cost;
    public float ExpensesPerTick;
    public float WorkPerTick;
    public int NumberOfCores;
}

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

    private (double, double)[] coreStates;

    private Stack<Packet> pending;

    private Area2D[] _ports;

    public override void _Ready()
    {
        _coresLabel = GetNode<Label>("Cores/Label");
        _infoLabel = GetNode<Label>("Info/Label");

        _infoLabel.Text = $"Cores: {NumberOfCores}\nWork Speed: {WorkPerTick}\nRunning Cost: {ExpensesPerTick}";

        _game = GetNode<Game>("/root/Root");

        _ports = GetNode<Node2D>("Ports").GetChildren().Cast<Area2D>().ToArray();

        var initialStates = new List<(double, double)>();
        var numcores = NumberOfCores;
        while (numcores > 0) {
            numcores -= 1;
            initialStates.Add((0.0d, 0.0d));
        }
        coreStates = initialStates.ToArray();
        pending = new Stack<Packet>();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
            // work off "debt" on core
            if (state.Item1 >= 0.0f) {
                coreStates[i].Item1 = state.Item1 - (WorkPerTick * _game.TickRateSeconds * delta);
            }
        }
        foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
            if (state.Item1 <= 0.0f) {
                // core is available to work
                if (pending.Count > 0) {

                    var pkt = pending.Pop();
                    coreStates[i] = (pkt.Work, pkt.Work);
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

        _coresLabel.Text = "";

        foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
            if (state.Item1 <= 0.0f) {
                _coresLabel.Text += $"Core {i}: idle\n";
            } else {
                _coresLabel.Text += $"Core {i}: {((state.Item1 / state.Item2) * 100).ToString("0")}%\n";
            }
        }
    }
}
