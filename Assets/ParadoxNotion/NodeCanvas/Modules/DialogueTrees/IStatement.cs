using DualityGame.Dialog;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{

    ///<summary>An interface to use for whats being said by a dialogue actor</summary>
    public interface IStatement
    {
        string Text { get; }
        AudioClip Audio { get; }
        Mood Mood { get; }
        string Meta { get; }
    }
}
