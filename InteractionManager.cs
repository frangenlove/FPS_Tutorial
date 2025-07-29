using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���ڹ����������Ʒ������Ϊ
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public Weapon hoverdWeapon=null;
    public AmmoBox hoverdAmmoBox=null;
    public Throwable hoverdThrowable=null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        Ray ray= Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

 
        if (Physics.Raycast(ray,out hit))
        {
            GameObject objectHitByRaycast=hit.transform.gameObject;

            //�������Ƿ�ͣ����������
            if (objectHitByRaycast.GetComponent<Weapon>()&&objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon==false)
            {
                if(hoverdWeapon)
                    hoverdWeapon.GetComponent<Outline>().enabled = false;//��������ʱ�ر���һ��������outline

                hoverdWeapon = objectHitByRaycast.GetComponent<Weapon>();
                hoverdWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject); 
                }
            }
            else if(hoverdWeapon)
            {
                hoverdWeapon.GetComponent<Outline>().enabled = false;
            }

            //�������Ƿ�ͣ���ڵ�ҩ����
            if (objectHitByRaycast.GetComponent<AmmoBox>() )
            {
                if (hoverdAmmoBox)
                    hoverdAmmoBox.GetComponent<Outline>().enabled = false;

                hoverdAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoverdAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoverdAmmoBox);
                    //���ٵ�ҩ��
                    Destroy(objectHitByRaycast.gameObject); 
                }
            }
            else if (hoverdAmmoBox)
            {
                hoverdAmmoBox.GetComponent<Outline>().enabled = false;
            }

            //�������Ƿ�ͣ����Ͷ������
            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                if (hoverdThrowable)
                    hoverdThrowable.GetComponent<Outline>().enabled = false;

                hoverdThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoverdThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupThrowable(hoverdThrowable);
                }
            }
            else if (hoverdThrowable)
            {
                hoverdThrowable.GetComponent<Outline>().enabled = false;
            }

        }
    }
}
