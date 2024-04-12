namespace Muticu;

using Godot;
using System;

public partial class NoteHold : NoteNormal
{
	public ulong endTime;
	private Line2D tailLine;
	private Polygon2D headPolygon;
	public bool held;
	public Muticu.Base.Judge.Levels level;
	public delegate void _untilEndCallback(NoteHold @this);
	public _untilEndCallback UntilEndCallback;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.endTime = this.time + (ulong)this.note.param;
		this.headPolygon = GetNode<Polygon2D>("HeadPolygon");
		this.tailLine = GetNode<Line2D>("TailLine");
		this.tailLine.AddPoint(new Vector2(0, -(this.note.param / 1000 * 400)));
		setPosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (!held && Time.GetTicksMsec() >= time + 100)
		{
			this.Hide();
			deleteNote(this);
		}
		else
		{
			this.Position += direction * 385f * (float)delta;
		}
		if (held && Time.GetTicksMsec() >= time)
		{
			this.Position=origin-direction*20.3f;
			this.headPolygon.Hide();
			var points = this.tailLine.Points;
			points[1] += new Vector2(0, (float)(379.7 * delta));
			this.tailLine.Points = points;
		}
		if (held && Time.GetTicksMsec() >= endTime)
		{
			UntilEndCallback(this);
		}
	}
}
