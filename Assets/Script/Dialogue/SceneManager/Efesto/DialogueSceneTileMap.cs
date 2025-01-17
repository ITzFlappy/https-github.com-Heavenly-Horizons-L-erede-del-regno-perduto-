using UnityEngine;

namespace Script.Dialogue.SceneManager.Efesto {
    public class DialogueSceneTileMap : DialogueSceneTileMapAbstract {
        public static byte K = 0;
        [SerializeField] private DialogueSystem dialogueSystem;

        protected override void Update() {
            switch (K) {
                case 0:
                    barFalse();
                    dialogueSystem.FirstDialogue();
                    break;
                default:
                    barTrue();
                    break;
            }

            if (dialogueSystem.isEnded)
                //per resettare
                dialogueSystem.ResetDialogueTrigger();
        }
    }
}