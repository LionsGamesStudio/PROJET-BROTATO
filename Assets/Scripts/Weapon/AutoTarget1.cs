
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AutoTarget1 : MonoBehaviour
// {
//     public GameObject bulletPrefab;
//     private Class_Weapon weapon;

//     public List<IEnemy> enemiesInRange = new List<IEnemy>();
//     public IEnemy monsterLocked;

//     private bool shootBullet = false;

//     void Start()
//     {
//         SphereCollider sphereCollider = GetComponent<SphereCollider>();
//         weapon = GetComponent<Class_Weapon>();
//         sphereCollider.radius = weapon.radius_Range;
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         IEnemy enemy = other.GetComponent<IEnemy>();

//         if (enemy != null)
//         {
//             enemiesInRange.Add(enemy);
//             UpdateFocus();
//         }
//     }

//     void OnTriggerExit(Collider other)
//     {
//         IEnemy enemy = other.GetComponent<IEnemy>();

//         if (enemy != null)
//         {
//             enemiesInRange.Remove(enemy);

//             if (enemy == monsterLocked)
//             {
//                 UpdateFocus();
//             }
//         }
//     }

//     void UpdateFocus()
//     {
//         monsterLocked = FindClosestEnemy();
//     }

//     // ✅ Méthode helper utilisant IsDestroyed
//     private bool IsEnemyValid(IEnemy enemy)
//     {
//         return enemy != null &&
//                !enemy.IsDestroyed &&
//                enemy.GameObject != null &&
//                enemy.GameObject.activeInHierarchy;
//     }

//     IEnemy FindClosestEnemy()
//     {
//         IEnemy closestEnemy = null;
//         float minDistance = float.MaxValue;

//         // ✅ Nettoyer la liste avec IsDestroyed
//         enemiesInRange.RemoveAll(enemy => enemy == null || enemy.IsDestroyed);

//         foreach (IEnemy enemy in enemiesInRange)
//         {
//             if (IsEnemyValid(enemy))
//             {
//                 float distance = Vector3.Distance(enemy.Transform.position, weapon.transform.position);

//                 if (distance < minDistance)
//                 {
//                     minDistance = distance;
//                     closestEnemy = enemy;
//                 }
//             }
//         }

//         return closestEnemy;
//     }

//     void Update()
//     {
//         // ✅ Vérification avec IsDestroyed
//         if (!IsEnemyValid(monsterLocked))
//         {
//             UpdateFocus();
//             return;
//         }

//         // Si on arrive ici, l'ennemi est valide
//         transform.LookAt(monsterLocked.Transform);

//         if (!shootBullet)
//         {
//             StartCoroutine(CreateBullet(weapon.shoot_Rate));
//         }
//     }

//     IEnumerator CreateBullet(float shoot_Rate)
//     {
//         shootBullet = true;

//         Projectile bullet = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation).GetComponent<Projectile>();
//         bullet.damage = weapon.damage;

//         yield return new WaitForSeconds(1 / shoot_Rate);

//         shootBullet = false;
//     }
// }
