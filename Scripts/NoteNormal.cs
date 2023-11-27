using Godot;
using Muticu.Base;
using System;
using System.Collections;

public partial class NoteNormal : NoteCommon
{
	/// <summary>
	/// the center of the screen
	/// </summary>
	public Vector2 origin;

	protected Vector2 direction;

	protected void setPosition()
	{

		switch (note.track)
		{
			case Note.Track.Normal1:
				this.direction = Vector2.Down;
				break;
			case Note.Track.Normal2:
				this.direction = -Vector2.FromAngle(Mathf.Pi / 6);
				this.Rotation = Mathf.Pi * 2 / 3;
				break;
			case Note.Track.Normal3:
				this.direction = -Vector2.FromAngle(Mathf.Pi * 5 / 6);
				this.Rotation = Mathf.Pi * 4 / 3; // 240deg
				break;
			default:
				this.direction = Vector2.Zero;
				break;
		}
		this.Position = origin - (direction * 400);
	}
	public override void _Ready()
	{
		setPosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		// speed: 379.7px/s
		this.Position += direction * 385f * (float)delta;

		if (Time.GetTicksMsec() >= time + 100)
		{
			this.Hide();
			deleteNote(this);
		}
	}
}
