using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Boss1 : MonoBehaviour
{
    [SerializeField] GameObject _core, _outerShell, _player, _explosion;
    [SerializeField] int _outerShellLives = 40, _coreLives = 5, _currentPhase = 0;
    [SerializeField] float _fireRate, _canFire;
    [SerializeField] GameObject[] _amunitions;
    UIManager _uiManager;
    Animator _animator;
    Boss1_OuterShell _boss1_OuterShell;
    


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _boss1_OuterShell = _outerShell.GetComponent<Boss1_OuterShell>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        //_player = 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CollisionCheck();
        Phase();
    }

    private void CollisionCheck()
    {
        GameObject otherOuterShell = _boss1_OuterShell.colliderObj;
        GameObject otherCore = _core.GetComponent<Boss1_OuterShell>().colliderObj;

        if (otherOuterShell != null)
        {
            if (otherOuterShell.tag == "PlayerBullet")
            {
                Debug.Log("Boss Shell touch by Player's bullet.");
                //_player.UpdateScore(10);
                //_player.RefillMunition(1);
                Destroy(otherOuterShell.gameObject);
                TakeDamage();
            }

            if (otherOuterShell.tag == "Player")
            {
                Debug.Log("Boss Shell touch by Player.");
                //_player.UpdateScore(10);
                //_player.TakeDamage();
                TakeDamage();
            }
        }

        if (otherCore != null)
        {
            if (otherCore.tag == "PlayerBullet")
            {
                Debug.Log("Boss Core touch by Player's bullet.");
                //_player.UpdateScore(10);
                //_player.RefillMunition(1);
                Destroy(otherCore.gameObject);
                TakeDamage();
            }

            if (otherCore.tag == "Player")
            {
                Debug.Log("Boss Core touch by Player.");
                //_player.UpdateScore(10);
                //_player.TakeDamage();
                TakeDamage();
            }
        }

    }

    private void TakeDamage()
    {
        if (_animator.GetInteger("ShellHealth") > 0 && _animator.GetInteger("Phase") == 1)
        {
            _animator.SetBool("TakeDmg", true);
            _outerShellLives--;
            _animator.SetInteger("ShellHealth", _outerShellLives);

            if (_animator.GetInteger("ShellHealth") <= 0)
            {
                Instantiate(_explosion, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                _outerShell.SetActive(false);
                _animator.SetInteger("Phase", 2);

            }
        }

        if (_coreLives > 0 && _animator.GetInteger("Phase") == 2)
        {
            _animator.SetBool("TakeDmg", true);
            _coreLives--;
            _animator.SetInteger("CoreHealth", _coreLives);

            if (_animator.GetInteger("CoreHealth") <= 0)
            {
                Instantiate(_explosion, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                
                _animator.SetInteger("Phase", 3);
                _uiManager.GameWin();
                Destroy(this.gameObject);

            }
        }
    }

    private void Phase()
    {
        switch (_animator.GetInteger("Phase"))
        {
            case 0:

                break;
            case 1:
                Shooting1();

                break;
            case 2:
                _animator.SetFloat("FireRate", 1.5f);

                Shooting2();
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    private void Shooting1()
    {
        if ((Time.time > _canFire))
        {
            _canFire = Time.time + _animator.GetFloat("FireRate");
            Instantiate(_amunitions[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, _core.transform.eulerAngles.z + 45));
            Instantiate(_amunitions[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, _core.transform.eulerAngles.z + 135));
            Instantiate(_amunitions[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, _core.transform.eulerAngles.z - 135));
            Instantiate(_amunitions[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, _core.transform.eulerAngles.z - 45));

        }
    }

    private void Shooting2()
    {
        if ((Time.time > _canFire))
        {
            _canFire = Time.time + _animator.GetFloat("FireRate");
            GameObject newLaser = Instantiate(_amunitions[1], new Vector3(transform.position.x, transform.position.y -5, transform.position.z), Quaternion.Euler(0, 0, _core.transform.eulerAngles.z));
            newLaser.transform.parent = _core.transform;

        }
    }

}

