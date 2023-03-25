using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Utilities.Tasks {

	[Category("GameObject")]
	public class GetComponentInChildren<T> : ActionTask<Transform> where T: Component {

		[BlackboardOnly]
		public BBParameter<T> saveAs;

		protected override string info => $"Get {typeof(T).Name} as {saveAs}";

		protected override void OnExecute() {
			var o = agent.GetComponentInChildren<T>();
			saveAs.value = o;
			EndAction(o != null);
		}
	}
}
