using UnityEngine;

namespace NodeCanvas.DialogueTrees
{

    ///<summary> An interface to use for DialogueActors within a DialogueTree.</summary>
	public interface IDialogueActor
    {
        string Name { get; }
        Sprite Portrait { get; }
        Transform Transform { get; }
    }
}
