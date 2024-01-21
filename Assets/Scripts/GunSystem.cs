using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    private GameObject Bullets;
    private bool shooting, readyToShoot, reloading;
    private AudioSource audioSource;

    [Header("Gun Parameters")]
    public int gold;
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public GameObject projectile;

    [Header("References")]
    private Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public GameOver gameOver;


    public int enemiesKilled = 0;

    [Header("UI Elements")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI magazineText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI damageBoostGoldCostText;
    public TextMeshProUGUI currentDamage;
    public TextMeshProUGUI healthText;
    public GameObject healthBar;
    public int health;
    public int damageBoostGoldCost;
    public TextMeshProUGUI healGoldCostText;
    public int healGoldCost;
    public TextMeshProUGUI magazineCapBoostGoldCostText;
    public int magazineCapGoldCost;


    private void Awake()
    {
        Bullets = GameObject.FindGameObjectWithTag("BulletsHolder");
        audioSource = GetComponent<AudioSource>();
        fpsCam = Camera.main;
        bulletsLeft = magazineSize;
        readyToShoot = true;
        currentDamage.text = $"{damage}";

    }
    private void Update()
    {


        MyInput();

        magazineText.SetText(bulletsLeft + " / " + magazineSize);
        healthText.SetText(health.ToString());
        healthBar.GetComponent<Slider>().value = health / 100f;
        goldText.SetText(gold + " G ");
        scoreText.SetText("Score " + enemiesKilled);

        if (health <= 0)
        {
            gameOver.Setup(enemiesKilled);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            BoostDamage();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Heal();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            MagazineCap();
        }

    }

    private void Heal()
    {
        if (gold >= healGoldCost)
        {
            gold -= healGoldCost;
            healGoldCost += 25;
            healGoldCostText.text = $"{healGoldCost} G";

            health = 100;
        }
    }
    private void MagazineCap()
    {
        if (gold >= magazineCapGoldCost)
        {
            gold -= magazineCapGoldCost;
            magazineCapGoldCost += 25;
            magazineCapBoostGoldCostText.text = $"{magazineCapGoldCost} G";

            magazineSize += 10;
        }
    }

    private void BoostDamage()
    {
        if (gold >= damageBoostGoldCost)
        {
            gold -= damageBoostGoldCost;
            damageBoostGoldCost += 25;
            damageBoostGoldCostText.text = $"{damageBoostGoldCost} G";

            damage += 10;
            currentDamage.text = $"{damage}";
        }
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        audioSource.Play();
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        float fireDistance = 1.5f; // Ateþ noktasýnýn fpsCam'den uzaklýðý
        Vector3 fireDirection = fpsCam.transform.forward + new Vector3(x, y, 0);
        Vector3 firePoint = fpsCam.transform.position + fpsCam.transform.forward * fireDistance;

        GameObject bullet = Instantiate(projectile, firePoint, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(fireDirection * 50f, ForceMode.Impulse);


        muzzleFlash.GetComponent<ParticleSystem>().Play();

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }


    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}