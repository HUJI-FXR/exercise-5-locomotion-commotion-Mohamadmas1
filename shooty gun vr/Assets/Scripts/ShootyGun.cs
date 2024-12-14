using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VolumetricLines;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;



public class ShootyGun : MonoBehaviour
{
    [SerializeField] private float damage; // damage per bullet
    [SerializeField] private float fireRate; // bullets per minute
    [SerializeField] private int magazineSize; // bullets per magazine
    [SerializeField] private float reloadSpeed; // seconds to reload
    [SerializeField] private Transform muzzle; // where the bullets come out
    private int bulletsInMagazine;
    private bool isReloading;
    private float reloadTimer;
    private float secondsBetweenShots;
    private float timeToNextShot;

    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text reloadText;
    [SerializeField] private Image crosshairImage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip shootSound;

    [SerializeField] private Recoil recoil;
    [SerializeField] private float recoilIntensity;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private ActionBasedController rightController;
    [SerializeField] private ActionBasedController leftController;

    void Start()
    {
        bulletsInMagazine = magazineSize;
        isReloading = false;
        reloadTimer = 0;
        secondsBetweenShots = 60 / fireRate;
        timeToNextShot = 0;

        ammoText.text = bulletsInMagazine + " / " + magazineSize;
        reloadText.gameObject.SetActive(false);
        crosshairImage.gameObject.SetActive(true);
    }

    void Update()
    {
        // Decrease timers
        timeToNextShot -= Time.deltaTime;
        if (timeToNextShot <= 0) timeToNextShot = 0;
        reloadTimer -= Time.deltaTime;
        if (reloadTimer <= 0) reloadTimer = 0;

        // fill magazine when reloading is done
        if (isReloading && reloadTimer == 0)
        {
            isReloading = false;
            bulletsInMagazine = magazineSize;

            ammoText.text = bulletsInMagazine + " / " + magazineSize;
            reloadText.gameObject.SetActive(false);
            crosshairImage.gameObject.SetActive(true);
        }

        // Reload when left controller A button is pressed
        if (leftController != null && leftController.selectAction.action.ReadValue<float>() > 0.1f && !isReloading)
        {
            reloadTimer = reloadSpeed;
            isReloading = true;

            reloadText.gameObject.SetActive(true);
            crosshairImage.gameObject.SetActive(false);

            //audioSource.PlayOneShot(reloadSound);
        }

        if (rightController != null && rightController.selectAction.action.ReadValue<float>() > 0.1f && timeToNextShot == 0 && !isReloading && bulletsInMagazine > 0)
        {
            timeToNextShot = secondsBetweenShots;
            bulletsInMagazine--;
            ammoText.text = bulletsInMagazine + " / " + magazineSize;

            // Raycast to check if we hit something
            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit))
            {
                // Check if we hit a monster
                Monster monster = hit.transform.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damage);
                }

                // show a laser beam
                GameObject laser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
                laser.transform.LookAt(hit.point);
                float distance = Vector3.Distance(laserOrigin.position, hit.point);
                laser.GetComponentInChildren<VolumetricLineBehavior>().StartPos = new Vector3(0, 0, 0);
                laser.GetComponentInChildren< VolumetricLineBehavior>().EndPos = new Vector3(0, 0, distance);

                //audioSource.PlayOneShot(shootSound);
                recoil.ApplyRecoil(recoilIntensity);
            }
            else {
                // show a laser beam that goes into the distance
                GameObject laser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
                laser.transform.LookAt(laserOrigin.position + muzzle.forward * 100);
                laser.GetComponentInChildren<VolumetricLineBehavior>().StartPos = new Vector3(0, 0, 0);
                laser.GetComponentInChildren<VolumetricLineBehavior>().EndPos = new Vector3(0, 0, 300);

                //audioSource.PlayOneShot(shootSound);
                recoil.ApplyRecoil(recoilIntensity);
            }
        }

        /*if (Input.GetMouseButton(0) && timeToNextShot == 0 && !isReloading && bulletsInMagazine > 0)
        {
            timeToNextShot = secondsBetweenShots;
            bulletsInMagazine--;
            ammoText.text = bulletsInMagazine + " / " + magazineSize;

            // Raycast to check if we hit something
            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit))
            {
                // Check if we hit a monster
                Monster monster = hit.transform.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damage);
                }

                // show a laser beam
                GameObject laser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
                laser.transform.LookAt(hit.point);
                float distance = Vector3.Distance(laserOrigin.position, hit.point);
                laser.GetComponentInChildren<VolumetricLineBehavior>().StartPos = new Vector3(0, 0, 0);
                laser.GetComponentInChildren< VolumetricLineBehavior>().EndPos = new Vector3(0, 0, distance);

                //audioSource.PlayOneShot(shootSound);
                recoil.ApplyRecoil(recoilIntensity);
            }
            else {
                // show a laser beam that goes into the distance
                GameObject laser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
                laser.transform.LookAt(laserOrigin.position + muzzle.forward * 100);
                laser.GetComponentInChildren<VolumetricLineBehavior>().StartPos = new Vector3(0, 0, 0);
                laser.GetComponentInChildren<VolumetricLineBehavior>().EndPos = new Vector3(0, 0, 300);

                //audioSource.PlayOneShot(shootSound);
                recoil.ApplyRecoil(recoilIntensity);
            }
        }*/
    }
}
