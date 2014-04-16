namespace YoutrackBoard
{
    using System;

    internal interface IRepository
    {
        event EventHandler Refresh;
    }
}