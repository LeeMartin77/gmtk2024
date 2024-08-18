using Godot;
using System;

public class RackRowBackdrop : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private Sprite _initialRackRow;

    [Export]
    public int NumberOfRows = 72;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _initialRackRow = GetNode<Sprite>("Rackrow");

        var numrows = NumberOfRows;

        var spriteHeight = _initialRackRow.Texture.GetHeight();

        for(var i = 1; i < NumberOfRows; i++) {
            var copy = _initialRackRow.Duplicate() as Sprite;
            var newpos = _initialRackRow.Position;
            AddChild(copy);
            newpos.y += i * spriteHeight;
            copy.Position = newpos;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
