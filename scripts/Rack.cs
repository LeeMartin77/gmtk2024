using Godot;
using System;
using System.Linq;

public class Rack : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private PackedScene _serverScene;

    private Server[] _serverRack;

    public override void _Ready()
    {
        _serverScene = GD.Load<PackedScene>("res://Server.tscn");
        var initialServer = GetNode<Server>("Server");
        _serverRack = new Server[8]{
            initialServer, 
            null,
            null,
            null,
            null,
            null,
            null,
            null,
        };
    }

    public Server[] GetServers() {
        var children = GetChildren();
        return children.Cast<Server>().ToArray();
    }

    public bool HasSpace() {
        return _serverRack.Any(x => x == null);
    }

    public void RemoveServer(Server server) {
        var removing = 0;
        foreach(var srv in _serverRack) {
            if (srv == server) {
                break;
            }
            removing +=1;
        }
        if (removing+1 > _serverRack.Length) {
            // no-op, can't find server
            GD.Print("Couldn't find server to remove");
            return;
        }
        _serverRack[removing] = null;
        server.QueueFree();
    }


    public void AddServer(ServerCreationArgs args) {
        var empty = 0;
        foreach(var srv in _serverRack) {
            if (srv == null) {
                break;
            }
            empty +=1;
        }
        if (empty+1 > _serverRack.Length) {
            // no-op, can't fit a server
            GD.Print("Tried to add a server with not space");
            return;
        }
        var nc = _serverScene.Instance() as Server;
        _serverRack[empty] = nc;
        nc.ExpensesPerTick = args.ExpensesPerTick;
        nc.WorkPerTick = args.WorkPerTick;
        nc.NumberOfCores = args.NumberOfCores;
        var serverHeight = 200f;
        var yOffset = serverHeight * empty;
        nc.Position = new Vector2(0, yOffset);
        AddChild(nc);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
