using System;

namespace Mutruc.Base;

public class Note : IComparable
{
    /// <summary>
    /// The enum represents the available tracks
    /// </summary>
    public enum Track {
        // UP
        Normal1,
        // RIGHT
        Normal2,
        //LEFT
        Normal3,
        Special1,
        Special2
    }
    public int CompareTo(object obj) {
        if (obj == null) return 1;

        Note otherNote = obj as Note;
        if (otherNote != null)
            return this.time.CompareTo(otherNote.time);
        else
           throw new ArgumentException("Object is not a Note");
    }

    /// <summary>
    /// this represents which track the note is on
    /// </summary>
    public Track track;
    /// <summary>
    /// This represents when the note will be hit
    /// with the unit of miliseconds
    /// </summary>
    public ulong time;
    /// <summary>
    /// if true, the note will be a slider
    /// only effective when this is on Normal tracks
    /// </summary>
    public bool spec;
    /// <summary>
    /// for sliders, this represents the length (in miliseconds)
    /// for Special2 notes, this represents the bias angle
    /// </summary>
    public int param;

    public Note(Track _track, ulong _time, bool _spec, int _param)
    {
        this.track=_track;
        this.time=_time;
        this.spec=_spec;
        this.param=_param;

    }
}

