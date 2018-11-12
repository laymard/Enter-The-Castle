using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

    public string LootName;
    public int LootQuantity;

    public float SmoothGoToTransform;
    private AudioSource CoinLootedSource;
	// Use this for initialization
	void Start () {
        CoinLootedSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public string GetLootName()
    {
        return LootName;
    }

    public int GetLootQuantity()
    {
        return LootQuantity;
    }

    IEnumerator GoToPlayer(Player _Player)
    {
        Transform playerTransform = _Player.transform;
        Debug.Log("Start Coroutine GoToPlayer");
        while (Vector3.Distance(transform.position, playerTransform.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position,Time.deltaTime);

            yield return null;
        }
        Debug.Log("Reached Target Then Disappear");
        gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator GoToTransform(Transform _TransformToAchieve)
    {
        
        while (Vector3.Distance(transform.position, _TransformToAchieve.position) > 0.1f)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, _TransformToAchieve.position, SmoothGoToTransform * Time.deltaTime);
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }

    public void OnLootedByPlayer(Player _pPlayer)
    {
        // go to player
        Vector3 vPlayerPosition = _pPlayer.transform.position;
        CoinLootedSource.Play();
        StartCoroutine(GoToTransform(Camera.main.transform));
        
        
        
    }
}
