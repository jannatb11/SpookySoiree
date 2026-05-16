using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class RandomRoomCharacter : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float spawnChance = 0.3f;

    [Header("Optional ID (prevents repeat spawns)")]
    public string uniqueID;

    [Header("References")]
    public GameObject characterUI;
    public AudioSource audioSource;

    [Header("Optional Spawn Sounds")]
    public AudioClip[] spawnSounds;

    private bool spawned = false;

    public VideoClip cutsceneVideo;

    public string cutsceneID;
    void Start()
    {
        TrySpawn();
    }

    void TrySpawn()
    {
        if (!string.IsNullOrEmpty(uniqueID))
        {
            if (GameState.triggeredIDs.Contains(uniqueID))
            {
                characterUI.SetActive(false);
                return;
            }
        }

        float roll = Random.value;

        if (roll <= spawnChance)
        {
            characterUI.SetActive(true);
            spawned = true;


            Animator anim = characterUI.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play(0, -1, 0f);
            }

     
            if (audioSource != null)
            {
                if (spawnSounds != null && spawnSounds.Length > 0)
                {
                    audioSource.clip = spawnSounds[Random.Range(0, spawnSounds.Length)];
                }

                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            characterUI.SetActive(false);
        }
    }

    public void OnCharacterClicked()
    {
        if (!spawned) return;

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();


        if (!string.IsNullOrEmpty(cutsceneID) &&
            !GameState.playedVideoCutscenes.Contains(cutsceneID))
        {
            GameState.playedVideoCutscenes.Add(cutsceneID);

            StartCoroutine(PlayVideoThenDespawn());
            return;
        }


        DespawnCharacter();
    }


    IEnumerator PlayVideoThenDespawn()
    {
        spawned = false;

        if (VideoCutsceneManager.Instance != null && cutsceneVideo != null)
        {
            yield return StartCoroutine(
                VideoCutsceneManager.Instance.PlayVideoCutscene(cutsceneVideo)
            );
        }

        DespawnCharacter();
    }

    void DespawnCharacter()
    {
        characterUI.SetActive(false);
        spawned = false;
    }
}