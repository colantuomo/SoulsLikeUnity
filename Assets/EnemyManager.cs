using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyManager : MonoBehaviour
{
    private Rigidbody _rb;
    private Renderer _renderer;
    [SerializeField]
    private CinemachineImpulseSource impulseSource;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        _renderer.material.color = Color.red;
        var dir = transform.position - hitPoint;
        StartCoroutine(HitStop(dir, damage));
    }

    private IEnumerator HitStop(Vector3 direction, float damageForce)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(.09f);
        Time.timeScale = 1f;
        Debug.Log("Damage! " + damageForce);
        PushEnemy(direction, damageForce);
        impulseSource.GenerateImpulse();
        _renderer.material.color = Color.white;
    }

    private void PushEnemy(Vector3 direction, float damageForce)
    {
        _rb.AddForce(direction * damageForce, ForceMode.Impulse);
    }
}
