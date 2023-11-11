using Godot;
using Mutruc.Base;
using System;
using System.Collections;

public partial class NoteCommon : Node2D
{

	/// <summary>
	/// The `note' object of the sprite
	/// </summary>
	public Note note;

	/// <summary>
	/// The time where the note should hit the triangle since Godot started
	/// (in milliseconds)
	/// </summary>
	public ulong time;

	public delegate void _deleteNote(NoteCommon note);
	public _deleteNote deleteNote;
}
