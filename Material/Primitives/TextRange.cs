﻿namespace Material.Primitives;

public struct TextRange(int start, int end)
{
    public int Start { get; set; } = start;
    public int End { get; set; } = end;

    public readonly int Length => this.End - this.Start;

    public readonly bool IsRange => this.Start != this.End;

    public TextRange() : this(0, 0) { }

    public TextRange(int positon) : this(positon, positon) { }

    public readonly TextRange Normalized()
    {
        return this.Start > this.End
            ? new TextRange(this.End, this.Start)
            : new TextRange(this.Start, this.End);
    }

    public static TextRange CopyOf(TextRange range)
    {
        return new TextRange(range.Start, range.End);
    }

    public readonly bool Equals(TextRange value)
    {
        return this.Start == value.Start && this.End == value.End;
    }

    public readonly override string ToString()
    {
        return $"TextRange: Start = {{{this.Start}, End = {this.End}, Length = {this.Length}}}";
    }
}
