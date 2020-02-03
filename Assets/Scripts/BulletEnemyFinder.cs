using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletEnemyFinder : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float MoveSpeed = 350f;
    [SerializeField] float RotateSpeed = 2000f;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {

        FindClosestEnemy();

        rb.velocity = transform.up * MoveSpeed * Time.deltaTime;

        if (Target != null)
        {
            Vector3 targetVector = Target.position - transform.position;

            float rotatingIndex = Vector3.Cross(targetVector, transform.up).z;

            rb.angularVelocity = -1 * rotatingIndex * RotateSpeed * Time.deltaTime;
        }
        
    }

    public void FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");

        if (gos.Length == 0)
        {
            gos = GameObject.FindGameObjectsWithTag("Boss");
        }

        GameObject closest = null;
                
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if (closest == null)
        {
            rb.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 10 * Time.deltaTime);
            Target = this.transform;
        }
        else
        {
            Target = closest.transform;
        }
        
        return;
    }
}
