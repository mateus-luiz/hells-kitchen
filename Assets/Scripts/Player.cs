using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    private bool isWalking;

    private void Update() {
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVector = inputVector.normalized;

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y); 
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        isWalking = moveDirection != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
    }

    public bool IsWalking() {
        return isWalking;
    }
}
