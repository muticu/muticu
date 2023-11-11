using Godot;
using Mutruc.Base;
using System;
using System.Collections;

public partial class NoteNormal : NoteCommon
{
	/// <summary>
	/// the center of the screen
	/// </summary>
	public Vector2 origin;

	private Vector2 direction;

	public override void _Ready()
	{
		switch (note.track)
		{
			case Note.Track.Normal1:
				this.origin -= new Vector2(35, 30.5f);
				this.direction = Vector2.Down;
				break;
			case Note.Track.Normal2:
				this.origin += new Vector2(0, 30.5f);
				this.direction = -Vector2.FromAngle(Mathf.Pi / 6);
				this.Rotation = -Mathf.Pi / 3;
				break;
			case Note.Track.Normal3:
				this.origin -= new Vector2(35, 30.5f);
				this.direction = -Vector2.FromAngle(Mathf.Pi * 5 / 6);
				this.Rotation = Mathf.Pi / 3;
				break;
			default:
				this.direction = Vector2.Zero;
				break;
		}
		this.Position = origin - (direction * 500);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// speed: 500px/s
		this.Position += direction * 500 * (float)delta;

		if(Time.GetTicksMsec()>=time+100){
			this.Hide();
			deleteNote(this);
		}		
	}
}
