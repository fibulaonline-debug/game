using System;
using System.Collections;
using UnityEngine;

public class FibulaTelekineticWeapon : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Transform character;

    [Header("Orbit")]
    [SerializeField] private float orbitDistance = 1.3f;
    [SerializeField] private float orbitHeight = 1.25f;
    [SerializeField] private float orbitSpeed = 70f;

    [Header("Attack")]
    [SerializeField] private float attackSpeed = 12f;
    [SerializeField] private float minAttackDuration = 0.20f;
    [SerializeField] private float maxAttackDuration = 0.60f;
    [SerializeField] private float returnDuration = 0.30f;
    [SerializeField] private float curveHeight = 1.2f;
    [SerializeField] private float rotationSpeed = 1080f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;

    private float orbitAngle;
    private bool attacking;
    private Coroutine attackCoroutine;
    private Action pendingOnFinished;

    private void Update()
    {
        if (!attacking && character != null)
            UpdateOrbit();
    }

    private void UpdateOrbit()
    {
        orbitAngle += orbitSpeed * Time.deltaTime;

        float radians = orbitAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(radians) * orbitDistance,
            orbitHeight,
            Mathf.Sin(radians) * orbitDistance
        );

        transform.position = character.position + offset;

        Vector3 lookDirection =
            character.position +
            Vector3.up * orbitHeight -
            transform.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(lookDirection);
        }
    }

    public bool PerformAttack(Transform target, Action onFinished)
    {
        if (!isActiveAndEnabled ||
            attacking ||
            target == null ||
            character == null)
        {
            return false;
        }

        pendingOnFinished = onFinished;

        attackCoroutine = StartCoroutine(
            AttackRoutine(target, onFinished)
        );

        if (attackCoroutine == null)
        {
            pendingOnFinished = null;
            return false;
        }

        return true;
    }

    private IEnumerator AttackRoutine(
        Transform target,
        Action onFinished)
    {
        attacking = true;

        Vector3 startPosition = transform.position;

        float initialDistance =
            Vector3.Distance(
                startPosition,
                target.position + Vector3.up * 0.8f
            );

        float attackDuration = CalculateAttackDuration(
            initialDistance
        );

        float timer = 0f;

        while (timer < attackDuration)
        {
            if (target == null)
            {
                FinishAttack(onFinished);
                yield break;
            }

            timer += Time.deltaTime;

            float t =
                Mathf.Clamp01(timer / attackDuration);

            Vector3 targetPosition =
                target.position + Vector3.up * 0.8f;

            Vector3 middlePoint = Vector3.Lerp(
                startPosition,
                targetPosition,
                0.5f
            );

            middlePoint += Vector3.up * curveHeight;

            transform.position = QuadraticBezier(
                startPosition,
                middlePoint,
                targetPosition,
                t
            );

            transform.Rotate(
                Vector3.forward,
                rotationSpeed * Time.deltaTime,
                Space.Self
            );

            yield return null;
        }

        if (target != null)
        {
            transform.position =
                target.position + Vector3.up * 0.8f;

            FibulaTarget targetComponent =
                target.GetComponent<FibulaTarget>();

            if (targetComponent != null)
                targetComponent.TakeDamage(damage);
        }

        yield return new WaitForSeconds(0.08f);

        timer = 0f;

        while (timer < returnDuration)
        {
            if (character == null)
            {
                FinishAttack(onFinished);
                yield break;
            }

            timer += Time.deltaTime;

            float t =
                Mathf.Clamp01(timer / returnDuration);

            Vector3 currentOrbitPosition =
                GetOrbitPosition();

            transform.position = Vector3.Lerp(
                transform.position,
                currentOrbitPosition,
                t
            );

            yield return null;
        }

        if (character != null)
        {
            transform.position = GetOrbitPosition();
            UpdateOrbitRotation();
        }

        FinishAttack(onFinished);
    }

    private float CalculateAttackDuration(float distance)
    {
        if (attackSpeed <= 0f)
            return maxAttackDuration;

        float duration = distance / attackSpeed;

        return Mathf.Clamp(
            duration,
            minAttackDuration,
            maxAttackDuration
        );
    }

    private Vector3 GetOrbitPosition()
    {
        float radians = orbitAngle * Mathf.Deg2Rad;

        return character.position + new Vector3(
            Mathf.Cos(radians) * orbitDistance,
            orbitHeight,
            Mathf.Sin(radians) * orbitDistance
        );
    }

    private void UpdateOrbitRotation()
    {
        if (character == null)
            return;

        Vector3 lookDirection =
            character.position +
            Vector3.up * orbitHeight -
            transform.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(lookDirection);
        }
    }

    private void FinishAttack(Action onFinished)
    {
        attacking = false;
        attackCoroutine = null;

        pendingOnFinished = null;
        onFinished?.Invoke();
    }

    public void CancelAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        attacking = false;

        if (character != null)
        {
            transform.position = GetOrbitPosition();
            UpdateOrbitRotation();
        }

        Action callback = pendingOnFinished;
        pendingOnFinished = null;

        callback?.Invoke();
    }

    private void OnDisable()
    {
        CancelAttack();
    }

    private Vector3 QuadraticBezier(
        Vector3 start,
        Vector3 control,
        Vector3 end,
        float t)
    {
        float inverse = 1f - t;

        return
            inverse * inverse * start +
            2f * inverse * t * control +
            t * t * end;
    }

    public void SetCharacter(Transform newCharacter)
    {
        character = newCharacter;
    }
}