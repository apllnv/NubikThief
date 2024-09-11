using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private Transform _shootPoint;

    [SerializeField]
    private float _timeBetweenShoot;

    private float _lastShootTime;

    private void Start()
    {
        _lastShootTime = -100f;
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    private void Shoot()
    {
        float time = Time.time;
        if (time - _lastShootTime >= _timeBetweenShoot)
        {
            _lastShootTime = time;
            GameObject bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = _shootPoint.up * _bulletSpeed;
        }
    }
}