using UnityEngine;
using UnityEngine.InputSystem;

public class FibulaCharacterController : MonoBehaviour
{
    [Header("Hand Gesture")]
    [SerializeField] private Transform handPoint;
    [SerializeField] private Vector3 handRestRotation = Vector3.zero;
    [SerializeField] private Vector3 handAttackRotation = new Vector3(-55f, 0f, 0f);
    [SerializeField] private float gestureSpeed = 10f;

    [Header("Weapon")]
    [SerializeField] private FibulaTelekineticWeapon weapon;

    [Header("Target")]
    [SerializeField] private Transform target;

    private bool attacking;

    private void Start()
    {
        if (handPoint != null)
            handPoint.localEulerAngles = handRestRotation;

        if (weapon != null)
            weapon.SetCharacter(transform);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            !attacking)
        {
            Attack();
        }

        UpdateHandGesture();
    }

    private void UpdateHandGesture()
    {
        if (handPoint == null)
            return;

        Quaternion desiredRotation = Quaternion.Euler(
            attacking ? handAttackRotation : handRestRotation
        );

        handPoint.localRotation = Quaternion.Lerp(
            handPoint.localRotation,
            desiredRotation,
            gestureSpeed * Time.deltaTime
        );
    }

    public void Attack()
    {
        if (weapon == null || target == null || attacking)
            return;

        bool attackStarted =
            weapon.PerformAttack(target, OnAttackFinished);

        if (attackStarted)
            attacking = true;
    }

    private void OnAttackFinished()
    {
        attacking = false;
    }

    private void OnDisable()
    {
        if (weapon != null)
            weapon.CancelAttack();

        attacking = false;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}