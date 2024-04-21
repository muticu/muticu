namespace Muticu;

using Godot;
using System;

public partial class NoteCatch : NoteCommon
{
    /// <summary>
    /// the center of the screen
    /// </summary>
    public Vector2 origin;

    protected Vector2 direction;

    protected void setPosition()
    {
        this.direction = Vector2.FromAngle(this.note.param/10000f);
        this.Rotation = (this.note.param / 10000f) - Mathf.Pi/2;
        this.Position = origin - (direction * 400);
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        setPosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        this.Position += direction * 300f * (float)delta;

        if (Time.GetTicksMsec() >= time + 100)
        {
            this.Hide();
            deleteNote(this);
        }
    }
}
