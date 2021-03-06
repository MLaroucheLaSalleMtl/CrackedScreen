﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeadCatch : Skill
{
    [SerializeField] private EnemyDetector skillHitBox;
    [SerializeField] private EnemyDetector aoeHitBox;
    [SerializeField] private int damage;
    [SerializeField] private Vector3 enemyKnockUpForce;
    private bool canRelease;
    private bool hasReleased;

    private bool _tookDamage;

    private void Update()
    {
//        if (!PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Blackhole"))
//        {
//            PlayerProperty.hasSuckedEnemy = false;
//        }
        
        if (PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Blackhole") && !canRelease)
        {
            hasReleased = false;
            PlayerProperty.controller.canControl = false;
        }
        else
        {
            if (!hasReleased)
            {
                PlayerProperty.controller.canControl = true;
                hasReleased = true;
            }
        }
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }


        
        if (suckEnemyDurationLeft > 0)
        {
            suckEnemyDurationLeft -= Time.deltaTime;
            if (!PlayerProperty.hasSuckedEnemy && suckEnemyDurationLeft<=0)
            {
                PlayerProperty.animator.SetTrigger("HeadCatchEmpty");
                print("Didn't absorb anyone");
                return;
            }

            if (PlayerProperty.hasSuckedEnemy)
            {
                enemyPicked.transform.position = skillHitBox.transform.position;

                if (suckEnemyDurationLeft <= 0)
                {
                    if (enemyPicked == null || !enemyPickedIsHitToAir)
                    {
                        PlayerProperty.hasSuckedEnemy = false;
                        return;
                    }

                    enemyPicked.GetComponent<Enemy>().enabled = true;

                    foreach (Enemy enemy in aoeHitBox._enemiesInRange.Select(col=>col.gameObject.GetComponent<Enemy>()))
                    {
                        if (PlayerProperty.playerPosition.x < enemy.transform.position.x)
                        {
                            enemy.GetComponent<Enemy>().GetKnockUp(enemyKnockUpForce);
                        }
                        else
                        {
                            enemy.GetComponent<Enemy>().GetKnockUp(new Vector3(-enemyKnockUpForce.x,enemyKnockUpForce.y,enemyKnockUpForce.z));
                        }
                        enemy.GetComponent<Enemy>().TakeDamage(damage);
                    }
                    

                    AudioManager.instance.PlaySound(AudioGroup.Character,"HeadCatchExplode");
                
                    // TODO enable headcatch explode effect if necessery
//                var explodeEffect = Instantiate(explodeParticleEffect, explodeSpawnPlace.position, explodeSpawnPlace.rotation);
//                explodeEffect.transform.parent = null;

                    enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",false);
                    PlayerProperty.hasSuckedEnemy = false;
                }
            }
            
        }
    }

    private float suckEnemyDuration = 1.1f;
    [SerializeField] private GameObject explodeParticleEffect;
    [SerializeField] private Transform explodeSpawnPlace;
    private float suckEnemyDurationLeft;
    private GameObject enemyPicked;
    private bool enemyPickedIsHitToAir;

    public override void Play()
    {
        if (_skillNotOnCooldown) // Check if the skill is on cooldown
        {
            playerController.canControl = false;
            StartCoroutine(PlayerCanControl(PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).length));
            AudioManager.instance.PlaySound(AudioGroup.Character,"HeadCatchPerform");
            GameManager.Instance.animator.SetTrigger("Black Hole");    // play player catch head animation

            _skillNotOnCooldown = false;
            base.Play();
            suckEnemyDurationLeft = suckEnemyDuration;

            if (!PlayerProperty.hasSuckedEnemy)
            {
                if (Time.time - GameManager.Instance.lastHitEnemyTime < 0.5f)
                {
                    enemyPicked = GameManager.Instance.lastHitEnemy;
                    if (enemyPicked != null)
                    {
                        if (enemyPicked.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HitToAir"))
                        {
                            PlayerProperty.hasSuckedEnemy = true;
                            enemyPickedIsHitToAir = true;
                            enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",true);
                            enemyPicked.GetComponent<Enemy>().enabled = false;
                            return;
                        }  
                    }

                }
                List<GameObject> enemies = skillHitBox._enemiesInRange;
                if (enemies.Count > 0)
                {
                    if (enemies[Random.Range(0, enemies.Count - 1)])
                    {
                        enemyPicked = enemies[Random.Range(0, enemies.Count - 1)].gameObject;
                    }
                    if (enemyPicked.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HitToAir"))
                    {
                        suckEnemyDurationLeft = suckEnemyDuration;
                        PlayerProperty.hasSuckedEnemy = true;
                        enemyPickedIsHitToAir = true;
                        enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",true);
                        print("is being sucked");
                        
                        enemyPicked.GetComponent<Enemy>().enabled = false;
                    }  
                }
                else {
                    PlayerProperty.hasSuckedEnemy = false;
                }
                   
                    
                
            }
        }
        else
        {
            print("Black hole attack is on cooldown");
        }
    }
}