using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeReference] private Slider healthBar;
    [SerializeReference] private Slider staminaBar;
    private PlayerStats playerStats;


    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (SceneManager.GetActiveScene().buildIndex != 0 && !playerStats.isPlayerDead){
            playerStats.LoadPlayerData(); 
        }else{
            playerStats.AfterDeadPlayer();
        }
    }
}
