using UnityEngine;
using System.Collections;

public class SniperWeapon : WeaponBase {
    public int bulletDamage;
    private LineRenderer lineRenderer;
    public float maxShowLineTime = 0.5f;
    private float showLineTime;

    public override void Start()
    {
        base.Start();
        lineRenderer = bulletSpawn.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        showLineTime = maxShowLineTime;
    }

    public override void Update()
    {
        base.Update();
        
        if(lineRenderer.enabled)
        {
            showLineTime -= Time.deltaTime;
            Color lineColor = Color.red;
            lineColor.a = showLineTime / maxShowLineTime * 2;
            lineRenderer.SetColors(lineColor, lineColor);

            if(showLineTime <= 0)
            {
                lineRenderer.enabled = false;
                showLineTime = maxShowLineTime;
            }
        }
    }

    protected override void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawn.position, this.transform.right);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, bulletSpawn.transform.position);
        lineRenderer.SetPosition(1, hit.point);

        if (hit.collider != null)
        {
            Health health = hit.transform.GetComponent<Health>();
            Breakable breakHealth = hit.transform.GetComponent<Breakable>();
            if (health != null)
            {
                health.Damage(bulletDamage);
            }
            else if (breakHealth != null)
            {
                breakHealth.Damage(bulletDamage);
            }
        }
    }
}
