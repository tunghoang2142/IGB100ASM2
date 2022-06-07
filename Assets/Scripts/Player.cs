using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 45f;
    public float meleeDam = 1f;
    public float meleeDelay = 0.6f;
    public float attackRange = 1f;

    float attackDelay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            Move();
            Rotate();
            Attack();
        }
    }

    void Attack()
    {
        attackDelay -= Time.deltaTime;
        //print(attackDelay);
        if (Input.GetMouseButton(0) && attackDelay <= 0)
        {
            attackDelay = meleeDelay;
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, attackRange))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    //print("Enemy Hit");
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(meleeDam);
                }
            }
        }
    }

    void Move()
    {
        Vector3 verticalMovement = this.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 horizontalMovement = this.transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        this.transform.position = this.transform.position + verticalMovement + horizontalMovement;
    }

    void Rotate()
    {
        float horizontalRotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotationSpeed;
        this.transform.Rotate(Vector3.up, horizontalRotation);
    }
}
