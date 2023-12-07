using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce;
    public float distanceToPlayer;

    public Rigidbody2D rb;

    private int magazineSize = 6;
    private int currentAmmo;
    private bool isReloading = false;

    public Text ammoText;
    public Text reloadText;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // �߰��� �κ�: �ʱ� ź�� ����
        currentAmmo = magazineSize;
    }
    void Update()
    {
        // �� �߻� ������ ������ ����
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            Fire();
        }

        // �߰��� �κ�: ������ ����
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize && !isReloading)
        {
            StartCoroutine(Reload());
        }

        
        UpdateAmmoUI();
    }
        public void Fire()
    {
        if (currentAmmo > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            currentAmmo--;
        }
        else if (currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        // ������ ���� �� "Reloading" �ؽ�Ʈ ǥ��
        if (reloadText != null)
        {
            reloadText.text = "Reloading";
        }

        isReloading = true;
        yield return new WaitForSeconds(2f);
        currentAmmo = Mathf.Min(magazineSize, currentAmmo + magazineSize);
        isReloading = false;

        // ������ �Ϸ� �� "Reloading" �ؽ�Ʈ �����
        if (reloadText != null)
        {
            reloadText.text = "";
        }
    }
    public void SetTarget(Vector3 target)
    {
        Vector2 lookDir = target - transform.parent.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        rb.position = (Vector2)transform.parent.position + lookDir.normalized * 5f;

    }

    private void UpdateAmmoUI()
    {
        // AmmoText�� ���� ���� ������Ʈ
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo + " / " + magazineSize;
        }
    }
}
    