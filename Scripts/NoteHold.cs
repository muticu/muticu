using Godot;
using System;

public partial class NoteHold : NoteNormal
{
	public ulong endTime;
	private Line2D tailLine;
	public bool held;
	public Mutruc.Base.Judge.Levels level;
	public delegate void _untilEndCallback(NoteHold @this);
	public _untilEndCallback UntilEndCallback;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.endTime=this.time + (ulong)this.note.param;
		this.tailLine=GetNode<Line2D>("TailLine");
		this.tailLine.AddPoint(new Vector2(0,-(this.note.param/1000*400)));
		setPosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// speed: 400px/s
		this.Position += direction * 400 * (float)delta;

		if (!held && Time.GetTicksMsec() >= time + 100)
		{
			this.Hide();
			deleteNote(this);
		}
		else if(held && Time.GetTicksMsec() >= endTime){
			UntilEndCallback(this);
		}
	}
}
