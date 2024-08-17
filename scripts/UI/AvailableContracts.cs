using Godot;
using System;

public class AvailableContracts : ItemList
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Game _game;
    public override void _Ready()
    {
        
        _game = GetNode<Game>("/root/Root");
    }

    public void _on_AvailableContracts_item_selected(int index) {
        //var item = GetItemText(index);
        var cca = new ContractCreationArgs{
			IncomePerTick = 10.0f,
			PacketsPerTick = 0.4f,
			PacketTimeoutTime = 20f,
			ContractId = Guid.NewGuid().ToString(),
		};

        _game.AcceptContract(cca);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
