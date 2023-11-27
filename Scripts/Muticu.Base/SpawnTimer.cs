namespace Muticu.Base;

using System;
using System.Linq;
using System.Collections.Generic;

public class SpawnTimer
{
    /// <summary>
    /// Current time(in miliseconds)
    /// </summary>
    private ulong time;

    private List<Note> notes;

    public delegate void _SpawnNoteCallback(Note _note);
    private _SpawnNoteCallback SpawnNoteCallback;

    public SpawnTimer(_SpawnNoteCallback callback)
    {
        this.time = 0;
        SpawnNoteCallback = callback;
    }

    public void UpdateTime(ulong _time)
    {
        time = _time;
        List<Note> deleteList=new();
        if (notes != null)
        {
            for (int i = 0; i < notes.Count; i++){
                if(notes[i].time<=time){deleteList.Add(notes[i]);}
                else if(notes[i].time<=time+1000){
                    SpawnNoteCallback(notes[i]);
                    deleteList.Add(notes[i]);
                }
                else break;
            }
        }
        notes.RemoveAll((node)=>deleteList.Contains(node));
    }

    public void initNotes(Note[] _notes)
    {
        Array.Sort(_notes);
        notes = _notes.ToList();
    }
}