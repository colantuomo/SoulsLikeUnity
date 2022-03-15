using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private float _hitTimeAwait = 0.09f;
    private Animator _animator;
    private Renderer _renderer;
    [SerializeField]
    private CinemachineImpulseSource impulseSource;
    private float _health = 5f;
    private ZombieMovements _zombieMovements;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();
        _zombieMovements = GetComponent<ZombieMovements>();
    }

    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        var dir = transform.position - hitPoint;
        StartCoroutine(HitStop(dir, damage));
    }

    private IEnumerator HitStop(Vector3 direction, float damageForce)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_hitTimeAwait);
        Time.timeScale = 1f;
        _animator.SetTrigger("Hit");
        impulseSource.GenerateImpulse();
        Debug.Log("Damage! " + damageForce);
        _zombieMovements.enabled = false;
    }

    // private void PushEnemy(Vector3 direction, float damageForce)
    // {
    //     _rb.AddForce(direction * damageForce, ForceMode.Impulse);
    // }
}
