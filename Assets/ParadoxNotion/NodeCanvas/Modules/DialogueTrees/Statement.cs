using System;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
    ///<summary>Holds data of what's being said usualy by an actor</summary>
    [Serializable]
    public class Statement : IStatement
    {

        [SerializeField]
        private string _text = string.Empty;
        [SerializeField]
        private AudioClip _audio;

        [SerializeField]
        private Mood _mood;
        
        [SerializeField]
        private string _meta = string.Empty;

        public string Text {
            get => _text;
            set => _text = value;
        }

        public AudioClip Audio {
            get => _audio;
            set => _audio = value;
        }

        public string Meta {
            get => _meta;
            set => _meta = value;
        }

        public Mood Mood
        {
            get => _mood;
            set => _mood = value;
        }

        //required
        public Statement() { }
        public Statement(string text) {
            Text = text;
        }

        public Statement(string text, AudioClip audio) {
            Text = text;
            Audio = audio;
        }

        public Statement(string text, AudioClip audio, string meta) {
            Text = text;
            Audio = audio;
            Meta = meta;
        }

        ///<summary>Replace the text of the statement found in brackets, with blackboard variables ToString and returns a Statement copy</summary>
        public IStatement BlackboardReplace(IBlackboard bb) {
            var copy = JSONSerializer.Clone(this);

            copy.Text = copy.Text.ReplaceWithin('[', ']', input =>
            {
                object o = null;
                if ( bb != null ) { //referenced blackboard replace
                    var v = bb.GetVariable(input, typeof(object));
                    if ( v != null ) { o = v.value; }
                }

                if ( input.Contains("/") ) { //global blackboard replace
                    var globalBB = GlobalBlackboard.Find(input.Split('/').First());
                    if ( globalBB != null ) {
                        var v = globalBB.GetVariable(input.Split('/').Last(), typeof(object));
                        if ( v != null ) { o = v.value; }
                    }
                }
                return o != null ? o.ToString() : input;
            });

            return copy;
        }

        public override string ToString() => Text;
    }
}
