using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 45f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
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
