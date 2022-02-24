using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoRagdoll : MonoBehaviour
{ 

    private Collider[] colliders;
    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;

    private Rigidbody dogRigidbody;
    private Collider dogCollider;
    private Animator dogAnimator;
    private AI ai;
    private NPCAttack npcAttack;
    private CharacterDamage charDamage;


    public GameObject ragdollRootObj;

    // Start is called before the first frame update
    void Start()
    {
        dogRigidbody = ragdollRootObj.GetComponent<Rigidbody>();
        dogCollider = ragdollRootObj.GetComponent<BoxCollider>();
        dogAnimator = GetComponent<Animator>();
        ai = GetComponent<AI>();
        npcAttack = GetComponent<NPCAttack>();
        charDamage = GetComponent<CharacterDamage>();

        colliders = ragdollRootObj.GetComponentsInChildren<Collider>();
        rigidbodies = ragdollRootObj.GetComponentsInChildren<Rigidbody>();

        SetRagdollState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRagdollState(bool state)
    {
        dogRigidbody.isKinematic = state;
        dogCollider.enabled = !state;
        dogAnimator.enabled = !state;

        /*foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }*/
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
        if (state == true)
        {
            foreach (CharacterJoint characterJoint in joints)
            {
                characterJoint.connectedBody = null;
            }
        }


        Debug.Log("Baaaa");

    }
}
