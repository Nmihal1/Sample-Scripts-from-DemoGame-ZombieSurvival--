using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    #region Properties

    public float health = 100f;
    public float force = 25f;
    public float velocityYBonus = 6f;
    [Range(0f, 2f)]
    public float targetRandomizer = .2f;

    private float currentHealth;
    private List<GameObject> children = new List<GameObject>();
    private Rigidbody rb;

    #endregion



    #region Lifecycle Methods

    void Awake()
    {
        currentHealth = health;

        for(var i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }

        rb = GetComponent<Rigidbody>();

        #if UNITY_EDITOR
        if (simulateDamage) StartCoroutine(SelfHit());
        if (randomThrow) StartCoroutine(RandomThrow());
        if (throwAt != null) StartCoroutine(DelayedThrow());
        #endif
    }

    #endregion



    #region Public Methods

    public void Hit(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0f)
        {
            // big explosion here

            // Destroyed
            Destroy(gameObject);
            return;
        }
        
        var max = children.Count;
        var percentageOfHealth = currentHealth / health;
        var partlyDestroyed = false; // used to set off explosion and such
        for (int i = 0; i < children.Count; i++)
        {
            var percentage = (float)(i + 1) / max;
            if (percentage > percentageOfHealth && children[i].activeSelf)
            {
                children[i].SetActive(false);
                partlyDestroyed = true;
            }
        }

        if (partlyDestroyed)
        {
            // small particle explosion
        }
    }


    public void ThrowTowards(Vector3 position)
    {
        var randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * targetRandomizer;
        rb.velocity = Vector3.Normalize(position + randomOffset) * force + Vector3.up * velocityYBonus;
    }

    public static GameObject CreateAndThrowTowards(GameObject prefab, Vector3 position)
    {
        GameObject instance = Instantiate(prefab);
        instance?.GetComponent<ProjectileController>()?.ThrowTowards(position);
        return instance;
    }

    #endregion



    #region Debug
    #if UNITY_EDITOR
    [Header("Debug options")]
    public bool simulateDamage = false;
    public bool randomThrow = false;
    public Transform throwAt;
    IEnumerator SelfHit()
    {
        float damage;
        string message;
        while(true)
        {
            yield return new WaitForSeconds(1f);
            damage = Random.Range(2f, 20f);
            message = $"[{gameObject.name}][{this.GetType().Name}] Hit for {damage}. HP: {currentHealth}/{health}";

            Hit(damage);
            Debug.Log(message);
            
            // if (damage > currentHealth) break;
        }
    }
    IEnumerator RandomThrow()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            var target = new Vector3(
                Random.Range(-10f, 10f),
                transform.position.y, //Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );
            Debug.Log($"Throw at {target}");
            ThrowTowards(target);
        }
    }
    IEnumerator DelayedThrow()
    {
        yield return new WaitForSeconds(3f);
        ThrowTowards(throwAt.position);
    }
    #endif
    #endregion

}
