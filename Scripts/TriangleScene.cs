using Godot;
using Mutruc.Base;
using System;
using System.Collections.Generic;

public partial class TriangleScene : Node2D
{
	[Export]
	public PackedScene NoteScene;
	public Vector2 ScreenSize; // Size of the game window.
	private ulong initTime;
	private Polygon2D MainTriangle;
	private Mutruc.Base.SpawnTimer timer;
	private List<NoteCommon> noteCommons;
	private int judge = 0;
	private Label judglabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.noteCommons = new();
		this.timer = new Mutruc.Base.SpawnTimer(TimerCallback);
		this.MainTriangle = GetNode<Polygon2D>("MainTriangle");
		this.initTime = Time.GetTicksMsec();
		this.judglabel = GetNode<Label>("JudgementLabel");
		ScreenSize = GetViewportRect().Size;
		Note[] A ={
			new Note(Note.Track.Normal2, 100, false, 0),
			new Note(Note.Track.Normal2, 1000, false, 0),
			new Note(Note.Track.Normal2, 1500, false, 0),
			new Note(Note.Track.Normal2, 2000, false, 0),
			new Note(Note.Track.Normal2, 3000, false, 0),
			new Note(Note.Track.Normal2, 4000, false, 0),
			new Note(Note.Track.Normal2, 500, false, 0),
			new Note(Note.Track.Normal2, 6000, false, 0),
			new Note(Note.Track.Normal2, 7000, false, 0),
			new Note(Note.Track.Normal2, 8000, false, 0),
			new Note(Note.Track.Normal2, 9000, false, 0),
			new Note(Note.Track.Normal2, 10000, false, 0),
			new Note(Note.Track.Normal2, 11000, false, 0),
			new Note(Note.Track.Normal2, 12000, false, 0),
			new Note(Note.Track.Normal2, 13000, false, 0),
			new Note(Note.Track.Normal2, 14000, false, 0),
			new Note(Note.Track.Normal2, 15000, false, 0),
			new Note(Note.Track.Normal2, 16000, false, 0),
			new Note(Note.Track.Normal2, 17000, false, 0),
			new Note(Note.Track.Normal2, 18000, false, 0),
			new Note(Note.Track.Normal2, 19000, false, 0),
			new Note(Note.Track.Normal2, 20000, false, 0),
			new Note(Note.Track.Normal2, 21000, false, 0),
			new Note(Note.Track.Normal2, 22000, false, 0),
			new Note(Note.Track.Normal2, 23000, false, 0),
			new Note(Note.Track.Normal2, 24000, false, 0),
			new Note(Note.Track.Normal2, 25000, false, 0),
			new Note(Note.Track.Normal2, 26000, false, 0),
			new Note(Note.Track.Normal2, 27000, false, 0),
			new Note(Note.Track.Normal2, 28000, false, 0),
			new Note(Note.Track.Normal2, 29000, false, 0),
			new Note(Note.Track.Normal2, 30000, false, 0),
		};
		timer.initNotes(A);
		MainTriangle.Position = ScreenSize / 2 - new Vector2(35, 30.5f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var curtime = Time.GetTicksMsec() - initTime;
		if (curtime >= 5000)
		{
			timer.UpdateTime(curtime - 5000);
		}

	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("track_normal1"))
		{
			Judgement(Note.Track.Normal1);
		}
		if (@event.IsActionPressed("track_normal2"))
		{
			Judgement(Note.Track.Normal2);
		}
		if (@event.IsActionPressed("track_normal3"))
		{
			Judgement(Note.Track.Normal3);

		}
	}

	public void TimerCallback(Mutruc.Base.Note note)
	{
		NoteCommon _note;
		switch (note.track)
		{
			case Note.Track.Normal1:
			case Note.Track.Normal2:
			case Note.Track.Normal3:
				NoteNormal noteNormal = NoteScene.Instantiate<NoteNormal>();
				noteNormal.note = note;
				noteNormal.origin = ScreenSize / 2;
				noteNormal.time = note.time + initTime + 5000;
				noteNormal.deleteNote = this.DeleteNoteCallback;
				_note = noteNormal;
				break;
			default:
				_note = new NoteCommon();
				break;
		}
		this.AddChild(_note);
		noteCommons.Add(_note);
	}

	private void MakeJudgement(Mutruc.Base.Judge.Levels level)
	{
		judge++;
		judglabel.Text = $"{judge}: {level}";
	}

	private void Judgement(Note.Track track)
	{
		var delete = true;
		NoteCommon currentNote = null;
		foreach (var i in noteCommons)
		{
			if (i.note.track == track)
			{
				currentNote = i;
				break;
			}
		}
		if (currentNote == null) return;
		switch (Math.Abs((decimal)Time.GetTicksMsec() - (decimal)currentNote.time))
		{
			case decimal i when i > 250:
				// Too early, do not do anything
				delete = false;
				break;
			case decimal i when 100 < i && i <= 250:
				MakeJudgement(Judge.Levels.Miss);
				break;
			case decimal i when 65 < i && i <= 100:
				MakeJudgement(Judge.Levels.Good);
				break;
			case decimal i when 30 < i && i <= 65:
				MakeJudgement(Judge.Levels.Great);
				break;
			case decimal i when i <= 30:
				MakeJudgement(Judge.Levels.Perfect);
				break;
			default:
				break;
		}
		if (delete && currentNote != null)
		{
			noteCommons.Remove(currentNote);
			this.RemoveChild(currentNote);
			currentNote.Free();
		}
	}
	public void DeleteNoteCallback(NoteCommon note)
	{
		MakeJudgement(Judge.Levels.Miss);
		noteCommons.Remove(note);
		this.RemoveChild(note);
		note.Free();
	}
}
