using UnityEngine;

namespace Script.Dialogue.SceneManager.AchilleBF___ToEnemy {
    public class DialogueSceneTileMap : MonoBehaviour {
        public static byte k = 0;
        private static readonly int DialogueEnded = Animator.StringToHash("DialogueEnded");
        [SerializeReference] public DialogueSystem dialogueSystem;
        private BossHealth _bossHealth;
        private bool isDefeat = false;

        private void Reset() {
            dialogueSystem.ResetDialogueTrigger();
            dialogueSystem.i = 0;
            dialogueSystem.j = 0;
        }

        void Start()
        {
            k = 0;
            Reset();
            _bossHealth = GameObject.FindGameObjectWithTag("Nemico").GetComponent<BossHealth>();
        }

        private void Update() {
            //se la vita è maggiore di 10 e i dialoghi non sono finiti
            if (_bossHealth.bossHealth > 10 && k == 0)
            {
                //dialoghi
                dialogueSystem.FirstDialogue();
            }
            //se la vita è inferiore o uguale a 10
            else if (_bossHealth.bossHealth <= 10 && k == 1) {
                _bossHealth.GetComponent<BossWeapon>().attackDamage = 0;
                _bossHealth.GetComponent<Animator>().GetBehaviour<BossWalk>().attackRange = -1;
                dialogueSystem.SecondDialogue();
                //dialoghi
                if (!isDefeat){
                    GameObject.FindGameObjectWithTag("Nemico").GetComponent<Animator>().SetTrigger("Defeat");
                    isDefeat = true;
                }
            }

            if (dialogueSystem.isEnded)
                //per resettare
                Reset();
            //bossFight
        }
    }
}
