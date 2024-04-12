namespace Muticu;

using Godot;
using Muticu.Base;
using System;
using System.Collections.Generic;
using System.Security.Principal;

public partial class TriangleScene : Node2D
{
	[Export]
	public PackedScene NormalNoteScene;

	[Export]
	public PackedScene TriangleNoteScene;

	[Export]
	public PackedScene HoldNoteScene;

	[Export]
	public PackedScene CatchNoteScene;

	public Vector2 ScreenCenter;
	private ulong initTime;
	private Node2D MainTriangle;
	private Node2D Catcher;
	private SpawnTimer timer;
	private List<NoteCommon> noteCommons;
	private int judge = 0;
	private Label judglabel;
	private Label timelbl;
	private List<NoteCommon> currentHolds;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.noteCommons = new();
		this.currentHolds = new();
		this.timer = new SpawnTimer(TimerCallback);
		this.MainTriangle = GetNode<Node2D>("MainTriangle");
		this.Catcher=GetNode<Node2D>("Catcher");
		this.initTime = Time.GetTicksMsec();
		this.judglabel = GetNode<Label>("JudgementLabel");
		this.timelbl=GetNode<Label>("timelbl");
		ScreenCenter=GetViewportRect().Size/2;
		Note[] A ={/*
			new Note(Note.Track.Special1, 1000, false, 0),
			new Note(Note.Track.Special1, 2000, false, 0),
			new Note(Note.Track.Special1, 3000, false, 0),
			new Note(Note.Track.Special1, 4000, false, 0),
			new Note(Note.Track.Special1, 5000, false, 0),
			new Note(Note.Track.Special1, 6000, false, 0),
			new Note(Note.Track.Special1, 7000, false, 0),
			new Note(Note.Track.Special1, 8000, false, 0),
			new Note(Note.Track.Special1, 9000, false, 0),
			new Note(Note.Track.Normal1, 10000, true, 1000),
			new Note(Note.Track.Normal2, 11000, true, 1000),
			new Note(Note.Track.Normal3, 12000, true, 1000),
			new Note(Note.Track.Normal1, 13000, true, 1500),
			new Note(Note.Track.Normal2, 14000, true, 1000),
			new Note(Note.Track.Normal3, 15000, true, 1000),
			new Note(Note.Track.Normal1, 16000, false, 0),
			new Note(Note.Track.Normal2, 17000, false, 0),
			new Note(Note.Track.Normal3, 18000, false, 0),
			new Note(Note.Track.Normal1, 19000, false, 0),
			new Note(Note.Track.Normal2, 20000, false, 0),
			new Note(Note.Track.Normal3, 21000, false, 0),
			new Note(Note.Track.Normal1, 22000, false, 0),
			new Note(Note.Track.Normal2, 23000, false, 0),
			new Note(Note.Track.Normal3, 24000, false, 0),
			new Note(Note.Track.Normal1, 25000, false, 0),
			new Note(Note.Track.Normal2, 26000, false, 0),
			new Note(Note.Track.Normal3, 27000, false, 0),
			new Note(Note.Track.Normal1, 28000, false, 0),
			new Note(Note.Track.Normal2, 29000, false, 0),
			new Note(Note.Track.Normal3, 30000, false, 0),*/
			/*
						new Note(Note.Track.Normal1, 1000, true, 1000),
						new Note(Note.Track.Normal2, 3000, true, 1000),
						new Note(Note.Track.Normal3, 5000, true, 1000),
						new Note(Note.Track.Normal1, 7000, true, 1000),
						new Note(Note.Track.Normal2, 9000, true, 1000),
						new Note(Note.Track.Normal3, 11000, true, 1000),*/
			new Note(Note.Track.Special2, 1000, false, 0),
            new Note(Note.Track.Special2, 2000, false, 10472),
            new Note(Note.Track.Special2, 3000, false, 20943),
            new Note(Note.Track.Special2, 4000, false, 10472),
            new Note(Note.Track.Special2, 5000, false, 5325),
            new Note(Note.Track.Normal2, 6000, false, 0),
        };
		timer.initNotes(A);
		MainTriangle.Position = ScreenCenter; /* - new Vector2(35, 20.3f);*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var curtime = Time.GetTicksMsec() - initTime;
		if (curtime >= 3000)
		{
			timer.UpdateTime(curtime - 3000);
		}
		Catcher.Rotation=(GetViewport().GetMousePosition()-ScreenCenter).Angle();
		
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("track_normal1"))
		{
			Judgement(Note.Track.Normal1, Time.GetTicksMsec());
		}
		if (@event.IsActionPressed("track_normal2"))
		{
			Judgement(Note.Track.Normal2, Time.GetTicksMsec());
		}
		if (@event.IsActionPressed("track_normal3"))
		{
			Judgement(Note.Track.Normal3, Time.GetTicksMsec());
		}
		if (@event.IsActionReleased("track_normal1"))
		{
			KeyReleaseJudgement(Note.Track.Normal1, Time.GetTicksMsec());
		}
		if (@event.IsActionReleased("track_normal2"))
		{
			KeyReleaseJudgement(Note.Track.Normal2, Time.GetTicksMsec());
		}
		if (@event.IsActionReleased("track_normal3"))
		{
			KeyReleaseJudgement(Note.Track.Normal3, Time.GetTicksMsec());
		}
		if (@event.IsActionPressed("track_special1"))
		{
			Judgement(Note.Track.Special1, Time.GetTicksMsec());
		}
	}

	public void TimerCallback(Note note)
	{
		NoteCommon _note;
		switch (note.track)
		{
			case Note.Track.Normal1:
			case Note.Track.Normal2:
			case Note.Track.Normal3:
				NoteNormal noteNormal;
				if (note.spec)
				{
					noteNormal = HoldNoteScene.Instantiate<NoteHold>();
					(noteNormal as NoteHold).UntilEndCallback = HoldUntilEndCallback;
				}
				else noteNormal = NormalNoteScene.Instantiate<NoteNormal>();
				noteNormal.deleteNote = this.DeleteNoteCallback;
				noteNormal.note = note;
				noteNormal.origin = ScreenCenter;
				noteNormal.time = note.time + initTime + 3000;
				_note = noteNormal;
				break;
			case Note.Track.Special1:
				NoteTriangle noteTriangle = TriangleNoteScene.Instantiate<NoteTriangle>();
				noteTriangle.note = note;
				noteTriangle.origin = ScreenCenter;
				noteTriangle.time = note.time + initTime + 5000;
				noteTriangle.deleteNote = this.DeleteNoteCallback;
				_note = noteTriangle;
				break;
			case Note.Track.Special2:
				NoteCatch noteCatch = CatchNoteScene.Instantiate<NoteCatch>();
				noteCatch.note = note;
				noteCatch.origin = ScreenCenter;
                noteCatch.time = note.time + initTime + 5000;
				noteCatch.deleteNote = this.DeleteNoteCallback;
				_note = noteCatch;
				break;
            default:
				_note = new NoteCommon();
				break;
		}
		this.AddChild(_note);
		noteCommons.Add(_note);
	}

	public void DeleteNoteCallback(NoteCommon note)
	{
		MakeJudgement(Judge.Levels.Miss);
		deleteNote(note);
	}

	public void HoldUntilEndCallback(NoteHold note)
	{
		MakeJudgement(note.level);
		currentHolds.Remove(note);
		deleteNote(note);
	}

	private void MakeJudgement(Muticu.Base.Judge.Levels level)
	{
		judge++;
		judglabel.Text = $"{judge}: {level}";
	}

	private void KeyReleaseJudgement(Note.Track track, decimal time)
	{
		NoteHold currentNote = null;
		foreach (var i in currentHolds)
		{
			if (i.note.track == track)
			{
				currentNote = i as NoteHold;
				break;
			}
		}
		if(currentNote==null) return;
		var delta = time - currentNote.endTime;
		if (delta < -100)
		{
			currentHolds.Remove(currentNote);
			MakeJudgement(currentNote.level == Judge.Levels.Good ? Judge.Levels.Good : currentNote.level + 1);
			deleteNote(currentNote);
		}
	}

	private void Judgement(Note.Track track, decimal time)
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
		var delta = Math.Abs(time - (decimal)currentNote.time);
		switch (delta)
		{
			case decimal i when i > 250:
				// Too early, do not do anything
				delete = false;
				break;
			case decimal i when 100 < i && i <= 250:
				MakeJudgement(Judge.Levels.Miss);
				break;
			case decimal i when 65 < i && i <= 100:
				if (currentNote.note.spec)
				{
					(currentNote as NoteHold).level = Judge.Levels.Good;
					(currentNote as NoteHold).held = true;
					currentHolds.Add(currentNote);
					delete = false;
				}
				else MakeJudgement(Judge.Levels.Good);
				break;
			case decimal i when 30 < i && i <= 65:
				if (currentNote.note.spec)
				{
					(currentNote as NoteHold).level = Judge.Levels.Great;
					(currentNote as NoteHold).held = true;
					currentHolds.Add(currentNote);
					delete = false;
				}
				else MakeJudgement(Judge.Levels.Great);
				break;
			case decimal i when i <= 30:
				if (currentNote.note.spec)
				{
					(currentNote as NoteHold).level = Judge.Levels.Perfect;
					(currentNote as NoteHold).held = true;
					currentHolds.Add(currentNote);
					delete = false;
				}
				else MakeJudgement(Judge.Levels.Perfect);
				break;
			default:
				break;
		}
		if (delete && currentNote != null)
		{
			deleteNote(currentNote);
		}
	}

	void deleteNote(NoteCommon note)
	{
		this.RemoveChild(note);
		noteCommons.Remove(note);
		note.Free();
	}
}
