using Godot;
using System;

public partial class NoteTriangle : NoteCommon
{
	/// <summary>
	/// center of the window
	/// </summary>
	public Vector2 origin;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Position=origin;
		this.Scale=new Vector2(6,6);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// speed: 4x scale /s
		this.Scale-=new Vector2(5,5)*(float)delta;

		if(Time.GetTicksMsec()>=time+100){
			this.Hide();
			deleteNote(this);
		}		
	}
}
