using DualityGame.Inventory;
using DualityGame.ServiceLocator;
using NodeCanvas.DialogueTrees;

namespace DualityGame.Player
{
    public class Player : ServiceBase<Player>
    {
        public DialogueActor Actor { get; private set; }
        public InventoryController InventoryController { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Actor = GetComponent<DialogueActor>();
            InventoryController = GetComponent<InventoryController>();
        }
    }
}
