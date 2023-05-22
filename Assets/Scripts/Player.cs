using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask clearCounterLayer;

    private bool isWalking;
    private Vector3 lastInteractionDir;

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y); 

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if(!canMove) {
            //Não pode se mover 
            //Verica o movimento no eixo X
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);
            
            if(canMove) {
                //Pode se mover somente no eixo X
                moveDirection = moveDirectionX;
            } else {
                //Não pode se mover no eixo X
                //Verifica o eixo Z
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if(canMove) {
                    //Pode se mover somente no eixo Z
                    moveDirection = moveDirectionZ;
                }
            }
        }

        if(canMove) transform.position += moveDirection * moveDistance;
        isWalking = moveDirection != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);   
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y); 

        if(moveDirection != Vector3.zero) lastInteractionDir = moveDirection;

        float interactionDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractionDir,  out RaycastHit raycastHit, interactionDistance, clearCounterLayer)) {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                clearCounter.Interact();
            }
        }
    }
}
