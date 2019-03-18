using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnController : MonoBehaviour {

	public GameObject[] spawnPrefab;
	private GameObject curItem;
	public float spawnTimeLowerBound;
	public float spawnTimeUpperBound;


	// Use this for initialization
	void Start () {
		spawnItem ();
	}

	IEnumerator spawn() {
		yield return new WaitForSeconds(Random.Range(spawnTimeLowerBound, spawnTimeUpperBound));
		int rand = Random.Range (0, spawnPrefab.Length);
		curItem = Instantiate (spawnPrefab[rand]) as GameObject;
		curItem.layer = 13; // Changes layer to Spawned Item
		curItem.GetComponent<Weapons> ().spawnpoint = gameObject;
		curItem.GetComponent<Collider2D> ().isTrigger = true;
		curItem.GetComponent<Rigidbody2D> ().isKinematic = true;
		curItem.transform.position = transform.position;
	}
		
	public void spawnItem() {
		StartCoroutine (spawn());
	}

	public void stopParticleEffect() {
		transform.Find ("Particle System").GetComponent<ParticleSystem> ().Clear ();
		spawnItem ();
	}

	public GameObject pickupItem() {
		return curItem;
	}

	public bool curItemExists() {
		return curItem != null;
	}

}
