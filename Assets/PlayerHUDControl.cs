using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDControl : MonoBehaviour {
    GameObject canvas;
    GameObject warnPanel;
    GameObject hurtPanel;

    float warnStart;
    float hurtStart;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("CommCanvas");
        warnPanel = canvas.transform.Find("WarnHint").gameObject;
        hurtPanel = canvas.transform.Find("HurtHint").gameObject;

        warnPanel.SetActive(false);
        hurtPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if(hurtStart != 0.0f && Time.time - hurtStart > 2.50f && hurtPanel.activeSelf)
        {
            hurtPanel.SetActive(false);
            hurtStart = 0.0f;
        }

        if(warnStart != 0.0f && Time.time - warnStart > 2.50f && warnPanel.activeSelf)
        {
            warnPanel.SetActive(false);
            warnStart = 0.0f;
        }
	}

    public void gotHurtBy(GameObject other)
    {
        Vector3 toOther = transform.InverseTransformPoint(other.transform.position);
        toOther.Normalize();
        toOther.Set((toOther.x+1)/2,(toOther.y+1)/2,(toOther.z+1)/2);
        hurtPanel.SetActive(true);
        hurtStart = Time.time;
        RectTransform canv_rect = canvas.GetComponent<RectTransform>();
        //hurtPanel.GetComponent<RectTransform>().position.Set(toOther.x*10, toOther.y*10, 0);
        hurtPanel.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(toOther.x * canv_rect.rect.width, toOther.z * canv_rect.rect.height, 0), new Quaternion());
    }

    public void gotWarnedBy(GameObject other)
    {
        Vector3 toOther = transform.InverseTransformPoint(other.transform.position);
        toOther.Normalize();
        toOther.Set((toOther.x + 1) / 2, (toOther.y + 1) / 2, (toOther.z + 1) / 2);
        warnPanel.SetActive(true);
        warnStart = Time.time;
        RectTransform canv_rect = canvas.GetComponent<RectTransform>();
        //hurtPanel.GetComponent<RectTransform>().position.Set(toOther.x*10, toOther.y*10, 0);
        warnPanel.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(toOther.x * canv_rect.rect.width, toOther.z * canv_rect.rect.height, 0), new Quaternion());
    }
}
